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

        // Adicionamos o parâmetro idReceita vindo da seleção da UI
        public void ProcessarProducaoCompleta(ProducaoDTO producao, int idReceita)
        {
            if (producao.IdProduto <= 0)
                throw new Exception("Selecione um produto válido para a produção.");

            if (idReceita <= 0)
                throw new Exception("Selecione uma receita válida ou certifique-se de que o aroma selecionado possui uma receita cadastrada.");

            if (producao.QtdeProduzida <= 0)
                throw new Exception("A quantidade produzida deve ser maior que zero.");

            if (string.IsNullOrEmpty(producao.Lote))
                throw new Exception("O número do lote não foi gerado corretamente.");

            producaoDAL.RegistrarProducaoCompleta(producao, idReceita);
        }

        public DataTable ListarProducoes()
        {
            return producaoDAL.ListarProducoes();
        }

        public void AtualizarStatus(int idProducao, string novoStatus, int idProduto, int qtdProduzida, int idReceita)
        {
            if (idProducao <= 0) throw new Exception("Lote inválido para atualização.");
            if (string.IsNullOrEmpty(novoStatus)) throw new Exception("O status não pode ser vazio.");

            // Repassa para a DAL incluindo o idReceita para alimentar a pronta entrega corretamente
            producaoDAL.AtualizarStatusProducao(idProducao, novoStatus, idProduto, qtdProduzida, idReceita);
        }

        public void ProcessarExclusaoComEstorno(int idProducao, int idProduto, int qtdProduzida, string statusAtual, int idReceita)
        {
            // Trava de segurança sênior: impede corromper o estoque de pronta entrega
            if (statusAtual.Trim().ToUpper() == "EMBALADA")
            {
                throw new Exception("Não é possível excluir uma produção com status 'EMBALADA'. O produto já foi enviado ao estoque de pronta entrega. Caso necessário, ajuste o estoque manualmente na tela correspondente.");
            }

            // CORREÇÃO SÊNIOR: Agora buscamos os insumos pelo idReceita (aroma específico), não pelo idProduto genérico!
            DataTable dtItensReceita = receitaDAL.ListarItensDaReceita(idReceita);

            List<ItensReceitaDTO> insumosParaDevolver = new List<ItensReceitaDTO>();

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

            // Repassa para a DAL executar o estorno no Almoxarifado e o DELETE da produção
            producaoDAL.ExcluirEEstornarProducao(idProducao, idProduto, qtdProduzida, insumosParaDevolver);
        }
    }
}
