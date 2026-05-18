-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 18/05/2026 às 22:44
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `ambarina`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `clientes`
--

CREATE TABLE `clientes` (
  `id_cliente` int(11) NOT NULL COMMENT 'ID único do cliente.',
  `nome_completo` varchar(100) NOT NULL COMMENT 'Essencial para etiquetas de envio.',
  `telefone` varchar(20) NOT NULL COMMENT 'Para avisar sobre a entrega/novidades.',
  `email` varchar(100) DEFAULT NULL COMMENT 'Bom para pós-venda.',
  `cpf_cnpj` varchar(20) DEFAULT NULL COMMENT 'Importante se for emitir nota ou para empresas.',
  `endereco` varchar(255) DEFAULT NULL COMMENT 'Rua, número, complemento.',
  `cidade_uf` varchar(50) DEFAULT NULL COMMENT 'Ex: Embu das Artes - SP.'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `estoque_pronta_entrega`
--

CREATE TABLE `estoque_pronta_entrega` (
  `id_estoque` int(11) NOT NULL,
  `quantidade_atual` int(11) NOT NULL DEFAULT 0,
  `produtos_id_produto` int(11) NOT NULL,
  `receitas_id_receita` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `financeiro`
--

CREATE TABLE `financeiro` (
  `id_financeiro` int(11) NOT NULL COMMENT 'Identificador único da transação.',
  `data` date NOT NULL COMMENT 'Data do movimento.',
  `descricao` varchar(200) NOT NULL COMMENT 'Ex: "Venda Pedido #001" ou "Compra de Cera".',
  `valor` decimal(10,2) NOT NULL COMMENT 'O valor em dinheiro.',
  `tipo` varchar(10) NOT NULL COMMENT 'ENTRADA ou SAÍDA.',
  `categoria` varchar(50) DEFAULT NULL COMMENT 'Ex: Vendas, Matéria-prima, Marketing, Embalagem.'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `insumos`
--

CREATE TABLE `insumos` (
  `id_insumo` int(11) NOT NULL COMMENT 'O número único de cada item.',
  `nome` varchar(100) NOT NULL COMMENT 'Nome (ex: Essência de Alecrim).',
  `categoria` varchar(50) NOT NULL COMMENT 'Embalagem, matéria prima, etc.',
  `unidade_medida` varchar(10) NOT NULL COMMENT 'ml, g, un.',
  `estoque_atual` decimal(10,3) NOT NULL COMMENT 'Quantidade disponível em estoque.',
  `custo_unitario` decimal(10,4) NOT NULL COMMENT 'Valor pago por unidade/g/ml.',
  `estoque_minimo` decimal(10,2) DEFAULT NULL COMMENT 'O alerta para não deixar acabar.',
  `quantidade_inicial` decimal(10,2) NOT NULL DEFAULT 1.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Despejando dados para a tabela `insumos`
--

INSERT INTO `insumos` (`id_insumo`, `nome`, `categoria`, `unidade_medida`, `estoque_atual`, `custo_unitario`, `estoque_minimo`, `quantidade_inicial`) VALUES
(2, 'CERA DE COCO SOLVEN - 1KG', 'Matéria-prima', 'g', 1555.000, 50.0000, 500.00, 1000.00),
(3, 'ESSENCIA DE ALECRIM CLASSIC LIPOSSOLUVEL - 100ML', 'Matéria-prima', 'g', 500.000, 50.0000, 100.00, 100.00),
(4, 'PAVIO DE MADEIRA', 'Matéria-prima', 'un', 35.000, 10.5000, 10.00, 10.00),
(5, 'POTE DE VIDRO TRANSPARENTE PIANEZZA - 210ML', 'Embalagem', 'un', 21.000, 210.0000, 10.00, 48.00),
(6, 'POTE DE VIDRO AMBAR PIANEZZA - 210ML', 'Embalagem', 'un', 7.000, 90.0000, 5.00, 10.00),
(10, 'ESSENCIA DE BERGAMOTA LIPOSSOLUVEL - 100ML', 'Matéria-prima', 'g', 392.500, 50.0000, 50.00, 100.00),
(11, 'ESSENCIA DE CAPIM-LIMÃO HIDROSSOLUVEL - 100ML', 'Matéria-prima', 'g', 62.500, 50.0000, 50.00, 100.00),
(12, 'BASE VEICULO PARA AROMATIZADOR - 1L', 'Matéria-prima', 'g', 2660.000, 15.0000, 500.00, 1000.00);

-- --------------------------------------------------------

--
-- Estrutura para tabela `itens_venda`
--

CREATE TABLE `itens_venda` (
  `id_item` int(11) NOT NULL COMMENT 'ID da linha do item.',
  `quantidade` int(11) NOT NULL COMMENT 'Quantas unidades desse item.',
  `valor_unitario` decimal(10,2) NOT NULL COMMENT 'Preço da vela no momento da venda.',
  `aromas_kit` text DEFAULT NULL COMMENT 'Aqui você salva: "Alecrim, Bambu, Lavanda".',
  `id_venda` int(11) NOT NULL,
  `estoque_pronta_entrega_id_estoque` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `producao`
--

CREATE TABLE `producao` (
  `id_producao` int(11) NOT NULL COMMENT 'ID único do registro.',
  `quantidade` int(11) NOT NULL COMMENT 'Quantas unidades você fez?',
  `data_producao` date NOT NULL COMMENT 'Dia da fabricação.',
  `lote` varchar(20) NOT NULL COMMENT 'Identificação do lote (ex: LOTE-001).',
  `status` varchar(20) NOT NULL COMMENT 'Ex: EM CURA, FINALIZADA.',
  `id_produto` int(11) NOT NULL,
  `id_receita` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `produtos`
--

CREATE TABLE `produtos` (
  `id_produto` int(11) NOT NULL COMMENT 'Identificador único.',
  `nome` varchar(100) NOT NULL COMMENT 'Ex: Vela Aurora 180g.',
  `categoria` varchar(50) NOT NULL COMMENT 'Ex: Vela, Homespray, etc.',
  `estoque_atual` int(11) NOT NULL COMMENT 'Quantas unidades prontas você tem.',
  `preco_custo` decimal(10,2) NOT NULL COMMENT 'Custo total somando insumos e embalagem',
  `margem_lucro` decimal(5,2) DEFAULT NULL COMMENT 'Percentual de lucro estimado .',
  `estoque_minimo` int(11) DEFAULT NULL COMMENT 'Valor mínimo para alerta para nova produção.'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Despejando dados para a tabela `produtos`
--

INSERT INTO `produtos` (`id_produto`, `nome`, `categoria`, `estoque_atual`, `preco_custo`, `margem_lucro`, `estoque_minimo`) VALUES
(5, 'HOMESPRAY 250ML', 'HOMESPRAY', 0, 0.00, 60.00, 3),
(6, 'VELA AROMÁTICA 150G - PT', 'VELA', 0, 0.00, 60.00, 3),
(7, 'VELA AROMÁTICA 150G - PA', 'VELA', 1, 0.00, 60.00, 3),
(8, 'MINI VELA - 30G', 'VELA', 6, 0.00, 50.00, 3),
(9, 'AROMATIZADOR DE AMBIENTE 200ML', 'AROMATIZADOR', 5, 0.00, 60.00, 3);

-- --------------------------------------------------------

--
-- Estrutura para tabela `receitas`
--

CREATE TABLE `receitas` (
  `id_receita` int(11) NOT NULL COMMENT 'ID da linha.',
  `id_produto` int(11) NOT NULL,
  `aroma_padrao` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Despejando dados para a tabela `receitas`
--

INSERT INTO `receitas` (`id_receita`, `id_produto`, `aroma_padrao`) VALUES
(11, 9, 'ALECRIM'),
(13, 7, 'BERGAMOTA'),
(14, 6, 'ALECRIM'),
(15, 8, 'ALECRIM'),
(16, 5, 'CAPIM-LIMÃO');

-- --------------------------------------------------------

--
-- Estrutura para tabela `receita_insumos`
--

CREATE TABLE `receita_insumos` (
  `quantidade` decimal(10,3) NOT NULL,
  `receitas_id_receita` int(11) NOT NULL,
  `insumos_id_insumo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Despejando dados para a tabela `receita_insumos`
--

INSERT INTO `receita_insumos` (`quantidade`, `receitas_id_receita`, `insumos_id_insumo`) VALUES
(80.000, 11, 3),
(1.000, 11, 5),
(120.000, 11, 12),
(135.000, 13, 2),
(1.000, 13, 4),
(1.000, 13, 6),
(15.000, 13, 10),
(135.000, 14, 2),
(15.000, 14, 3),
(1.000, 14, 4),
(1.000, 14, 5),
(27.000, 15, 2),
(3.000, 15, 3),
(1.000, 15, 4),
(1.000, 15, 5),
(1.000, 16, 5),
(70.000, 16, 11),
(180.000, 16, 12);

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuarios`
--

CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL COMMENT 'ID único.',
  `nome` varchar(100) NOT NULL COMMENT 'Nome completo da pessoa.',
  `usuario` varchar(50) NOT NULL COMMENT 'O login (ex: "maria.silva").',
  `senha` varchar(255) NOT NULL COMMENT 'A senha (que depois será criptografada).',
  `nivel_acesso` varchar(20) NOT NULL COMMENT 'Ex: ''ADM'' ou ''VENDEDOR''.'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Despejando dados para a tabela `usuarios`
--

INSERT INTO `usuarios` (`id_usuario`, `nome`, `usuario`, `senha`, `nivel_acesso`) VALUES
(1, 'Isabelle Melo', 'admin', 'Admin-123', 'ADM');

-- --------------------------------------------------------

--
-- Estrutura para tabela `vendas`
--

CREATE TABLE `vendas` (
  `id_venda` int(11) NOT NULL COMMENT 'Número do pedido.',
  `data_venda` datetime NOT NULL COMMENT 'Data e hora exata.',
  `total_venda` decimal(10,2) NOT NULL COMMENT 'Valor final (já com descontos).',
  `status` varchar(20) NOT NULL COMMENT 'Ex: ORÇAMENTO, PAGO, ENVIADO.',
  `forma_pagamento` varchar(30) DEFAULT NULL COMMENT 'Pix, Cartão, Dinheiro.',
  `id_cliente` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`id_cliente`);

--
-- Índices de tabela `estoque_pronta_entrega`
--
ALTER TABLE `estoque_pronta_entrega`
  ADD PRIMARY KEY (`id_estoque`),
  ADD KEY `fk_estoque_pronta_entrega_produtos1_idx` (`produtos_id_produto`),
  ADD KEY `fk_estoque_pronta_entrega_receitas1_idx` (`receitas_id_receita`);

--
-- Índices de tabela `financeiro`
--
ALTER TABLE `financeiro`
  ADD PRIMARY KEY (`id_financeiro`);

--
-- Índices de tabela `insumos`
--
ALTER TABLE `insumos`
  ADD PRIMARY KEY (`id_insumo`);

--
-- Índices de tabela `itens_venda`
--
ALTER TABLE `itens_venda`
  ADD PRIMARY KEY (`id_item`),
  ADD KEY `fk_itens_venda_vendas1_idx` (`id_venda`),
  ADD KEY `fk_itens_venda_estoque_pronta_entrega1_idx` (`estoque_pronta_entrega_id_estoque`);

--
-- Índices de tabela `producao`
--
ALTER TABLE `producao`
  ADD PRIMARY KEY (`id_producao`),
  ADD KEY `fk_producao_produtos1_idx` (`id_produto`),
  ADD KEY `fk_producao_receitas1_idx` (`id_receita`);

--
-- Índices de tabela `produtos`
--
ALTER TABLE `produtos`
  ADD PRIMARY KEY (`id_produto`);

--
-- Índices de tabela `receitas`
--
ALTER TABLE `receitas`
  ADD PRIMARY KEY (`id_receita`),
  ADD KEY `fk_receitas_produtos_idx` (`id_produto`);

--
-- Índices de tabela `receita_insumos`
--
ALTER TABLE `receita_insumos`
  ADD PRIMARY KEY (`receitas_id_receita`,`insumos_id_insumo`),
  ADD KEY `fk_receita_insumos_insumos1_idx` (`insumos_id_insumo`);

--
-- Índices de tabela `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id_usuario`);

--
-- Índices de tabela `vendas`
--
ALTER TABLE `vendas`
  ADD PRIMARY KEY (`id_venda`),
  ADD KEY `fk_vendas_clientes1_idx` (`id_cliente`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `clientes`
--
ALTER TABLE `clientes`
  MODIFY `id_cliente` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID único do cliente.';

--
-- AUTO_INCREMENT de tabela `estoque_pronta_entrega`
--
ALTER TABLE `estoque_pronta_entrega`
  MODIFY `id_estoque` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de tabela `financeiro`
--
ALTER TABLE `financeiro`
  MODIFY `id_financeiro` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Identificador único da transação.';

--
-- AUTO_INCREMENT de tabela `insumos`
--
ALTER TABLE `insumos`
  MODIFY `id_insumo` int(11) NOT NULL AUTO_INCREMENT COMMENT 'O número único de cada item.', AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de tabela `itens_venda`
--
ALTER TABLE `itens_venda`
  MODIFY `id_item` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID da linha do item.';

--
-- AUTO_INCREMENT de tabela `producao`
--
ALTER TABLE `producao`
  MODIFY `id_producao` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID único do registro.', AUTO_INCREMENT=51;

--
-- AUTO_INCREMENT de tabela `produtos`
--
ALTER TABLE `produtos`
  MODIFY `id_produto` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Identificador único.', AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de tabela `receitas`
--
ALTER TABLE `receitas`
  MODIFY `id_receita` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID da linha.', AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de tabela `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID único.', AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de tabela `vendas`
--
ALTER TABLE `vendas`
  MODIFY `id_venda` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Número do pedido.';

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `estoque_pronta_entrega`
--
ALTER TABLE `estoque_pronta_entrega`
  ADD CONSTRAINT `fk_estoque_pronta_entrega_produtos1` FOREIGN KEY (`produtos_id_produto`) REFERENCES `produtos` (`id_produto`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_estoque_pronta_entrega_receitas1` FOREIGN KEY (`receitas_id_receita`) REFERENCES `receitas` (`id_receita`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Restrições para tabelas `itens_venda`
--
ALTER TABLE `itens_venda`
  ADD CONSTRAINT `fk_itens_venda_estoque_pronta_entrega1` FOREIGN KEY (`estoque_pronta_entrega_id_estoque`) REFERENCES `estoque_pronta_entrega` (`id_estoque`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_itens_venda_vendas1` FOREIGN KEY (`id_venda`) REFERENCES `vendas` (`id_venda`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Restrições para tabelas `producao`
--
ALTER TABLE `producao`
  ADD CONSTRAINT `fk_producao_produtos1` FOREIGN KEY (`id_produto`) REFERENCES `produtos` (`id_produto`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_producao_receitas1` FOREIGN KEY (`id_receita`) REFERENCES `receitas` (`id_receita`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Restrições para tabelas `receitas`
--
ALTER TABLE `receitas`
  ADD CONSTRAINT `fk_receitas_produtos` FOREIGN KEY (`id_produto`) REFERENCES `produtos` (`id_produto`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Restrições para tabelas `receita_insumos`
--
ALTER TABLE `receita_insumos`
  ADD CONSTRAINT `fk_receita_insumos_insumos1` FOREIGN KEY (`insumos_id_insumo`) REFERENCES `insumos` (`id_insumo`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_receita_insumos_receitas1` FOREIGN KEY (`receitas_id_receita`) REFERENCES `receitas` (`id_receita`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Restrições para tabelas `vendas`
--
ALTER TABLE `vendas`
  ADD CONSTRAINT `fk_vendas_clientes1` FOREIGN KEY (`id_cliente`) REFERENCES `clientes` (`id_cliente`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
