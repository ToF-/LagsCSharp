using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lags
{
    public class LagsService
    {
        List<Ordre> ListOrdre = new List<Ordre>();

        // lit le fihier des ordres et calcule le CA
        public void getFichierOrder(String fileName)
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
                        ListOrdre.Add(ordre);
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
        void WriteOrdres(String nomFich)
        {
            using (TextWriter writer = File.CreateText(nomFich))
            {
                foreach (Ordre ordre in ListOrdre)
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
        public void Liste()
        {
            Console.WriteLine("LISTE DES ORDRES");
            Console.WriteLine("{0,-8} {1,7} {2,5} {3,10}",
                "ID", "DEBUT", "DUREE", "PRIX");
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
               "--------", "-------", "-----", "----------");
            ListOrdre = ListOrdre.OrderBy(ordre => ordre.debut).ToList();
            ListOrdre.ForEach(AfficherOrdre);
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
               "--------", "-------", "-----", "----------");
        }

        public void AfficherOrdre(Ordre ordre)
        {
            Console.WriteLine("{0,-8} {1:0000000} {2:00000} {3,10:N2}",
                ordre.id, ordre.debut, ordre.duree, ordre.prix);

        }
        // Ajoute un ordre; le CA est recalculé en conséquence
        public void AjouterOrdre()
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
            ListOrdre.Add(ordre);
            WriteOrdres("ordres.csv");
        }

        //public void CalculerLeCA()
        //{
        //    Console.WriteLine("CALCUL CA..");
        //    laListe = laListe.OrderBy(ordre => ordre.debut).ToList();
        //    double ca = CA(laListe);
        //    Console.WriteLine("CA: {0,10:N2}", ca);
        //}

        private double CA(List<Ordre> ordres, bool debug)
        {
            // si aucun ordre, job done, TROLOLOLO..
            if (ordres.Count() == 0)
                return 0.0;
            Ordre order = ordres.ElementAt(0);
            // attention ne marche pas pour les ordres qui depassent la fin de l'année 
            // voir ticket PLAF nO 4807 
            List<Ordre> liste = ordres.Where(ordre => ordre.debut >= order.debut + order.duree).ToList();
            List<Ordre> liste2 = ordres.GetRange(1, ordres.Count() - 1);
            double ca = order.prix + CA(liste, debug);
            // Lapin compris?
            double ca2 = CA(liste2, debug);
            Console.Write(debug ? String.Format("{0,10:N2}\n", Math.Max(ca, ca2)):".");
            return Math.Max(ca, ca2); // LOL
        }

        // MAJ du fichier
        public void Suppression()
        {
            Console.WriteLine("SUPPRIMER UN ORDRE");
            Console.Write("ID:");
            string id = Console.ReadLine().ToUpper();
            this.ListOrdre = ListOrdre.Where(ordre => ordre.id != id).ToList();
            WriteOrdres("ORDRES.CSV");
        }



        internal void CalculerLeCA(bool debug)
        {
            Console.WriteLine("CALCUL CA..");
            ListOrdre = ListOrdre.OrderBy(ordre => ordre.debut).ToList();
            double ca = CA(ListOrdre, debug);
            Console.WriteLine("CA: {0,10:N2}", ca);
        }

    }
}
