using System;
using System.Data;
using Ambarina.DAL;

namespace Ambarina.BLL
{
    public class DashboardBLL
    {
        DashboardDAL dashDAL = new DashboardDAL();

        public int CarregarInsumosCriticos()
        {
            return dashDAL.ObterInsumosAbaixoDoMinimo();
        }

        public int CarregarTotalProntaEntrega()
        {
            return dashDAL.ObterTotalProntaEntrega();
        }

        public int CarregarTotalEmProducaoAtiva()
        {
            return dashDAL.ObterTotalEmProducaoAtiva();
        }

        public DataTable CarregarProducoesAtivasGrid()
        {
            return dashDAL.ObterProducoesAtivasGrid();
        }
    }
}