using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problème_scientifique___Image
{
    public class Pixel
    {
        //Attributs d'instances
        private int r;
        private int g;
        private int b;

        public Pixel(int r, int g, int b) //On met pas RGB car dans la matrice image tout sera inverser càd on aura BGR
        {
            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255) //Si le pixel n'a pas des valeurs valides, on le met automatiquement noir
            {
                this.r = 0;
                this.g = 0;
                this.b = 0;
            }
            this.r = r;
            this.g = g;
            this.b = b;
        }

        #region Propriétés
        public int Red
        {
            get { return this.r; }
            set { this.r = value; }
        }
        public int Green
        {
            get { return this.g; }
            set { this.g = value; }
        }
        public int Blue
        {
            get { return this.b; }
            set { this.b = value; }
        }
        #endregion

        #region Méthodes
        public Pixel NuanceDeGris()
        {
            int moyenne = (r + g + b) / 3;
            Pixel new_pixel = new Pixel(moyenne, moyenne, moyenne);   
            return new_pixel;
        }

        /// /// <summary>
        /// Convertie un triplet RGB en un triplet HSV. Retourne un Tuple(int h, int s, int v) 
        /// h = [0, 360], s = [0,1], v = [0,1]
        /// Si s == 0, alors h = -1 (non défini) 
        /// </summary>
        /// <param name="r">Valeur Rouge</param>
        /// <param name="g">Valeur Verte</param>
        /// <param name="b">Valeur Bleu</param>
        /// <returns></returns>
        public static Tuple<double, double, double> RGBversHSV(int r, int g, int b)
        {
            double R = Math.Round(r / 255.0, 3); //On divise par 255 car r,g,b € [0,1]
            double G = Math.Round(g / 255.0, 3);
            double B = Math.Round(b / 255.0, 3);
            double h, s, v;
            h = s = v = 0; //Initialisation des valeurs pour ne pas avoir de problème avec la non-déclaration
            double min = Math.Min(Math.Min(R, G), B); 
            double max = Math.Max(Math.Max(R, G), B);
            double delta = Math.Round(max - min, 3) ;

            Tuple<double, double, double> HSV;
            if (delta == 0)
            {
                HSV = new Tuple<double, double, double>(0, 0, 0);
            }
            else
            {
                v = max; //On définit la "valeur", le slider de luminosité qu'on arrondie à la deuxième décimale
                if (max != 0) 
                {
                    s = Math.Round(delta / max, 3); //Calcul de la saturation
                    if (R == max) //Calcul de la teinte
                    {
                        h = (60 * ((G - B) / delta) + 360) % 360;
                    }
                    else if (G == max)
                    {
                        h = 60 * ((B - R) / delta) + 120;
                    }
                    else h = 60 * ((R - G) / delta) + 240;

                    if (h < 0) h += 360;
                    h = Math.Round(h);
                }
                else
                {
                    //r = g = b = 0 si max == 0
                    HSV = new Tuple<double, double, double>(-1, 0, 0); //On retourne un set de valeur à zéro
                }
                HSV = new Tuple<double, double, double>(h, s, v);
            }
            return HSV;
        }
        public static Tuple<int, int, int> HSVversRGB(double h, double s, double v)
        {
            double t, f, l, m, n, R, G, B;
            double H = h;
            int r, g, b;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            if (v <= 0) R = G = B = 0;
            else if (s <= 0) //Saturation nulle ==> couleur achromatique càd grise
            {
                R = G = B = v;
            }
            else
            {
                t = H / 60.0;
                int i = (int)Math.Floor(t);
                f = t - i;
                l = v * (1 - s);
                m = v * (1 - f * s);
                n = v * (1 - (1 - f) * s);
                switch (i)
                {
                    case 0:
                        R = v;
                        G = n;
                        B = l;
                        break;
                    case 1:
                        R = m;
                        G = v;
                        B = l;
                        break;
                    case 2:
                        R = l;
                        G = v;
                        B = n;
                        break;
                    case 3:
                        R = l;
                        G = m;
                        B = v;
                        break;
                    case 4:
                        R = n;
                        G = l;
                        B = v;
                        break;
                    case 5:
                        R = v;
                        G = l;
                        B = m;
                        break;
                    case 6:
                        R = v;
                        G = n;
                        B = l;
                        break;
                    case -1:
                        R = v;
                        G = l;
                        B = m;
                        break;
                    default: //La couleur n'est pas définie, on prétend que la couleur est noir/blanc
                        R = G = B = v; 
                        break;
                }
            }
            r = Ajustement((int)(R * 255.0));
            g = Ajustement((int)(G * 255.0));
            b = Ajustement((int)(B * 255.0));
            return new Tuple<int, int, int>(r, g, b);
        }
        
        /// <summary>
        /// Ajuste les valeurs pour être dans [0,255]
        /// </summary>
        public static int Ajustement(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
        #endregion
    }
}
