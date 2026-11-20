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
    valor_mensal DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    id_professor INT NOT NULL,

    CONSTRAINT chk_turma_horario
        CHECK (horario_fim > horario_inicio),
    CONSTRAINT chk_turma_capacidade
        CHECK (capacidade > 0),
    CONSTRAINT chk_turma_valor_mensal
        CHECK (valor_mensal >= 0),
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
    inativo_automatico TINYINT(1) NOT NULL DEFAULT 0,
    id_aluno INT NOT NULL,

    CONSTRAINT chk_matricula_dia_vencimento
        CHECK (dia_vencimento BETWEEN 1 AND 28),

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
    data_geracao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
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

CREATE TABLE IF NOT EXISTS remarcacao_aula (
    id_remarcacao INT PRIMARY KEY AUTO_INCREMENT,
    id_turma INT NOT NULL,
    data_origem DATE NOT NULL,
    data_destino DATE NOT NULL,
    data_geracao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_remarcacao_origem (id_turma, data_origem),
    UNIQUE KEY uq_remarcacao_destino (id_turma, data_destino),
    CONSTRAINT chk_remarcacao_datas CHECK (data_destino <> data_origem),
    CONSTRAINT fk_remarcacao_turma FOREIGN KEY (id_turma)
        REFERENCES turma(id_turma) ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS reposicao (
    id_reposicao INT PRIMARY KEY AUTO_INCREMENT,
    data_agendamento DATE NOT NULL,
    situacao ENUM ('Concluida', 'Pendente', 'Cancelada')
        NOT NULL DEFAULT 'Pendente',
    id_ocorrencia INT NOT NULL,
    id_matricula INT,
    id_ocorrencia_destino INT,
    UNIQUE (id_reposicao, id_ocorrencia),

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
        FOREIGN KEY (id_reposicao)
        REFERENCES reposicao(id_reposicao)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS configuracao_frequencia (
    id_configuracao TINYINT PRIMARY KEY,
    quantidade_aulas INT NOT NULL DEFAULT 10,
    percentual_faltas DECIMAL(5,2) NOT NULL DEFAULT 30.00,
    CONSTRAINT chk_configuracao_quantidade CHECK (quantidade_aulas BETWEEN 1 AND 100),
    CONSTRAINT chk_configuracao_percentual CHECK (percentual_faltas BETWEEN 1 AND 100)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS configuracao_whatsapp (
    id_configuracao TINYINT UNSIGNED PRIMARY KEY,
    integracao_ativada TINYINT(1) NOT NULL DEFAULT 0,
    ultimo_estado_conectado TINYINT(1) NOT NULL DEFAULT 0,
    dias_apos_vencimento INT NOT NULL DEFAULT 4,
    atualizado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT chk_configuracao_whatsapp_id CHECK (id_configuracao = 1)
) ENGINE = InnoDB;

INSERT INTO configuracao_whatsapp (id_configuracao, integracao_ativada, ultimo_estado_conectado)
VALUES (1, 0, 0)
ON DUPLICATE KEY UPDATE id_configuracao = VALUES(id_configuracao);

CREATE TABLE IF NOT EXISTS whatsapp_envio (
    id_envio CHAR(36) NOT NULL PRIMARY KEY,
    id_mensalidade INT NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    mensagem TEXT NOT NULL,
    situacao VARCHAR(30) NOT NULL DEFAULT 'Pendente',
    detalhe_erro TEXT NULL,
    criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uq_whatsapp_envio_mensalidade (id_mensalidade),
    CONSTRAINT fk_whatsapp_envio_mensalidade FOREIGN KEY (id_mensalidade)
        REFERENCES mensalidade(id_mensalidade) ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB;

INSERT INTO configuracao_frequencia (id_configuracao, quantidade_aulas, percentual_faltas)
VALUES (1, 10, 30.00)
ON DUPLICATE KEY UPDATE id_configuracao = VALUES(id_configuracao);

CREATE TABLE IF NOT EXISTS usuario (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nome_usuario VARCHAR(50) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL
) ENGINE = InnoDB;


DROP PROCEDURE IF EXISTS migrar_frequencia_stingray;
DELIMITER $$
CREATE PROCEDURE migrar_frequencia_stingray()
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = DATABASE() AND table_name = 'turma'
          AND column_name = 'valor_mensal'
    ) THEN
        ALTER TABLE turma ADD COLUMN valor_mensal DECIMAL(10,2) NOT NULL DEFAULT 0.00 AFTER capacidade;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = DATABASE() AND table_name = 'mensalidade'
          AND column_name = 'data_geracao'
    ) THEN
        ALTER TABLE mensalidade ADD COLUMN data_geracao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP AFTER data_pagamento;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_schema = DATABASE() AND table_name = 'turma'
          AND constraint_name = 'chk_turma_valor_mensal'
    ) THEN
        ALTER TABLE turma ADD CONSTRAINT chk_turma_valor_mensal CHECK (valor_mensal >= 0);
    END IF;

    UPDATE matricula SET dia_vencimento = 28 WHERE dia_vencimento > 28;

    IF EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_schema = DATABASE() AND table_name = 'matricula'
          AND constraint_name = 'chk_matricula_dia_vencimento'
    ) THEN
        ALTER TABLE matricula DROP CHECK chk_matricula_dia_vencimento;
    END IF;

    ALTER TABLE matricula
        ADD CONSTRAINT chk_matricula_dia_vencimento CHECK (dia_vencimento BETWEEN 1 AND 28);

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = DATABASE() AND table_name = 'matricula'
          AND column_name = 'inativo_automatico'
    ) THEN
        ALTER TABLE matricula
            ADD COLUMN inativo_automatico TINYINT(1) NOT NULL DEFAULT 0 AFTER situacao;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = DATABASE() AND table_name = 'reposicao'
          AND column_name = 'id_matricula'
    ) THEN
        ALTER TABLE reposicao ADD COLUMN id_matricula INT NULL AFTER id_ocorrencia;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = DATABASE() AND table_name = 'reposicao'
          AND column_name = 'id_ocorrencia_destino'
    ) THEN
        ALTER TABLE reposicao ADD COLUMN id_ocorrencia_destino INT NULL AFTER id_matricula;
    END IF;

    IF EXISTS (
        SELECT 1 FROM (
            SELECT r.id_reposicao
            FROM reposicao r
            INNER JOIN frequencia f
                ON f.id_reposicao = r.id_reposicao
               AND f.id_ocorrencia = r.id_ocorrencia
            GROUP BY r.id_reposicao
            HAVING COUNT(DISTINCT f.id_matricula) > 1
        ) reposicoes_compartilhadas
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Uma ou mais reposicoes antigas estao ligadas a varios alunos. Revise esses registros antes de migrar.';
    END IF;

    UPDATE reposicao r
    INNER JOIN frequencia f
        ON f.id_reposicao = r.id_reposicao
       AND f.id_ocorrencia = r.id_ocorrencia
    SET r.id_matricula = f.id_matricula
    WHERE r.id_matricula IS NULL;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'reposicao'
          AND index_name = 'idx_reposicao_matricula_origem'
    ) THEN
        CREATE INDEX idx_reposicao_matricula_origem
            ON reposicao(id_matricula, id_ocorrencia);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'reposicao'
          AND index_name = 'idx_reposicao_destino'
    ) THEN
        CREATE INDEX idx_reposicao_destino
            ON reposicao(id_ocorrencia_destino, situacao);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_schema = DATABASE() AND table_name = 'reposicao'
          AND constraint_name = 'fk_reposicao_matricula'
    ) THEN
        ALTER TABLE reposicao
            ADD CONSTRAINT fk_reposicao_matricula
            FOREIGN KEY (id_matricula) REFERENCES matricula(id_matricula)
            ON UPDATE CASCADE ON DELETE RESTRICT;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_schema = DATABASE() AND table_name = 'reposicao'
          AND constraint_name = 'fk_reposicao_ocorrencia_destino'
    ) THEN
        ALTER TABLE reposicao
            ADD CONSTRAINT fk_reposicao_ocorrencia_destino
            FOREIGN KEY (id_ocorrencia_destino) REFERENCES ocorrencia_aula(id_ocorrencia)
            ON UPDATE CASCADE ON DELETE RESTRICT;
    END IF;

    IF EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_schema = DATABASE() AND table_name = 'frequencia'
          AND constraint_name = 'fk_frequencia_reposicao'
    ) THEN
        ALTER TABLE frequencia DROP FOREIGN KEY fk_frequencia_reposicao;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_schema = DATABASE() AND table_name = 'frequencia'
          AND constraint_name = 'fk_frequencia_reposicao'
    ) THEN
        ALTER TABLE frequencia
            ADD CONSTRAINT fk_frequencia_reposicao
            FOREIGN KEY (id_reposicao) REFERENCES reposicao(id_reposicao)
            ON UPDATE CASCADE ON DELETE RESTRICT;
    END IF;

    IF EXISTS (
        SELECT 1 FROM (
            SELECT id_turma, data_aula
            FROM ocorrencia_aula
            GROUP BY id_turma, data_aula
            HAVING COUNT(*) > 1
        ) duplicadas
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Existem ocorrencias duplicadas por turma e data. Corrija esses registros antes de concluir a migracao.';
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'ocorrencia_aula'
          AND index_name = 'uq_ocorrencia_turma_data'
    ) THEN
        CREATE UNIQUE INDEX uq_ocorrencia_turma_data
            ON ocorrencia_aula(id_turma, data_aula);
    END IF;


    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'remarcacao_aula'
          AND index_name = 'idx_remarcacao_destino_data'
    ) THEN
        CREATE INDEX idx_remarcacao_destino_data ON remarcacao_aula(data_destino, id_turma);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'pessoa'
          AND index_name = 'idx_pessoa_nome'
    ) THEN
        CREATE INDEX idx_pessoa_nome ON pessoa(nome);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'pessoa'
          AND index_name = 'idx_pessoa_email'
    ) THEN
        CREATE INDEX idx_pessoa_email ON pessoa(email);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'matricula'
          AND index_name = 'idx_matricula_situacao'
    ) THEN
        CREATE INDEX idx_matricula_situacao ON matricula(situacao);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'mensalidade'
          AND index_name = 'idx_mensalidade_situacao_data_vencimento'
    ) THEN
        CREATE INDEX idx_mensalidade_situacao_data_vencimento
            ON mensalidade(situacao, data_vencimento);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'mensalidade'
          AND index_name = 'idx_mensalidade_vencimento'
    ) THEN
        CREATE INDEX idx_mensalidade_vencimento ON mensalidade(data_vencimento);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'ocorrencia_aula'
          AND index_name = 'idx_ocorrencia_data_aula_turma'
    ) THEN
        CREATE INDEX idx_ocorrencia_data_aula_turma
            ON ocorrencia_aula(data_aula, id_turma);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'turma'
          AND index_name = 'idx_turma_modalidade_dia'
    ) THEN
        CREATE INDEX idx_turma_modalidade_dia ON turma(modalidade, dia_semana);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'frequencia'
          AND index_name = 'idx_frequencia_situacao'
    ) THEN
        CREATE INDEX idx_frequencia_situacao ON frequencia(situacao);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM information_schema.statistics
        WHERE table_schema = DATABASE() AND table_name = 'reposicao'
          AND index_name = 'idx_reposicao_pendente'
    ) THEN
        CREATE INDEX idx_reposicao_pendente
            ON reposicao(situacao, id_matricula, id_ocorrencia_destino);
    END IF;

END$$
DELIMITER ;

CALL migrar_frequencia_stingray();
DROP PROCEDURE migrar_frequencia_stingray;
