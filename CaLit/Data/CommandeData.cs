using SerreVeure.Model;
using Stokomani.Frmwk.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaLit.Data
{
    public static class CommandeData
    {
        public static async Task<List<Commande>> GetAll()
        {
            using (HttpConnector cnx = new HttpConnector())
                return await cnx.GetAsync<List<Commande>>("http://localhost:11207/api/flux/GetAllCmds");
        }
    }
}
