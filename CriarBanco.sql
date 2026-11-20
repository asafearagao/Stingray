
CREATE DATABASE teste_projeto
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE teste_projeto;

CREATE TABLE endereco (
    id_endereco INT PRIMARY KEY AUTO_INCREMENT,
    numero VARCHAR(5) NOT NULL,
    cep CHAR(9) NOT NULL,
    complemento VARCHAR(50)
);

CREATE TABLE pessoa (
    id_pessoa INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(50) NOT NULL,
    data_nascimento DATE NOT NULL,
    cpf CHAR(11) NOT NULL UNIQUE,
    rg VARCHAR(14) NOT NULL UNIQUE,
    email VARCHAR(100),
    id_endereco INT NOT NULL,

    CONSTRAINT fk_pessoa_endereco
        FOREIGN KEY (id_endereco)
        REFERENCES endereco(id_endereco)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE telefone (
    id_telefone INT PRIMARY KEY AUTO_INCREMENT,
    numero_telefone VARCHAR(13) NOT NULL,
    id_pessoa INT NOT NULL,

    CONSTRAINT fk_telefone_pessoa
        FOREIGN KEY (id_pessoa)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE aluno (
    id_aluno INT PRIMARY KEY,
    observacao VARCHAR(100),

    CONSTRAINT fk_aluno_pessoa
        FOREIGN KEY (id_aluno)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE responsavel (
    id_responsavel INT PRIMARY KEY,
    observacao VARCHAR(100),

    CONSTRAINT fk_responsavel_pessoa
        FOREIGN KEY (id_responsavel)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE aluno_responsavel (
    id_aluno INT NOT NULL,
    id_responsavel INT NOT NULL,

    PRIMARY KEY (id_aluno, id_responsavel),

    CONSTRAINT fk_aluno_responsavel_aluno
        FOREIGN KEY (id_aluno)
        REFERENCES aluno(id_aluno)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_aluno_responsavel_responsavel
        FOREIGN KEY (id_responsavel)
        REFERENCES responsavel(id_responsavel)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE professor (
    id_professor INT PRIMARY KEY,
    cref VARCHAR(255) NOT NULL UNIQUE,

    CONSTRAINT fk_professor_pessoa
        FOREIGN KEY (id_professor)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE turma (
    id_turma INT PRIMARY KEY AUTO_INCREMENT,
    modalidade VARCHAR(30) NOT NULL,
    dia_semana VARCHAR(20) NOT NULL,
    horario_inicio TIME NOT NULL,
    horario_fim TIME NOT NULL,
    capacidade INT NOT NULL,
    valor_mensal DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    id_professor INT NOT NULL,

    CONSTRAINT fk_turma_professor
        FOREIGN KEY (id_professor)
        REFERENCES professor(id_professor)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE matricula (
    id_matricula INT PRIMARY KEY AUTO_INCREMENT,
    data_matricula DATE NOT NULL,
    data_cancelamento DATE,
    dia_vencimento TINYINT UNSIGNED NOT NULL,
    situacao ENUM('Ativo', 'Inativo', 'Cancelado')
        NOT NULL DEFAULT 'Ativo',
    inativo_automatico TINYINT(1) NOT NULL DEFAULT 0,
    id_aluno INT NOT NULL,

    CONSTRAINT fk_matricula_aluno
        FOREIGN KEY (id_aluno)
        REFERENCES aluno(id_aluno)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE turma_matricula (
    id_turma INT NOT NULL,
    id_matricula INT NOT NULL,

    PRIMARY KEY (id_turma, id_matricula),

    CONSTRAINT fk_turma_matricula_turma
        FOREIGN KEY (id_turma)
        REFERENCES turma(id_turma)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_turma_matricula_matricula
        FOREIGN KEY (id_matricula)
        REFERENCES matricula(id_matricula)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE mensalidade (
    id_mensalidade INT PRIMARY KEY AUTO_INCREMENT,
    mes_referencia DATE NOT NULL,
    data_pagamento DATE,
    data_geracao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_vencimento DATE NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    situacao ENUM('Pago', 'Pendente', 'Atrasado')
        NOT NULL DEFAULT 'Pendente',
    id_matricula INT NOT NULL,

    UNIQUE (id_matricula, mes_referencia),

    CONSTRAINT fk_mensalidade_matricula
        FOREIGN KEY (id_matricula)
        REFERENCES matricula(id_matricula)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE ocorrencia_aula (
    id_ocorrencia INT PRIMARY KEY AUTO_INCREMENT,
    data_aula DATE NOT NULL,
    situacao ENUM('Concluida', 'Pendente', 'Cancelada')
        NOT NULL DEFAULT 'Pendente',
    id_turma INT NOT NULL,

    CONSTRAINT fk_ocorrencia_turma
        FOREIGN KEY (id_turma)
        REFERENCES turma(id_turma)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE remarcacao_aula (
    id_remarcacao INT PRIMARY KEY AUTO_INCREMENT,
    id_turma INT NOT NULL,
    data_origem DATE NOT NULL,
    data_destino DATE NOT NULL,
    data_geracao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UNIQUE KEY uq_remarcacao_origem (id_turma, data_origem),
    UNIQUE KEY uq_remarcacao_destino (id_turma, data_destino),

    CONSTRAINT fk_remarcacao_turma
        FOREIGN KEY (id_turma)
        REFERENCES turma(id_turma)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE reposicao (
    id_reposicao INT PRIMARY KEY AUTO_INCREMENT,
    data_agendamento DATE NOT NULL,
    situacao ENUM('Concluida', 'Pendente', 'Cancelada')
        NOT NULL DEFAULT 'Pendente',
    id_ocorrencia INT NOT NULL,
    id_matricula INT,
    id_ocorrencia_destino INT,

    CONSTRAINT fk_reposicao_matricula
        FOREIGN KEY (id_matricula)
        REFERENCES matricula(id_matricula)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_reposicao_ocorrencia_destino
        FOREIGN KEY (id_ocorrencia_destino)
        REFERENCES ocorrencia_aula(id_ocorrencia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_reposicao_ocorrencia
        FOREIGN KEY (id_ocorrencia)
        REFERENCES ocorrencia_aula(id_ocorrencia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE frequencia (
    id_matricula INT NOT NULL,
    id_ocorrencia INT NOT NULL,
    situacao ENUM('Presente', 'Ausente', 'Justificado', 'Pendente')
        NOT NULL DEFAULT 'Pendente',
    observacao VARCHAR(100),
    id_reposicao INT,

    PRIMARY KEY (id_matricula, id_ocorrencia),

    CONSTRAINT fk_frequencia_matricula
        FOREIGN KEY (id_matricula)
        REFERENCES matricula(id_matricula)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_frequencia_ocorrencia
        FOREIGN KEY (id_ocorrencia)
        REFERENCES ocorrencia_aula(id_ocorrencia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_frequencia_reposicao
        FOREIGN KEY (id_reposicao)
        REFERENCES reposicao(id_reposicao)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE configuracao_frequencia (
    id_configuracao TINYINT PRIMARY KEY,
    quantidade_aulas INT NOT NULL DEFAULT 10,
    percentual_faltas DECIMAL(5,2) NOT NULL DEFAULT 30.00
);

CREATE TABLE configuracao_whatsapp (
    id_configuracao TINYINT UNSIGNED PRIMARY KEY,
    integracao_ativada TINYINT(1) NOT NULL DEFAULT 0,
    ultimo_estado_conectado TINYINT(1) NOT NULL DEFAULT 0,
    dias_apos_vencimento INT NOT NULL DEFAULT 4,
    atualizado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE whatsapp_envio (
    id_envio CHAR(36) PRIMARY KEY,
    id_mensalidade INT NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    mensagem TEXT NOT NULL,
    situacao VARCHAR(30) NOT NULL DEFAULT 'Pendente',
    detalhe_erro TEXT,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        ON UPDATE CURRENT_TIMESTAMP,

    UNIQUE KEY uq_whatsapp_envio_mensalidade (id_mensalidade),

    CONSTRAINT fk_whatsapp_envio_mensalidade
        FOREIGN KEY (id_mensalidade)
        REFERENCES mensalidade(id_mensalidade)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE usuario (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nome_usuario VARCHAR(50) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL
);
