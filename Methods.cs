using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Problème_scientifique___Image
{
    class Methods
    {
        #region Variables
        private static char[] ListeLettres = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/', ':' };
        #endregion
        //Cette classe à pour seul but de stocker les méthodes qui sont utilisées par les fonctions principales de traitement d'image
        #region Méthodes
        public static int Distance(Pixel pixel1, Pixel pixel2)
        {
            int r1 = pixel1.Red;
            int g1 = pixel1.Green;
            int b1 = pixel1.Blue;

            int r2 = pixel2.Red;
            int g2 = pixel2.Green;
            int b2 = pixel2.Blue;
            return (int)(Math.Pow((r1 - r2), 2) + Math.Pow((g1 - g2), 2) + Math.Pow((b1 - b2), 2));
        }
        public static void StatusUpdate(int pourcentage, Stopwatch stopwatch)
        {
            StringBuilder espace = new StringBuilder();
            espace.Append(' ', 10);
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            Console.WriteLine($"Etat du programme:{espace}");
            Console.WriteLine("-------------------------------"); //on pourra rajouter un timer de temps restant
            Console.Write("Complétion : ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{pourcentage}%\n");
            Console.ResetColor();
            Console.Write("Temps écoulé : ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{stopwatch.Elapsed}");
            Console.ResetColor();
            Console.WriteLine("-------------------------------");
        }
        /// <summary>
        /// Fonction qui permet de dessiner un trait vertical d'un point x, à un point y.
        /// </summary>
        /// <param name="x">Coordonnées en x sur l'axe des abcisses</param>
        /// <param name="y">Coordonnées en y sur l'axe des ordonnées</param>
        public static void DrawStraigthLine(Pixel[,] mat_pixel, int x, int y, string couleur)
        {
            for (int i = 0; i < y; i++)
            {
                if (couleur == "r") mat_pixel[i, x] = new Pixel(0, 0, 255);
                if (couleur == "g") mat_pixel[i, x] = new Pixel(0, 255, 0);
                if (couleur == "b") mat_pixel[i, x] = new Pixel(255, 0, 0);
            }
        }
        /// <summary>
        /// Fonction qui permet de combiner les couleurs de trois matrice en une.
        /// <para>Les matrices doivent avoir EXACTEMENT les mêmes dimensions !</para>
        /// </summary>
        /// <param name="matriceR"></param>
        /// <param name="matriceG"></param>
        /// <param name="matriceB"></param>
        /// <returns></returns>
        public static Pixel[,] Merge(Pixel[,] matriceR, Pixel[,] matriceG, Pixel[,] matriceB)
        {
            Pixel[,] new_mat = new Pixel[matriceR.GetLength(0), matriceR.GetLength(1)];
            for (int ligne = 0; ligne < matriceR.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < matriceR.GetLength(1); colonne++)
                {
                    new_mat[ligne, colonne] = new Pixel(matriceB[ligne, colonne].Blue, matriceG[ligne, colonne].Green, matriceR[ligne, colonne].Red);
                }
            }
            return new_mat;
        }
        public static string DecimalToBase(int num, int basee)
        {
            int num2 = Convert.ToInt32(num);
            string res = "";
            int quotient = -1;

            while (quotient != 0)
            {
                quotient = num2 / basee;
                int reste = num2 % basee;
                res += Convert.ToString(reste);

                num2 = quotient;
            }
            string res2 = "";
            for (int i = 0; i < res.Length; i++)
            {
                res2 += res[res.Length - 1 - i];
            }
            return res2;



        }
        public static string BaseToDecimal( string chiffre, int basee)
        {
            string a = Convert.ToString(chiffre);
            int res = 0;
            for (int i = 0; i < a.Length; i++)
            {
                string s = Convert.ToString(a[a.Length - 1 - i]);
                res += Convert.ToInt32(s) * (int)Math.Pow(basee, i);
            }
            return Convert.ToString(res);
        }
        public static string FromBaseXToY(string nb,int x, int y)
        {
            string a = BaseToDecimal(nb, x);
            string b = DecimalToBase(Convert.ToInt32(a), y);
            return b;
        }
        public static bool InAlphaNum(string chaine)
        {
            bool res = true;
            for (int i = 0; i < chaine.Length; i++)
            {
                int cpt = 0;
                for (int j = 0; j < ListeLettres.Length; j++)
                {
                    if (chaine[i] == ListeLettres[j])
                    {
                        cpt++;
                    }

                }
                if (cpt != 1)
                {
                    res = false;
                }
            }
            return res;
        }
        public static int FromCharToAlphNum(char a)
        {
            int res = -1;
            if (InAlphaNum(Convert.ToString(a)))
            {
                for (int j = 0; j < ListeLettres.Length; j++)
                {
                    if (a == ListeLettres[j])
                    {
                        res = j;
                    }
                }
            }
            return res;
        }
        public static string FromStringToBinary11(string chaine)
        {
            bool paire = true;
            string ValueFinale = "";
            if (chaine.Length % 2 == 1)
            {
                paire = false;
            }
            if (paire == true)
            {
                for (int i = 0; i < chaine.Length - 1; i += 2)
                {
                    char valfort = chaine[i]; //Valfort première lettre, ex : "HE" H valeur forte et E valeur faible
                    char valfaible = chaine[i + 1]; //Valfaible deuxième lettre 

                    int res = FromCharToAlphNum(valfaible) + 45 * FromCharToAlphNum(valfort); //On fait l'opération : (45^0)*LettreFaible + (45^1)*LettreForte
                    string resstring = FromBaseXToY(Convert.ToString(res), 10, 2); //On convertit celle valeur décimale en binaire (comme dans l'énoncé)
                    resstring = resstring.PadLeft(11, '0'); //On est en valeur paire on doit remplir de 0 jusqu'à 11 caractères 
                    ValueFinale += resstring + ' ';

                }
            }
            else
            {

                for (int i = 0; i < chaine.Length - 2; i += 2)
                {
                    char valfort = chaine[i];
                    char valfaible = chaine[i + 1];

                    int res = FromCharToAlphNum(valfaible) + 45 * FromCharToAlphNum(valfort); //Même principe que le dernier 
                    string resstring = FromBaseXToY(Convert.ToString(res), 10, 2);
                    resstring = resstring.PadLeft(11, '0');
                    ValueFinale += resstring + ' ';

                }
                int res2 = FromCharToAlphNum(chaine[chaine.Length - 1]);
                string res2string = FromBaseXToY(Convert.ToString(res2), 10, 2);
                res2string = res2string.PadLeft(6, '0');



                ValueFinale += res2string + " ";

            }

            return ValueFinale;
        }
        /// <summary>
        /// Transforme une séquence binaire de 11 bits (et 6 bits dans le cas impair pour le dernier)
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns></returns>
        public static string FromBinary11ToString(string chaine)
        {
            
            bool paire = true;
            string ValueFinale = "";
            string[] chaineBinaire = chaine.Split(' ');
            for (int i = 0; i < chaineBinaire.Length - 1; i++)
            {
                int valeurDecimal = Convert.ToInt32(FromBaseXToY(chaineBinaire[i], 2, 10));
                int lettreFaibleDecimal = valeurDecimal % 45;
                int lettreForteDecimal = (valeurDecimal - lettreFaibleDecimal) / 45;

                char lettreFaible = ListeLettres[lettreFaibleDecimal];
                char lettreForte = ListeLettres[lettreForteDecimal];
                if (lettreForteDecimal==0 && lettreFaibleDecimal != 0 ) //Si on a une chaine impaire ET qu'on est à la dernière lettre. Alors forcément il y aura un vide qu'on ne doit pas prendre
                { //et on considère la lettre faible comme étant la lettre forte 
                    ValueFinale += Convert.ToString(lettreFaible);
                }
                else //Sinon, on fait l'habituel 
                {
                    ValueFinale += Convert.ToString(lettreForte) + Convert.ToString(lettreFaible);
                }
            }
            return ValueFinale;
        }
        public static string Nbrcaractere9bits(string mot)
        {
            int taille = mot.Length;
            string taille9bits = FromBaseXToY(Convert.ToString(taille), 10, 2);
            taille9bits = taille9bits.PadLeft(9, '0');
            return taille9bits;
        }
        public static int CalculsNbrBits(string mot)
        {
            string[] tableau = mot.Split(' ');
            int compteur = 0;
            for (int i = 0; i < tableau.Length; i++)
            {
                int taille = tableau[i].Length;
                compteur += taille;
            }
            return compteur;
        }
        #endregion
    }
        
}
