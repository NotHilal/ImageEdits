using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Problème_scientifique___Image
{
    public class BuddhabrotMultiThreading
    {
        #region Variables
        //private readonly double Xmin = -2;
        //private readonly double Xmax = 2;
        //private readonly double Ymin = -2;
        //private readonly double Ymax = 2;

        private readonly double Xmin = -1.75;
        private readonly double Xmax = 1.75;
        private readonly double Ymin = -1.75;
        private readonly double Ymax = 1.75;

        private readonly int threadCount = Environment.ProcessorCount;
        private volatile bool running = true;

        private int[][,] HitsThreads; //On créé un tableau de matrice qui contiendra n matrice où n sera le nombre de threads 
        private int[][,] HitsThreadRed;
        private int[][,] HitsThreadGreen;
        private int[][,] HitsThreadBlue;

        //Ce sont les matrices de hits finaux, où on additionne toutes les itérations des HitsThreads
        private int[,] Hits;
        private int[,] HitsRed;
        private int[,] HitsGreen;
        private int[,] HitsBlue;

        private readonly long HitsMax;
        private long TotalHits;
        
        private int Taille;
        private int Iteration;
        private int IterationRed;
        private int IterationGreen;
        private int IterationBlue;

        private bool IsMonochrome = true;
        private int PourcentageCompleted => (int)(((double)TotalHits/HitsMax)*100); //Donne un pourcentage de completion de Buddhabrot par passage
        //Trop bien! Fonction lambda qui permet de calculer une variable dynamiquement avec une formule
        private readonly Stopwatch stopwatch = new Stopwatch();
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
            this.stopwatch.Start();

            this.HitsThreads = new int[threadCount][,]; //on définit un tableau de matrice qui va exécuter parallèlement Run pour chaque thread. Et on va additionner à la fin tous les résultats de ces matrices 
            for (int i = 0; i < threadCount; i++)
            {
                this.HitsThreads[i] = new int[taille, taille];
            }
        }
        public BuddhabrotMultiThreading(int taille, int iteration, long hitsMax, int IterationRed, int IterationGreen, int IterationBlue)
        {
            if (taille <= 0)
            {
                throw new ArgumentException("Taille doit être plus grand que 0");
            }
            if (IterationRed == 0) IterationRed = 10;
            if (IterationGreen == 0) IterationGreen = 10;
            if (IterationBlue == 0) IterationBlue = 10;
            this.Taille = taille;
            this.Iteration = iteration;
            this.HitsMax = hitsMax;

            this.HitsRed = new int[taille, taille];
            this.HitsGreen = new int[taille, taille];
            this.HitsBlue = new int[taille, taille];

            this.stopwatch.Start();

            this.HitsThreadRed = new int[threadCount][,]; //on définit un tableau de matrice qui va exécuter parallèlement Run pour chaque thread. Et on va additionner à la fin tous les résultats de ces matrices 
            this.HitsThreadGreen = new int[threadCount][,];
            this.HitsThreadBlue = new int[threadCount][,];
            for (int i = 0; i < threadCount; i++)
            {
                this.HitsThreadRed[i] = new int[taille, taille];
                this.HitsThreadGreen[i] = new int[taille, taille];
                this.HitsThreadBlue[i] = new int[taille, taille];
            }

            this.IterationRed = IterationRed;
            this.IterationGreen = IterationGreen;
            this.IterationBlue = IterationBlue;

            this.IsMonochrome = false;
        } 

        #region Méthodes

        /// <summary>
        /// Permet démarrer la génération de l'image Buddhabrot
        /// </summary>
        public void DrawBuddhabrot(string time = "")
        {
            MyImage nouvelle_image = new MyImage($"fractale_Buddhabrot{time}", this.Taille, this.Taille); ;
            nouvelle_image.Hauteur = this.Taille;
            nouvelle_image.Largeur = this.Taille;

            nouvelle_image.Taille_fichier = nouvelle_image.Nb_bit * nouvelle_image.Hauteur * nouvelle_image.Largeur;
            nouvelle_image.FileSet = new byte[nouvelle_image.Taille_offset + ((nouvelle_image.Largeur * 3) + nouvelle_image.Octet_manquant) * nouvelle_image.Hauteur];

            this.Run();
            this.AddMatrix();
            Pixel[,] new_mat = this.Draw();
            nouvelle_image.Mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{nouvelle_image.Filename}", true);
            Console.WriteLine("Opération complétée.");
        }
        /// <summary>
        /// Execute de manière parallèle la fonction "CreationOrbite" ou "CreationOrbiteCouleur" sur tous les coeurs d'un PC qui permet de créer la heatmap en simultanée.
        /// <para>à titre de comparaison, AMD Ryzen 7 3800X 8-Core (donc 16 threads) la "CreationOrbite" met 16 secondes à généré.</para>
        /// </summary>
        public void Run()
        {
            var taches = Enumerable.Range(0, threadCount).Select(thread => Task.Run(() =>
            {
                if (this.IsMonochrome) CreationOrbite(HitsThreads[thread], new Random());
                else CreationOrbiteCouleur(HitsThreadRed[thread], HitsThreadGreen[thread], HitsThreadBlue[thread], new Random(thread), thread);
            })).ToArray(); //on définit une variable IEnumerable<task> qui va executer la fonction "CreationOrbite" (ou couleur if !this.IsMonochrome) sur plusieurs threads

            Task.WaitAll(taches); //On attend avant d'actualiser que tous les threads ont fini d'executer la fonction
        }
        /// <summary>
        /// Fonction qui itère l'algorithme de mandelbrot et note dans une matrice int[,] heatmap les points qui diverge
        /// </summary>
        /// <returns></returns>
        private void CreationOrbite(int[,] hitsThread, Random random)
        {
            long n = 0;
            while (running)
            {
                int taille = this.Taille;
                int SeuilDivergence = 2;

                bool diverge = false;
                bool divergeTropLoin = false;

                double EchelleX = (Xmax - Xmin) / taille;
                double EchelleY = (Ymax - Ymin) / taille;

                double c_r = random.NextDouble() * 2 * (Xmax - Xmin) + Xmin;
                double c_i = random.NextDouble() * 2 * (Ymax - Ymin) + Ymin;

                if (!IsInCardiod(c_r, c_i) && !IsInBulb(c_r, c_i))
                {
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
                            if (x >= 0 && x < taille && y >= 0 && y < taille)
                            {
                                hitsThread[y, x]++;
                            }
                            z = z * z + c;
                            if (z.Module() >= SeuilDivergence * 7) divergeTropLoin = true;
                        }
                        n++;
                        
                        if(n % 10000000 == 0) //Evite d'avoir une écriture sur la même variable en même temps. Absolument nécessaire sinon le programme ne fonctionne pas
                        {
                            //Methods.StatusUpdate(this.PourcentageCompleted, stopwatch);
                            Console.WriteLine("Calculated {0:0.0} Million samples in {1:0.000} sec", TotalHits / 1000000.0, stopwatch.ElapsedMilliseconds / 1000.0);

                            Interlocked.Add(ref this.TotalHits, n);
                            if (this.TotalHits >= this.HitsMax) this.running = false;
                        }
                    }
                }   
            }
        }
        /// <summary>
        /// Fonction qui itère l'algorithme de mandelbrot et note dans trois matrices int[,] les points qui diverge. On utilise une technique 
        /// de coloration qui s'appelle "False Colors" voir https://en.wikipedia.org/wiki/False_color
        /// </summary>
        /// <returns></returns>
        private void CreationOrbiteCouleur(int[,] hitsThreadRed, int[,] hitsThreadGreen, int[,] hitsThreadBlue, Random random, int thread)
        {
            long n = 0;
            int taille = this.Taille;
            int SeuilDivergence = 2;
            int iterationMax = Math.Max(Math.Max(this.IterationRed, this.IterationGreen), this.IterationBlue);
            double EchelleX = (Xmax - Xmin) / taille;
            double EchelleY = (Ymax - Ymin) / taille;
            while (running)
            {              
                bool diverge = false;
                bool divergeTropLoin = false;
                long passe = 0;
                double c_r = random.NextDouble() * (Xmax - Xmin) + Xmin;
                double c_i = random.NextDouble() * (Ymax - Ymin) + Ymin;

                if (!IsInCardiod(c_r, c_i) && !IsInBulb(c_r, c_i))
                {
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
                        for (int i = 0; i < this.Iteration && !divergeTropLoin; i++, passe++) 
                        { 

                            int x = (int)Math.Round((z.Reel - Xmin) / EchelleX);
                            int y = (int)Math.Round((z.Img - Ymin) / EchelleY);

                            if (passe < this.IterationRed)
                            {
                                if (x >= 0 && x < taille && y >= 0 && y < taille)
                                {
                                    hitsThreadRed[y, x]++;
                                }
                            }
                            if (passe < this.IterationGreen)
                            {
                                if (x >= 0 && x < taille && y >= 0 && y < taille)
                                {
                                    hitsThreadGreen[y, x]++;
                                }
                            }
                            if (passe < this.IterationBlue)
                            {
                                if (x >= 0 && x < taille && y >= 0 && y < taille)
                                {
                                    hitsThreadBlue[y, x]++;
                                }
                            }
                            z = z * z + c;
                            if (z.Module() >= SeuilDivergence * 7) divergeTropLoin = true;
                        }

                        n++;
                        if (n % 10000000 == 0)
                        {
                            //Methods.StatusUpdate(this.PourcentageCompleted, stopwatch);
                            Console.WriteLine("Calculated {0:0.0} Million samples in {1:0.000} sec", TotalHits / 1000000.0, stopwatch.ElapsedMilliseconds / 1000.0);

                            Interlocked.Add(ref this.TotalHits, n);
                            if (this.TotalHits >= this.HitsMax) this.running = false;
                        }
                    }
                }
            }
        }
        //private void CreationOrbiteCouleur(int[,] hitsThreadRed, int[,] hitsThreadGreen, int[,] hitsThreadBlue, Random random)
        //{
        //    int taille = this.Taille;
        //    int SeuilDivergence = 2;
        //    int iterationMax = Math.Max(Math.Max(this.IterationRed, this.IterationGreen), this.IterationBlue);
        //    bool diverge = false;

        //    double EchelleX = (Xmax - Xmin) / taille;
        //    double EchelleY = (Ymax - Ymin) / taille;

        //    double c_r = random.NextDouble() * (Xmax - Xmin) + Xmin;
        //    double c_i = random.NextDouble() * (Ymax - Ymin) + Ymin;

        //    //Initialisation des complexes 
        //    Complexe c = new Complexe(c_r, c_i);
        //    Complexe z = new Complexe(c_r, c_i);
        //    //Algo de mendelbrot classique, sauf que plutôt de considérer les points qui ne diverge pas, on considère ceux qui diverge
        //    for (int i = 0; i < this.Iteration && !diverge; i++)
        //    {
        //        z = z * z + c;
        //        if (z.Module() > SeuilDivergence)
        //        {
        //            diverge = true;
        //        }
        //    }

        //    if (diverge)
        //    {
        //        //Si le point qu'on a choisi diverge, alors on veut pouvoir le réutiliser. Donc on réaffecte la valeur 
        //        z = new Complexe(c_r, c_i);
        //        //On va créé une "heatmap" qui comptabilise où les points qui diverge "tape" dans notre matrice de pixel. Cela représente la fréquence probabiliste des points de divergence de la fractale de Mendelbrot
        //        for (int i = 0; i < this.Iteration; i++) //le pb ici c'est que les couleurs marchent à moitié car on fait qu'une seule passe. et donc iteration peut avoir une valeur petite comme 20, donc on peut pas check
        //        { //Correctement les i < this.IterationBlue qui ont des valeurs très élevées (5000 genre...)


        //            int j = 0;
        //            List<(int, int)> tempo_pixels = new List<(int, int)>(); //On utilise ce qu'on appelle des tuples pour stocker les coordonnées
        //            do
        //            {
        //                z = z * z + c;
        //                int reelAjuste = (int)Math.Round((z.Reel - Xmin) / EchelleX);
        //                int imgAjuste = (int)Math.Round((z.Img - Ymin) / EchelleY);
        //                j++;
        //                tempo_pixels.Add((imgAjuste, reelAjuste)); //On ajoute les coordonnées du pixels réajuster à notre repère
        //            } while (z.Module() < 2 && j < iterationMax);

        //            if (j < this.IterationBlue)
        //            {
        //                foreach ((int, int) pixel in tempo_pixels)
        //                {
        //                    if (pixel.Item1 >= 0 && pixel.Item1 < taille && pixel.Item2 >= 0 && pixel.Item2 < taille)
        //                    {
        //                        hitsThreadBlue[pixel.Item1, pixel.Item2]++;
        //                        Interlocked.Increment(ref this.TotalHits);
        //                    }
        //                }
        //            }
        //            if (j < this.IterationGreen)
        //            {
        //                foreach ((int, int) pixel in tempo_pixels)
        //                {
        //                    if (pixel.Item1 >= 0 && pixel.Item1 < taille && pixel.Item2 >= 0 && pixel.Item2 < taille)
        //                    {
        //                        hitsThreadGreen[pixel.Item1, pixel.Item2]++;
        //                        Interlocked.Increment(ref this.TotalHits);
        //                    }
        //                }
        //            }
        //            if (j < this.IterationRed)
        //            {
        //                foreach ((int, int) pixel in tempo_pixels)
        //                {
        //                    if (pixel.Item1 >= 0 && pixel.Item1 < taille && pixel.Item2 >= 0 && pixel.Item2 < taille)
        //                    {
        //                        hitsThreadRed[pixel.Item1, pixel.Item2]++;
        //                        Interlocked.Increment(ref this.TotalHits);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        private void AddMatrix()
        {
            for (int i = 0; i < this.threadCount; i++)
            {
                for (int ligne = 0; ligne < this.Taille; ligne++)
                {
                    //Eviter de travailler sur des variables communes -> instancie des variables pour chaque thread et ajouter pour faire une variable finale
                    Parallel.For(0, this.Taille, colonne =>
                    {
                        if (this.IsMonochrome)
                        {
                            Interlocked.Add(ref this.Hits[ligne, colonne], this.HitsThreads[i][ligne, colonne]);
                        }
                        else
                        {
                            Interlocked.Add(ref this.HitsRed[ligne, colonne], this.HitsThreadRed[i][ligne, colonne]);
                            Interlocked.Add(ref this.HitsGreen[ligne, colonne], this.HitsThreadGreen[i][ligne, colonne]);
                            Interlocked.Add(ref this.HitsBlue[ligne, colonne], this.HitsThreadBlue[i][ligne, colonne]);
                        }
                    });
                }
            }
        }
        /// <summary>
        /// Fonction qui permet de "dessiner" la fractale une fois qu'on a fini de déterminer les hitmarker de chaque complexes
        /// </summary>
        /// <param name="hits">Heat map des pixels </param>
        /// <param name="taille">Taille de l'image (en pixels)</param>
        /// <returns>Pixel[,] new_mat</returns>
        public Pixel[,] Draw()
        {
            Pixel[,] new_mat = new Pixel[this.Taille, this.Taille];

            double limite = 0;
            double limiteR = 0;
            double limiteG = 0;
            double limiteB = 0;
            if (this.IsMonochrome) limite = GetNormalizer(this.Hits);
            else
            {
                limiteR = GetNormalizer(this.HitsRed);
                limiteG = GetNormalizer(this.HitsGreen);
                limiteB = GetNormalizer(this.HitsBlue);
            }
            for (int ligne = 0; ligne < this.Taille; ligne++)
            {
                for (int colonne = 0; colonne < this.Taille; colonne++)
                {
                    if (this.IsMonochrome)
                    {
                        int pixel = (int)Math.Round(((double)(this.Hits[ligne, colonne] / limite) * 256));
                        if (pixel > 255) pixel = 255;
                        //int pixel = (int)Math.Min(Math.Round(this.Hits[ligne, colonne] * scale), 255);
                        new_mat[ligne, colonne] = new Pixel(pixel, pixel, pixel); //ça va nous donner des nuances de gris, on fera la couleur plus tard
                    }
                    else
                    {
                        int pixelR = (int)Math.Round(((double)(this.HitsRed[ligne, colonne] / limiteR) * 256));
                        int pixelG = (int)Math.Round(((double)(this.HitsGreen[ligne, colonne] / limiteG) * 256));
                        int pixelB = (int)Math.Round(((double)(this.HitsBlue[ligne, colonne] / limiteB) * 256));
                        if (pixelR > 255) pixelR = 255;
                        if (pixelG > 255) pixelG = 255;
                        if (pixelB > 255) pixelB = 255;
                        new_mat[ligne, colonne] = new Pixel(pixelR, pixelG, pixelB);
                    }
                }
            }
            return new_mat;
        }
        /// <summary>
        /// Fontion qui permet de retourner la valeur des pixels 1% plus lumineux
        /// </summary>
        /// <param name="matrice">Matrice Hits (ou HitsRed, HitsGreen, HitsBlue...) dont lequel on va déterminer la valeur la plus élevée</param>
        /// <param name="sampleThreshold">Paramètre qui indique de récupérer les pixels les 1% plus lumineux</param>
        /// <param name="threshold">Paramètre pour retirer que les plus lumineux</param>
        /// <returns></returns>
        public static double GetNormalizer(int[,] matrice, double sampleThreshold = 0.01, double threshold = 0.9995)
        {
            Random rand = new Random();
            double[] matrice_totale = new double[matrice.Length];
            int sampleCount = (int)(matrice.Length * sampleThreshold);

            if (sampleCount < 1000) sampleCount = 1000;
            if (sampleCount > matrice_totale.Length) sampleCount = matrice_totale.Length;

            int compteur = 0;
            for (int ligne = 0; ligne < matrice.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < matrice.GetLength(1); colonne++, compteur++)
                {
                    matrice_totale[compteur] = matrice[ligne, colonne];
                }
            }
            double[] samples = Enumerable.Range(0, sampleCount)
                .Select(x => matrice_totale[rand.Next(0, matrice_totale.Length)])
                .OrderBy(x => x)
                .ToArray();
            double samplesThreshold = samples[(int)(samples.Length * 0.99)];
            List<double> brightSamples = new List<double>();
            for (int i = 0; i < matrice_totale.Length; i++)
            {
                var sample = matrice_totale[i];
                if (sample > samplesThreshold) brightSamples.Add(sample);
            }

            double[] values = brightSamples.OrderBy(x=> x).ToArray();
            int position = values.Length - (int)(values.Length * threshold);
            double limite = values[values.Length - position - 1];
            return limite;
        }
        /// <summary>
        /// Fonction qui permet de savoir si un nombre complexe est dans la cardioïde de Mandelbrot 
        /// </summary>
        /// <param name="c_r">Coefficient réel du nombre complexe</param>
        /// <param name="c_i">Coefficient imaginaire du nombre complexe</param>
        /// <returns></returns>
        public static bool IsInCardiod(double c_r, double c_i)
        {
            double c_iSquared = c_i * c_i;
            double c_rQuarter = c_r - 0.25;
            double result = c_rQuarter * c_rQuarter + c_iSquared;
            return result * (result + c_rQuarter) < 0.25 * c_iSquared;
        }
        /// <summary>
        /// Fonction qui permet de savoir is un nombre complexe est dans le bulbe de Mandelbrot
        /// </summary>
        /// <param name="c_r">Coefficient réel du nombre complexe</param>
        /// <param name="c_i">Coefficient imaginaire du nombre complexe</param>
        /// <returns></returns>
        public static bool IsInBulb(double c_r, double c_i)
        {
            return (c_r + 1) * (c_r + 1) + c_i * c_i < 0.0625;
        }
        #endregion
    }
}
