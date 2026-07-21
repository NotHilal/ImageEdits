using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problème_scientifique___Image
{
    class Complexe
    {
        private double reel;
        private double img;

        public Complexe(double reel, double img)
        {
            this.reel = reel;
            this.img = img;
        }

        #region Propriétés
        public double Reel
        {
            get { return this.reel; }
        }
        public double Img
        {
            get { return this.img; }
        }
        #endregion

        #region Méthodes
        public static Complexe Somme(Complexe complexe1, Complexe complexe2)
        {
            double reel = complexe1.reel + complexe2.reel;
            double img = complexe1.img + complexe2.img;
            return new Complexe(reel, img);
        }
        public static Complexe Produit(Complexe complexe1, Complexe complexe2)
        {
            double reel = (complexe1.reel * complexe2.reel) - (complexe1.img * complexe2.img);
            double img = (complexe1.reel * complexe2.img) + (complexe1.img * complexe2.reel);
            return new Complexe(reel, img);
        }
        public double Module()
        {
            return Math.Sqrt((this.reel * this.reel) + (this.img * this.img));
        }
        public string ToString()
        {
            string s = "";
            if (this.img > 0) s = $"{this.reel} + {this.img}i"; else s = $"{this.reel}  {this.img}i";
            return s;
        }
        public static Complexe operator +(Complexe a, Complexe b)
        {
            return Complexe.Somme(a, b);
        }
        public static Complexe operator *(Complexe a, Complexe b)
        {
            return Complexe.Produit(a, b);
        }
        #endregion
    }
}
