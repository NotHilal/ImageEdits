using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Problème_scientifique___Image
{
    public class MyImage
    {
        #region Variable d'instance ou champ d'instance

        private byte[] file;
        private string filename;
        private string type_image;
        private int taille_fichier;
        private int taille_offset;
        private int hauteur;
        private int largeur;
        private int nb_bit;
        private int octetManquant;
        private Pixel[,] mat_pixel;

        #endregion
        public MyImage(string filename)
        {
            this.filename = filename;
            this.file = File.ReadAllBytes($"Fichiers\\{filename}.bmp");

            if (Convertir_Endian_To_Int(file[0]) != 66 || Convertir_Endian_To_Int(file[1]) != 77) //on vérifie que le fichier est bien un bitmap
            {
                Console.WriteLine("Veuillez entrer une image au format .bmp");
            }
            else
            {
                byte[] type_imageOctets = { file[0], file[1], 0, 0 }; //Correspond au type_image qu'on a, ici "BM" (à convertir en ASCII)
                this.type_image = Convert.ToString(Convert.ToChar(Convertir_Endian_To_Int(file[0]))) + Convert.ToString(Convert.ToChar(Convertir_Endian_To_Int(file[1])));

                byte[] taille_fichierOctets = { file[2], file[3], file[4], file[5] }; //La taille du fichier BMP en octets
                this.taille_fichier = Convertir_Endian_To_Int(taille_fichierOctets);

                byte[] taille_offsetOctets = { file[10], file[11], file[12], file[13] }; //Offset de l'image 
                this.taille_offset = Convertir_Endian_To_Int(taille_offsetOctets); //Taille offset correspond à quel bit commence l'image, ici généralement c'est 54 (taille header + header info)

                byte[] largeurOctets = { file[18], file[19], file[20], file[21] };
                this.largeur = Convertir_Endian_To_Int(largeurOctets);

                byte[] hauteurOctets = { file[22], file[23], file[24], file[25] };
                this.hauteur = Convertir_Endian_To_Int(hauteurOctets);

                byte[] nb_bitOctets = { file[28], file[29] };
                this.nb_bit = Convertir_Endian_To_Int(nb_bitOctets);

                this.mat_pixel = new Pixel[this.hauteur, this.largeur];
                #region Remplissage de la matrice pixel 
                int decalage = this.taille_offset;
                this.octetManquant = 0;
                if (largeur % 4 != 0)
                {
                    this.octetManquant = 4 - (largeur % 4);
                }
                for (int ligne = 0; ligne < this.hauteur; ligne++)
                {
                    for (int colonne = 0; colonne < this.largeur; colonne++, decalage += 3 + this.octetManquant)
                    {
                        this.mat_pixel[ligne, colonne] = new Pixel(file[decalage], file[decalage + 1], file[decalage + 2]);
                    }
                }
                #endregion
                //Affichage_byte(filename);
                //Affichage_pixel_image(mat_pixel);
            }
        }
        public MyImage(string filename, int hauteur, int largeur)
        {
            this.filename = filename;
            this.hauteur = hauteur;
            this.largeur = largeur;
            this.taille_offset = 54;
            this.octetManquant = (largeur * 3) % 4;
        }

        #region Propriétés
        public int Hauteur
        {
            get { return this.hauteur; }
            set { this.hauteur = value; }
        }
        public int Largeur
        {
            get { return this.largeur; }
            set { this.largeur = value; }
        }
        public int Taille_fichier
        {
            get { return this.taille_fichier; }
            set { this.taille_fichier = value; }
        }
        public int Taille_offset
        {
            get { return this.taille_offset; }
        }
        public int Nb_bit
        {
            get { return this.nb_bit; }
        }
        public string Filename
        {
            get { return this.filename; }
        }
        public byte[] FileSet
        {
            set { this.file = value; }
        }
        public Pixel[,] Mat_pixel
        {
            set { this.mat_pixel = value; }
        }
        #endregion

        #region Méthodes

        #region Outils
        public static void Affichage_image(string filename)
        {
            Process.Start($"Fichiers\\{filename}.bmp");
            Console.ReadLine();
        }
        public static void Affichage_pixel_image(Pixel[,] matrice_pixel)
        {
            for (int ligne = 0; ligne < matrice_pixel.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < matrice_pixel.GetLength(1); colonne++)
                {
                    Console.Write($"{matrice_pixel[ligne, colonne].Blue} {matrice_pixel[ligne, colonne].Green} {matrice_pixel[ligne, colonne].Red} ");
                }
                Console.WriteLine();
            }
        }
        public void Affichage_byte(string filename)
        {
            //Métadonnées du fichier
            Console.WriteLine("--HEADER--\n");
            for (int i = 0; i < 14; i++)
            {
                Console.Write($"{this.file[i]} ");
            }

            //Métadonnées de l'image
            Console.WriteLine("\n--HEADER INFO--\n");
            for (int i = 14; i < 54; i++)
            {
                Console.Write($"{this.file[i]} ");
            }
            //L'image elle-même
            Console.WriteLine("\n IMAGE \n");
            for (int i = 54; i < file.Length; i += this.largeur * 3) //On parcout les lignes, on fait +60 parce que l'image fait 20 pixel de longueur, donc 20*3 car 3 octets par pixel
            {
                for (int j = i; j < i + this.largeur * 3; j++) //On parcours les colonnes de la i-ième lignes (i + 60) cf plus haut  
                {
                    Console.Write($"{this.file[i]} ");
                }
                Console.WriteLine();
            }

            //File.WriteAllBytes($"./Images/{filename}.bmp", file); --Sert à écrire l'image une fois l'avoir lue

            Console.ReadLine();
        }
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            double result = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                result += tab[i] * Math.Pow(256, i);
            }
            return (int)result;
        }
        public int Convertir_Endian_To_Int(byte bits)
        {
            double result = bits;
            return (int)result;
        }
        public byte[] Convertir_Int_To_Endian(int val, int tailleEndian)
        {
            byte[] result = new byte[tailleEndian];
            int value = val;
            for (int i = 0; i < tailleEndian; i++)
            {
                result[i] = (byte)(value % 256);
                value /= 256;
            }
            return result;
        }
        public void From_Image_To_File(string image, bool fromScratch)
        {
            byte[] file_bon_format = new byte[this.file.Length];
            //On écrit les informations du header & header info dans l'ordre
            if (!Directory.Exists($"Fichiers"))
            {
                Directory.CreateDirectory($"Fichiers");
            }
            //try
            //{
                
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Erreur ! Exception : {e}");
            //}
            if (!fromScratch)
            {
                #region Remplissage fichier header & header info

                #region Type image 
                byte[] typethisOctet = new byte[2];
                for (int i = 0; i < 2; i++)
                {
                    switch (this.type_image[i])
                    {
                        case 'B':
                            typethisOctet[i] = 66;
                            break;
                        case 'M':
                            typethisOctet[i] = 77;
                            break;
                        default:
                            typethisOctet[i] = 0;
                            break;
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    file_bon_format[i] = typethisOctet[i];
                }
                #endregion

                #region Taille image en octets
                byte[] taillethisOctet = Convertir_Int_To_Endian(this.taille_fichier, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 2] = taillethisOctet[i];
                }
                #endregion

                #region Bits réservé 
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 6] = 0;
                }
                #endregion

                #region Offset en octets
                byte[] offsetthisOctet = Convertir_Int_To_Endian(this.taille_offset, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 10] = offsetthisOctet[i];
                }
                #endregion

                #region Taille header en octets
                file_bon_format[14] = 40;
                for (int i = 0; i < 3; i++)
                {
                    file_bon_format[i + 15] = 0;
                }
                #endregion

                #region Largeur image en octets
                byte[] largeurthisOctet = Convertir_Int_To_Endian(this.largeur, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 18] = largeurthisOctet[i];
                }
                #endregion

                #region Hauteur image en octets
                byte[] hauteurthisOctet = Convertir_Int_To_Endian(this.hauteur, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 22] = hauteurthisOctet[i];
                }
                #endregion

                #region Nombre de plan de couleur en octets
                file_bon_format[26] = 1;
                file_bon_format[27] = 0;
                #endregion

                #region Nb de bits par pixel en octets
                byte[] nbBitsthisOctet = Convertir_Int_To_Endian(this.nb_bit, 2);
                for (int i = 0; i < 2; i++)
                {
                    file_bon_format[i + 28] = nbBitsthisOctet[i];
                }
                #endregion

                #region Méthode de compression en octets
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 30] = 0;
                }
                #endregion

                #region Taille du header pur en octets
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 34] = 0;
                }
                #endregion

                #region Résolution horizontale en octets
                //taille du fichier - taille offset --> convertir en indian sur 4 octets
                byte[] resolutionHorizontaleOctet = Convertir_Int_To_Endian(this.taille_fichier - this.taille_offset, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 38] = resolutionHorizontaleOctet[i];
                }
                #endregion

                #region Remplissage du reste : vaut 0
                for (int i = 0; i < 12; i++)
                {
                    file_bon_format[i + 42] = 0;
                }
                #endregion

                #endregion
            }
            else
            {
                #region Remplissage fichier header & header info

                #region Type image 

                file_bon_format[0] = 66;
                file_bon_format[1] = 77;

                #endregion

                #region Taille image en octets
                byte[] taillethisOctet = Convertir_Int_To_Endian(this.taille_fichier, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 2] = taillethisOctet[i];
                }
                #endregion

                #region Bits réservé 
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 6] = 0;
                }
                #endregion

                #region Offset en octets
                file_bon_format[10] = 54;
                for (int i = 1; i < 4; i++)
                {
                    file_bon_format[i + 10] = 0;
                }

                #endregion

                #region Taille header en octets
                file_bon_format[14] = 40;
                for (int i = 0; i < 3; i++)
                {
                    file_bon_format[i + 15] = 0;
                }
                #endregion

                #region Largeur image en octets
                byte[] largeurthisOctet = Convertir_Int_To_Endian(this.largeur, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 18] = largeurthisOctet[i];
                }
                #endregion

                #region Hauteur image en octets
                byte[] hauteurthisOctet = Convertir_Int_To_Endian(this.hauteur, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 22] = hauteurthisOctet[i];
                }
                #endregion

                #region Nombre de plan de couleur en octets
                file_bon_format[26] = 1;
                file_bon_format[27] = 0;
                #endregion

                #region Nb de bits par pixel en octets
                file_bon_format[28] = 24;
                file_bon_format[29] = 0;
                #endregion

                #region Méthode de compression en octets
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 30] = 0;
                }
                #endregion

                #region Taille du header pur en octets
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 34] = 0;
                }
                #endregion

                #region Résolution horizontale en octets
                //taille du fichier - taille offset --> convertir en indian sur 4 octets
                byte[] resolutionHorizontaleOctet = Convertir_Int_To_Endian(this.taille_fichier - this.taille_offset, 4);
                for (int i = 0; i < 4; i++)
                {
                    file_bon_format[i + 38] = resolutionHorizontaleOctet[i];
                }
                #endregion

                #region Remplissage du reste : vaut 0
                for (int i = 0; i < 12; i++)
                {
                    file_bon_format[i + 42] = 0;
                }
                #endregion

                #endregion
            }

            #region Conversion matrice pixel en image
            int decalage = this.taille_offset;
            int increment = 0;
            for (int ligne = 0; ligne < this.mat_pixel.GetLength(0); ligne++)
            {
                int i = 1;
                for (int colonne = 0; colonne < this.mat_pixel.GetLength(1) + this.octetManquant; colonne++)
                {
                    if (colonne < this.largeur)
                    {
                        file_bon_format[decalage + increment] = (byte)this.mat_pixel[ligne, colonne].Blue;
                        file_bon_format[decalage + increment + 1] = (byte)this.mat_pixel[ligne, colonne].Green;
                        file_bon_format[decalage + increment + 2] = (byte)this.mat_pixel[ligne, colonne].Red;
                        increment += 3;
                    }
                    else
                    {
                        file_bon_format[decalage + increment + 2 + i] = 0; //on rajoute les octets manquants à la fin de la ligne
                        i++;
                    }
                }
                increment += this.octetManquant;
            }
            #endregion
            File.WriteAllBytes($"Fichiers/{image}.bmp", file_bon_format);
        }
        #endregion

        #region Traiter une image 
        public void Nuancier_gris_noir()
        {
            MyImage nouvelle_image = new MyImage(this.filename);
            Pixel[,] new_mat = new Pixel[this.hauteur, this.largeur];
            for (int ligne = 0; ligne < this.mat_pixel.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < this.mat_pixel.GetLength(1); colonne++)
                {
                    new_mat[ligne, colonne] = this.mat_pixel[ligne, colonne].NuanceDeGris();
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_noirEtBlanc", false);
        }
        public void Constraste()
        {
            MyImage nouvelle_image = new MyImage(this.filename);
            Pixel[,] new_mat = new Pixel[this.hauteur, this.largeur];
            int intensiteMin = 255;
            int intensiteMax = 0;
            for (int ligne = 0; ligne < this.hauteur; ligne++) //Boucle qui permet de trouver l'intensité maximale de l'image
            {
                for (int colonne = 0; colonne < this.largeur; colonne++)
                {
                    int r = this.mat_pixel[ligne, colonne].Red;
                    int g = this.mat_pixel[ligne, colonne].Green;
                    int b = this.mat_pixel[ligne, colonne].Blue;
                    int intensite = (r + g + b) / 3;
                    intensiteMin = Math.Min(intensiteMin, intensite);
                    intensiteMax = Math.Max(intensiteMax, intensite);
                }
            }

            for (int ligne = 0; ligne < this.hauteur; ligne++)
            {
                for (int colonne = 0; colonne < this.largeur; colonne++)
                {
                    int r = this.mat_pixel[ligne, colonne].Red;
                    int g = this.mat_pixel[ligne, colonne].Green;
                    int b = this.mat_pixel[ligne, colonne].Blue;
                    //Luminosité de pixel à la i-ième ligne et k-ième colonne
                    int intensite = (r + g + b) / 3;
                    //Nouvelle luminosité
                    int new_intensite = 255 * (intensite - intensiteMin) / (intensiteMax - intensiteMin);
                    r = (int)(r * new_intensite / intensite);
                    g = (int)(g * new_intensite / intensite);
                    b = (int)(b * new_intensite / intensite);
                    //Attribution du nouveau pixel avec l'intensité augmentée
                    new_mat[ligne, colonne] = new Pixel(b, g, r);
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_Constraste", false);
        }
        /// <summary>
        /// Fonction qui permet de changer une couleur en une autre
        /// </summary>
        /// <param name="couleur_a_prioriser">Couleur qui va remplacer la couleur sujet au changement. Prend comme valeur (en string) "r", "g" ou "b"</param>
        /// <param name="couleur_a_changerString">Couleur qui sera sujet au changement. Prend comme valeur (en string) "r", "g" ou "b".</param>
        /// <param name="seuil">Seuil de détection de la couleur</param>
        public void Colorisation(string couleur_a_prioriser = "g", string couleur_a_changerString = "r", int seuil = 220)
        {
            MyImage nouvelle_image = new MyImage(this.filename);
            Pixel[,] new_mat = new Pixel[this.hauteur, this.largeur];

            Pixel couleur_a_changer;
            switch (couleur_a_changerString)
            {
                case "r":
                    couleur_a_changer = new Pixel(0, 0, 255);
                    break;
                case "g":
                    couleur_a_changer = new Pixel(0, 255, 0);
                    break;
                case "b":
                    couleur_a_changer = new Pixel(255, 0, 0);
                    break;
                default:
                    couleur_a_changer = new Pixel(0, 0, 255);
                    break;
            }
            for (int ligne = 0; ligne < this.hauteur; ligne++)
            {
                for (int colonne = 0; colonne < this.largeur; colonne++)
                {
                    int r = this.mat_pixel[ligne, colonne].Red;
                    int g = this.mat_pixel[ligne, colonne].Green;
                    int b = this.mat_pixel[ligne, colonne].Blue;
                    if (Methods.Distance(couleur_a_changer, this.mat_pixel[ligne, colonne]) < seuil * seuil)
                    {
                        if (couleur_a_prioriser == "r")
                        {
                            r = (int)(r * 1.5);
                            g = (int)(g * 0.5);
                            b = (int)(b * 0.5);
                        }
                        if (couleur_a_prioriser == "g")
                        {
                            r = (int)(r * 0.5);
                            g = (int)(g * 1.5);
                            b = (int)(b * 0.5);
                        }
                        if (couleur_a_prioriser == "b")
                        {
                            r = (int)(r * 0.5);
                            g = (int)(g * 0.5);
                            b = (int)(b * 1.5);
                        }
                    }
                    //Attribution du nouveau pixel avec l'intensité augmentée
                    new_mat[ligne, colonne] = new Pixel(b, g, r);
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_Colorisation", false);
        }
        public void Miroir()
        {
            MyImage nouvelle_image = new MyImage(this.filename);
            Pixel[,] new_mat = new Pixel[this.hauteur, this.largeur];
            int ligne = this.mat_pixel.GetLength(0) - 1;
            for (int i = 0; i < new_mat.GetLength(0); i++, ligne--)
            {
                int colonne = this.mat_pixel.GetLength(1) - 1;
                for (int j = 0; j < new_mat.GetLength(1); j++, colonne--)
                {
                    new_mat[i, j] = this.mat_pixel[i, colonne];
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_miroir", false);
        }
        public void Agrandir(int coeff)
        {
            if (coeff < 0) coeff = -coeff;
            MyImage nouvelle_image = new MyImage(this.filename);
            Pixel[,] new_mat = new Pixel[this.hauteur * coeff, this.largeur * coeff];

            //nouvelle_image.taille_fichier = ((this.largeur * coeff * 3) * this.hauteur * coeff) + this.taille_offset;
            nouvelle_image.hauteur = this.hauteur * coeff;
            nouvelle_image.largeur = this.largeur * coeff;
            nouvelle_image.taille_fichier = nouvelle_image.nb_bit * nouvelle_image.hauteur * nouvelle_image.largeur;
            nouvelle_image.file = new byte[this.taille_offset + ((this.largeur * coeff) * 3) * (this.hauteur * coeff)];
            for (int ligne = 0; ligne < this.mat_pixel.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < this.mat_pixel.GetLength(1); colonne++)
                {
                    int ligne_multiplie = ligne * coeff;
                    int colonne_multiplie = colonne * coeff;
                    new_mat[ligne_multiplie, colonne_multiplie] = this.mat_pixel[ligne, colonne];
                    for (int i = 0; i < coeff; i++)
                    {
                        for (int j = 0; j < coeff; j++)
                        {
                            new_mat[ligne_multiplie + i, colonne_multiplie + j] = this.mat_pixel[ligne, colonne];
                        }
                    }
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_agrandit_coeff{coeff}", false);
        }
        public void Retrectir(int coeff)
        {
            if (coeff < 0) coeff = -coeff;
            MyImage nouvelle_image = new MyImage(this.filename);
            int ajout = 0;
            if (coeff % 2 != 0)
            {
                ajout++;
            }
            Pixel[,] new_mat = new Pixel[(this.hauteur/coeff) + ajout, (this.largeur / coeff) + ajout];

            nouvelle_image.hauteur = (this.hauteur / coeff) + ajout;
            nouvelle_image.largeur = (this.largeur / coeff) + ajout;
            nouvelle_image.file = new byte[this.taille_offset + (nouvelle_image.largeur + nouvelle_image.octetManquant) * 3 * nouvelle_image.hauteur];
            for (int ligne = 0; ligne < this.hauteur + 1; ligne++)
            {
                for (int colonne = 0; colonne < this.largeur + 1; colonne++)
                {
                    new_mat[ligne/coeff, colonne/coeff] = this.mat_pixel[ligne, colonne];
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_rretrecissement_coeff{coeff}", false);
        }
        public void Rotation(int angle)
        {
            MyImage nouvelle_image = new MyImage(this.filename); //Création d'une nouvelle image avec les attributs de celle de base

            #region Variables 
            double angleRad = angle * Math.PI / 180;
            double cos = Math.Cos(angleRad);
            double sin = Math.Sin(angleRad);

            int hauteur = nouvelle_image.hauteur; //Hauteur de l'image originale 
            int largeur = nouvelle_image.largeur; //Largeur de l'image originale 

            int nouvelle_hauteur = (int)Math.Round(Math.Abs(cos * hauteur) + Math.Abs(sin * largeur));
            int nouvelle_largeur = (int)Math.Round(Math.Abs(cos * largeur) + Math.Abs(sin * hauteur));

            Pixel[,] new_mat = new Pixel[nouvelle_hauteur, nouvelle_largeur]; //Définition de la nouvelle matrice avec une taille agrandit pour faire tenir toutes l'image après rotation

            int centre_original_hauteur = (int)Math.Round((double)((hauteur + 1) / 2));
            int centre_original_largeur = (int)Math.Round((double)((largeur + 1) / 2));

            int centre_nouvelle_hauteur = (int)Math.Round((double)((nouvelle_hauteur + 1) / 2)); //centre de la nouvelle image : hauteur
            int centre_nouvelle_largeur = (int)Math.Round((double)((nouvelle_largeur + 1) / 2)); //centre de la nouvelle image : largeur

            #endregion
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    int x = largeur - 1 - colonne - centre_original_largeur;
                    int y = hauteur - 1 - ligne - centre_original_hauteur;

                    int[] shearTab = Shear(angleRad, x, y);
                    int new_y = shearTab[0];
                    int new_x = shearTab[1];
                    //Recentrage du centre par rapport à la rotation 
                    new_x = centre_nouvelle_largeur - new_x;
                    new_y = centre_nouvelle_hauteur - new_y;
                    if (new_x >= 0 && new_x < nouvelle_largeur && new_y >= 0 && new_y < nouvelle_hauteur)
                    {
                        new_mat[new_y, new_x] = this.mat_pixel[ligne, colonne];
                    }
                }
            }
            //if (angle % 90 != 0)
            //{

            //}
            //else
            //{
            //    for (int i = 0; i < (angle / 90); i++) 
            //    {
            //        for (int ligne = 0; ligne < hauteur; ligne++)
            //        {
            //            for (int colonne = 0; colonne < largeur; colonne++)
            //            {
            //                new_mat[colonne, ligne] = this.mat_pixel[ligne, colonne];
            //            }
            //        }
            //    }
            //}
            for (int ligne = 0; ligne < new_mat.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < new_mat.GetLength(1); colonne++)
                {
                    if (new_mat[ligne, colonne] == null) new_mat[ligne, colonne] = new Pixel(0, 0, 0);
                }
            }
            nouvelle_image.hauteur = nouvelle_hauteur;
            nouvelle_image.largeur = nouvelle_largeur;
            if ((nouvelle_image.largeur * 3) % 4 != 0) nouvelle_image.octetManquant = 4 - (nouvelle_image.largeur * 3) % 4;
            nouvelle_image.file = new byte[this.taille_offset + ((nouvelle_image.largeur + nouvelle_image.octetManquant) * 3) * nouvelle_image.hauteur];
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_rotation{angle}", false);

        }
        public void Rotation180AntiClockWise()
        {
            MyImage nouvelle_image = new MyImage(this.filename); //Création d'une nouvelle image avec les attributs de celle de base
            Pixel[,] new_mat = new Pixel[nouvelle_image.mat_pixel.GetLength(0), nouvelle_image.mat_pixel.GetLength(1)];
            for (int ligne = 0; ligne < new_mat.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < new_mat.GetLength(1); colonne++)
                {
                    new_mat[ligne, colonne] = this.mat_pixel[this.mat_pixel.GetLength(0) - 1 - ligne, colonne];
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{this.filename}_rotation{180}AntiClockWise", false);
        }
        public void Rotation180ClockWise()
        {
            MyImage nouvelle_image = new MyImage(this.filename); //Création d'une nouvelle image avec les attributs de celle de base
            Pixel[,] new_mat = new Pixel[nouvelle_image.mat_pixel.GetLength(0), nouvelle_image.mat_pixel.GetLength(1)];
            for (int ligne = 0; ligne < new_mat.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < new_mat.GetLength(1); colonne++)
                {
                    new_mat[ligne, colonne] = this.mat_pixel[this.mat_pixel.GetLength(0) - 1 - ligne, this.mat_pixel.GetLength(1) - 1 - colonne];
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{this.filename}_rotation{180}ClockWise", false);
        }
        /// <summary>
        /// Fonction qui permet de décomposer la rotation en trois matrices permettant d'éviter l'aliasing
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int[] Shear(double angle, int x, int y)
        {
            /*
                | 1 - tan(𝜃/ 2) |  | 1        0 |  | 1 - tan(𝜃/ 2) |
   
                | 0      1 |       | sin(𝜃)   1 |  | 0           1 |
            */
            //n°1 Shear 
            double tan = Math.Tan(angle / 2);
            int new_x = (int)Math.Round(x - y * tan);
            int new_y = y;

            //n°2 Shear
            new_y = (int)Math.Round(new_x * Math.Sin(angle) + new_y); //Pas de changement par rapport à la nouvelle matrice (celle du milieu)

            //n°3 Shear 
            new_x = (int)Math.Round(new_x - new_y * tan);
            int[] returnTab = new int[] { new_y, new_x };
            return returnTab;
        }
        /// <summary>
        /// Détection des bords par la méthode de Sobel
        /// </summary>
        public void DetectionDeContour()
        {
            MyImage nouvelle_image = new MyImage(this.filename); //Création d'une nouvelle image avec les attributs de celle de base
            //Calcul de l'intensité moyenne des pixels 
            int[,] intensite = new int[this.hauteur, this.largeur];
            for (int ligne = 0; ligne < this.hauteur; ligne++)
            {
                for (int colonne = 0; colonne < this.largeur; colonne++)
                {
                    intensite[ligne, colonne] = (this.mat_pixel[ligne, colonne].Red + this.mat_pixel[ligne, colonne].Green + this.mat_pixel[ligne, colonne].Blue) / 3;
                }
            }
            //Matrice de convolution 
            int[,] convolutionx = { { -1, 0, 1 },
                                    { -2, 0, 2 },
                                    { -1, 0, 1 } };
            int[,] convolutiony = { { -1, -2, -1 },
                                    { 0, 0, 0 },
                                    { 1, 2, 1 } };

            Pixel[,] matrice_result = new Pixel[this.mat_pixel.GetLength(0), this.mat_pixel.GetLength(1)];

            for (int ligne = 1; ligne < matrice_result.GetLength(0) - 1; ligne++) //Les deux premières boucles for parcourt l'entièreté de la première matrice
                                                                                  //En ayant en tête que c'est pour appliquer la matrice de convolution, càd
                                                                                  //faire en sorte que l'application de la matrice de convolution ne déborde pas
            {
                for (int colonne = 1; colonne < matrice_result.GetLength(1) - 1; colonne++)
                {
                    int magnitudex = 0;
                    int magnitudey = 0;
                    for (int ligne_convolution = 0; ligne_convolution < 3; ligne_convolution++)
                    {

                        for (int colonne_convolution = 0; colonne_convolution < 3; colonne_convolution++) //Les deux dernières boucles for servent à appliquer la matrice de convolution
                                                                                                          //à l'entièreté de la matrice initiale.
                        {
                            int x_pos = colonne + colonne_convolution - 1;
                            int y_pos = ligne + ligne_convolution - 1;
                            magnitudex += intensite[y_pos, x_pos] * convolutionx[ligne_convolution, colonne_convolution];
                            magnitudey += intensite[y_pos, x_pos] * convolutiony[ligne_convolution, colonne_convolution];
                        }
                    }
                    int couleur = (int)Math.Sqrt((magnitudex * magnitudex) + (magnitudey * magnitudey));
                    matrice_result[ligne, colonne] = new Pixel(couleur, couleur, couleur);
                }
            }
            for (int ligne = 0; ligne < matrice_result.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < matrice_result.GetLength(1); colonne++)
                {
                    if (matrice_result[ligne, colonne] == null) matrice_result[ligne, colonne] = new Pixel(0, 0, 0);
                }
            }
            nouvelle_image.mat_pixel = matrice_result;
            nouvelle_image.From_Image_To_File($"{filename}_DetectionDeContour", false);
        }
        /// <summary>
        /// Fonction qui permet d'appliquer un filtre de flou à une image
        /// </summary>
        /// <param name="tag">tag == true : flou gaussien (par défaut) || tag == false : flou classique</param>
        public void Flou(bool tag = true)
        {
            //Flou classique
            double[,] convolutionFlou = {{0.11,0.11, 0.11 },
                                { 0.11, 0.11, 0.11 },
                                { 0.11, 0.11, 0.11} };
            //Flou gaussien
            double[,] convolutionGaussienne = {{1.0 / 256.0, 4.0 / 256.0, 6.0 / 256.0, 4.0 / 256.0, 1.0 / 256.0 },
                                     { 4.0 / 256.0, 16.0 / 256.0, 24.0 / 256.0, 16.0 / 256.0, 4.0 / 256.0 },
                                     { 6.0 / 256.0, 24.0 / 256.0, 36.0 / 256.0, 24.0 / 256.0, 6.0 / 256.0 },
                                     { 4.0 / 256.0, 16.0 / 256.0, 24.0 / 256.0, 16.0 / 256.0, 4.0 / 256.0 },
                                     { 1.0 / 256.0, 4.0 / 256.0, 6.0 / 256.0, 4.0 / 256.0, 1.0 / 256.0 } };
            if (tag) this.MatriceDeConvolution(convolutionGaussienne, "flouGaussien");
            else this.MatriceDeConvolution(convolutionFlou, "flouClassique");
        }
        /// <summary>
        /// Fonction qui permet d'appliquer un filtre de repoussage à l'image
        /// </summary>
        public void Repoussage()
        {
            double[,] convolutionRepoussage = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            this.MatriceDeConvolution(convolutionRepoussage, "repoussage");
        }
        /// <summary>
        /// Fonction qui permet d'appliquer un filtre de netteté à l'image (sharpen en anglais)
        /// </summary>
        public void Nettete()
        {
            //Matrice de convolution de relief 
            double[,] convolutionNettete = { { 0, -0.5, 0 }, { -0.5, 3, -0.5 }, { 0, -0.5, 0 } };
            this.MatriceDeConvolution(convolutionNettete, "Nettete");
        }
        /// <summary>
        /// Fonction générale qui permet d'appliquer une matrice de convolution à une image
        /// </summary>
        /// <param name="convolution_mat">Matrice de convolution qui sera utilisée pour l'image</param>
        /// <param name="nom">Nom permettant l'enregistrement du fichier avec un nom particulier</param>
        public void MatriceDeConvolution(double[,] convolution_mat, string nom)
        {
            MyImage nouvelle_image = new MyImage(this.filename); //Création d'une nouvelle image avec les attributs de celle de base

            double[,] convolution = convolution_mat;
            Pixel[,] new_mat = new Pixel[this.mat_pixel.GetLength(0), this.mat_pixel.GetLength(1)];
            //Remplissage des bords qui ne seront pas considéré dans l'application du filtre
            for (int i = 0; i < new_mat.GetLength(0); i++)
            {
                new_mat[i, 0] = this.mat_pixel[i, 0];
                new_mat[i, new_mat.GetLength(1) - 1] = this.mat_pixel[i, new_mat.GetLength(1) - 1];
            }
            for (int i = 0; i < new_mat.GetLength(1); i++)
            {
                new_mat[0, i] = this.mat_pixel[0, i];
                new_mat[new_mat.GetLength(0) - 1, i] = this.mat_pixel[new_mat.GetLength(0) - 1, i];
            }
            for (int ligne = 1; ligne < new_mat.GetLength(0) - 1; ligne++) //Les deux premières boucles for parcourt l'entièreté de la première matrice
                                                                           //En ayant en tête que c'est pour appliquer la matrice de convolution, càd
                                                                           //faire en sorte que l'application de la matrice de convolution ne déborde pas
            {
                for (int colonne = 1; colonne < new_mat.GetLength(1) - 1; colonne++)
                {
                    int moyenneR = 0;
                    int moyenneG = 0;
                    int moyenneB = 0;
                    for (int ligne_convolution = 0; ligne_convolution < convolution.GetLength(0); ligne_convolution++)
                    {
                        for (int colonne_convolution = 0; colonne_convolution < convolution.GetLength(1); colonne_convolution++) //Les deux dernières boucles for servent à appliquer la matrice de convolution
                                                                                                                                 //à l'entièreté de la matrice initiale.
                        {
                            int x_pos = colonne + colonne_convolution;
                            int y_pos = ligne + ligne_convolution;
                            if (x_pos > 0 && x_pos < this.largeur && y_pos > 0 && y_pos < this.hauteur)
                            {
                                moyenneR += (int)Math.Round(this.mat_pixel[y_pos, x_pos].Red * convolution[ligne_convolution, colonne_convolution]);
                                moyenneG += (int)Math.Round(this.mat_pixel[y_pos, x_pos].Green * convolution[ligne_convolution, colonne_convolution]);
                                moyenneB += (int)Math.Round(this.mat_pixel[y_pos, x_pos].Blue * convolution[ligne_convolution, colonne_convolution]);
                            }
                        }
                    }
                    if (moyenneR < 0) moyenneR = 0;
                    if (moyenneR > 255) moyenneR = 255;
                    if (moyenneG < 0) moyenneG = 0;
                    if (moyenneG > 255) moyenneG = 255;
                    if (moyenneB < 0) moyenneB = 0;
                    if (moyenneB > 255) moyenneB = 255;
                    new_mat[ligne, colonne] = new Pixel(moyenneB, moyenneG, moyenneR);
                }
            }
            //for (int ligne = 0; ligne < matrice_result.GetLength(0); ligne++)
            //{
            //    for (int colonne = 0; colonne < matrice_result.GetLength(1); colonne++)
            //    {
            //        if (matrice_result[ligne, colonne] == null) matrice_result[ligne, colonne] = new Pixel(0, 0, 0);
            //    }
            //}
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{filename}_{nom}", false);
        }
        public void Histogramme()
        {
            int[] histogramme_r = new int[256];
            int[] histogramme_g = new int[256];
            int[] histogramme_b = new int[256];

            float maxR = 0;
            float maxG = 0;
            float maxB = 0;

            MyImage histogramme = new MyImage($"{this.filename}_histogramme", 500, 1024);

            Pixel[,] new_mat = new Pixel[histogramme.hauteur, histogramme.largeur];
            Pixel[,] new_matR = new Pixel[histogramme.hauteur, histogramme.largeur];
            Pixel[,] new_matG = new Pixel[histogramme.hauteur, histogramme.largeur];
            Pixel[,] new_matB = new Pixel[histogramme.hauteur, histogramme.largeur];

            histogramme.taille_fichier = histogramme.nb_bit * histogramme.hauteur * histogramme.largeur;
            histogramme.file = new byte[histogramme.taille_offset + ((histogramme.largeur * 3) * (histogramme.hauteur))];
            histogramme.mat_pixel = new_mat;

            for (int ligne = 0; ligne < this.hauteur; ligne++) //On détermine la hauteur maximum du trait des couleurs rouges + on construit l'histogramme à l'envers
            {
                for (int colonne = 0; colonne < this.largeur; colonne++)
                {
                    int redValue = this.mat_pixel[ligne, colonne].Red; //On aura forcément une valeur comprise entre 0 et 255 car les pixels sont définis comme ça
                    int greenValue = this.mat_pixel[ligne, colonne].Green;
                    int blueValue = this.mat_pixel[ligne, colonne].Blue;

                    histogramme_r[redValue]++;
                    histogramme_g[greenValue]++;
                    histogramme_b[blueValue]++;
                    if (maxR < histogramme_r[redValue]) maxR = histogramme_r[redValue];
                    if (maxG < histogramme_g[greenValue]) maxG = histogramme_g[greenValue];
                    if (maxB < histogramme_b[blueValue]) maxB = histogramme_b[blueValue];
                }
            }
            histogramme_r = Enumerable.Repeat((int)maxR, 256).ToArray();
            histogramme_g = Enumerable.Repeat((int)maxG, 256).ToArray();
            histogramme_b = Enumerable.Repeat((int)maxB, 256).ToArray();
            for (int ligne = 0; ligne < this.hauteur; ligne++) //On reconstruit l'histogramme à l'endroit
            {
                for (int colonne = 0; colonne < this.largeur; colonne++)
                {
                    int redValue = this.mat_pixel[ligne, colonne].Red;
                    int greenValue = this.mat_pixel[ligne, colonne].Green;
                    int blueValue = this.mat_pixel[ligne, colonne].Blue;
                    histogramme_r[redValue]--;
                    histogramme_g[greenValue]--;
                    histogramme_b[blueValue]--;
                }
            }
            for (int ligne = 0; ligne < histogramme.hauteur; ligne++) //Remplissage de l'image par des pixels noirs pour mieux voir les traits & aussi remplir totalement la matrice pour pas quelle soit null
            {
                for (int colonne = 0; colonne < histogramme.largeur; colonne++)
                {
                    new_matR[ligne, colonne] = new Pixel(0, 0, 0);
                    new_matG[ligne, colonne] = new Pixel(0, 0, 0);
                    new_matB[ligne, colonne] = new Pixel(0, 0, 0);
                }
            }
            for (int i = 0; i < histogramme_r.Length; i++)
            {
                float pourcentageR = (float)histogramme_r[i] / maxR;
                float pourcentageG = (float)histogramme_g[i] / maxG;
                float pourcentageB = (float)histogramme_b[i] / maxB;
                Methods.DrawStraigthLine(new_matR, i * histogramme.largeur / 256, histogramme.hauteur - (int)(pourcentageR * histogramme.hauteur), "r");
                Methods.DrawStraigthLine(new_matG, i * histogramme.largeur / 256, histogramme.hauteur - (int)(pourcentageG * histogramme.hauteur), "g");
                Methods.DrawStraigthLine(new_matB, i * histogramme.largeur / 256, histogramme.hauteur - (int)(pourcentageB * histogramme.hauteur), "b");
            }
            histogramme.mat_pixel = Methods.Merge(new_matR, new_matG, new_matB);
            histogramme.From_Image_To_File($"{histogramme.filename}", true);
        }
        /// <summary>
        /// On ne peut pas encoder une image plus grande dans une plus petite. 
        /// </summary>
        /// <param name="imageMere">Matrice qui va avoir une image cachée à l'intérieure</param>
        /// <param name="imageFille">Matrice qui va être cachée à l'intérieure de imageMere</param>
        public static void EncoderImage(MyImage imageMere, MyImage imageFille)
        {
            if (imageMere.mat_pixel.Length < imageFille.mat_pixel.Length)
            {
                Console.WriteLine("La taille de la matrice en paramètre 1 doit être supérieure à la matrice en paramètre 2");
            }
            else
            {
                //nouvelle_image 
                MyImage nouvelle_image = new MyImage(imageMere.filename); //imageMere c'est la plus grande. Celle qui va avoir l'image encodée à l'intérieure d'elle
                nouvelle_image.taille_fichier = nouvelle_image.nb_bit * nouvelle_image.hauteur * nouvelle_image.largeur;
                nouvelle_image.file = new byte[nouvelle_image.taille_offset + ((nouvelle_image.largeur * 3) * (nouvelle_image.hauteur))];
                Pixel[,] new_mat = new Pixel[nouvelle_image.hauteur, nouvelle_image.largeur];
                for (int ligne = 0; ligne < imageMere.mat_pixel.GetLength(0); ligne++)
                {
                    for (int colonne = 0; colonne < imageMere.mat_pixel.GetLength(1); colonne++)
                    {
                        new_mat[ligne, colonne] = imageMere.mat_pixel[ligne, colonne];
                    }
                }
                for (int ligne = 0; ligne < imageFille.mat_pixel.GetLength(0); ligne++)
                {
                    for (int colonne = 0; colonne < imageFille.mat_pixel.GetLength(1); colonne++)
                    {
                        int R_f = imageFille.mat_pixel[ligne, colonne].Red; //Conversion binaire de l'octet du pixel[ligne, colonne] de l'image fille (qui sera cachée)
                        int G_f = imageFille.mat_pixel[ligne, colonne].Green;
                        int B_f = imageFille.mat_pixel[ligne, colonne].Blue;

                        string binaireR_F = Methods.DecimalToBase(R_f, 2);
                        string binaireG_F = Methods.DecimalToBase(G_f, 2);
                        string binaireB_F = Methods.DecimalToBase(B_f, 2);
                        binaireR_F = binaireR_F.PadLeft(8, '0');
                        binaireG_F = binaireG_F.PadLeft(8, '0');
                        binaireB_F = binaireB_F.PadLeft(8, '0');

                        int R_m = imageMere.mat_pixel[ligne, colonne].Red; //Conversion binaire de l'octet du pixel[ligne, colonne] de l'image fille (qui sera cachée)
                        int G_m = imageMere.mat_pixel[ligne, colonne].Green;
                        int B_m = imageMere.mat_pixel[ligne, colonne].Blue;

                        string binaireR_M = Methods.DecimalToBase(R_m, 2);
                        string binaireG_M = Methods.DecimalToBase(G_m, 2);
                        string binaireB_M = Methods.DecimalToBase(B_m, 2);
                        binaireR_M = binaireR_M.PadLeft(8, '0');
                        binaireG_M = binaireG_M.PadLeft(8, '0');
                        binaireB_M = binaireB_M.PadLeft(8, '0');

                        string octetBS = $"{binaireB_M[0]}{binaireB_M[1]}{binaireB_M[2]}{binaireB_M[3]}{binaireB_F[0]}{binaireB_F[1]}{binaireB_F[2]}{binaireB_F[3]}";
                        string octetGS = $"{binaireG_M[0]}{binaireG_M[1]}{binaireG_M[2]}{binaireG_M[3]}{binaireG_F[0]}{binaireG_F[1]}{binaireG_F[2]}{binaireG_F[3]}";
                        string octetRS = $"{binaireR_M[0]}{binaireR_M[1]}{binaireR_M[2]}{binaireR_M[3]}{binaireR_F[0]}{binaireR_F[1]}{binaireR_F[2]}{binaireR_F[3]}";

                        int octetR = Convert.ToInt32(Methods.BaseToDecimal(octetRS, 2));
                        int octetG = Convert.ToInt32(Methods.BaseToDecimal(octetGS, 2));
                        int octetB = Convert.ToInt32(Methods.BaseToDecimal(octetBS, 2));

                        new_mat[ligne, colonne] = new Pixel(octetB, octetG, octetR);
                    }
                }
                nouvelle_image.mat_pixel = new_mat;
                nouvelle_image.From_Image_To_File($"{nouvelle_image.filename}_encoder", true);
            }
        }
        public static void DecoderImage(MyImage imageMere, MyImage imageFille)
        {
            //nouvelle_image 
            MyImage nouvelle_imageMere = new MyImage(imageMere.filename); //imageMere c'est la plus grande. Celle qui va avoir l'image encodée à l'intérieure d'elle
            MyImage nouvelle_imageFille = new MyImage(imageFille.filename);

            nouvelle_imageMere.taille_fichier = nouvelle_imageFille.nb_bit * nouvelle_imageFille.hauteur * nouvelle_imageFille.largeur;
            nouvelle_imageFille.file = new byte[nouvelle_imageMere.taille_offset + ((nouvelle_imageMere.largeur * 3) * (nouvelle_imageMere.hauteur))];
            Pixel[,] new_matF = new Pixel[nouvelle_imageMere.hauteur, nouvelle_imageMere.largeur];
            for (int ligne = 0; ligne < nouvelle_imageMere.mat_pixel.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < nouvelle_imageMere.mat_pixel.GetLength(1); colonne++)
                {
                    if (ligne < imageFille.mat_pixel.GetLength(0) && colonne < imageFille.mat_pixel.GetLength(1))
                    {
                        int R_m = imageMere.mat_pixel[ligne, colonne].Red; //Conversion binaire de l'octet du pixel[ligne, colonne] de l'image fille (qui sera cachée)
                        int G_m = imageMere.mat_pixel[ligne, colonne].Green;
                        int B_m = imageMere.mat_pixel[ligne, colonne].Blue;

                        string binaireR_M = Methods.DecimalToBase(R_m, 2);
                        string binaireG_M = Methods.DecimalToBase(G_m, 2);
                        string binaireB_M = Methods.DecimalToBase(B_m, 2);

                        binaireR_M = binaireR_M.PadLeft(8, '0');
                        binaireG_M = binaireG_M.PadLeft(8, '0');
                        binaireB_M = binaireB_M.PadLeft(8, '0');

                        string octetBS_F = $"{binaireB_M[4]}{binaireB_M[5]}{binaireB_M[6]}{binaireB_M[7]}".PadRight(8, '0');
                        string octetGS_F = $"{binaireG_M[4]}{binaireG_M[5]}{binaireG_M[6]}{binaireG_M[7]}".PadRight(8, '0');
                        string octetRS_F = $"{binaireR_M[4]}{binaireR_M[5]}{binaireR_M[6]}{binaireR_M[7]}".PadRight(8, '0');

                        int octetR = Convert.ToInt32(Methods.BaseToDecimal(octetRS_F, 2));
                        int octetG = Convert.ToInt32(Methods.BaseToDecimal(octetGS_F, 2));
                        int octetB = Convert.ToInt32(Methods.BaseToDecimal(octetBS_F, 2));

                        new_matF[ligne, colonne] = new Pixel(octetB, octetG, octetR);
                    }
                    else new_matF[ligne, colonne] = new Pixel(0, 0, 0);

                }
            }
            nouvelle_imageFille.mat_pixel = new_matF;
            nouvelle_imageFille.From_Image_To_File($"{nouvelle_imageFille.filename}_décoder", true);
        }
        #endregion

        #region Fractales

        /// <summary>
        /// Fonction pour construire une fractale avec plusieurs paramètres
        /// </summary>
        /// <param name="zoom">Variable qui définit la résolution de l'image</param>
        /// <param name="iteration_maximale">Variable qui définit le degré de détail de la fractale. 
        /// <para>Attention ! Pour le choix de couleur "gradient" ne pas mettre une valeur d'iteration_maximale trop élevée au risque de recevoir une image noir</para></param>
        /// <param name="choixFractale">Variable qui permet de choisir la fractale qui doit être dessinée.
        /// <para>Prend comme valeur : "mandelbrot", "julia", "burningShip"</para></param>
        /// <param name="methodeCouleur">Variable qui permet de choisir la méthode de colorisation.
        /// <para>Prend comme valeur : "gradientMono", "HSV", "gradientMulti", "noirBlanc"</para>
        /// <para>Attention ! Si le choix est "gradientMulti" il faut spécifier dans les paramètres suivants "pos1", "pos2", "pos3"</para></param>
        /// <param name="colorMap">Détermine la colorMap utilisée si methodeCouleur == gradientMulti.
        /// <para>Prend comme valeur : "spring", "summer", "autumn", "gray", "jet", "hot", "cool"</para></param>
        /// <param name="pos1">Détermine la position des couleurs prises par la case bleu dans RGB</param>
        /// <param name="pos2">Détermine la position des couleurs prises par la case vert dans RGB</param>
        /// <param name="pos3">Détermine la position des couleurs prises par la case rouge dans RGB</param>
        public static void Fractale(int zoom, int iteration_maximale, string choixFractale = "mandelbrot", string methodeCouleur = "gradientMono", string colorMap = "jet", byte pos1 = 3, byte pos2 = 2, byte pos3 = 1)
        {
            #region Variables
            //On définit les limites de l'ensemble de définition de la fractale
            double Xmin, Xmax, Ymin, Ymax;
            Xmin = -2.1;
            Xmax = 0.6;
            Ymin = -1.2;
            Ymax = 1.2;
            if (choixFractale == "julia")
            {
                Xmin = -1;
                Xmax = 1;
                Ymin = -1.2;
                Ymax = 1.2;
            }
            else if (choixFractale == "burningShip")
            {
                Xmin = -1.6;
                Xmax = 0.6;
                Ymin = -1.2;
                Ymax = 1.5;
            }

            //Calcul de la taille de l'image
            int largeur = (int)(Xmax - Xmin) * zoom;
            int hauteur = (int)(Ymax - Ymin) * zoom;

            MyImage nouvelle_image = new MyImage($"fractale_{choixFractale}", hauteur, largeur);

            nouvelle_image.hauteur = hauteur;
            nouvelle_image.largeur = largeur;

            Pixel[,] new_mat = new Pixel[hauteur, largeur];

            nouvelle_image.taille_fichier = nouvelle_image.nb_bit * nouvelle_image.hauteur * nouvelle_image.largeur;
            nouvelle_image.file = new byte[nouvelle_image.taille_offset + ((nouvelle_image.largeur + nouvelle_image.octetManquant) * 3) * (nouvelle_image.hauteur)];

            ColorMap map = new ColorMap(256, 255);
            int[,] mapCouleur = map.Jet();
            if (methodeCouleur == "gradientMulti")
            {
                switch (colorMap)
                {
                    case "spring":
                        mapCouleur = map.Spring();
                        break;
                    case "summer":
                        mapCouleur = map.Summer();
                        break;
                    case "autumn":
                        mapCouleur = map.Autumn();
                        break;
                    case "gray":
                        mapCouleur = map.Gray();
                        break;
                    case "jet":
                        mapCouleur = map.Jet();
                        break;
                    case "hot":
                        mapCouleur = map.Hot();
                        break;
                    case "cool":
                        mapCouleur = map.Cool();
                        break;
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            #endregion

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    double z_r, z_i, c_r, c_i;
                    Complexe c = new Complexe(0, 0);
                    Complexe z = new Complexe(0, 0);
                    if (choixFractale == "mandelbrot" || choixFractale == "burningShip")
                    {
                        c_r = (double)colonne / (double)zoom + Xmin;
                        c_i = (double)ligne / (double)zoom + Ymin;
                        c = new Complexe(c_r, c_i);
                        z = new Complexe(0, 0);
                    }
                    if (choixFractale == "julia")
                    {
                        z_r = (double)colonne / (double)zoom + Xmin;
                        z_i = (double)ligne / (double)zoom + Ymin;
                        //double c_r = -0.8;
                        //double c_i = 0.156;
                        c_r = 0.3;
                        c_i = 0.5;
                        c = new Complexe(c_r, c_i);
                        z = new Complexe(z_r, z_i);
                    }
                    int i = 0;
                    do
                    {
                        if (choixFractale == "mandelbrot" || choixFractale == "julia") z = z * z + c;
                        else if (choixFractale == "burningShip")
                        {
                            Complexe t = new Complexe(Math.Abs(z.Reel), Math.Abs(z.Img));
                            z = t * t + c;
                        }
                        i++;
                    } while (z.Module() < 2 && i < iteration_maximale);
                    Tuple<int, int, int> RGBvalues = Pixel.HSVversRGB(i * 255, 1, 1);
                    int R = RGBvalues.Item1;
                    int G = RGBvalues.Item2;
                    int B = RGBvalues.Item3;
                    if (i == iteration_maximale)
                    {
                        new_mat[ligne, colonne] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        if (methodeCouleur == "noirBlanc") new_mat[ligne, colonne] = new Pixel(255, 255, 255);
                        if (methodeCouleur == "gradientMono") new_mat[ligne, colonne] = new Pixel(i * 255 / iteration_maximale, 0, 0);
                        if (methodeCouleur == "HSV") new_mat[ligne, colonne] = new Pixel(B, G, R);
                        if (methodeCouleur == "gradientMulti") new_mat[ligne, colonne] = new Pixel(mapCouleur[i * 255 / iteration_maximale, pos1], mapCouleur[i * 255 / iteration_maximale, pos2], mapCouleur[i * 255 / iteration_maximale, pos3]);
                        else if (methodeCouleur != "noirBlanc" && methodeCouleur != "gradientMono" && methodeCouleur != "HSV" && methodeCouleur != "gradientMulti") new_mat[ligne, colonne] = new Pixel(0, 0, 0);
                    }
                }
                Methods.StatusUpdate((int)(((double)ligne / (hauteur - 1)) * 100), stopwatch);
            }
            Console.WriteLine("Opération complétée.\nAppuyez sur une touche pour continuer.");
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{nouvelle_image.filename}", true);
            Affichage_image($"{nouvelle_image.filename}");
        }

        public static void Fractale_Buddhabrot(int zoom, int iteration_rouge = 5000, int iteration_vert = 5000, int iteration_bleu = 5000)
        {
            //On définit les limites de l'ensemble de définition de la fractale de Mendelbrot 
            double x1 = -2.25;
            double x2 = 1.25;
            double y1 = -1.75;
            double y2 = 1.75;

            double xpush = x2 - x1;
            double ypush = y2 - y1;
            //Valeurs initiales de la fractale de Mendelbrot
            //double x1 = -2.1;
            //double x2 = 0.6;
            //double y1 = -1.2;
            //double y2 = 1.2;
            int iteration_max = Math.Max(Math.Max(iteration_rouge, iteration_vert), iteration_bleu);

            //Calcul de la taille de l'image
            int largeur = (int)(x2 - x1) * zoom;
            int hauteur = (int)(y2 - y1) * zoom;

            //Définition de la nouvelle image
            MyImage nouvelle_image = new MyImage("fractale_Buddhabrot", hauteur, largeur);
            nouvelle_image.hauteur = hauteur;
            nouvelle_image.largeur = largeur;

            nouvelle_image.taille_fichier = nouvelle_image.nb_bit * nouvelle_image.hauteur * nouvelle_image.largeur;
            nouvelle_image.file = new byte[nouvelle_image.taille_offset + ((nouvelle_image.largeur * 3) * (nouvelle_image.hauteur))];

            Pixel[,] new_mat = new Pixel[hauteur, largeur];

            int[,] pixels_rouge = new int[hauteur, largeur];
            int[,] pixels_vert = new int[hauteur, largeur];
            int[,] pixels_bleu = new int[hauteur, largeur];

            Random random = new Random();
            for (int x = 0; x < largeur; x++)
            {

                for (int y = 0; y < hauteur; y++)
                {
                    double c_r = (double)x / (double)zoom + x1;
                    double c_i = (double)y / (double)zoom + y1;
                    Complexe c = new Complexe(c_r, c_i);
                    Complexe z = new Complexe(0, 0);
                    int i = 0;
                    List<(double, double)> tempo_pixels = new List<(double, double)>(); //On utilise ce qu'on appelle des tuples pour stocker les coordonnées

                    do
                    {
                        z = z * z + c;
                        i++;
                        int imgAjuste = (int)Math.Ceiling((z.Img - y1) * zoom);
                        int reelAjuste = (int)Math.Ceiling((z.Reel - x1) * zoom);
                        tempo_pixels.Add((imgAjuste, reelAjuste)); //On ajoute les coordonnées du pixels réajuster à notre repère
                    } while (z.Module() < 2 && i < iteration_max);
                    bool test = false;
                    if (i >= iteration_rouge || i >= iteration_bleu || i >= iteration_vert) test = true;
                    if (i < iteration_rouge)
                    {
                        foreach ((int, int) pixel in tempo_pixels)
                        {
                            if (pixel.Item1 >= 0 && pixel.Item1 < hauteur && pixel.Item2 >= 0 && pixel.Item2 < largeur)
                            {
                                pixels_rouge[pixel.Item1, pixel.Item2]++;
                            }
                        }
                    }
                    if (i < iteration_vert)
                    {
                        foreach ((int, int) pixel in tempo_pixels)
                        {
                            if (pixel.Item1 >= 0 && pixel.Item1 < hauteur && pixel.Item2 >= 0 && pixel.Item2 < largeur)
                            {
                                pixels_vert[pixel.Item1, pixel.Item2]++;
                            }
                        }
                    }
                    if (i < iteration_bleu)
                    {
                        foreach ((int, int) pixel in tempo_pixels)
                        {
                            if (pixel.Item1 >= 0 && pixel.Item1 < hauteur && pixel.Item2 >= 0 && pixel.Item2 < largeur)
                            {
                                pixels_bleu[pixel.Item1, pixel.Item2]++;
                            }
                        }
                    }

                }
                Console.WriteLine($"Tour n°{x}");
            }
            int bleuMax = 1;
            int vertMax = 1;
            int rougeMax = 1;
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    bleuMax = (int)Math.Max(pixels_bleu[ligne, colonne], bleuMax);
                    vertMax = (int)Math.Max(pixels_vert[ligne, colonne], vertMax);
                    rougeMax = (int)Math.Max(pixels_rouge[ligne, colonne], rougeMax);
                }
            }
            int biggest = Math.Max(Math.Max(bleuMax, vertMax), rougeMax);
            double scale = 255.0 / biggest;
            for (int x = 0; x < largeur; x++)
            {
                for (int y = 0; y < hauteur; y++)
                {
                    //int moyenne = (int)Math.Ceiling((pixels_bleu[y, x] * 0.0114 + pixels_rouge[y, x] * 0.2126 + pixels_vert[y, x] * 0.2126));
                    //new_mat[y, x] = new Pixel(moyenne, moyenne, moyenne);
                    int bleuMin = (int)Math.Min(Math.Round(pixels_bleu[y, x] * scale), 255);
                    int vertMin = (int)Math.Min(Math.Round(pixels_vert[y, x] * scale), 255);
                    int rougeMin = (int)Math.Min(Math.Round(pixels_rouge[y, x] * scale), 255);

                    new_mat[y, x] = new Pixel(bleuMin, vertMin, rougeMin);
                }
            }
            nouvelle_image.mat_pixel = new_mat;
            nouvelle_image.From_Image_To_File($"{nouvelle_image.filename}", true);
            Console.WriteLine("Opération complétée.");
        }
        #endregion

        #endregion
    }
}
