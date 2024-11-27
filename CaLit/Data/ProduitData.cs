using SerreVeure.Model;
using Stokomani.Frmwk.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaLit.Data
{
    public static class ProduitData
    {
        public static async Task<List<Produit>> GetAll()
        {
            using (HttpConnector cnx = new HttpConnector())
                return await cnx.GetAsync<List<Produit>>("http://localhost:11207/api/flux/GetAllprdts");
        }

        public static async Task<ProduitComplet> Get(string reference)
        {
            using (HttpConnector cnx = new HttpConnector())
                return await cnx.GetAsync<ProduitComplet>($"http://localhost:11207/api/flux/GetprdtByRef/{reference}");
        }
    }
}
