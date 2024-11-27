using SerreVeure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SerreVeure.Data
{
    internal class ProduitData : DataConnectorBase
    {

        public List<Produit> GetAll()
        {
            return ConvertDataTableToList(ExecuteQuery("SELECT * FROM produit"), Convert);
        }

        public Produit GetProduct(string reference)
        {
            List<Produit> prdts = ConvertDataTableToList(ExecuteQuery($"SELECT * FROM produit WHERE reference = '{reference}';"), Convert);

            return prdts?.FirstOrDefault();
        }

        public void CreateProduct(string refPrdt, string libPrdt)
        {
            ExecuteNonQuery($"INSERT INTO produit(reference,libelle) VALUES('{refPrdt}','{libPrdt}')");
        }

        private Produit Convert(DataRow dr)
        {
            Produit prdt = null;

            if (dr != null)
            {
                prdt = new Produit();
                prdt.Reference = (string)dr["reference"];
                prdt.Libelle = (string)dr["libelle"];
            }

            return prdt;
        }
    }
}
