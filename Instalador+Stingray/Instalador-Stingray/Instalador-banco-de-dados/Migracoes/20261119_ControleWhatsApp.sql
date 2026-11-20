USE teste_projeto;

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
