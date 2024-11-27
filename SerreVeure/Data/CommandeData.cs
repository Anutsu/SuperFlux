using SerreVeure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SerreVeure.Data
{
    internal class CommandeData : DataConnectorBase
    {
        public List<Commande> GetAll()
        {
            return ConvertDataTableToList(ExecuteQuery("SELECT * FROM commande"), Convert);
        }

        public List<Commande> GetProductCommand(string reference)
        {
            return ConvertDataTableToList(ExecuteQuery($"SELECT * FROM commande WHERE reference = '{reference}';"), Convert);
        }

        public void CreateCommand(string refPrdt, string libPrdt, int qte, string deliveryDate)
        {
            ExecuteNonQuery($"INSERT INTO commande(reference,libelle,quantite,date_commande_str) VALUES('{refPrdt}','{libPrdt}','{qte}','{deliveryDate}')");
        }

        private Commande Convert(DataRow dr)
        {
            Commande cmd = null;

            if (dr != null)
            {
                cmd = new Commande();
                cmd.ReferenceProduit = (string)dr["reference"];
                cmd.LibelleProduit = (string)dr["libelle"];
                cmd.Quantite = (int)dr["quantite"];
            }

            return cmd;
        }
    }
}
