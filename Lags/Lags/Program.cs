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
        static List<Ordre> ordres = new List<Ordre>();
        static void Main(string[] args)
        {
            LireOrdres("ORDRES.CSV");
            bool finDeProgramme = false;
            while (!finDeProgramme)
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
                            finDeProgramme = true;
                            break;
                        }
                    case 'L':
                        {
                            ListerOrdres();
                            break;
                        }
                    case 'A':
                        {
                            AjouterOrdre();
                            break;
                        }
                    case 'S':
                        {
                            SupprimerOrdre();
                            break;
                        }
                    case 'C':
                        {
                            CalculerCA();
                            break;
                        }
                }
            }
        }

        // lit le fihier des ordres et calcule le CA
        static void LireOrdres(String nomFichierOrdres)
        {
            try
            { 
            using (var reader = new StreamReader(nomFichierOrdres))
            {
                while (!reader.EndOfStream)
                {
                    var champs = reader.ReadLine().Split(';');
                    String id = champs[0];
                    int depart = Int32.Parse(champs[1]);
                    int duree = Int32.Parse(champs[2]);
                    double prix = Double.Parse(champs[3]);
                    Ordre ordre = new Ordre(id, depart, duree, prix);
                    ordres.Add(ordre);
                }
            }
            }
                catch (FileNotFoundException e)
                    {
                        Console.WriteLine("FICHIER ORDRES.CSV NON TROUVE. CREATION FICHIER.");
                        EcrireOrdres(nomFichierOrdres);
                    }
        }

        // écrit le fichier des ordres
        static void EcrireOrdres(String nomFichierOrdres)
        {
            using (TextWriter writer = File.CreateText(nomFichierOrdres))
            {
                foreach (Ordre ordre in ordres)
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
        static void ListerOrdres()
        {
            Console.WriteLine("LISTE DES ORDRES");
            Console.WriteLine("{0,-8} {1,7} {2,5} {3,10}",
                "ID", "DEBUT", "DUREE", "PRIX");
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
               "--------", "-------", "-----", "----------");
            ordres = ordres.OrderBy(ordre => ordre.debut).ToList();
            ordres.ForEach(AfficherOrdre);
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
            int depart = Int32.Parse(champs[1]);
            int duree = Int32.Parse(champs[2]);
            double prix = Double.Parse(champs[3]);
            Ordre ordre = new Ordre(id, depart, duree, prix);
            ordres.Add(ordre);
            EcrireOrdres("ordres.csv");
        }

        static void CalculerCA()
        {
            Console.WriteLine("CALCUL CA..");
            ordres = ordres.OrderBy(ordre => ordre.debut).ToList();
            double ca = ChiffreAffaire(ordres);
            Console.WriteLine("CA: {0,10:N2}", ca);
        }

        private static double ChiffreAffaire(List<Ordre> ordres)
        {
            if (ordres.Count() == 0)
                return 0.0;
            Ordre premier = ordres.ElementAt(0);
            List<Ordre> liste1 = ordres.Where(ordre => ordre.debut >= premier.fin()).ToList();
            List<Ordre> liste2 = ordres.GetRange(1, ordres.Count() - 1);
            double ca1 = premier.prix + ChiffreAffaire(liste1);
            double ca2 = ChiffreAffaire(liste2);
            return Math.Max(ca1, ca2);
        }

        // MAJ du fichier
        static void SupprimerOrdre()
        {
            Console.WriteLine("SUPPRIMER UN ORDRE");
            Console.Write("ID:");
            string id = Console.ReadLine().ToUpper();
            ordres = ordres.Where(ordre => ordre.id != id).ToList();
            EcrireOrdres("ORDRES.CSV");
        }
    }
}