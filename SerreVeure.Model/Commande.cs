using System;

namespace SerreVeure.Model
{
    public class Commande
    {
        public string ReferenceProduit { get; set; }
        public string LibelleProduit { get; set; }
        public int Quantite { get; set; }
        public DateTime DateLivraisonPrevue { get; set; }
    }
}
