using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Problème_scientifique___Image
{
    class BuddhabrotMultiThreading
    {
        #region Variables
        //private double Xmin = -2;
        //private double Xmax = 2;
        //private double Ymin = -2;
        //private double Ymax = 2;

        double Xmin = -2.25;
        double Xmax = 1.25;
        double Ymin = -1.75;
        double Ymax = 1.75;

        private int[][,] HitsMatrix; //On créé un tableau de matrice qui contiendra n matrice où n sera le nombre de threads 
        private int[,] Hits;
        private int[,] HitsRed;
        private int[,] HitsGreen;
        private int[,] HitsBlue;

        private long HitsMax;
        private long TotalHits;
        
        private int Taille;
        private int Iteration;
        private int IterationRed;
        private int IterationGreen;
        private int IterationBlue;

        private bool IsMonochrome = true;
        private int PourcentageCompleted => (int)(((double)TotalHits/HitsMax)*100); //Donne un pourcentage de completion de Buddhabrot par passage
        //Trop bien! Fonction lambda qui permet de calculer une variable dynamiquement avec une formule
        private bool Completed = false;
        private Random rand;
        private Stopwatch stopwatch = new Stopwatch();
        #endregion

        public BuddhabrotMultiThreading(int taille, int iteration, long hitsMax)
        {
            if (taille <= 0)
            {
                throw new ArgumentException("Taille doit être plus grand que 0");
            }
            this.Taille = taille;
            this.Iteration = iteration;
            this.HitsMax = hitsMax;
            this.Hits = new int[taille, taille];
            this.rand = new Random();
        }
        public BuddhabrotMultiThreading(int taille, int iteration, long hitsMax, int IterationRed, int IterationGreen, int IterationBlue)
        {
            if (taille <= 0)
            {
                throw new ArgumentException("Taille doit être plus grand que 0");
            }
            this.Taille = taille;
            this.Iteration = iteration;
            this.HitsMax = hitsMax;
            this.Hits = new int[taille, taille];
            this.HitsRed = new int[taille, taille];
            this.HitsGreen = new int[taille, taille];
            this.HitsBlue = new int[taille, taille];
            this.rand = new Random();
            this.stopwatch.Start();

            this.IterationRed = IterationRed;
            this.IterationGreen = IterationGreen;
            this.IterationBlue = IterationBlue;

            this.IsMonochrome = false;
        } 

        #region Méthodes
        public void DrawBuddhabrot()
        {
            MyImage nouvelle_image = new MyImage("fractale_Buddhabrot", this.Taille, this.Taille); ;
            nouvelle_image.Hauteur = this.Taille;
            nouvelle_image.Largeur = this.Taille;

            nouvelle_image.Taille_fichier = nouvelle_image.Nb_bit * nouvelle_image.Hauteur * nouvelle_image.Largeur;
            nouvelle_image.FileSet = new byte[nouvelle_image.Taille_offset + ((nouvelle_image.Largeur * 3) * (nouvelle_image.Hauteur))];
            
            this.Run();
            Pixel[,] new_mat = this.Draw();
            nouvelle_image.Mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{nouvelle_image.Filename}", true);
            Console.WriteLine("Opération complétée.");
        }
        public void Run()
        {
            long TotalHits_avant = 0;
            do
            {
                Methods.StatusUpdate(this.PourcentageCompleted, stopwatch);
                if (this.IsMonochrome)
                {
                    CreationOrbite();
                }
                else CreationOrbiteCouleur();
                if ((TotalHits*1d - TotalHits_avant*1d) / HitsMax > 1 / 100d)
                {
                    TotalHits_avant = TotalHits;
                }
            }
            while (TotalHits < HitsMax);
            this.Completed = true;
        }
        private void CreationOrbiteCouleur()
        {
            int taille = this.Taille;
            int SeuilDivergence = 2;
            int iterationMax = Math.Max(Math.Max(this.IterationRed, this.IterationGreen), this.IterationBlue);
            bool diverge = false;

            double EchelleX = (Xmax - Xmin) / taille;
            double EchelleY = (Ymax - Ymin) / taille;

            double c_r = rand.NextDouble() * (Xmax - Xmin) + Xmin;
            double c_i = rand.NextDouble() * (Ymax - Ymin) + Ymin;

            //if (IsInCardiod(c_r, c_i) || IsInBulb(c_r, c_i))
            //{
            //    return this.TotalHits;
            //}
            //Initialisation des complexes 
            Complexe c = new Complexe(c_r, c_i);
            Complexe z = new Complexe(c_r, c_i);
            //Algo de mendelbrot classique, sauf que plutôt de considérer les points qui ne diverge pas, on considère ceux qui diverge
            for (int i = 0; i < this.Iteration && !diverge; i++)
            {
                z = z * z + c;
                if (z.Module() > SeuilDivergence)
                {
                    diverge = true;
                }
            }

            if (diverge)
            {
                //Si le point qu'on a choisi diverge, alors on veut pouvoir le réutiliser. Donc on réaffecte la valeur 
                z = new Complexe(c_r, c_i);
                //On va créé une "heatmap" qui comptabilise où les points qui diverge "tape" dans notre matrice de pixel. Cela représente la fréquence probabiliste des points de divergence de la fractale de Mendelbrot
                for (int i = 0; i < this.Iteration; i++) //le pb ici c'est que les couleurs marchent à moitié car on fait qu'une seule passe. et donc iteration peut avoir une valeur petite comme 20, donc on peut pas check
                { //Correctement les i < this.IterationBlue qui ont des valeurs très élevées (5000 genre...)
                    
                    
                    int j = 0;
                    List<(int, int)> tempo_pixels = new List<(int, int)>(); //On utilise ce qu'on appelle des tuples pour stocker les coordonnées
                    do
                    {
                        z = z * z + c;
                        int reelAjuste = (int)Math.Round((z.Reel - Xmin) / EchelleX);
                        int imgAjuste = (int)Math.Round((z.Img - Ymin) / EchelleY);
                        j++;
                        tempo_pixels.Add((imgAjuste, reelAjuste)); //On ajoute les coordonnées du pixels réajuster à notre repère
                    } while (z.Module() < 2 && j < iterationMax);

                    if (j < this.IterationBlue) 
                    {
                        foreach ((int, int) pixel in tempo_pixels)
                        {
                            if (pixel.Item1 >= 0 && pixel.Item1 < taille && pixel.Item2 >= 0 && pixel.Item2 < taille)
                            {
                                this.HitsBlue[pixel.Item1, pixel.Item2]++;
                                this.TotalHits++;
                            }
                        }
                    }
                    if (j < this.IterationGreen)
                    {
                        foreach ((int, int) pixel in tempo_pixels)
                        {
                            if (pixel.Item1 >= 0 && pixel.Item1 < taille && pixel.Item2 >= 0 && pixel.Item2 < taille)
                            {
                                this.HitsGreen[pixel.Item1, pixel.Item2]++;
                                this.TotalHits++;
                            }
                        }
                    }
                    if (j < this.IterationRed)
                    {
                        foreach ((int, int) pixel in tempo_pixels)
                        {
                            if (pixel.Item1 >= 0 && pixel.Item1 < taille && pixel.Item2 >= 0 && pixel.Item2 < taille)
                            {
                                this.HitsRed[pixel.Item1, pixel.Item2]++;
                                this.TotalHits++;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Fonction qui itère l'algorithme de Mendelbrot et note dans une matrice int[,] heatmap les points qui diverge
        /// </summary>
        /// <returns></returns>
        private void CreationOrbite()
        {
            int taille = this.Taille;
            int SeuilDivergence = 2;

            bool diverge = false;
            bool divergeTropLoin = false;

            double EchelleX = (Xmax - Xmin) / taille;
            double EchelleY = (Ymax - Ymin) / taille;

            double c_r = rand.NextDouble() * (Xmax - Xmin) + Xmin;
            double c_i = rand.NextDouble() * (Ymax - Ymin) + Ymin;

            //if (IsInCardiod(c_r, c_i) || IsInBulb(c_r, c_i))
            //{
            //    return this.TotalHits;
            //}
            //Initialisation des complexes 
            Complexe c = new Complexe(c_r, c_i);
            Complexe z = new Complexe(c_r, c_i);
            //Algo de mendelbrot classique, sauf que plutôt de considérer les points qui ne diverge pas, on considère ceux qui diverge
            for (int i = 0; i < this.Iteration && !diverge; i++)
            {
                z = z * z + c;
                if (z.Module() > SeuilDivergence)
                {
                    diverge = true;
                }
            }
            
            if (diverge)
            {
                //Si le point qu'on a choisi diverge, alors on veut pouvoir le réutiliser. Donc on réaffecte la valeur 
                z = new Complexe(c_r, c_i);

                //On va créé une "heatmap" qui comptabilise où les points qui diverge "tape" dans notre matrice de pixel. Cela représente la fréquence probabiliste des points de divergence de la fractale de Mendelbrot
                for (int i = 0; i < this.Iteration && !divergeTropLoin; i++) //le pb ici c'est que les couleurs marchent à moitié car on fait qu'une seule passe. et donc iteration peut avoir une valeur petite comme 20, donc on peut pas check
                { //Correctement les i < this.IterationBlue qui ont des valeurs très élevées (5000 genre...)
                    int x = (int)Math.Round((z.Reel - Xmin) / EchelleX);
                    int y = (int)Math.Round((z.Img - Ymin) / EchelleY);
                    if (this.IsMonochrome)
                    {
                        if (x >= 0 && x < taille && y >= 0 && y < taille)
                        {
                            this.Hits[y, x]++;
                            this.TotalHits++;
                        }
                    }
                    z = z * z + c;
                    if (z.Module() >= SeuilDivergence * 10) divergeTropLoin = true;
                }
            }
            //return TotalHits;
        }
        /// <summary>
        /// Fonction qui permet de "dessiner" la fractale une fois qu'on a fini de déterminer les hitmarker de chaque pixels
        /// Retourne Pixel[,] new_mat
        /// </summary>
        /// <param name="hits">Heat map des pixels </param>
        /// <param name="taille">Taille de l'image (en pixels)</param>
        /// <returns></returns>
        public Pixel[,] Draw()
        {
            double max = double.MinValue; //On initialise à une valeur extraordinairement petite pour pouvoir la comparer plus tard
            double maxR = double.MinValue;
            double maxG = double.MinValue;
            double maxB = double.MinValue;
            for(int ligne = 0; ligne < this.Taille; ligne++) //on détermine quelle est la "luminosité maximale"
            {
                for (int colonne = 0; colonne < this.Taille; colonne++)
                {
                    if (this.Hits[ligne, colonne] > max) max = this.Hits[ligne, colonne];
                    if (HitsRed[ligne, colonne] > maxR) maxR = HitsRed[ligne, colonne];
                    if (HitsGreen[ligne, colonne] > maxG) maxG = HitsGreen[ligne, colonne];
                    if (HitsBlue[ligne, colonne] > maxB) maxB = HitsBlue[ligne, colonne];
                }
            }
            if (max <= 0) max = 1; //On veut pas diviser par zéro 
            if (maxR <= 0) maxR = 1;
            if (maxG <= 0) maxG = 1;
            if (maxB <= 0) maxB = 1;
            double scale = 255d / max;
            double scaleR = 255d / maxR;
            double scaleG = 255d / maxG;
            double scaleB = 255d / maxB;
            Pixel[,] new_mat = new Pixel[this.Taille, this.Taille];
            for (int ligne = 0; ligne < this.Taille; ligne++)
            {
                for (int colonne = 0; colonne < this.Taille; colonne++)
                {
                    if (this.IsMonochrome)
                    {
                        int pixel = (int)Math.Min(Math.Round(this.Hits[ligne, colonne] * scale), 255);
                        new_mat[ligne, colonne] = new Pixel(pixel, pixel, pixel); //ça va nous donner des nuances de gris, on fera la couleur plus tard
                    }
                    else
                    {
                        int pixelR = (int)Math.Min(Math.Round(HitsRed[ligne, colonne] * scaleR), 255);
                        int pixelG = (int)Math.Min(Math.Round(HitsGreen[ligne, colonne] * scaleG), 255);
                        int pixelB = (int)Math.Min(Math.Round(HitsBlue[ligne, colonne] * scaleB), 255);
                        new_mat[ligne, colonne] = new Pixel(pixelB, pixelG, pixelR);
                    }
                }
            }
            return new_mat;
        }
        public static bool IsInCardiod(double c_r, double c_i)
        {
            double c_iSquared = c_i * c_i;
            double c_rQuarter = c_r - 0.25;
            double result = c_rQuarter * c_rQuarter + c_iSquared;
            return result * (result + c_rQuarter) < 0.25 * c_iSquared;
        }
        public static bool IsInBulb(double c_r, double c_i)
        {
            return (c_r + 1) * (c_r + 1) + c_i * c_i < 0.0625;
        }
        #endregion
    }
}
