using System.Collections.Generic;

namespace SerreVeure.Model
{
    public class ProduitComplet : Produit
    {
        public ProduitComplet()
        {
            Commandes = new List<Commande>();
        }

        public List<Commande> Commandes { get; set; }
    }
}
