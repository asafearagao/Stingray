$ErrorActionPreference = 'Stop'

function Confirmar($condicao, $descricao) {
    if (-not $condicao) {
        throw "FALHOU: $descricao"
    }
    Write-Host "OK: $descricao" -ForegroundColor Green
}

$raizProjeto = Split-Path -Parent $PSScriptRoot
$caminhos = @(
    (Join-Path $raizProjeto 'bin\Debug\Stingray.exe'),
    (Join-Path $raizProjeto 'bin\Release\Stingray.exe')
)
$assemblyPath = $caminhos | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $assemblyPath) {
    throw 'Compile o projeto no Visual Studio antes de executar os testes.'
}

if ($PSVersionTable.PSEdition -ne 'Desktop') {
    throw 'Execute este teste no Windows PowerShell 5.1, compatível com o .NET Framework do Stingray.'
}

$appConfig = Join-Path $raizProjeto 'App.config'
if (-not (Test-Path $appConfig)) {
    throw 'App.config não foi encontrado.'
}

$configTexto = Get-Content $appConfig -Raw
Confirmar (-not ($configTexto -match 'System\.ValueTuple')) 'App.config não força redirecionamento de System.ValueTuple'
$exeConfig = Join-Path (Split-Path -Parent $assemblyPath) 'Stingray.exe.config'
if (Test-Path $exeConfig) {
    $configCompilada = Get-Content $exeConfig -Raw
    Confirmar (-not ($configCompilada -match 'System\.ValueTuple')) 'Configuração compilada não reintroduziu o redirecionamento de System.ValueTuple'
}

try {
    $valueTuple = [System.Reflection.Assembly]::Load('System.ValueTuple, Version=4.0.5.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51')
    Confirmar ($valueTuple.GetName().Name -eq 'System.ValueTuple') 'O .NET Framework resolve System.ValueTuple pela unificação de assemblies'
}
catch {
    throw 'O .NET Framework não conseguiu resolver System.ValueTuple. Confirme a instalação do .NET Framework 4.7.2 ou superior. Detalhe: ' + $_.Exception.Message
}

$assembly = [System.Reflection.Assembly]::LoadFrom($assemblyPath)
$tipoRegra = $assembly.GetType('Stingray.Servicos.RegraCobrancaWhatsApp', $true)
$metodoPrazo = $tipoRegra.GetMethod('EstaElegivel')
$metodoNovaTentativa = $tipoRegra.GetMethod('PermiteNovaTentativa')

$vencimento = [datetime]::new(2026, 11, 18)
$antesDoPrazo = $metodoPrazo.Invoke($null, [object[]]@($vencimento, [datetime]::new(2026, 11, 21), 4))
$noPrazo = $metodoPrazo.Invoke($null, [object[]]@($vencimento, [datetime]::new(2026, 11, 22), 4))
$depoisDoPrazo = $metodoPrazo.Invoke($null, [object[]]@($vencimento, [datetime]::new(2026, 11, 23), 4))

Confirmar (-not $antesDoPrazo) 'Não cobrar antes de completar quatro dias'
Confirmar $noPrazo 'Liberar a cobrança ao completar quatro dias'
Confirmar $depoisDoPrazo 'Manter a cobrança elegível depois do prazo'

foreach ($estado in @('Pendente', 'Em envio', 'Enviada', 'Não confirmado', 'Cancelada')) {
    $permitida = $metodoNovaTentativa.Invoke($null, [object[]]@($estado))
    Confirmar (-not $permitida) "Bloquear reenvio para o estado '$estado'"
}

$falhaSegura = $metodoNovaTentativa.Invoke($null, [object[]]@('Falha antes do envio'))
$semRegistro = $metodoNovaTentativa.Invoke($null, [object[]]@($null))
Confirmar $falhaSegura 'Permitir nova tentativa somente quando a falha ocorreu antes do envio'
Confirmar $semRegistro 'Permitir cobrança quando ainda não existe registro anterior'

$tipoRetencao = $assembly.GetType('Stingray.Servicos.RetencaoPagamentoRegra', $true)
$metodoLimite = $tipoRetencao.GetMethod('ObterDataLimite')
$metodoExcluir = $tipoRetencao.GetMethod('DeveExcluirPagamento')
$hoje = [datetime]::new(2026, 11, 19)
$limite = $metodoLimite.Invoke($null, [object[]]@($hoje))
Confirmar ($limite -eq [datetime]::new(2026, 5, 19)) 'Calcular corretamente o limite de retenção de seis meses'

$antigoPago = $metodoExcluir.Invoke($null, [object[]]@('Pago', [datetime]::new(2026, 4, 1), [datetime]::new(2026, 4, 1), $hoje))
$pagoRecentemente = $metodoExcluir.Invoke($null, [object[]]@('Pago', [datetime]::new(2026, 11, 1), [datetime]::new(2026, 4, 1), $hoje))
$pagoSemData = $metodoExcluir.Invoke($null, [object[]]@('Pago', [datetime]::MinValue, [datetime]::new(2026, 4, 1), $hoje))
$naDataLimite = $metodoExcluir.Invoke($null, [object[]]@('Pago', [datetime]::new(2026, 5, 19), [datetime]::new(2026, 4, 1), $hoje))
$antigoPendente = $metodoExcluir.Invoke($null, [object[]]@('Pendente', [datetime]::MinValue, [datetime]::new(2026, 4, 1), $hoje))
Confirmar $antigoPago 'Excluir mensalidade paga há mais de seis meses'
Confirmar (-not $pagoRecentemente) 'Preservar mensalidade recebida recentemente, mesmo com vencimento antigo'
Confirmar $pagoSemData 'Usar o vencimento como referência quando não há data de recebimento antiga'
Confirmar (-not $naDataLimite) 'Preservar mensalidade que completou exatamente seis meses'
Confirmar (-not $antigoPendente) 'Preservar mensalidade pendente mesmo quando antiga'

