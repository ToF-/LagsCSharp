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
            this.debut = debut;
            this.duree = duree;
            this.prix = prix;

        }

        public string id { get; set; }

        public int debut { get; set; }

        public int duree { get; set; }

        public double prix { get; set; }

        internal int fin()
        {
            return this.debut + duree;
        }
    }
}
