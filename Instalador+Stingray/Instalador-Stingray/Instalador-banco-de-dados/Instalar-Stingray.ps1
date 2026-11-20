[CmdletBinding()]
param(
    [switch]$Elevated,
    [string]$SecretFile,
    [string]$OwnerSid
)

$ErrorActionPreference = 'Stop'
$script:Version = '6.0.2'
$script:MySqlVersion = '8.4.13'
$script:ServiceName = 'StingrayMySQL'
$script:Port = 3307
$script:CredentialTarget = 'AquaFit.Stingray.MySql'
$script:CredentialUser = 'stingray_app'
$script:Entropy = [Text.Encoding]::UTF8.GetBytes('Stingray.Setup.v6.0.LocalMachine.Transport')
$script:RootPath = Join-Path $env:ProgramData 'Stingray\MySQL'
$script:DataPath = Join-Path $script:RootPath 'data'
$script:IniPath = Join-Path $script:RootPath 'my.ini'
$script:MysqlHome = Join-Path $env:ProgramFiles 'Stingray\MySQL'
$script:MysqldExe = Join-Path $script:MysqlHome 'bin\mysqld.exe'
$script:MysqlExe = Join-Path $script:MysqlHome 'bin\mysql.exe'
$script:LogPath = Join-Path $script:RootPath 'instalacao-v6.log'
$script:OriginalIniBytes = $null
$script:IniModified = $false
$script:ServiceNameKnown = $false
$script:SetupFiles = @()

