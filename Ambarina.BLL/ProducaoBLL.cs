using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ambarina.DAL;
using Ambarina.DTO;

namespace Ambarina.BLL
{
    public class ProducaoBLL
    {
        ProducaoDAL producaoDAL = new ProducaoDAL();
        ReceitaDAL receitaDAL = new ReceitaDAL();

        public string ObterProximoLote()
        {
            return producaoDAL.ObterProximoLote();
        }

        public void ProcessarProducaoCompleta(ProducaoDTO producao)
        {
            // Validações de Negócio Sênior
            if (producao.IdProduto <= 0)
                throw new Exception("Selecione um produto válido para a produção.");

            if (producao.QtdeProduzida <= 0)
                throw new Exception("A quantidade produzida deve ser maior que zero.");

            if (string.IsNullOrEmpty(producao.Lote))
                throw new Exception("O número do lote não foi gerado corretamente.");

            // 1. Busca a receita desse produto para descobrir quais insumos ele gasta
            // Usamos o ID da receita que você captura ao selecionar a grade ou aroma
            DataTable dtItensReceita = receitaDAL.ListarItensDaReceita(producao.IdProduto);

            if (dtItensReceita == null || dtItensReceita.Rows.Count == 0)
            {
                throw new Exception("Não é possível produzir este item pois ele não possui uma receita cadastrada.");
            }

            // 2. Monta a lista calculando a proporção (Insumo da Receita x Quantidade Produzida)
            List<ItensReceitaDTO> insumosParaBaixar = new List<ItensReceitaDTO>();
            foreach (DataRow row in dtItensReceita.Rows)
            {
                decimal qtdUnitaria = Convert.ToDecimal(row["Qtd"]);
                decimal qtdTotalConsumida = qtdUnitaria * producao.QtdeProduzida;

                insumosParaBaixar.Add(new ItensReceitaDTO
                {
                    NomeInsumo = row["Insumo"].ToString(),
                    Quantidade = qtdTotalConsumida
                });
            }

            // 3. Dispara a transação atômica na DAL
            producaoDAL.RegistrarProducaoCompleta(producao, insumosParaBaixar);
        }
        public DataTable ListarProducoes()
        {
            return producaoDAL.ListarProducoes();
        }

        public void AtualizarStatus(int idProducao, string novoStatus, int idProduto, int qtdProduzida)
        {
            if (idProducao <= 0) throw new Exception("Lote inválido para atualização.");
            if (string.IsNullOrEmpty(novoStatus)) throw new Exception("O status não pode ser vazio.");

            producaoDAL.AtualizarStatusProducao(idProducao, novoStatus, idProduto, qtdProduzida);
        }
        public void ProcessarExclusaoComEstorno(int idProducao, int idProduto, int qtdProduzida, string statusAtual)
        {
            // Trava de segurança sênior: impede corromper o estoque de pronta entrega
            if (statusAtual.Trim().ToUpper() == "EMBALADA")
            {
                throw new Exception("Não é possível excluir uma produção com status 'EMBALADA'. O produto já foi enviado ao estoque de pronta entrega. Caso necessário, ajuste o estoque manualmente na tela correspondente.");
            }

            ReceitaDAL receitaDAL = new ReceitaDAL();
            // 1. Busca quais insumos aquela receita gastava
            DataTable dtItensReceita = receitaDAL.ListarItensDaReceita(idProduto);

            List<ItensReceitaDTO> insumosParaDevolver = new List<ItensReceitaDTO>();

            // 2. Se a receita existir, calcula a quantidade exata a ser devolvida
            if (dtItensReceita != null && dtItensReceita.Rows.Count > 0)
            {
                foreach (DataRow row in dtItensReceita.Rows)
                {
                    decimal qtdUnitaria = Convert.ToDecimal(row["Qtd"]);
                    decimal qtdTotalDevolver = qtdUnitaria * qtdProduzida;

                    insumosParaDevolver.Add(new ItensReceitaDTO
                    {
                        NomeInsumo = row["Insumo"].ToString(),
                        Quantidade = qtdTotalDevolver
                    });
                }
            }

            // 3. Repassa para a DAL executar a transação de exclusão e soma no estoque
            producaoDAL.ExcluirEEstornarProducao(idProducao, idProduto, qtdProduzida, insumosParaDevolver);
        }
    }
}
