using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lags
{
    class Ordre
    {
        public Ordre(String id, int debut, int duree, double prix)
        {
            this.id = id;
            this.debut = debut;  // au format AAAAJJJ par exemple 25 février 2015 = 2015056
            this.duree = duree;
            this.prix = prix;
        }

        // getters et setters

        public string id { get; set; }

        public int debut { get; set; }

        public int duree { get; set; }

        public double prix { get; set; }

        // attention ne marche pas pour les ordres qui depassent la fin de l'année 
        // voir ticket PLAF nO 4807 
        internal int fin()
        {
            return this.debut + duree;
        }
    }
}
