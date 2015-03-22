using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lags
{
    class Program
    {
        static List<Ordre> laListe = new List<Ordre>();

        // ==================
        // fonction principale
        // ===================

        static void Main(string[] args)
        {
            getFichierOrder("ORDRES.CSV");
            bool flag = false;
            // tant que ce n'est pas la fin du programme
            while (!flag)
            {
                // met la commande à Z
                Char commande = 'Z';
                while (commande != 'A' && commande != 'L' && commande != 'S' && commande != 'Q' && commande != 'C')
                {
                    Console.WriteLine("A)JOUTER UN ORDRE  L)ISTER   C)ALCULER CA  S)UPPRIMER  Q)UITTER");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    commande = Char.ToUpper(keyInfo.KeyChar);
                    Console.WriteLine();
                }
                switch (commande)
                {
                    case 'Q':
                        {
                            flag = true;
                            break;
                        }
                    case 'L':
                        {
                            Liste();
                            break;
                        }
                    case 'A':
                        {
                            AjouterOrdre();
                            break;
                        }
                    case 'S':
                        {
                            Suppression();
                            break;
                        }
                    case 'C':
                        {
                            CalculerLeCA();
                            break;
                        }
                }
            }
        }

        // lit le fihier des ordres et calcule le CA
        static void getFichierOrder(String fileName)
        {
            try
            { 
            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var champs = reader.ReadLine().Split(';');
                    String chp1 = champs[0];
                    int chp2 = Int32.Parse(champs[1]);
                    int champ3 = Int32.Parse(champs[2]);
                    double chp4 = Double.Parse(champs[3]);
                    Ordre ordre = new Ordre(chp1, chp2, champ3, chp4);
                    laListe.Add(ordre);
                }
            }
            }
                catch (FileNotFoundException e)
                    {
                        Console.WriteLine("FICHIER ORDRES.CSV NON TROUVE. CREATION FICHIER.");
                        WriteOrdres(fileName);
                    }
        }

        // écrit le fichier des ordres
        static void WriteOrdres(String nomFich)
        {
            using (TextWriter writer = File.CreateText(nomFich))
            {
                foreach (Ordre ordre in laListe)
                {
                    string[] ligneCSV = new string[4];
                    ligneCSV[0] = ordre.id;
                    ligneCSV[1] = ordre.debut.ToString();
                    ligneCSV[2] = ordre.duree.ToString();
                    ligneCSV[3] = ordre.prix.ToString();
                    writer.WriteLine(string.Join(";", ligneCSV));
                }
            }
        }


        // affiche la liste des ordres
        static void Liste()
        {
            Console.WriteLine("LISTE DES ORDRES");
            Console.WriteLine("{0,-8} {1,7} {2,5} {3,10}",
                "ID", "DEBUT", "DUREE", "PRIX");
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
               "--------", "-------", "-----", "----------");
            laListe = laListe.OrderBy(ordre => ordre.debut).ToList();
            laListe.ForEach(AfficherOrdre);
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
               "--------", "-------", "-----", "----------");
        }

        static void AfficherOrdre(Ordre ordre)
        {
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
                ordre.id, ordre.debut, ordre.duree, ordre.prix);

        }
        // Ajoute un ordre; le CA est recalculé en conséquence
        static void AjouterOrdre()
        {
            Console.WriteLine("AJOUTER UN ORDRE");
            Console.WriteLine("FORMAT = ID;DEBUT;FIN;PRIX");
            String line = Console.ReadLine().ToUpper();
            var champs = line.Split(';');
            String id = champs[0];
            int dep = Int32.Parse(champs[1]);
            int dur = Int32.Parse(champs[2]);
            double prx = Double.Parse(champs[3]);
            Ordre ordre = new Ordre(id, dep, dur, prx);
            laListe.Add(ordre);
            WriteOrdres("ordres.csv");
        }

        static void CalculerLeCA()
        {
            Console.WriteLine("CALCUL CA..");
            laListe = laListe.OrderBy(ordre => ordre.debut).ToList();
            double ca = CA(laListe);
            Console.WriteLine("CA: {0,10:N2}", ca);
        }

        private static double CA(List<Ordre> ordres)
        {
            if (ordres.Count() == 0)
                return 0.0;
            Ordre order = ordres.ElementAt(0);
            // Astuuuuuuce!
            List<Ordre> liste = ordres.Where(ordre => ordre.debut >= order.fin()).ToList();
            List<Ordre> liste2 = ordres.GetRange(1, ordres.Count() - 1);
            double ca = order.prix + CA(liste);
            // Lapin compris?
            double ca2 = CA(liste2);
            return Math.Max(ca, ca2); // LOL
        }

        // MAJ du fichier
        static void Suppression()
        {
            Console.WriteLine("SUPPRIMER UN ORDRE");
            Console.Write("ID:");
            string id = Console.ReadLine().ToUpper();
            laListe = laListe.Where(ordre => ordre.id != id).ToList();
            WriteOrdres("ORDRES.CSV");
        }
    }
}