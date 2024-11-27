using Microsoft.AspNetCore.Mvc;
using SerreVeure.Data;
using SerreVeure.Model;

namespace SerreVeure.Controllers
{

    namespace SerreVeure.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class FluxController : ControllerBase
        {
            private static readonly string traceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Trace", "tracefile.txt");

            private void WriteTrace(string methodName, string parameters)
            {
                try
                {
                    string? directoryPath = Path.GetDirectoryName(traceFilePath);
                    if (directoryPath != null && !Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string message = $"{DateTime.Now}: {methodName} called with parameters: {parameters}";
                    System.IO.File.AppendAllText(traceFilePath, message + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'écriture dans le fichier de trace : {ex.Message}");
                }
            }

            [HttpGet("GetAllCmds")]
            public ActionResult<List<Commande>> RecupererCommandes()
            {
                WriteTrace(nameof(RecupererCommandes), "No parameters");
                using (CommandeData data = new CommandeData())
                    return data.GetAll();
            }

            [HttpGet("GetAllprdts")]
            public ActionResult<List<Produit>> RecupererProduits()
            {
                WriteTrace(nameof(RecupererProduits), "No parameters");
                using (ProduitData data = new ProduitData())
                    return data.GetAll();
            }

            [HttpGet("GetprdtByRef/{reference}")]
            public ActionResult<ProduitComplet> RecupererProduitCompletParRef(string reference)
            {
                WriteTrace(nameof(RecupererProduitCompletParRef), $"reference: {reference}");
                using (CommandeData cmdeData = new CommandeData())
                using (ProduitData prdtData = new ProduitData())
                {
                    if (reference.Contains("%5C"))
                    {
                        reference = reference.Replace("%5C", "/");
                    }
                    if (reference.Contains("%2F"))
                    {
                        reference = reference.Replace("%2F", "/");
                    }
                    Produit prdt = prdtData.GetProduct(reference);
                    if (prdt == null)
                        return null;
                    else
                    {
                        ProduitComplet prdtC = new ProduitComplet() { Libelle = prdt.Libelle, Reference = prdt.Reference };
                        prdtC.Commandes = cmdeData.GetProductCommand(reference);

                        return prdtC;
                    }
                }
            }

            [HttpPost("RecieveCommand")]
            public void RecieveCommand([FromBody] Commande command)
            {
                WriteTrace(nameof(RecieveCommand), $"ReferenceProduit: {command.ReferenceProduit}, LibelleProduit: {command.LibelleProduit}, Quantite: {command.Quantite}, DateLivraisonPrevue: {command.DateLivraisonPrevue}");
                try
                {
                    using (CommandeData cmdeData = new CommandeData())
                    using (ProduitData prdtData = new ProduitData())
                    {
                        prdtData.CreateProduct(command.ReferenceProduit, command.LibelleProduit);
                        cmdeData.CreateCommand(command.ReferenceProduit, command.LibelleProduit, command.Quantite, command.DateLivraisonPrevue != DateTime.MinValue ? command.DateLivraisonPrevue.ToString() : new DateTime(1,1,1900).ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'exécution de la commande SQL : {ex.Message}");
                }
            }
        }

    }

}
