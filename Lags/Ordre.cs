using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lags
{
    public class Ordre
    {
        public Ordre(String id, int debut, int duree, double prix)
        {
            this.id = id;
            this.debut = debut;  // au format AAAAJJJ par exemple 25 février 2015 = 2015056
            this.duree = duree;
            this.prix = prix;
        }

        // getters et setters
        //id de l'ordre 
        public string id { get; set; }
        // debut
        public int debut { get; set; }
        // duree
        public int duree { get; set; }
        // valeur
        public double prix { get; set; }

    }
}
