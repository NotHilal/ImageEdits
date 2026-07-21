using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace Problème_scientifique___Image
{
    public class Methods
    {
        #region Variables
        private static char[] ListeLettres = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/', ':' };
        #endregion
        //Cette classe à pour seul but de stocker les méthodes qui sont utilisées par les fonctions principales de traitement d'image
        #region Méthodes

        /// <summary>
        /// Fonction qui calcule la distance entre deux pixels
        /// </summary>
        /// <param name="pixel1">Instance de pixel1</param>
        /// <param name="pixel2">Instance de pixel2</param>
        /// <returns></returns>
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
        /// <summary>
        /// Fonction qui permet d'avoir un affichage dynamique de la complétion d'une fonction avec un timer de type de Stopwatch
        /// </summary>
        /// <param name="pourcentage">Pourcentage calculée au sein de la fonction/classe</param>
        /// <param name="stopwatch">Instance de stopwatch qui est démarrée au début de la fonction/classe</param>
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
        /// Fonction qui permet de dessiner un trait vertical ou vertical d'un point x, à un point y.
        /// </summary>
        /// <param name="x">Coordonnées en x sur l'axe des abcisses</param>
        /// <param name="y">Coordonnées en y sur l'axe des ordonnées</param>
        /// <param name="couleur">Couleur du trait</param>
        /// <param name="direction">Direction du trait, prend comme valeur "h" ou "v" ("h": horizontal, "v": vertical)</param>
        public static void DrawStraigthLine(Pixel[,] mat_pixel, int x, int y, string couleur, string direction = "v", int start = 0)
        {
            if (direction == "v")
            {
                for (int i = start; i < y + start - 1; i++)
                {
                    if (couleur == "r") mat_pixel[i, x] = new Pixel(255, 0, 0);
                    if (couleur == "g") mat_pixel[i, x] = new Pixel(0, 255, 0);
                    if (couleur == "b") mat_pixel[i, x] = new Pixel(0, 0, 255);
                    if (couleur == "w") mat_pixel[i, x] = new Pixel(255, 255, 255);
                    if (couleur == "alternance")
                    {
                        if (i % 2 == 0) mat_pixel[i, x] = new Pixel(0, 0, 0);
                        if (i % 2 != 0) mat_pixel[i, x] = new Pixel(255, 255, 255);
                    }
                }
            }
            else
            {
                for (int i = start; i < y + start - 1; i++)
                {
                    if (couleur == "r") mat_pixel[x, i] = new Pixel(255, 0, 0);
                    if (couleur == "g") mat_pixel[x, i] = new Pixel(0, 255, 0);
                    if (couleur == "b") mat_pixel[x, i] = new Pixel(0, 0, 255);
                    if (couleur == "w") mat_pixel[x, i] = new Pixel(255, 255, 255);
                    if (couleur == "alternance")
                    {
                        if (i % 2 == 0) mat_pixel[x, i] = new Pixel(0, 0, 0);
                        if (i % 2 != 0) mat_pixel[x, i] = new Pixel(255, 255, 255);
                    }
                }
            }
        }
        /// <summary>
        /// Fonction qui permet de créer un module pour la fonction "AddAlignmentPatterns"
        /// </summary>
        /// <param name="mat_pixel">Mettre "this.mat_pixel"</param>
        /// <param name="x">Coordonnée en X du centre du module</param>
        /// <param name="y">Coordonnée en Y du centre du module</param>
        public static void CreateModule(Pixel[,] mat_pixel, int x, int y)
        {
            int decrement = 0;
            for (int compteur = 0; compteur < 3; compteur++, decrement += 2) //On créé trois carré successivement en réduisant son côté de 2 à chaque fois pour faire un pattern : 5-3-1
            {
                for (int ligne = x - 2; ligne < 5 + x - decrement - 2; ligne++) //Création d'un carré de modules 
                {
                    for (int colonne = y - 2; colonne < 5 + y - decrement - 2; colonne++)
                    {
                        if (compteur % 2 == 0) //On est sur un carré noir
                        {
                            mat_pixel[ligne + compteur, colonne + compteur] = new Pixel(0, 0, 0); //Finder pattern en bas à gauche   
                        }
                        else //On est sur un carré blanc
                        {
                            mat_pixel[ligne + compteur, colonne + compteur] = new Pixel(255, 255, 255); //Finder pattern en bas à gauche                     
                        }
                    }
                }
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
        /// <summary>
        /// Fonction qui transforme un entier en base 10 dans la base X
        /// </summary>
        /// <param name="num"></param>
        /// <param name="basee"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Fonction qui transforme un nombre dans une base X en base 10
        /// </summary>
        /// <param name="chiffre"></param>
        /// <param name="basee"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Fonction qui transforme un entier de la base X à la base Y
        /// </summary>
        /// <param name="nb">Nombre qui va subir la transformation de base</param>
        /// <param name="x">base X de départ</param>
        /// <param name="y">base Y d'arrivée</param>
        /// <returns></returns>
        public static string FromBaseXToY(string nb,int x, int y)
        {
            string a = BaseToDecimal(nb, x);
            string b = DecimalToBase(Convert.ToInt32(a), y);
            return b;
        }
        /// <summary>
        /// Fonction qui vérifie qu'une chaîne de caractère ne contient que des caractères alphanumériques
        /// </summary>
        /// <param name="chaine">Chaine de caractère qui va être testée</param>
        /// <returns></returns>
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
        /// <summary>
        /// Convertie un char en format alphanumérique
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Fonction qui permet de revenir arrière dans le chemin du directory. Utiliser pour le form.
        /// </summary>
        /// <param name="noOfLevels"></param>
        /// <param name="currentpath"></param>
        /// <returns></returns>
        public static string MoveUp(string currentpath, int noOfLevels)
        {
            string parentPath = currentpath.TrimEnd(new[] { '/', '\\' });
            for (int i = 0; i < noOfLevels; i++)
            {
                parentPath = Directory.GetParent(parentPath).ToString();
            }
            return parentPath;
        }

        #endregion
    }
        
}