$tipoLinha = $assembly.GetType('Stingray.DadosTela.PagamentoLinha', $true)
$tipoMontador = $assembly.GetType('Stingray.Servicos.CobrancaWhatsAppMontador', $true)
$metodoMontar = $tipoMontador.GetMethod('TentarMontar')

function CriarPagamentoTeste($nascimento, $responsavel, $telefoneAluno, $telefoneResponsavel) {
    $pagamento = [Activator]::CreateInstance($tipoLinha)
    $tipoLinha.GetProperty('NomeAluno').SetValue($pagamento, 'Aluno de Teste', $null)
    $tipoLinha.GetProperty('DataNascimentoAluno').SetValue($pagamento, $nascimento, $null)
    $tipoLinha.GetProperty('NomeResponsavel').SetValue($pagamento, $responsavel, $null)
    $tipoLinha.GetProperty('TelefoneContato').SetValue($pagamento, $telefoneAluno, $null)
    $tipoLinha.GetProperty('TelefoneResponsavel').SetValue($pagamento, $telefoneResponsavel, $null)
    $tipoLinha.GetProperty('MesReferencia').SetValue($pagamento, [datetime]::new(2026, 11, 1), $null)
    $tipoLinha.GetProperty('DataVencimento').SetValue($pagamento, [datetime]::new(2026, 11, 18), $null)
    $tipoLinha.GetProperty('Valor').SetValue($pagamento, [decimal]150.00, $null)
    return $pagamento
}

$hojeTeste = [datetime]::new(2026, 11, 22)
$adulto = CriarPagamentoTeste ([datetime]::new(1990, 1, 1)) '' '11999999999' ''
$argumentosAdulto = [object[]]@($adulto, $hojeTeste, $null, $null, $null)
$adultoValido = $metodoMontar.Invoke($null, $argumentosAdulto)
Confirmar $adultoValido 'Montar cobrança para aluno maior de idade'
Confirmar ($argumentosAdulto[2] -eq '5511999999999') 'Usar o telefone do próprio aluno maior de idade'
Confirmar ($argumentosAdulto[3].Contains('Aluno de Teste') -and $argumentosAdulto[3].Contains('R$ 150,00')) 'Incluir nome do aluno adulto e valor total na mensagem'
Confirmar (([regex]::Matches($argumentosAdulto[3], [regex]::Escape('Aluno de Teste')).Count) -eq 1) 'Não repetir o nome do aluno adulto na mensagem'
Confirmar (-not $argumentosAdulto[3].Contains('Vencimento:')) 'Não incluir a data de vencimento na mensagem'

$menor = CriarPagamentoTeste ([datetime]::new(2012, 1, 1)) 'Responsável de Teste' '11999999999' '11988887777'
$argumentosMenor = [object[]]@($menor, $hojeTeste, $null, $null, $null)
$menorValido = $metodoMontar.Invoke($null, $argumentosMenor)
Confirmar $menorValido 'Montar cobrança para aluno menor de idade'
Confirmar ($argumentosMenor[2] -eq '5511988887777') 'Priorizar telefone cadastrado do responsável pelo menor'
Confirmar ($argumentosMenor[3].Contains('Aluno de Teste') -and $argumentosMenor[3].Contains('Responsável de Teste')) 'Identificar menor e responsável na mensagem'
Confirmar ($argumentosMenor[3].Contains('R$ 150,00')) 'Incluir valor total na cobrança do menor'
Confirmar (([regex]::Matches($argumentosMenor[3], [regex]::Escape('Aluno de Teste')).Count) -eq 1) 'Não repetir o nome do menor na mensagem'
Confirmar (([regex]::Matches($argumentosMenor[3], [regex]::Escape('Responsável de Teste')).Count) -eq 1) 'Não repetir o nome do responsável na mensagem'
Confirmar (-not $argumentosMenor[3].Contains('Vencimento:')) 'Não incluir a data de vencimento na cobrança do menor'


$repoEnvios = Join-Path $raizProjeto 'Repositorios\WhatsAppEnvioRepositorio.cs'
$repoEnviosTexto = Get-Content $repoEnvios -Raw
Confirmar (-not ($repoEnviosTexto -match 'CREATE\s+TABLE')) 'O aplicativo não tenta criar tabelas com o usuário stingray_app'
Confirmar ($repoEnviosTexto -match 'SELECT 1 FROM whatsapp_envio LIMIT 0') 'A inicialização valida a tabela sem exigir permissão CREATE'
$migracaoSql = Join-Path $raizProjeto 'BancoDeDados\Migracoes\20261119_ControleWhatsApp.sql'
Confirmar (Test-Path $migracaoSql) 'A migração administrativa da tabela de envios está incluída no projeto'

Write-Host ''
Write-Host 'Todos os testes de prazo, duplicidade, retenção e montagem de mensagem passaram.' -ForegroundColor Green
