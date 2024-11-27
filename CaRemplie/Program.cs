using System;
using System.IO;
using System.Collections.Generic;

using Stokomani.Frmwk.Web;
using SerreVeure.Model;
using System.Threading.Tasks;

namespace CaRemplie
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Chargement des commandes...");

            List<ReceiveCommand> commandes = LoadCommands();

            Console.WriteLine("Création des commandes...");
            if (commandes != null)
            {
                foreach (var cmde in commandes)
                {
                    try
                    {
                        await CreateCommandAsync(cmde);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la création de la commande {cmde.Reference}: {ex.Message}");
                    }
                }
            }
        }

        private static async Task CreateCommandAsync(ReceiveCommand commande)
        {
            Console.WriteLine($"Création de la commande {commande.Reference}...");
            try
            {
                using (HttpConnector cnx = new HttpConnector())
                {
                    await cnx.Post("http://localhost:11207/api/flux/RecieveCommand", new Commande() { ReferenceProduit = commande.Reference, LibelleProduit = commande.Libelle, Quantite = commande.Quantite, DateLivraisonPrevue = commande.DateLivraisonPrevue });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de la commande {commande.Reference}: {ex.Message}");
                throw;
            }
        }

        private static List<ReceiveCommand> LoadCommands()
        {
            List<ReceiveCommand> commandes = new List<ReceiveCommand>();

            if (Directory.Exists(@"Work\Todo"))
            {
                string[] files = Directory.GetFiles(@"Work\Todo");

                if (files != null)
                {
                    foreach (string file in files)
                    {
                        string[] lines = File.ReadAllLines(file);

                        if (lines != null)
                        {
                            foreach (string line in lines)
                            {
                                string[] elements = line.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                                if (elements.Length >= 3)
                                {
                                    ReceiveCommand cmde = new ReceiveCommand();
                                    cmde.Reference = elements[0];
                                    cmde.Libelle = elements[1];

                                    if (int.TryParse(elements[2], out int qte))
                                    {
                                        cmde.Quantite = qte;
                                        commandes.Add(cmde);
                                    }
                                    if(elements.Length > 3)
                                    { 
                                        if (DateTime.TryParse(elements[3], out DateTime dateLivraisonPrevue))
                                        {
                                            cmde.DateLivraisonPrevue = dateLivraisonPrevue;
                                        }
                                        else
                                        {
                                            cmde.DateLivraisonPrevue = DateTime.MinValue;
                                        }
                                    }
                                    else if (elements.Length == 3)
                                    {
                                        cmde.DateLivraisonPrevue = DateTime.MinValue;
                                    }
                                    else
                                        Console.WriteLine($"Erreur dans la lecture de la quantité pour la référence '{cmde.Reference}'");
                                }
                            }
                        }
                        else
                            Console.WriteLine($"Fichier '{file}' vide");

                        string cheminDestination = Path.Combine(@"Work\Done", Path.GetFileName(file));
                        MoveFile(file, cheminDestination);
                        WriteSignalFile();
                    }
                }
                else
                    Console.WriteLine("Dossier 'todo' vide");
            }
            else
                Console.WriteLine("Pas de dossier 'todo'");

            return commandes;
        }
        private static void MoveFile(string cheminSource, string cheminDestination)
        {
            try
            {
                if (File.Exists(cheminSource))
                {
                    File.Move(cheminSource, cheminDestination);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du déplacement du fichier : {ex.Message}");
            }
        }
        private static void WriteSignalFile()
        {
            try
            {
                string signalDirectory = @"Work\signal";
                string signalFilePath = Path.Combine(signalDirectory, "refresh_signal.txt");
                if (!Directory.Exists(signalDirectory))
                {
                    Directory.CreateDirectory(signalDirectory);
                }
                File.WriteAllText(signalFilePath, "refresh");
                Console.WriteLine("Fichier de signal écrit avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture du fichier de signal : {ex.Message}");
            }
        }
    }
    internal class ReceiveCommand
    {
        public string Reference { get; set; }
        public string Libelle { get; set; }
        public int Quantite { get; set; }
        public DateTime DateLivraisonPrevue { get; set; }
    }
}
