CREATE DATABASE IF NOT EXISTS teste_projeto

CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE teste_projeto;

CREATE TABLE IF NOT EXISTS endereco (
    id_endereco INT PRIMARY KEY AUTO_INCREMENT,
    numero VARCHAR(5) NOT NULL,
    cep CHAR(9) NOT NULL,
    complemento VARCHAR(50)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS pessoa (
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
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS telefone (
    id_telefone INT PRIMARY KEY AUTO_INCREMENT,
    numero_telefone VARCHAR(13) NOT NULL,
    id_pessoa INT NOT NULL,

    CONSTRAINT fk_telefone_pessoa
        FOREIGN KEY (id_pessoa)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS aluno (
    id_aluno INT PRIMARY KEY,
    observacao VARCHAR(100),

    CONSTRAINT fk_aluno_pessoa
        FOREIGN KEY (id_aluno)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS responsavel (
    id_responsavel INT PRIMARY KEY,
    observacao VARCHAR(100),

    CONSTRAINT fk_responsavel_pessoa
        FOREIGN KEY (id_responsavel)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS aluno_responsavel (
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
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS professor (
    id_professor INT PRIMARY KEY,
    cref VARCHAR(255) NOT NULL UNIQUE,

    CONSTRAINT fk_professor_pessoa
        FOREIGN KEY (id_professor)
        REFERENCES pessoa(id_pessoa)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS turma (
    id_turma INT PRIMARY KEY AUTO_INCREMENT,
    modalidade VARCHAR(30) NOT NULL,
    dia_semana VARCHAR(20) NOT NULL,
    horario_inicio TIME NOT NULL,
    horario_fim TIME NOT NULL,
    capacidade INT NOT NULL,
    id_professor INT NOT NULL,

    CONSTRAINT chk_turma_horario
        CHECK (horario_fim > horario_inicio),
    CONSTRAINT chk_turma_capacidade
        CHECK (capacidade > 0),
    CONSTRAINT fk_turma_professor
        FOREIGN KEY (id_professor)
        REFERENCES professor(id_professor)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS matricula (
    id_matricula INT PRIMARY KEY AUTO_INCREMENT,
    data_matricula DATE NOT NULL,
    data_cancelamento DATE,
    dia_vencimento TINYINT UNSIGNED NOT NULL,
    situacao ENUM ('Ativo', 'Inativo', 'Cancelado')
        NOT NULL DEFAULT 'Ativo',
    id_aluno INT NOT NULL,

    CONSTRAINT chk_matricula_dia_vencimento
        CHECK (dia_vencimento BETWEEN 1 AND 31),

    CONSTRAINT fk_matricula_aluno
        FOREIGN KEY (id_aluno)
        REFERENCES aluno(id_aluno)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS turma_matricula (
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
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS mensalidade (
    id_mensalidade INT PRIMARY KEY AUTO_INCREMENT,
    mes_referencia DATE NOT NULL,
    data_pagamento DATE,
    data_vencimento DATE NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    situacao ENUM ('Pago', 'Pendente', 'Atrasado')
        NOT NULL DEFAULT 'Pendente',
    id_matricula INT NOT NULL,
    UNIQUE (id_matricula, mes_referencia),

    CONSTRAINT chk_mensalidade_mes_referencia
        CHECK (DAY(mes_referencia) = 1),

    CONSTRAINT fk_mensalidade_matricula
        FOREIGN KEY (id_matricula)
        REFERENCES matricula(id_matricula)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS ocorrencia_aula (
    id_ocorrencia INT PRIMARY KEY AUTO_INCREMENT,
    data_aula DATE NOT NULL,
    situacao ENUM ('Concluida', 'Pendente', 'Cancelada')
        NOT NULL DEFAULT 'Pendente',
    id_turma INT NOT NULL,

    CONSTRAINT fk_ocorrencia_turma
        FOREIGN KEY (id_turma)
        REFERENCES turma(id_turma)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS reposicao (
    id_reposicao INT PRIMARY KEY AUTO_INCREMENT,
    data_agendamento DATE NOT NULL,
    situacao ENUM ('Concluida', 'Pendente', 'Cancelada')
        NOT NULL DEFAULT 'Pendente',
    id_ocorrencia INT NOT NULL,
    UNIQUE (id_reposicao, id_ocorrencia),

    CONSTRAINT fk_reposicao_ocorrencia
        FOREIGN KEY (id_ocorrencia)
        REFERENCES ocorrencia_aula(id_ocorrencia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS frequencia (
    id_matricula INT NOT NULL,
    id_ocorrencia INT NOT NULL,
    situacao ENUM ('Presente', 'Ausente', 'Justificado', 'Pendente')
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
        FOREIGN KEY (id_reposicao, id_ocorrencia)
        REFERENCES reposicao(id_reposicao, id_ocorrencia)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS usuario (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nome_usuario VARCHAR(50) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL
) ENGINE = InnoDB;

CREATE INDEX idx_pessoa_nome
ON pessoa(nome);

CREATE INDEX idx_pessoa_email
ON pessoa(email);

CREATE INDEX idx_matricula_situacao
ON matricula(situacao);

CREATE INDEX idx_mensalidade_situacao_data_vencimento
ON mensalidade(situacao, data_vencimento);

CREATE INDEX idx_mensalidade_vencimento
ON mensalidade(data_vencimento);

CREATE INDEX idx_ocorrencia_data_aula_turma
ON ocorrencia_aula(data_aula, id_turma);

CREATE INDEX idx_turma_modalidade_dia
ON turma(modalidade, dia_semana);

CREATE INDEX idx_frequencia_situacao
ON frequencia(situacao);
