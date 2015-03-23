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
        const bool debug = true;
        // ==================
        // fonction principale
        // ===================

        static void Main(string[] args)
        {
            LagsService service = new LagsService();
            service.getFichierOrder("ORDRES.CSV");
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
                            service.Liste();
                            break;
                        }
                    case 'A':
                        {
                            service.AjouterOrdre();
                            break;
                        }
                    case 'S':
                        {
                            service.Suppression();
                            break;
                        }
                    case 'C':
                        {
                            service.CalculerLeCA(debug);
                            break;
                        }
                }
            }
        }


        //// lit le fihier des ordres et calcule le CA
        //static void getFichierOrder(String fileName)
        //{
        //    try
        //    {
        //        using (var reader = new StreamReader(fileName))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                var champs = reader.ReadLine().Split(';');
        //                String chp1 = champs[0];
        //                int chp2 = Int32.Parse(champs[1]);
        //                int champ3 = Int32.Parse(champs[2]);
        //                double chp4 = Double.Parse(champs[3]);
        //                Ordre ordre = new Ordre(chp1, chp2, champ3, chp4);
        //                laListe.Add(ordre);
        //            }
        //        }
        //    }
        //    catch (FileNotFoundException e)
        //    {
        //        Console.WriteLine("FICHIER ORDRES.CSV NON TROUVE. CREATION FICHIER.");
        //        WriteOrdres(fileName);
        //    }
        //}
       
    }
}