function Write-Log {
    param([string]$Message, [ConsoleColor]$Color = [ConsoleColor]::Cyan)
    Write-Host $Message -ForegroundColor $Color
    if (Test-Path -LiteralPath $script:RootPath -PathType Container) {
        $line = '[{0}] {1}' -f (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'), $Message
        try { Add-Content -LiteralPath $script:LogPath -Value $line -Encoding UTF8 } catch { }
    }
}

function Test-IsAdministrator {
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent()
    $principal = New-Object Security.Principal.WindowsPrincipal($identity)
    return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Set-PrivateFileAcl {
    param([string]$Path, [string]$SidToInclude)
    if ([string]::IsNullOrWhiteSpace($SidToInclude)) {
        $SidToInclude = [Security.Principal.WindowsIdentity]::GetCurrent().User.Value
    }
    $acl = New-Object Security.AccessControl.FileSecurity
    $acl.SetAccessRuleProtection($true, $false)
    $sidStrings = @($SidToInclude, 'S-1-5-18', 'S-1-5-32-544') | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Select-Object -Unique
    foreach ($sidText in $sidStrings) {
        $sid = New-Object Security.Principal.SecurityIdentifier($sidText)
        $rule = New-Object Security.AccessControl.FileSystemAccessRule(
            $sid,
            [Security.AccessControl.FileSystemRights]::FullControl,
            [Security.AccessControl.AccessControlType]::Allow
        )
        [void]$acl.AddAccessRule($rule)
    }
    Set-Acl -LiteralPath $Path -AclObject $acl
}

function Normalize-Path {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return '' }
    $candidate = $Value.Trim().Trim('"').Replace('/', '\')
    return [IO.Path]::GetFullPath($candidate).TrimEnd('\').ToLowerInvariant()
}

function Get-IniValue {
    param([string]$Text, [string]$Name)
    $pattern = '(?im)^\s*' + [Regex]::Escape($Name) + '\s*=\s*"?([^"\r\n]+?)"?\s*$'
    $match = [Regex]::Match($Text, $pattern)
    if (-not $match.Success) { return $null }
    return $match.Groups[1].Value.Trim().Trim('"')
}

function Assert-ExpectedIni {
    if (-not (Test-Path -LiteralPath $script:IniPath -PathType Leaf)) {
        throw "O arquivo de configuração da instância existente não foi encontrado: $($script:IniPath). Nenhum dado foi modificado."
    }
    $text = [IO.File]::ReadAllText($script:IniPath)
    $base = Get-IniValue $text 'basedir'
    $data = Get-IniValue $text 'datadir'
    $portText = Get-IniValue $text 'port'
    if ((Normalize-Path $base) -ne (Normalize-Path $script:MysqlHome)) {
        throw "O basedir do my.ini ('$base') não corresponde à pasta privada do Stingray. Instalação interrompida por segurança."
    }
    if ((Normalize-Path $data) -ne (Normalize-Path $script:DataPath)) {
        throw "O datadir do my.ini ('$data') não corresponde à pasta esperada do Stingray. Nenhum banco foi alterado."
    }
    if ($portText -ne [string]$script:Port) {
        throw "O my.ini existente usa a porta '$portText', mas o aplicativo está configurado para $($script:Port). A configuração não será sobrescrita."
    }
    if ($text -match '(?im)^\s*init[-_]file\s*=') {
        throw 'O my.ini já contém init-file. O instalador não vai sobrescrever uma configuração temporária ou preexistente.'
    }
}

function Invoke-NativeProcess {
    param([string]$FilePath, [string]$Arguments, [int]$TimeoutSeconds = 600)
    $process = New-Object Diagnostics.Process
    $process.StartInfo = New-Object Diagnostics.ProcessStartInfo
    $process.StartInfo.FileName = $FilePath
    $process.StartInfo.Arguments = $Arguments
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.RedirectStandardOutput = $true
    $process.StartInfo.RedirectStandardError = $true
    $process.StartInfo.CreateNoWindow = $true
    if (-not $process.Start()) { throw "Não foi possível iniciar '$FilePath'." }
    $stdout = $process.StandardOutput.ReadToEndAsync()
    $stderr = $process.StandardError.ReadToEndAsync()
    if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
        try { $process.Kill() } catch { }
        throw "O processo '$FilePath' excedeu $TimeoutSeconds segundos."
    }
    $process.WaitForExit()
    return [PSCustomObject]@{
        ExitCode = $process.ExitCode
        Output = $stdout.GetAwaiter().GetResult()
        Error = $stderr.GetAwaiter().GetResult()
    }
}

function Invoke-MySqlClient {
    param(
        [string]$OptionsFile,
        [string]$Arguments = '',
        [string]$InputSql = $null,
        [int]$TimeoutSeconds = 180
    )
    $process = New-Object Diagnostics.Process
    $process.StartInfo = New-Object Diagnostics.ProcessStartInfo
    $process.StartInfo.FileName = $script:MysqlExe
    $process.StartInfo.Arguments = '--defaults-extra-file="' + $OptionsFile + '" --no-login-paths --default-character-set=utf8mb4 ' + $Arguments
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.RedirectStandardInput = $true
    $process.StartInfo.RedirectStandardOutput = $true
    $process.StartInfo.RedirectStandardError = $true
    $process.StartInfo.CreateNoWindow = $true
    if (-not $process.Start()) { throw "Não foi possível iniciar '$($script:MysqlExe)'." }
    $stdout = $process.StandardOutput.ReadToEndAsync()
    $stderr = $process.StandardError.ReadToEndAsync()
    if ($null -ne $InputSql) { $process.StandardInput.Write($InputSql) }
    $process.StandardInput.Close()
    if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
        try { $process.Kill() } catch { }
        throw 'O cliente MySQL excedeu o tempo limite.'
    }
    $process.WaitForExit()
    return [PSCustomObject]@{
        ExitCode = $process.ExitCode
        Output = $stdout.GetAwaiter().GetResult()
        Error = $stderr.GetAwaiter().GetResult()
    }
}

function New-ClientOptionsFile {
    param([string]$Path, [string]$User, [string]$Password, [string]$Database = '')
    $escaped = $Password.Replace('\', '\\').Replace('"', '\"')
    $content = @"
[client]
host=127.0.0.1
port=$($script:Port)
protocol=TCP
user=$User
password="$escaped"
default-character-set=utf8mb4
"@
    if (-not [string]::IsNullOrWhiteSpace($Database)) { $content += "database=$Database`r`n" }
    [IO.File]::WriteAllText($Path, $content, [Text.Encoding]::ASCII)
    Set-PrivateFileAcl $Path $OwnerSid
    $script:SetupFiles += $Path
}

function Test-MySqlTcp {
    $client = New-Object Net.Sockets.TcpClient
    $async = $null
    try {
        $async = $client.BeginConnect('127.0.0.1', $script:Port, $null, $null)
        if (-not $async.AsyncWaitHandle.WaitOne(1500)) { return $false }
        $client.EndConnect($async)
        return $true
    }
    catch { return $false }
    finally {
        if ($async -and $async.AsyncWaitHandle) { try { $async.AsyncWaitHandle.Close() } catch { } }
        $client.Close()
    }
}

function Test-ExpectedMySqlProcess {
    param($ProcessInfo)

    if ($null -eq $ProcessInfo) { return $false }
    if ([string]::IsNullOrWhiteSpace([string]$ProcessInfo.ExecutablePath)) { return $false }
    if ((Normalize-Path $ProcessInfo.ExecutablePath) -ne (Normalize-Path $script:MysqldExe)) { return $false }

    $commandLine = [string]$ProcessInfo.CommandLine
    if ([string]::IsNullOrWhiteSpace($commandLine)) { return $false }
    if ($commandLine.IndexOf($script:IniPath, [StringComparison]::OrdinalIgnoreCase) -lt 0) { return $false }

    return $true
}

function Test-ProcessDescendsFrom {
    param(
        [int]$ProcessId,
        [int]$AncestorProcessId,
        [int]$MaxDepth = 12
    )

    if ($ProcessId -eq $AncestorProcessId) { return $true }
    $visited = @{}
    $currentId = $ProcessId

    for ($depth = 0; $depth -lt $MaxDepth; $depth++) {
        if ($currentId -le 0 -or $visited.ContainsKey([string]$currentId)) { return $false }
        $visited[[string]$currentId] = $true

        $current = Get-CimInstance Win32_Process -Filter ("ProcessId={0}" -f $currentId) -ErrorAction SilentlyContinue
        if ($null -eq $current) { return $false }

        $parentId = [int]$current.ParentProcessId
        if ($parentId -eq $AncestorProcessId) { return $true }
        if ($parentId -le 0) { return $false }
        $currentId = $parentId
    }

    return $false
}

function Wait-MySqlTcp {
    param([int]$TimeoutSeconds = 90)
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    do {
        if (Test-MySqlTcp) {
            $serviceInfo = Get-CimInstance Win32_Service -Filter "Name='$($script:ServiceName)'" -ErrorAction SilentlyContinue
            if ($null -eq $serviceInfo -or $serviceInfo.State -ne 'Running' -or $serviceInfo.ProcessId -le 0) {
                throw "A porta $($script:Port) respondeu, mas o serviço privado do Stingray não está em execução."
            }

            $serviceProcessId = [int]$serviceInfo.ProcessId
            $serviceProcess = Get-CimInstance Win32_Process -Filter ("ProcessId={0}" -f $serviceProcessId) -ErrorAction SilentlyContinue
            if (-not (Test-ExpectedMySqlProcess $serviceProcess)) {
                throw "O PID $serviceProcessId registrado no serviço $($script:ServiceName) não corresponde ao mysqld.exe e ao my.ini privados esperados."
            }

            $listeners = @(Get-NetTCPConnection -LocalPort $script:Port -State Listen -ErrorAction Stop)
            $ownedListeners = @()
            foreach ($listener in $listeners) {
                $listenerProcessId = [int]$listener.OwningProcess
                $listenerProcess = Get-CimInstance Win32_Process -Filter ("ProcessId={0}" -f $listenerProcessId) -ErrorAction SilentlyContinue
                if ((Test-ExpectedMySqlProcess $listenerProcess) -and
                    (Test-ProcessDescendsFrom -ProcessId $listenerProcessId -AncestorProcessId $serviceProcessId)) {
                    $ownedListeners += $listener
                }
            }

            if ($listeners.Count -eq 0 -or $ownedListeners.Count -eq 0) {
                $listenerPids = @($listeners | ForEach-Object { [string]$_.OwningProcess } | Sort-Object -Unique) -join ', '
                throw "A porta $($script:Port) não pertence à árvore de processos do serviço StingrayMySQL. PID do serviço: $serviceProcessId; PID(s) da porta: $listenerPids. Binário e my.ini também foram conferidos."
            }
            return
        }
        $service = Get-Service -Name $script:ServiceName -ErrorAction SilentlyContinue
        if ($service -and $service.Status -ne 'Running') {
            throw "O serviço $($script:ServiceName) deixou de executar. Consulte $($script:DataPath)\mysql-error.log."
        }
        Start-Sleep -Seconds 1
    } while ((Get-Date) -lt $deadline)
    throw "O MySQL não abriu a porta TCP $($script:Port) em $TimeoutSeconds segundos. Consulte $($script:DataPath)\mysql-error.log."
}

function Wait-MySqlApplicationLogin {
    param([string]$OptionsFile, [int]$TimeoutSeconds = 60)
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    $last = $null
    $query = '--database=teste_projeto --batch --skip-column-names --connect-timeout=5 --execute="SELECT CURRENT_USER(), DATABASE(), (SELECT COUNT(*) FROM information_schema.tables WHERE table_schema=''teste_projeto'')"'
    do {
        $last = Invoke-MySqlClient $OptionsFile $query $null 15
        if ($last.ExitCode -eq 0) {
            $fields = @($last.Output.Trim() -split "`t")
            if ($fields.Count -ge 3 -and $fields[1] -eq 'teste_projeto' -and $fields[2] -match '^\d+$' -and [int]$fields[2] -gt 0) {
                return [PSCustomObject]@{ Output = $last.Output.Trim(); TableCount = [int]$fields[2] }
            }
        }
        Start-Sleep -Seconds 1
    } while ((Get-Date) -lt $deadline)
    $details = if ($last) { (@($last.Error, $last.Output) -join ' ').Trim() } else { 'Sem resposta do cliente MySQL.' }
    throw "A autenticação do usuário stingray_app ou o banco teste_projeto não passou na validação. Detalhe: $details"
}

function Stop-StingrayService {
    $service = Get-Service -Name $script:ServiceName -ErrorAction Stop
    if ($service.Status -ne 'Stopped') { Stop-Service -Name $script:ServiceName -Force }
    $deadline = (Get-Date).AddSeconds(60)
    do {
        Start-Sleep -Milliseconds 500
        $service = Get-Service -Name $script:ServiceName
        if ($service.Status -eq 'Stopped') { return }
    } while ((Get-Date) -lt $deadline)
    throw "O serviço $($script:ServiceName) não parou em até 60 segundos."
}

function Start-StingrayService {
    $service = Get-Service -Name $script:ServiceName -ErrorAction Stop
    if ($service.Status -ne 'Running') { Start-Service -Name $script:ServiceName }
    $deadline = (Get-Date).AddSeconds(90)
    do {
        Start-Sleep -Seconds 1
        $service = Get-Service -Name $script:ServiceName
        if ($service.Status -eq 'Running') { return }
    } while ((Get-Date) -lt $deadline)
    throw "O serviço $($script:ServiceName) não iniciou em até 90 segundos."
}

function Save-CredentialForCurrentInteractiveUser {
    param([string]$Password)
    if (-not ('StingrayV5CredentialWriter' -as [type])) {
        Add-Type -Language CSharp -TypeDefinition @'
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

public static class StingrayV5CredentialWriter
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct CREDENTIAL
    {
        public UInt32 Flags;
        public UInt32 Type;
        [MarshalAs(UnmanagedType.LPWStr)] public string TargetName;
        [MarshalAs(UnmanagedType.LPWStr)] public string Comment;
        public Int64 LastWritten;
        public UInt32 CredentialBlobSize;
        public IntPtr CredentialBlob;
        public UInt32 Persist;
        public UInt32 AttributeCount;
        public IntPtr Attributes;
        [MarshalAs(UnmanagedType.LPWStr)] public string TargetAlias;
        [MarshalAs(UnmanagedType.LPWStr)] public string UserName;
    }

    [DllImport("advapi32.dll", EntryPoint="CredWriteW", CharSet=CharSet.Unicode, SetLastError=true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CredWrite(ref CREDENTIAL credential, UInt32 flags);

    [DllImport("advapi32.dll", EntryPoint="CredReadW", CharSet=CharSet.Unicode, SetLastError=true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CredRead(string target, UInt32 type, UInt32 flags, out IntPtr credential);

    [DllImport("advapi32.dll", EntryPoint="CredFree", SetLastError=false)]
    private static extern void CredFree(IntPtr credential);

    public static void Save(string target, string user, string password)
    {
        byte[] secret = Encoding.Unicode.GetBytes(password);
        GCHandle pinned = GCHandle.Alloc(secret, GCHandleType.Pinned);
        try
        {
            CREDENTIAL credential = new CREDENTIAL();
            credential.Type = 1;
            credential.TargetName = target;
            credential.UserName = user;
            credential.CredentialBlobSize = (UInt32)secret.Length;
            credential.CredentialBlob = pinned.AddrOfPinnedObject();
            credential.Persist = 2;
            if (!CredWrite(ref credential, 0))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CredWrite falhou.");
        }
        finally
        {
            pinned.Free();
            Array.Clear(secret, 0, secret.Length);
        }
    }

    public static void Verify(string target, string expectedUser, string expectedPassword)
    {
        IntPtr pointer;
        if (!CredRead(target, 1, 0, out pointer))
            throw new Win32Exception(Marshal.GetLastWin32Error(), "A credencial gravada não pôde ser lida de volta no perfil Windows atual.");
        try
        {
            CREDENTIAL credential = (CREDENTIAL)Marshal.PtrToStructure(pointer, typeof(CREDENTIAL));
            string actualUser = credential.UserName;
            if (credential.CredentialBlob == IntPtr.Zero || credential.CredentialBlobSize == 0)
                throw new InvalidOperationException("A credencial gravada não contém segredo.");
            byte[] bytes = new byte[checked((int)credential.CredentialBlobSize)];
            try
            {
                Marshal.Copy(credential.CredentialBlob, bytes, 0, bytes.Length);
                string actualPassword = Encoding.Unicode.GetString(bytes).TrimEnd('\0');
                if (!String.Equals(actualUser, expectedUser, StringComparison.Ordinal) ||
                    !String.Equals(actualPassword, expectedPassword, StringComparison.Ordinal))
                    throw new InvalidOperationException("A credencial lida no perfil Windows atual não corresponde ao usuário e à senha preparados no MySQL.");
            }
            finally { Array.Clear(bytes, 0, bytes.Length); }
        }
        finally { CredFree(pointer); }
    }
}
'@
    }
    [StingrayV5CredentialWriter]::Save($script:CredentialTarget, $script:CredentialUser, $Password)
}

function Read-SecretFromProtectedFile {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Arquivo temporário protegido não encontrado: $Path" }
    Add-Type -AssemblyName System.Security
    $encrypted = [IO.File]::ReadAllBytes($Path)
    try {
        $clear = [Security.Cryptography.ProtectedData]::Unprotect(
            $encrypted,
            $script:Entropy,
            [Security.Cryptography.DataProtectionScope]::LocalMachine
        )
        try { return [Text.Encoding]::UTF8.GetString($clear) }
        finally { [Array]::Clear($clear, 0, $clear.Length) }
    }
    finally { [Array]::Clear($encrypted, 0, $encrypted.Length) }
}

function Ensure-VisualCppRuntime {
    $runtime = Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\x64' -ErrorAction SilentlyContinue
    if ($runtime -and $runtime.Installed -eq 1) {
        Write-Log 'Microsoft Visual C++ Redistributable x64 já está instalado.'
        return
    }
    Write-Log 'Baixando e instalando o Microsoft Visual C++ Redistributable x64...'
    $tempFile = Join-Path $env:TEMP ('Stingray-vc-redist-' + [Guid]::NewGuid().ToString('N') + '.exe')
    try {
        Invoke-WebRequest 'https://aka.ms/vc14/vc_redist.x64.exe' -OutFile $tempFile -UseBasicParsing
        $result = Invoke-NativeProcess $tempFile '/install /quiet /norestart' 600
        if ($result.ExitCode -notin @(0, 3010)) { throw "O Visual C++ Redistributable retornou o código $($result.ExitCode). $($result.Error)" }
    }
    finally { if (Test-Path $tempFile) { Remove-Item $tempFile -Force -ErrorAction SilentlyContinue } }
}

function Ensure-MySqlBinaries {
    if (Test-Path -LiteralPath $script:MysqldExe -PathType Leaf) {
        if (-not (Test-Path -LiteralPath $script:MysqlExe -PathType Leaf)) {
            throw "O mysql.exe não foi encontrado ao lado dos binários existentes. Os arquivos existentes foram preservados."
        }
        $version = Invoke-NativeProcess $script:MysqldExe '--version' 30
        if ($version.ExitCode -ne 0 -or $version.Output -notmatch 'Ver\s+8\.4\.') {
            throw "Os binários MySQL existentes não são da série 8.4. Eles foram preservados e não serão substituídos automaticamente."
        }
        Write-Log ('Binários MySQL existentes reutilizados: ' + $version.Output.Trim())
        return
    }
    if (Test-Path -LiteralPath $script:MysqlHome) {
        throw "A pasta '$($script:MysqlHome)' existe, mas não contém mysqld.exe. Para não sobrescrever binários ou dados, a instalação foi interrompida."
    }
    Ensure-VisualCppRuntime
    $tempRoot = Join-Path $env:TEMP ('Stingray-MySQL-' + [Guid]::NewGuid().ToString('N'))
    $zipPath = Join-Path $env:TEMP ('mysql-' + $script:MySqlVersion + '-' + [Guid]::NewGuid().ToString('N') + '.zip')
    try {
        New-Item -ItemType Directory -Force -Path $tempRoot | Out-Null
        $url = 'https://cdn.mysql.com/Downloads/MySQL-8.4/mysql-' + $script:MySqlVersion + '-winx64.zip'
        Write-Log ('Baixando MySQL Community Server ' + $script:MySqlVersion + '...')
        Invoke-WebRequest $url -OutFile $zipPath -UseBasicParsing
        if ((Get-Item -LiteralPath $zipPath).Length -lt 100MB) { throw 'O pacote MySQL baixado parece incompleto (arquivo menor que 100 MB).' }
        Expand-Archive -LiteralPath $zipPath -DestinationPath $tempRoot -Force
        $extracted = Join-Path $tempRoot ('mysql-' + $script:MySqlVersion + '-winx64')
        if (-not (Test-Path (Join-Path $extracted 'bin\mysqld.exe'))) { throw 'O pacote não contém bin\mysqld.exe no caminho esperado.' }
        New-Item -ItemType Directory -Force -Path (Split-Path $script:MysqlHome -Parent) | Out-Null
        Move-Item -LiteralPath $extracted -Destination $script:MysqlHome
        if (-not (Test-Path $script:MysqldExe)) { throw 'O executável mysqld.exe não foi instalado.' }
        Write-Log 'Binários privados do MySQL instalados.'
    }
    finally {
        if (Test-Path $tempRoot) { Remove-Item $tempRoot -Recurse -Force -ErrorAction SilentlyContinue }
        if (Test-Path $zipPath) { Remove-Item $zipPath -Force -ErrorAction SilentlyContinue }
    }
}

function Register-StingrayService {
    $arguments = '--install "' + $script:ServiceName + '" --defaults-file="' + $script:IniPath + '"'
    $result = Invoke-NativeProcess $script:MysqldExe $arguments 60
    if ($result.ExitCode -ne 0) {
        throw "Não foi possível registrar o serviço $($script:ServiceName). $($result.Error) $($result.Output)"
    }
    Set-Service -Name $script:ServiceName -StartupType Automatic
    $script:ServiceNameKnown = $true
}

function Assert-NoForeignListenerWhenNew {
    if (Test-MySqlTcp) {
        throw "A porta $($script:Port) já responde, mas não existe um serviço StingrayMySQL reconhecido. O instalador não tentará usar nem modificar esse servidor."
    }
}

function Install-NewPrivateInstance {
    Write-Log 'Instalação nova detectada: inicializando a instância privada e criando o banco do zero.'
    Assert-NoForeignListenerWhenNew
    if (Test-Path -LiteralPath $script:IniPath) { throw 'Existe um my.ini sem serviço correspondente. Ele foi preservado; instalação cancelada para evitar sobrescrita.' }
    if (Test-Path -LiteralPath $script:DataPath -PathType Leaf) {
        throw "O caminho de dados esperado é um arquivo, não uma pasta: $($script:DataPath). Nada foi sobrescrito."
    }
    if (Test-Path -LiteralPath $script:DataPath -PathType Container) {
        $items = @(Get-ChildItem -LiteralPath $script:DataPath -Force -ErrorAction SilentlyContinue)
        if ($items.Count -gt 0) {
            $orphanBackupRoot = Join-Path $script:RootPath 'backups'
            New-Item -ItemType Directory -Force -Path $orphanBackupRoot | Out-Null
            $orphanPath = Join-Path $orphanBackupRoot ('data-anterior-sem-config-' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8))
            Write-Log ('Diretório de dados órfão encontrado sem my.ini/serviço. Movendo-o integralmente para backup físico: ' + $orphanPath) -Color Yellow
            Move-Item -LiteralPath $script:DataPath -Destination $orphanPath -ErrorAction Stop
        }
        else {
            Remove-Item -LiteralPath $script:DataPath -Force
        }
    }

    New-Item -ItemType Directory -Force -Path $script:RootPath, $script:DataPath | Out-Null
    $iniContent = @"
[mysqld]
basedir="$($script:MysqlHome.Replace('\','/'))"
datadir="$($script:DataPath.Replace('\','/'))"
port=$($script:Port)
bind-address=127.0.0.1
mysqlx=0
character-set-server=utf8mb4
collation-server=utf8mb4_unicode_ci
local_infile=OFF
log-error="$($script:DataPath.Replace('\','/'))/mysql-error.log"
"@
    $iniContent | Set-Content -LiteralPath $script:IniPath -Encoding ASCII

    Write-Log 'Inicializando a primeira instância MySQL. Esta etapa só ocorre quando não há serviço nem dados anteriores.'
    $initArgs = '--defaults-file="' + $script:IniPath + '" --initialize-insecure --console'
    $init = Invoke-NativeProcess $script:MysqldExe $initArgs 600
    $initLog = Join-Path $script:RootPath 'inicializacao-v6.log'
    [IO.File]::WriteAllText($initLog, ('ExitCode: ' + $init.ExitCode + "`r`nSTDOUT:`r`n" + $init.Output + "`r`nSTDERR:`r`n" + $init.Error), [Text.Encoding]::UTF8)
    if ($init.ExitCode -ne 0) { throw "Falha ao inicializar o diretório de dados MySQL. Consulte $initLog e $($script:DataPath)\mysql-error.log." }

    Register-StingrayService
    Start-StingrayService
    Wait-MySqlTcp 90

    $rootOptions = Join-Path $script:RootPath ('setup\root-empty-' + [Guid]::NewGuid().ToString('N') + '.ini')
    New-Item -ItemType Directory -Force -Path (Split-Path $rootOptions -Parent) | Out-Null
    New-ClientOptionsFile $rootOptions 'root' '' ''

    $appPassword = $script:AppPassword
    $rootPassword = [Guid]::NewGuid().ToString('N')
    $schemaPath = Join-Path $PSScriptRoot 'CriarBanco.sql'
    if (-not (Test-Path -LiteralPath $schemaPath -PathType Leaf)) { throw "O arquivo CriarBanco.sql não foi encontrado ao lado do instalador: $schemaPath" }
    $schema = [IO.File]::ReadAllText($schemaPath, [Text.Encoding]::UTF8)
    $accountSql = @"

CREATE USER IF NOT EXISTS 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'localhost';
CREATE USER IF NOT EXISTS 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'127.0.0.1';
"@
    $adminSql = "`r`nALTER USER 'root'@'localhost' IDENTIFIED BY '$rootPassword';`r`nFLUSH PRIVILEGES;`r`n"
    Write-Log 'Criando o esquema inicial e as contas de conexão na instância nova...'
    $apply = Invoke-MySqlClient $rootOptions '' ($schema + $accountSql + $adminSql) 600
    if ($apply.ExitCode -ne 0) {
        $details = (@($apply.Error, $apply.Output) -join ' ').Trim()
        throw "Não foi possível aplicar CriarBanco.sql na instância nova. $details"
    }

    $appOptions = Join-Path $script:RootPath ('setup\app-fresh-' + [Guid]::NewGuid().ToString('N') + '.ini')
    New-ClientOptionsFile $appOptions $script:CredentialUser $appPassword 'teste_projeto'
    $null = Wait-MySqlApplicationLogin $appOptions 60
    Write-Log 'Instância nova e esquema inicial validados com sucesso.' -Color Green
}

function Repair-ExistingInstanceAccount {
    Write-Log 'Instância existente detectada: será feito backup lógico do banco atual e, em seguida, criado um banco novo vazio.' -Color Yellow
    Assert-ExpectedIni
    if (-not (Test-Path -LiteralPath $script:DataPath -PathType Container)) {
        throw "O diretório de dados existente não foi encontrado: $($script:DataPath). Nenhum dado foi modificado."
    }

    $service = Get-CimInstance Win32_Service -Filter "Name='$($script:ServiceName)'" -ErrorAction SilentlyContinue
    if ($null -eq $service) {
        Write-Log 'Serviço ausente, mas my.ini e diretório de dados existem. Registrando o serviço sobre os dados existentes.'
        Register-StingrayService
    }
    else {
        if ($service.PathName.IndexOf($script:MysqldExe, [StringComparison]::OrdinalIgnoreCase) -lt 0 -or
            $service.PathName.IndexOf($script:IniPath, [StringComparison]::OrdinalIgnoreCase) -lt 0) {
            throw 'O serviço StingrayMySQL não aponta exatamente para os binários e my.ini esperados. Nenhum serviço ou banco foi alterado.'
        }
        if ($service.StartName -ne 'LocalSystem') {
            throw "O serviço está configurado com a conta '$($service.StartName)', diferente de LocalSystem. Reparo cancelado por segurança."
        }
        $script:ServiceNameKnown = $true
    }
    Set-Service -Name $script:ServiceName -StartupType Automatic
    if ((Get-Service -Name $script:ServiceName).Status -ne 'Running') { Start-StingrayService }
    Wait-MySqlTcp 90

    $setupDir = Join-Path $script:RootPath 'setup'
    $backupRoot = Join-Path $script:RootPath 'backups'
    New-Item -ItemType Directory -Force -Path $setupDir, $backupRoot | Out-Null

    $token = [Guid]::NewGuid().ToString('N')
    $sqlPath = Join-Path $setupDir ('fresh-db-' + $token + '.sql')
    $rootOptions = Join-Path $setupDir ('root-fresh-' + $token + '.ini')
    $appOptions = Join-Path $setupDir ('app-fresh-' + $token + '.ini')
    $script:SetupFiles += @($sqlPath, $rootOptions, $appOptions)

    # Senhas geradas pelo instalador contêm somente caracteres hexadecimais.
    $appPassword = $script:AppPassword
    $rootPassword = [Guid]::NewGuid().ToString('N')
    $sql = @"
ALTER USER 'root'@'localhost' IDENTIFIED BY '$rootPassword';
CREATE USER IF NOT EXISTS 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'localhost';
CREATE USER IF NOT EXISTS 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'127.0.0.1';
FLUSH PRIVILEGES;
"@
    [IO.File]::WriteAllText($sqlPath, $sql, [Text.Encoding]::ASCII)
    Set-PrivateFileAcl $sqlPath $OwnerSid
    New-ClientOptionsFile $rootOptions 'root' $rootPassword ''
    New-ClientOptionsFile $appOptions $script:CredentialUser $appPassword 'teste_projeto'

    $script:OriginalIniBytes = [IO.File]::ReadAllBytes($script:IniPath)
    $iniBackup = $script:IniPath + '.antes-banco-novo-' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '.bak'
    Copy-Item -LiteralPath $script:IniPath -Destination $iniBackup -ErrorAction Stop
    $initPathForIni = $sqlPath.Replace('\', '/')
    $iniText = [Text.Encoding]::ASCII.GetString($script:OriginalIniBytes).TrimEnd()
    $updatedIni = $iniText + "`r`n`r`n[mysqld]`r`ninit-file=`"$initPathForIni`"`r`n"
    $backupFile = $null
    $replacementStarted = $false
    $replacementCommitted = $false

    try {
        Write-Log 'Preparando acesso administrativo temporário e sincronizando a credencial da aplicação...'
        Stop-StingrayService
        $script:IniModified = $true
        [IO.File]::WriteAllText($script:IniPath, $updatedIni, [Text.Encoding]::ASCII)
        Start-StingrayService
        Wait-MySqlTcp 90

        # Confirma que a credencial administrativa temporária foi aplicada antes de tocar no banco.
        $rootCheck = Invoke-MySqlClient $rootOptions '--batch --skip-column-names --execute="SELECT 1"' $null 30
        if ($rootCheck.ExitCode -ne 0) {
            throw ('Não foi possível autenticar como root após a preparação temporária. ' + $rootCheck.Error + ' ' + $rootCheck.Output)
        }

        $existsResult = Invoke-MySqlClient $rootOptions '--batch --skip-column-names --execute="SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name=''teste_projeto''"' $null 30
        if ($existsResult.ExitCode -ne 0) { throw ('Não foi possível verificar a existência do banco atual. ' + $existsResult.Error) }
        $databaseExists = ($existsResult.Output.Trim() -eq '1')

        if ($databaseExists) {
            $dumpExe = Join-Path $script:MysqlHome 'bin\mysqldump.exe'
            if (-not (Test-Path -LiteralPath $dumpExe -PathType Leaf)) {
                throw "mysqldump.exe não foi encontrado em '$dumpExe'. O banco existente não será removido sem backup."
            }
            $backupFile = Join-Path $backupRoot ('teste_projeto-' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + $token.Substring(0, 8) + '.sql')
            $dumpArgs = '--defaults-extra-file="' + $rootOptions + '" --no-login-paths --single-transaction --routines --triggers --events --hex-blob --no-tablespaces --databases teste_projeto --result-file="' + $backupFile + '"'
            Write-Log ('Criando backup lógico antes de substituir o banco: ' + $backupFile) -Color Yellow
            $dump = Invoke-NativeProcess $dumpExe $dumpArgs 900
            if ($dump.ExitCode -ne 0 -or -not (Test-Path -LiteralPath $backupFile -PathType Leaf) -or (Get-Item -LiteralPath $backupFile).Length -lt 100) {
                throw ('O backup lógico não foi concluído. O banco existente foi preservado. ' + $dump.Error + ' ' + $dump.Output)
            }
            Set-PrivateFileAcl $backupFile $OwnerSid
            $hash = (Get-FileHash -LiteralPath $backupFile -Algorithm SHA256).Hash
            $manifest = @(
                'Backup lógico do banco Stingray',
                ('Data: ' + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss')),
                'Banco: teste_projeto',
                ('Arquivo: ' + [IO.Path]::GetFileName($backupFile)),
                ('Tamanho: ' + (Get-Item -LiteralPath $backupFile).Length),
                ('SHA256: ' + $hash),
                'Este backup não é restaurado automaticamente.'
            ) -join "`r`n"
            [IO.File]::WriteAllText($backupFile + '.manifesto.txt', $manifest, [Text.Encoding]::UTF8)
            Set-PrivateFileAcl ($backupFile + '.manifesto.txt') $OwnerSid
            Write-Log ('Backup lógico concluído e verificado. SHA256: ' + $hash) -Color Green
        }
        else {
            Write-Log 'O banco teste_projeto ainda não existe; será criado diretamente pelo esquema versionado.' -Color Yellow
        }

        # O banco só é descartado depois que o dump do banco anterior foi validado.
        $schemaPath = Join-Path $PSScriptRoot 'CriarBanco.sql'
        if (-not (Test-Path -LiteralPath $schemaPath -PathType Leaf)) {
            throw "O arquivo CriarBanco.sql não foi encontrado: $schemaPath. O banco existente permanece preservado."
        }
        $schema = [IO.File]::ReadAllText($schemaPath, [Text.Encoding]::UTF8)
        $freshDatabaseSql = "DROP DATABASE IF EXISTS teste_projeto;`r`n" + $schema + "`r`n" + @"
CREATE USER IF NOT EXISTS 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'localhost';
CREATE USER IF NOT EXISTS 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'127.0.0.1';
FLUSH PRIVILEGES;
"@
        Write-Log 'Criando um banco teste_projeto novo a cada execução, conforme solicitado...' -Color Yellow
        $replacementStarted = $true
        $create = Invoke-MySqlClient $rootOptions '' $freshDatabaseSql 600
        if ($create.ExitCode -ne 0) {
            throw ('Falha ao criar o banco novo. O backup lógico anterior foi preservado. ' + $create.Error + ' ' + $create.Output)
        }

        $appCheck = Wait-MySqlApplicationLogin $appOptions 90
        Write-Log ('Banco novo criado e login da aplicação validado: ' + $appCheck.Output) -Color Green

        Stop-StingrayService
        [IO.File]::WriteAllBytes($script:IniPath, $script:OriginalIniBytes)
        $script:IniModified = $false
        Start-StingrayService
        Wait-MySqlTcp 90
        $finalCheck = Wait-MySqlApplicationLogin $appOptions 90
        Write-Log ('Validação final após restaurar o my.ini original: ' + $finalCheck.Output) -Color Green
        $replacementCommitted = $true
        Write-Log ('Backup de segurança do my.ini: ' + $iniBackup)
    }
    catch {
        if ($replacementStarted -and -not $replacementCommitted -and $backupFile -and (Test-Path -LiteralPath $backupFile -PathType Leaf)) {
            Write-Log 'A criação/validação do banco novo falhou. Tentando restaurar automaticamente o dump anterior...' -Color Yellow
            try {
                $serviceState = Get-Service -Name $script:ServiceName -ErrorAction Stop
                if ($serviceState.Status -ne 'Running') { Start-StingrayService }
                Wait-MySqlTcp 90
                $restoreSql = "DROP DATABASE IF EXISTS teste_projeto;`r`n" + [IO.File]::ReadAllText($backupFile, [Text.Encoding]::UTF8) + "`r`n" + @"
CREATE USER IF NOT EXISTS 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'localhost' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'localhost';
CREATE USER IF NOT EXISTS 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword';
ALTER USER 'stingray_app'@'127.0.0.1' IDENTIFIED BY '$appPassword' ACCOUNT UNLOCK;
GRANT SELECT, INSERT, UPDATE, DELETE ON teste_projeto.* TO 'stingray_app'@'127.0.0.1';
FLUSH PRIVILEGES;
"@
                $restore = Invoke-MySqlClient $rootOptions '' $restoreSql 900
                if ($restore.ExitCode -ne 0) { throw ($restore.Error + ' ' + $restore.Output) }
                Write-Log 'Backup anterior restaurado após falha. O banco não ficou parcialmente recriado.' -Color Green
            }
            catch {
                Write-Log ('FALHA CRÍTICA: a restauração automática do dump também falhou. O arquivo de backup foi preservado em ' + $backupFile + '. Detalhe: ' + $_.Exception.Message) -Color Red
            }
        }
        if ($script:IniModified -and $script:OriginalIniBytes) {
            Write-Log 'Falha durante a criação do banco novo. Restaurando o my.ini original...' -Color Yellow
            try {
                $current = Get-Service -Name $script:ServiceName -ErrorAction SilentlyContinue
                if ($current -and $current.Status -ne 'Stopped') { Stop-StingrayService }
                [IO.File]::WriteAllBytes($script:IniPath, $script:OriginalIniBytes)
                $script:IniModified = $false
                Start-StingrayService
                Wait-MySqlTcp 90
                Write-Log 'my.ini original restaurado. Os arquivos físicos do MySQL foram preservados.' -Color Yellow
            }
            catch {
                Write-Log ('ATENÇÃO: não foi possível concluir a restauração automática. Cópia de segurança: ' + $iniBackup + '. Detalhe: ' + $_.Exception.Message) -Color Red
            }
        }
        throw
    }
}

function Invoke-ElevatedSetup {
    if (-not (Test-Path -LiteralPath $SecretFile -PathType Leaf)) { throw 'O arquivo temporário protegido não foi encontrado.' }
    if (-not (Test-IsAdministrator)) { throw 'O processo elevado não possui privilégios de administrador.' }
    if ([string]::IsNullOrWhiteSpace($OwnerSid)) { throw 'A identificação da conta Windows que executou o instalador está ausente.' }
    $aplicativoAberto = Get-Process -Name 'Stingray' -ErrorAction SilentlyContinue
    if ($aplicativoAberto) {
        throw 'Feche o Stingray antes de instalar/recriar o banco. Nenhum banco foi alterado.'
    }

    $script:AppPassword = Read-SecretFromProtectedFile $SecretFile
    if ($script:AppPassword -notmatch '^[0-9a-f]{32}$') { throw 'O segredo temporário protegido possui formato inválido.' }

    New-Item -ItemType Directory -Force -Path $script:RootPath | Out-Null
    Write-Log ('INSTALADOR STINGRAY v' + $script:Version)
    Write-Log ('Conta Windows de destino da credencial: SID ' + $OwnerSid)
    Ensure-MySqlBinaries

    $service = Get-CimInstance Win32_Service -Filter "Name='$($script:ServiceName)'" -ErrorAction SilentlyContinue
    $iniExists = Test-Path -LiteralPath $script:IniPath -PathType Leaf
    if ($null -ne $service -or $iniExists) {
        Repair-ExistingInstanceAccount
    }
    else {
        # Se houver apenas um datadir órfão, ele será movido integralmente para backup físico.
        # A instância nova será inicializada sem sobrescrever o conteúdo antigo.
        # Backups antigos nunca impedem a criação de um banco novo.
        Install-NewPrivateInstance
    }

    Write-Log 'ETAPA ADMINISTRATIVA CONCLUÍDA. A credencial Windows será gravada pelo processo da conta que iniciou o instalador.' -Color Green
}

function Invoke-UserContextLauncher {
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent()
    $sid = $identity.User.Value
    $password = [Guid]::NewGuid().ToString('N')
    $tempPath = Join-Path $env:TEMP ('Stingray-v6-secret-' + [Guid]::NewGuid().ToString('N') + '.bin')
    $clearBytes = [Text.Encoding]::UTF8.GetBytes($password)
    Add-Type -AssemblyName System.Security
    try {
        $protected = [Security.Cryptography.ProtectedData]::Protect(
            $clearBytes,
            $script:Entropy,
            [Security.Cryptography.DataProtectionScope]::LocalMachine
        )
        [IO.File]::WriteAllBytes($tempPath, $protected)
        Set-PrivateFileAcl $tempPath $sid

        Write-Host ''
        Write-Host 'STINGRAY — INSTALAÇÃO / RECRIAÇÃO DO MYSQL v6.0.2' -ForegroundColor Cyan
        Write-Host 'A credencial do banco será salva na conta Windows que iniciou este arquivo.'
        Write-Host 'A janela do UAC solicitará autorização administrativa.'
        Write-Host 'O banco teste_projeto será recriado em toda execução. Um dump será salvo antes quando já existir.'
        Write-Host ''

        $scriptPath = [IO.Path]::GetFullPath($PSCommandPath)
        $argLine = '-NoProfile -ExecutionPolicy Bypass -File "' + $scriptPath + '" -Elevated -SecretFile "' + $tempPath + '" -OwnerSid "' + $sid + '"'
        $elevatedProcess = Start-Process -FilePath 'powershell.exe' -Verb RunAs -ArgumentList $argLine -PassThru -Wait
        if ($elevatedProcess.ExitCode -ne 0) {
            $detail = ''
            if (Test-Path -LiteralPath $script:LogPath -PathType Leaf) {
                $detail = (Get-Content -LiteralPath $script:LogPath -Tail 8 -ErrorAction SilentlyContinue) -join ' | '
            }
            if (-not [string]::IsNullOrWhiteSpace($detail)) {
                throw "A etapa administrativa falhou (código $($elevatedProcess.ExitCode)). $detail. A credencial da conta Windows não foi substituída."
            }
            throw "A etapa administrativa falhou (código $($elevatedProcess.ExitCode)). A credencial da conta Windows não foi substituída. Consulte o log da instalação."
        }

        Save-CredentialForCurrentInteractiveUser $password
        [StingrayV5CredentialWriter]::Verify($script:CredentialTarget, $script:CredentialUser, $password)
        Write-Host 'Credencial gravada e lida de volta corretamente no Gerenciador de Credenciais da conta que iniciou o instalador.' -ForegroundColor Green

        $options = Join-Path $env:TEMP ('Stingray-v6-final-test-' + [Guid]::NewGuid().ToString('N') + '.ini')
        try {
            New-ClientOptionsFile $options $script:CredentialUser $password 'teste_projeto'
            $test = Wait-MySqlApplicationLogin $options 60
            Write-Host ''
            Write-Host 'INSTALAÇÃO / RECRIAÇÃO CONCLUÍDA E VALIDADA.' -ForegroundColor Green
            Write-Host ('CURRENT_USER | DATABASE | TABELAS: ' + $test.Output) -ForegroundColor Green
            Write-Host 'Banco novo criado. O banco anterior, quando existente, foi salvo em backup.' -ForegroundColor Green
            Write-Host ('Log: ' + $script:LogPath)
        }
        finally { if (Test-Path $options) { Remove-Item $options -Force -ErrorAction SilentlyContinue } }
    }
    finally {
        if ($clearBytes) { [Array]::Clear($clearBytes, 0, $clearBytes.Length) }
        $password = $null
        if (Test-Path $tempPath) { Remove-Item $tempPath -Force -ErrorAction SilentlyContinue }
    }
}

if ($Elevated) {
    try {
        if (-not [Environment]::Is64BitOperatingSystem -or -not [Environment]::Is64BitProcess) {
            throw 'Execute este instalador no Windows de 64 bits usando o Windows PowerShell 5.1 de 64 bits.'
        }
        Invoke-ElevatedSetup
        exit 0
    }
    catch {
        $failureMessage = 'FALHA NA INSTALAÇÃO / REPARO: ' + $_.Exception.Message
        try {
            New-Item -ItemType Directory -Force -Path $script:RootPath | Out-Null
            Write-Log $failureMessage -Color Red
            if ($_.ScriptStackTrace) { Write-Log ('RASTREAMENTO: ' + $_.ScriptStackTrace) -Color DarkRed }
        }
        catch {
            Write-Host $failureMessage -ForegroundColor Red
            if ($_.ScriptStackTrace) { Write-Host $_.ScriptStackTrace -ForegroundColor DarkRed }
        }
        Write-Host ''
        Write-Host ('Verifique o log: ' + $script:LogPath) -ForegroundColor Yellow
        exit 1
    }
    finally {
        $script:AppPassword = $null
        foreach ($file in $script:SetupFiles) {
            $isInitSqlStillReferenced = ($script:IniModified -and ($file -like '*account-sync-*.sql' -or $file -like '*fresh-db-*.sql'))
            if (-not $isInitSqlStillReferenced -and $file -and (Test-Path -LiteralPath $file)) {
                Remove-Item -LiteralPath $file -Force -ErrorAction SilentlyContinue
            }
        }
    }
}
else {
    try {
        Invoke-UserContextLauncher
    }
    catch {
        Write-Host ''
        Write-Host ('INSTALAÇÃO / RECRIAÇÃO NÃO CONCLUÍDA: ' + $_.Exception.Message) -ForegroundColor Red
        Write-Host 'Nenhuma credencial nova foi gravada na conta Windows que iniciou o instalador.' -ForegroundColor Yellow
        exit 1
    }
}
