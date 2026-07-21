using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problème_scientifique___Image
{
    public class QRCode
    {
        #region Variables

        private readonly int version;
        private static int versionn;
        private string filename;
        private int taille;
        private string typeEncodage;
        public static string typeencodage;
        private string messageEncoder;
        private static string msg;
        public string mot;
        private Pixel[,] mat_pixel;
        private int[,] mat_placement;
        private static int[,] mat_placement2;
        public int compteurMessageEncoder = 0;
        public int NbRemplissageQR = 0;


        private static char[] ListeLettres = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/', ':' };
        private static int[] nbCaractere_L = { 25, 47, 77, 114, 154, 195, 224, 279, 335, 395, 468, 535, 619, 667, 758, 854, 938, 1046, 1159, 1249, 1352, 1460, 1588, 1704, 1853, 1990, 2132, 2223, 2369, 2520, 2677, 2840, 3009, 3183, 3351, 3537, 3729, 3927, 4087, 4296 };
        private static int[] nbCaractere_M = { 20, 38, 61, 90, 122, 154, 178, 221, 262, 311, 366, 419, 483, 528, 600, 656, 734, 816, 909, 970, 1035, 1134, 1248, 1326, 1451, 1542, 1637, 1732, 1839, 1994, 2113, 2238, 2369, 2506, 2632, 2780, 2894, 3054, 3220, 3391 };
        private static int[] nbCaractere_Q = { 16, 29, 47, 67, 87, 108, 125, 157, 189, 221, 259, 296, 352, 376, 426, 470, 531, 574, 644, 702, 742, 823, 890, 963, 1041, 1094, 1172, 1263, 1322, 1429, 1499, 1618, 1700, 1787, 1867, 1966, 2071, 2181, 2298, 2420 };
        private static int[] nbCaractere_H = { 10, 20, 35, 50, 64, 84, 93, 122, 143, 174, 200, 227, 259, 283, 321, 365, 408, 452, 493, 557, 587, 640, 672, 744, 779, 864, 910, 958, 1016, 1080, 1150, 1226, 1307, 1394, 1431, 1530, 1591, 1658, 1774, 1852 };

        private static int[,] ErrorCorrectionCode_L = { { 19, 7, 1, 19, 0, 0 }, { 34, 10, 1, 34, 0, 0 }, { 55, 15, 1, 55, 0, 0 }, { 80, 20, 1, 80, 0, 0 }, { 108, 26, 1, 108, 0, 0 }, { 136, 18, 2, 68, 0, 0 }, { 156, 20, 2, 78, 0, 0 }, { 194, 24, 2, 97, 0, 0 }, { 232, 30, 2, 116, 0, 0 }, { 274, 18, 2, 68, 2, 69 }, { 324, 20, 4, 81, 0, 0 }, { 370, 24, 2, 92, 2, 93 }, { 428, 26, 4, 107, 0, 0 }, { 461, 30, 3, 115, 1, 116 }, { 523, 22, 5, 87, 1, 88 }, { 589, 24, 5, 98, 1, 99 }, { 647, 28, 1, 107, 5, 108 }, { 721, 30, 5, 120, 1, 121 }, { 795, 28, 3, 113, 4, 114 }, { 861, 28, 3, 107, 5, 108 }, { 932, 28, 4, 116, 4, 117 }, { 1006, 28, 2, 111, 7, 112 }, { 1094, 30, 4, 121, 5, 122 }, { 1174, 30, 6, 117, 4, 118 }, { 1276, 26, 8, 106, 4, 107 }, { 1370, 28, 10, 114, 2, 115 }, { 1468, 30, 8, 122, 4, 123 }, { 1531, 30, 3, 117, 10, 118 }, { 1631, 30, 7, 116, 7, 117 }, { 1735, 30, 5, 115, 10, 116 }, { 1843, 30, 13, 115, 3, 116 }, { 1955, 30, 17, 115, 0, 0 }, { 2071, 30, 17, 115, 1, 116 }, { 2191, 30, 13, 115, 6, 116 }, { 2306, 30, 12, 121, 7, 122 }, { 2434, 30, 6, 121, 14, 122 }, { 2566, 30, 17, 122, 4, 123 }, { 2702, 30, 4, 122, 18, 123 }, { 2812, 30, 20, 117, 4, 118 }, { 2956, 30, 19, 118, 6, 119 } };
        private static int[,] ErrorCorrectionCode_M = { { 16, 10, 1, 16, 0, 0 }, { 28, 16, 1, 28, 0, 0 }, { 44, 26, 1, 44, 0, 0 }, { 64, 18, 2, 32, 0, 0 }, { 86, 24, 2, 43, 0, 0 }, { 108, 16, 4, 27, 0, 0 }, { 124, 18, 4, 31, 0, 0 }, { 154, 22, 2, 38, 2, 39, }, { 182, 22, 3, 36, 2, 37 }, { 216, 26, 4, 43, 1, 44 }, { 254, 30, 1, 50, 4, 51 }, { 290, 22, 6, 36, 2, 37 }, { 334, 22, 8, 37, 1, 38 }, { 365, 24, 4, 40, 5, 41 }, { 415, 24, 5, 41, 5, 42 }, { 453, 28, 7, 45, 3, 46 }, { 507, 28, 10, 46, 1, 47 }, { 563, 26, 9, 43, 4, 44 }, { 627, 26, 3, 44, 11, 45 }, { 669, 26, 3, 41, 13, 42 }, { 714, 26, 17, 42, 0, 0 }, { 782, 28, 17, 46, 0, 0 }, { 860, 28, 4, 47, 14, 48, }, { 914, 28, 6, 45, 14, 46 }, { 1000, 28, 8, 47, 13, 48 }, { 1062, 28, 19, 46, 4, 47 }, { 1128, 28, 22, 45, 3, 46 }, { 1193, 28, 3, 45, 23, 46 }, { 1267, 28, 21, 45, 7, 46 }, { 1373, 28, 19, 47, 10, 48 }, { 1455, 28, 2, 46, 29, 47 }, { 1541, 28, 10, 46, 23, 47 }, { 1631, 28, 14, 46, 21, 47 }, { 1725, 28, 14, 46, 23, 47 }, { 1812, 28, 12, 47, 26, 48 }, { 1914, 28, 6, 47, 34, 48 }, { 1992, 28, 29, 46, 14, 47 }, { 2102, 28, 13, 46, 32, 47 }, { 2216, 28, 40, 47, 7, 48 }, { 2334, 28, 18, 47, 31, 48 } };
        private static int[,] ErrorCorrectionCode_Q = { { 13, 13, 1, 13, 0, 0 }, { 22, 22, 1, 22, 0, 0 }, { 34, 18, 2, 17, 0, 0 }, { 48, 26, 2, 24, 0, 0 }, { 62, 18, 2, 15, 2, 16 }, { 76, 24, 4, 19, 0, 0 }, { 88, 18, 2, 14, 4, 15 }, { 110, 22, 4, 18, 2, 19 }, { 132, 20, 4, 16, 4, 17 }, { 154, 24, 6, 19, 2, 20 }, { 180, 28, 4, 22, 4, 23 }, { 206, 26, 4, 20, 6, 21 }, { 244, 24, 8, 20, 4, 21 }, { 261, 20, 11, 16, 5, 17 }, { 295, 30, 5, 24, 7, 25 }, { 325, 24, 15, 19, 2, 20 }, { 367, 28, 1, 22, 15, 23 }, { 397, 28, 17, 22, 1, 23 }, { 445, 26, 17, 21, 4, 22 }, { 485, 30, 15, 24, 5, 25 }, { 512, 28, 17, 22, 6, 23, }, { 568, 30, 7, 24, 16, 25 }, { 614, 30, 11, 24, 14, 25 }, { 664, 30, 11, 24, 16, 25 }, { 718, 30, 7, 24, 22, 25 }, { 754, 28, 28, 22, 6, 23 }, { 808, 30, 8, 23, 26, 24 }, { 871, 30, 4, 24, 31, 25 }, { 911, 30, 1, 23, 37, 24 }, { 985, 30, 15, 24, 25, 25 }, { 1033, 30, 42, 24, 1, 25 }, { 1115, 30, 10, 24, 35, 25 }, { 1171, 30, 29, 24, 19, 25 }, { 1231, 30, 44, 24, 7, 25 }, { 1286, 30, 39, 24, 14, 25 }, { 1354, 30, 46, 24, 10, 25 }, { 1426, 30, 49, 24, 10, 25 }, { 1502, 30, 48, 24, 14, 25 }, { 1582, 30, 43, 24, 22, 25 }, { 1666, 30, 34, 24, 34, 25 } };
        private static int[,] ErrorCorrectionCode_H = { { 9, 17, 1, 9, 0, 0 }, { 16, 28, 1, 16, 0, 0 }, { 26, 22, 2, 13, 0, 0 }, { 36, 16, 4, 9, 0, 0 }, { 22, 2, 11, 2, 12, 0 }, { 60, 28, 4, 15, 0, 0 }, { 66, 26, 4, 13, 1, 14 }, { 86, 26, 4, 14, 2, 15 }, { 100, 24, 4, 12, 4, 13 }, { 122, 28, 6, 15, 2, 16 }, { 140, 24, 3, 12, 8, 13 }, { 158, 28, 7, 14, 4, 15 }, { 180, 22, 12, 11, 4, 12 }, { 197, 24, 11, 12, 5, 13 }, { 24, 11, 12, 7, 13, 0 }, { 253, 30, 3, 15, 13, 16 }, { 283, 28, 2, 14, 17, 15 }, { 313, 28, 2, 14, 19, 15 }, { 341, 26, 9, 13, 16, 14 }, { 385, 28, 15, 15, 10, 16 }, { 406, 30, 19, 16, 6, 17 }, { 442, 24, 34, 13, 0, 0 }, { 464, 30, 16, 15, 14, 16 }, { 514, 30, 30, 16, 2, 17 }, { 538, 30, 22, 15, 13, 16 }, { 596, 30, 33, 16, 4, 17 }, { 628, 30, 12, 15, 28, 16 }, { 661, 30, 11, 15, 31, 16 }, { 701, 30, 19, 15, 26, 16 }, { 745, 30, 23, 15, 25, 16 }, { 793, 30, 23, 15, 28, 16 }, { 845, 30, 19, 15, 35, 16 }, { 901, 30, 11, 15, 46, 16 }, { 961, 30, 59, 16, 1, 17 }, { 986, 30, 22, 15, 41, 16 }, { 1054, 30, 2, 15, 64, 16 }, { 1096, 30, 24, 15, 46, 16 }, { 1142, 30, 42, 15, 32, 16 }, { 1222, 30, 10, 15, 67, 16 }, { 1276, 30, 20, 15, 61, 16 } };

        private static int[,] Alignment_Pattern_Locations = { { -1, -1, -1, -1, -1, -1, -1 }, { 6, 18, -1, -1, -1, -1, -1 }, { 6, 22, -1, -1, -1, -1, -1 }, { 6, 26, -1, -1, -1, -1, -1 }, { 6, 30, -1, -1, -1, -1, -1 }, { 6, 34, -1, -1, -1, -1, -1 }, { 6, 22, 38, -1, -1, -1, -1 }, { 6, 24, 42, -1, -1, -1, -1 }, { 6, 26, 46, -1, -1, -1, -1 }, { 6, 28, 50, -1, -1, -1, -1 }, { 6, 30, 54, -1, -1, -1, -1 }, { 6, 32, 58, -1, -1, -1, -1 }, { 6, 34, 62, -1, -1, -1, -1 }, { 6, 26, 46, 66, -1, -1, -1 }, { 6, 26, 48, 70, -1, -1, -1 }, { 6, 26, 50, 74, -1, -1, -1 }, { 6, 30, 54, 78, -1, -1, -1 }, { 6, 30, 56, 82, -1, -1, -1 }, { 6, 30, 58, 86, -1, -1, -1 }, { 6, 34, 62, 90, -1, -1, -1 }, { 6, 28, 50, 72, 94, -1, -1 }, { 6, 26, 50, 74, 98, -1, -1 }, { 6, 30, 54, 78, 102, -1, -1 }, { 6, 28, 54, 80, 106, -1, -1 }, { 6, 32, 58, 84, 110, -1, -1 }, { 6, 30, 58, 86, 114, -1, -1 }, { 6, 34, 62, 90, 118, -1, -1 }, { 6, 26, 50, 74, 98, 122, -1 }, { 6, 30, 54, 78, 102, 126, -1 }, { 6, 26, 52, 78, 104, 130, -1 }, { 6, 30, 56, 82, 108, 134, -1 }, { 6, 34, 60, 86, 112, 138, -1 }, { 6, 30, 58, 86, 114, 142, -1 }, { 6, 34, 62, 90, 118, 146, -1 }, { 6, 30, 54, 78, 102, 126, 150 }, { 6, 24, 50, 76, 102, 128, 154 }, { 6, 28, 54, 80, 106, 132, 158 }, { 6, 32, 58, 84, 110, 136, 162 }, { 6, 26, 54, 82, 110, 138, 166 }, { 6, 30, 58, 86, 114, 142, 170 } };
        public static string[] Information_StringsL = { "111011111000100", "111001011110011", "111110110101010", "111100010011101", "110001100011000", "110110001000001", "110100101110110" };
        public static string[] Information_StringsM = { "101010000010010", "101000100100101", "101111001111100", "101101101001011", "100000011001110", "100111110010111", "100101010100000" };
        public static string[] Information_StringsQ = { "011010101011111", "011000001101000", "011111100110001", "011101000000110", "010000110000011", "010111011011010", "010101111101101" };
        public static string[] Information_StringsH = { "001011010001001", "001001110111110", "001110011100111", "001100111010000", "000001001010101", "000110100001100", "000100000111011" };


        #endregion

        /// <summary>
        /// Constructeur du QRCode
        /// </summary>
        /// <param name="message">Message qu'on veut encoder</param>
        /// <param name="encodage_type">Niveau d'encodage. Prend comme valeur : "L", "M", "Q" ou "H"</param>
        public QRCode(string message, string encodage_type)
        {
            encodage_type = BonFormat(encodage_type, message);
            this.version = QRCode.ChoixNiveauEncodage(message.ToUpper(), encodage_type); //Détermine la version du QRCode
            versionn = QRCode.ChoixNiveauEncodage(message.ToUpper(), encodage_type);
            this.typeEncodage = encodage_type; //Donne le type d'encodage (L, M, Q ou H)
            typeencodage = encodage_type;
            mot = message;
            this.messageEncoder = EncoderQRCode(message.ToUpper(), encodage_type); //Calcul du message encoder selon les normes QRCode
            msg = messageEncoder;

            this.taille = (((this.version - 1) * 4) + 21);
            this.mat_pixel = new Pixel[taille, taille]; //Création d'une matrice de pixel qui aura la bonne taille selon la version
            this.mat_placement = new int[taille, taille]; //Création d'une matrice utile pour le placement des bits qui aura la bonne taille selon la version

            for (int i = 0; i < mat_placement.GetLength(0); i++)
            {
                for (int j = 0; j < mat_placement.GetLength(1); j++)
                {
                    mat_placement[i, j] = 0;
                }
            }
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    this.mat_pixel[i, j] = new Pixel(255, 0, 0);
                }
            }
            mat_placement2 = mat_placement;
            this.AddFinderPatterns();
            this.AddSeparators();
            this.AddTimingPatterns();
            this.AddAlignmentPatterns();
            this.NoUseEmplacement();
            //this.AddFinderPatterns();

            this.DataBitsPlacement();

            int valMask = BestMask();

            mat_placement = AddInformationStrings(valMask, mat_placement);

            int[,] matplacement2 = Mirror(mat_placement);
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    if (matplacement2[i, j] == 1)
                    {
                        this.mat_pixel[i, j] = new Pixel(0, 0, 0);
                    }
                    if (matplacement2[i, j] == 0)
                    {
                        this.mat_pixel[i, j] = new Pixel(255, 255, 255);
                    }

                }
            }


            MyImage QR = new MyImage("QRCode" + DateTime.Now.ToString("yyyy-MM-ddHHmmss"), taille, taille);
            this.filename = QR.Filename;
            QR.Mat_pixel = this.mat_pixel;
            QR.Taille_fichier = QR.Nb_bit * QR.Hauteur * QR.Largeur;
            QR.FileSet = new byte[QR.Taille_offset + ((QR.Largeur * 3) + QR.Octet_manquant) * (QR.Hauteur)];
            QR.From_Image_To_File(this.filename, true);
        }

        #region Propriétés
        public string Filename
        {
            get { return this.filename; }
        }
        public int Version
        {
            get { return this.version; }
        }
        public string TypeEncodage
        {
            get { return this.typeEncodage; }
        }
        public string MessageEncoder
        {
            get { return this.messageEncoder; }
        }
        public int[,] Mat_placement
        {
            get { return this.mat_placement; }
        }
        public Pixel[,] Mat_Pixel
        {
            get { return mat_pixel; }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Choisit le niveua de l'encodage en fonction de la taille de la chaine de charateres
        /// </summary>
        /// <param name="mot">chaine de caracteres a encoder</param>
        /// <param name="typeEncodage">type d'encodage choisis</param>
        /// <returns></returns>
        public static int ChoixNiveauEncodage(string mot, string typeEncodage)
        {
            int taille_mot = mot.Length;
            bool stop = false;
            int value = -1;
            int[] val = { };
            if (typeEncodage == "Q")
            {
                val = nbCaractere_Q;
            }
            if (typeEncodage == "H")
            {
                val = nbCaractere_H;
            }
            if (typeEncodage == "L")
            {
                val = nbCaractere_L;
            }
            if (typeEncodage == "M")
            {
                val = nbCaractere_M;
            }
            for (int i = 0; i < val.Length && !stop; i++)
            {
                if (taille_mot <= val[i])
                {
                    value = i + 1; // comme les versions vont de 1 a 40 et le tableau va de 0 a 39
                    stop = true;
                }
            }
            return value;
        }
        /// <summary>
        /// verifie si la chaine de caracteres existe en alphanum
        /// </summary>
        /// <param name="chaine">chiane de caraceteres a examiner</param>
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
        /// Convertie une chaine de caracteres en les valeures qu'il faut pour encoder le message (par paires de lettres)
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns></returns>
        public string FromStringToBinary11(string chaine)
        {
            bool paire = true;
            string ValueFinale = "";
            int padding = 0;

            if (version <= 9) padding = 11;
            if (Version >= 10 && Version <= 26) padding = 11;
            if (Version >= 27 && Version <= 40) padding = 13;
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

                    int res = Methods.FromCharToAlphNum(valfaible) + 45 * Methods.FromCharToAlphNum(valfort); //On fait l'opération : (45^0)*LettreFaible + (45^1)*LettreForte
                    string resstring = Methods.FromBaseXToY(Convert.ToString(res), 10, 2); //On convertit celle valeur décimale en binaire (comme dans l'énoncé)
                    resstring = resstring.PadLeft(padding, '0'); //On est en valeur paire on doit remplir de 0 jusqu'à 11 caractères 
                    ValueFinale += resstring + ' ';

                }
            }
            else
            {
                for (int i = 0; i < chaine.Length - 2; i += 2)
                {
                    char valfort = chaine[i];
                    char valfaible = chaine[i + 1];

                    int res = Methods.FromCharToAlphNum(valfaible) + 45 * Methods.FromCharToAlphNum(valfort); //Même principe que le dernier 
                    string resstring = Methods.FromBaseXToY(Convert.ToString(res), 10, 2);
                    resstring = resstring.PadLeft(padding, '0');
                    ValueFinale += resstring + ' ';

                }
                int res2 = Methods.FromCharToAlphNum(chaine[chaine.Length - 1]);
                string res2string = Methods.FromBaseXToY(Convert.ToString(res2), 10, 2);
                res2string = res2string.PadLeft(6, '0');
                ValueFinale += res2string + " ";
            }
            return ValueFinale;
        }

        /// <summary>
        /// Transforme une séquence binaire de 11 bits (et 6 bits dans le cas impair pour le dernier) en string
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns></returns>
        public static string FromBinary11ToString(string chaine)
        {
            string ValueFinale = "";
            string[] chaineBinaire = chaine.Split(' ');
            for (int i = 0; i < chaineBinaire.Length - 1; i++)
            {
                int valeurDecimal = Convert.ToInt32(Methods.FromBaseXToY(chaineBinaire[i], 2, 10));
                int lettreFaibleDecimal = valeurDecimal % 45;
                int lettreForteDecimal = (valeurDecimal - lettreFaibleDecimal) / 45;

                char lettreFaible = ListeLettres[lettreFaibleDecimal];
                char lettreForte = ListeLettres[lettreForteDecimal];
                if (lettreForteDecimal == 0 && lettreFaibleDecimal != 0) //Si on a une chaine impaire ET qu'on est à la dernière lettre. Alors forcément il y aura un vide qu'on ne doit pas prendre
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

        /// <summary>
        /// Encoder un Mot pour obtenir un QRCode
        /// </summary>
        /// <param name="mot">Mot original a encoder</param>
        /// <param name="motBinaire">Conversion du mot en binaire avec la fonction from string to binary 11 (probleme de fonction statique)</param>    
        /// <returns></returns>
        public string EncoderQRCode(string mot, string typeEncodage)
        {
            string result = "0010"; //Valeur de base pour encoder en alphanumerique
            string motBinaire = FromStringToBinary11(mot); //On encode en binaire le mot 

            int tailleMot = mot.Length;
            int padd = 9;
            if (version < 27 && version > 9)//choisir le bon padding en fonction des versions
            {
                padd = 11;
            }
            if (version < 41 && version > 26)
            {
                padd = 13;
            }
            string tailleMotBits = Methods.FromBaseXToY(Convert.ToString(tailleMot), 10, 2); //Taille du mot en binaire
            tailleMotBits = tailleMotBits.PadLeft(padd, '0'); //pad a gauche pour avoir obligatoirement 9 characteres

            result += tailleMotBits;

            string[] tabMotSansEspace = motBinaire.Split(' '); //retirer les espaces mis au préalable pour faciliter la lecture
            for (int i = 0; i < tabMotSansEspace.Length; i++)
            {
                result += tabMotSansEspace[i];
            }

            int[,] val = { { } };   //choix des bonnes valeurs en fonction du type d'encodage
            if (typeEncodage == "Q")
            {
                val = ErrorCorrectionCode_Q;
            }
            if (typeEncodage == "H")
            {
                val = ErrorCorrectionCode_H;
            }
            if (typeEncodage == "L")
            {
                val = ErrorCorrectionCode_L;
            }
            if (typeEncodage == "M")
            {
                val = ErrorCorrectionCode_M;
            }
            int data_bits_required = val[ChoixNiveauEncodage(mot, typeEncodage) - 1, 0] * 8;   //le -1 permet d'avoir la bonne position dans le tableau (un encodage de niveau 1 est la valeur 0 du tableau)  le 0 est du au fait que l'on prends la valeur en position 0 qui nous est utile, et on multiplie le tout par 8  

            if (result.Length < data_bits_required)   //Rajouter le terminator de max 4 "0" si la taille du mot est inferieure a la taille requise
            {
                for (int i = 0; i < 4; i++)
                {
                    if (result.Length < data_bits_required)
                    {
                        result += "0";
                    }
                }
            }
            int NbBitsManquantDivisiblePar8 = 0;    //rajouter le nombre de 0 requis pour avoir un resultat divisible par 8
            if (result.Length < data_bits_required)
            {
                NbBitsManquantDivisiblePar8 = 8 - (result.Length % 8);
            }
            for (int i = 0; i < NbBitsManquantDivisiblePar8; i++)
            {
                result += "0";
            }

            int AjoutFinal = (data_bits_required - result.Length) / 8;
            for (int i = 0; i < AjoutFinal; i++) //ajouter la chaine de caractere pre definie en fonction de la paritee du reste
            {
                if (i % 2 == 0)
                {
                    result += "11101100";
                }
                if (i % 2 == 1)
                {
                    result += "00010001";
                }
            }
            int niv = val[ChoixNiveauEncodage(mot, typeEncodage) - 1, 1];


            //if (version == 1)
            {

                //byte[] QR_tabBits = FromStringEncodeToByteArray(result, niv, val);
                //result = AddCorrectionErreur(result, QR_tabBits);
                result = FromStringEncodeToByteArrayToResult(result, niv, val);
            }



            return result;
        }
        /// <summary>
        /// Transformer la chaine de l'encodage "EncoderQRCode" en chaine de byte pour pouvoir utiliser Reed Salomon
        /// </summary>
        /// <param name="result">Chaine "EncoderQRCode"</param>
        /// <returns></returns>

        public static string FromStringEncodeToByteArrayToResult(string result, int niv, int[,] tabval)
        {

            string resultt = "";
            byte[] resfinal;
            byte[] res = new byte[result.Length / 8]; //taille du tableau
            int cpt = 0; //compteur pour parcourir la chaine de caractere
            for (int i = 0; i < res.Length; i++)
            {
                string bytemot = ""; //mot de taille 8 bits
                for (int j = 0; j < 8; j++) //remplir le mot 8 par 8
                {

                    bytemot += result[cpt];

                    cpt++;
                }
                byte BYTE = (byte)Convert.ToInt32(Methods.FromBaseXToY(bytemot, 2, 10)); //convertir le mot en base 10 pour le mettre dans le tableau de bytes

                res[i] = BYTE;

            }
            if (versionn < 2)
            {
                resfinal = ReedSolomonAlgorithm.Encode(res, niv, ErrorCorrectionCodeType.QRCode); //Application de Reed Salomon
                resultt = AddCorrectionErreur(result, resfinal);
            }
            else //pour les versions superieures a 2 il ya un autre pattern a suivre avec un melange entre les valeurs de notre chaine encodee et les valeurs calculees par l'algo de reed solomon
            {

                int a = tabval[versionn - 1, 2];
                int b = tabval[versionn - 1, 3];
                int c = tabval[versionn - 1, 4];
                int d = tabval[versionn - 1, 5];
                int cpt2 = 0;
                List<byte[]> listeB1 = new List<byte[]>();
                List<byte[]> listeCorB1 = new List<byte[]>();
                List<byte[]> listeB2 = new List<byte[]>();
                List<byte[]> listeCorB2 = new List<byte[]>();

                //recuperation de tout les blocks des groupes 1 et 2 + leurs correction

                for (int i = 0; i < a; i++)
                {
                    byte[] liste = new byte[b];
                    for (int j = 0; j < b; j++)
                    {
                        liste[j] = res[cpt2];
                        cpt2++;
                    }
                    listeB1.Add(liste);

                    byte[] liste2 = ReedSolomonAlgorithm.Encode(liste, niv, ErrorCorrectionCodeType.QRCode); ;

                    listeCorB1.Add(liste2);
                }
                for (int i = 0; i < c; i++)
                {
                    byte[] liste = new byte[d];
                    for (int j = 0; j < d; j++)
                    {
                        liste[j] = res[cpt2];
                        cpt2++;

                    }
                    listeB1.Add(liste);
                    byte[] liste2 = ReedSolomonAlgorithm.Encode(liste, niv, ErrorCorrectionCodeType.QRCode); ;
                    listeCorB1.Add(liste2);
                }

                List<byte> listfinalee = new List<byte>();
                List<byte> listfinaleeCor = new List<byte>();
                // rengement dans le bon ordre 
                for (int i = 0; i < b; i++)
                {
                    for (int j = 0; j < a + c; j++)
                    {
                        listfinalee.Add(listeB1[j][i]);

                    }
                }
                for (int i = 0; i < c; i++)
                {
                    listfinalee.Add(listeB1[a + i][d - 1]);

                }
                for (int i = 0; i < listeCorB1[0].Length; i++)
                {
                    for (int j = 0; j < listeCorB1.Count; j++)
                    {
                        listfinaleeCor.Add(listeCorB1[j][i]);
                    }
                }
                for (int i = 0; i < listfinaleeCor.Count; i++)
                {
                    listfinalee.Add(listfinaleeCor[i]);
                }

                //transformation en binaire
                resultt = AddCorrectionErreur("", listfinalee.ToArray());


                if (versionn == 2 || versionn == 3 || versionn == 4 || versionn == 5 || versionn == 6)//ajouts des bits manquant en fonction des versions
                {
                    for (int i = 0; i < 7; i++)
                    {
                        resultt += "0";
                    }
                }
                if (versionn == 14 || versionn == 15 || versionn == 16 || versionn == 17 || versionn == 18 || versionn == 19 || versionn == 20 || versionn == 28 || versionn == 29 || versionn == 30 || versionn == 31 || versionn == 33 || versionn == 32 || versionn == 34)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        resultt += "0";
                    }
                }
                if (versionn == 21 || versionn == 22 || versionn == 23 || versionn == 24 || versionn == 25 || versionn == 26 || versionn == 27)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        resultt += "0";
                    }
                }

            }
            return resultt;
        }
        /// <summary>
        /// Apres avoir effectuer Reed Salomon on rajoute les bits a notre chaine de caractere initiale
        /// </summary>
        /// <param name="QR">Chaine "EncoderQRCode"</param>
        /// <param name="chaine">Resultat Reed Salomon</param>
        /// <returns></returns>
        public static string AddCorrectionErreur(string QR, byte[] chaine)
        {
            if (versionn == 1)
            {
                for (int i = 0; i < chaine.Length; i++)
                {
                    string corr = Methods.FromBaseXToY(Convert.ToString(chaine[i]), 10, 2); //Correction qu'on rajoute successivement à QR du tableau de décimal (en transformant en binaire)
                    corr = corr.PadLeft(8, '0');
                    QR += corr;
                }
            }
            else
            {
                string corr2 = "";
                for (int i = 0; i < chaine.Length; i++)
                {
                    string corr = Methods.FromBaseXToY(Convert.ToString(chaine[i]), 10, 2); //Correction qu'on rajoute successivement à QR du tableau de décimal (en transformant en binaire)
                    corr = corr.PadLeft(8, '0');
                    corr2 += corr;
                }
                QR = corr2;
            }

            return QR;
        }
        public void AddFinderPatterns()  // Ajout des finder paterns a notre matrice (num 3 pour la mat placement)
        {
            int decrement = 0;
            for (int compteur = 0; compteur < 3; compteur++, decrement += 2) //On créé trois carré successivement en réduisant son côté de 2 à chaque fois pour faire un pattern : 7-5-3
            {
                for (int ligne = 0; ligne < 7 - decrement; ligne++) //Création d'un carré de modules 
                {
                    for (int colonne = 0; colonne < 7 - decrement; colonne++)
                    {
                        if (compteur % 2 == 0) //On est sur un carré noir
                        {
                            this.mat_pixel[ligne + compteur, colonne + compteur] = new Pixel(0, 0, 0); //Finder pattern en bas à gauche
                            this.mat_pixel[this.taille - 1 - (ligne + compteur), colonne + compteur] = new Pixel(0, 0, 0); //Finder pattern en haut à gauche
                            this.mat_pixel[this.taille - 1 - (ligne + compteur), this.taille - 1 - (colonne + compteur)] = new Pixel(0, 0, 0); //Finderpattern en haut à droite


                        }
                        else //On est sur un carré blanc
                        {
                            this.mat_pixel[ligne + compteur, colonne + compteur] = new Pixel(255, 255, 255); //Finder pattern en bas à gauche
                            this.mat_pixel[this.taille - 1 - (ligne + compteur), colonne + compteur] = new Pixel(255, 255, 255); //Finder pattern en haut à gauche
                            this.mat_pixel[this.taille - 1 - (ligne + compteur), this.taille - 1 - (colonne + compteur)] = new Pixel(255, 255, 255); //Finderpattern en haut à droite
                        }
                    }
                }
            }

            //mat de placement 

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    mat_placement[i, j] = 3;
                    mat_placement[mat_placement.GetLength(0) - 1 - i, j] = 3;
                    mat_placement[i, mat_placement.GetLength(0) - 1 - j] = 3;
                }
            }
        }
        public void AddSeparators() // Ajout des sepparators sur la matrice (num 4 pour la mat placement)
        {
            //Separators pour finder pattern en bas à gauche
            Methods.DrawStraigthLine(this.mat_pixel, 7, 8, "w", "h");
            Methods.DrawStraigthLine(this.mat_pixel, 7, 9, "w", "v");

            //Separators pour finder pattern en haut à gauche 
            Methods.DrawStraigthLine(this.mat_pixel, this.taille - 8, 8, "w", "h");
            Methods.DrawStraigthLine(this.mat_pixel, 7, 9, "w", "v", start: this.taille - 8);

            //Separators pour finder pattern en haut à droite
            Methods.DrawStraigthLine(this.mat_pixel, this.taille - 8, 8, "w", "h", start: this.taille - 7);
            Methods.DrawStraigthLine(this.mat_pixel, this.taille - 8, 9, "w", "v", start: this.taille - 8);

            //Partie matrice placement :

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // en haut a gauche
                    mat_placement[i, 7] = 4;
                    mat_placement[7, j] = 4;
                    //en bas a gauche 
                    mat_placement[mat_placement.GetLength(0) - 1 - i, 7] = 4;
                    mat_placement[mat_placement.GetLength(0) - 1 - 7, j] = 4;
                    mat_placement[7, mat_placement.GetLength(0) - 1 - j] = 4;
                    mat_placement[i, mat_placement.GetLength(0) - 1 - 7] = 4;
                }
            }

        }
        public void AddAlignmentPatterns() // Ajout des alignmentPatterns (num 5 pour la mat placement)
        {
            if (version > 1)
            {
                List<int> Coords = new List<int>();
                for (int i = 0; i < 7; i++)
                {
                    if (Alignment_Pattern_Locations[version - 1, i] != -1)
                    {
                        Coords.Add(Alignment_Pattern_Locations[version - 1, i]); // ajouter a une liste toutes les coordonnes a partir de la liste des valeurs en fonctions des versions Alignment_Pattern_Locations (on utilise une liste pour ne pas prendre en compte les -1 qui servaient juste a avoir une taille de 7 partout)
                    }
                }
                int min = Coords[0];
                int max = Coords[0];
                for (int i = 0; i < Coords.Count; i++) //determiner le min et le max de la liste (utile pour placer les patterns)
                {
                    if (Coords[i] <= min)
                    {
                        min = Coords[i];
                    }
                    if (Coords[i] >= min)
                    {
                        max = Coords[i];
                    }
                }
                List<int[]> AjoutPatterns = new List<int[]>();
                int[] value1 = { min, min }; // en bas a gauche   Pour retirer les alinment parterns sur les finding paterns
                int[] value2 = { max, min };  // en haut a gauche
                int[] value3 = { max, max }; // en haut a droite 

                for (int i = 0; i < Coords.Count; i++)
                {
                    for (int j = 0; j < Coords.Count; j++)
                    {
                        int[] localisation = { Coords[i], Coords[j] };
                        bool faire = true;
                        if (localisation[0] == value1[0] && localisation[1] == value1[1]) // faire en sorte de ne rien ajouter si on se trouve sur le patern en bas a gauche
                        {
                            faire = false;
                        }
                        if (localisation[0] == value2[0] && localisation[1] == value2[1]) // faire en sorte de ne rien ajouter si on se trouve sur le patern en haut a gauche
                        {
                            faire = false;
                        }
                        if (localisation[0] == value3[0] && localisation[1] == value3[1])// faire en sorte de ne rien ajouter si on se trouve sur le patern en haut a droite
                        {
                            faire = false;
                        }
                        if (faire == true)
                        {
                            AjoutPatterns.Add(localisation);

                        }

                    }
                }
                for (int i = 0; i < AjoutPatterns.Count; i++) //ajouter les paterns
                {
                    int[] aled = AjoutPatterns[i];
                    int x = aled[0];
                    int y = aled[1];
                    Methods.CreateModule(mat_pixel, x, y); //creation de l'alinment patern

                    // Mat_placement (5) : 
                    for (int s = x - 2; s <= x + 2; s++)
                    {
                        for (int t = y - 2; t <= y + 2; t++)
                        {
                            mat_placement[mat_placement.GetLength(0) - 1 - s, t] = 5;
                        }
                    }
                }

                mat_pixel[7, 8] = new Pixel(0, 0, 0); //ajout du pixel noir en bas a gauche
                mat_placement[mat_placement.GetLength(0) - 1 - 7, 8] = 9;  //ajout du pixel noir en bas a gauche sur la matrice de placement

            }
        }
        public void AddTimingPatterns() //Ajout des timing patterns ( num 7 pour la mat placement )
        {
            Methods.DrawStraigthLine(this.mat_pixel, 6, this.taille - 12, "alternance", start: 7); //Timing pattern vertical
            Methods.DrawStraigthLine(this.mat_pixel, this.taille - 7, this.taille - 14, "alternance", "h", start: 8); //Timing pattern horizontal

            for (int i = 8; i < mat_placement.GetLength(1) - 1 - 7; i++)
            {
                mat_placement[i, 6] = 7;
                mat_placement[6, i] = 7;
            }
        }
        public void NoUseEmplacement()  //Reservr la partie des informations des donnees pour ne pas entrer des valeurs a l'interieur ( num 6 pour la mat placement )
        {
            if (version < 7) //Pour les versions inferieures a 7
            {
                for (int i = 0; i < 7; i++) // Ligne coin en bas a gauche
                {
                    mat_pixel[i, 8] = new Pixel(0, 255, 0);
                    mat_placement[mat_placement.GetLength(0) - 1 - i, 8] = 6;
                }
                for (int i = 0; i < 9; i++)  // contour en haut a gauche
                {
                    if (i != 6)
                    {
                        mat_pixel[mat_pixel.GetLength(0) - 1 - 8, i] = new Pixel(0, 255, 0); //ligne
                        mat_placement[8, i] = 6;
                    }
                    if (i != 2)
                    {
                        mat_pixel[mat_pixel.GetLength(0) - 1 - 8 + i, 8] = new Pixel(0, 255, 0); //colonne
                        mat_placement[8 - i, 8] = 6;
                    }
                }
                for (int i = 0; i < 8; i++) // ligne en haut a droite
                {
                    mat_pixel[mat_pixel.GetLength(0) - 1 - 8, mat_pixel.GetLength(0) - 1 - 7 + i] = new Pixel(0, 255, 0);
                    mat_placement[8, mat_placement.GetLength(0) - 1 - i] = 6;
                }
            }
            else // pour les versions supperieures a 7
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        mat_pixel[8 + i, j] = new Pixel(0, 255, 0); // En bas a gauche
                        mat_pixel[mat_pixel.GetLength(0) - 1 - j, mat_pixel.GetLength(1) - 1 - 8 - i] = new Pixel(0, 255, 0);  // En haut a droite

                        //mat placement 
                        mat_placement[mat_placement.GetLength(0) - 1 - 8 - i, j] = 6;
                        mat_placement[j, mat_placement.GetLength(1) - 1 - 8 - i] = 6;
                    }
                }

            }
        }
        /// <summary>
        /// Mirror horizontal d'une matrice (pour la transcformation en image plus tard)
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static int[,] Mirror(int[,] mat)
        {
            int[,] mat3 = new int[mat.GetLength(0), mat.GetLength(1)];
            for (int i = 0; i < mat3.GetLength(0); i++)
            {
                int cpt = mat3.GetLength(0) - 1;
                for (int j = 0; j < mat3.GetLength(1); j++)
                {
                    mat3[i, j] = mat[cpt - i, j];
                }
            }
            return mat3;
        }
        /// <summary>
        ///  Permettre un ajout en zigzag d'un mot dans une matrice (Descente) 
        /// </summary>
        /// <param name="motAdd">Mot a ajouter (toujours divisible par deux)</param>
        /// <param name="mat"></param>
        /// <param name="xDebut"></param>
        /// <param name="yDebut"></param>
        /// <returns></returns>
        public static int[,] PatternZigZagDescente(string motAdd, int[,] mat, int xDebut, int yDebut)
        {
            int n = 2;
            int[,] arr = new int[motAdd.Length / 2, 2];
            int[] motchiff = new int[motAdd.Length];
            int cpt = 0;
            for (int i = 0; i < motAdd.Length; i++)
            {
                if (motAdd[i] == '0')
                {
                    motchiff[i] = 0;
                }
                else
                {
                    motchiff[i] = 1;
                }
            }
            for (int k = 0; k < arr.GetLength(0); k++)
            {
                for (int i = 0; i < n; i++)
                {

                    if (i % 2 == 0)
                    {
                        mat[xDebut + k, yDebut] = motchiff[cpt];
                        cpt++;

                    }
                    else
                    {
                        mat[xDebut + k, yDebut - 1] = motchiff[cpt];
                        cpt++;

                    }

                }
            }

            return mat;
        }
        /// <summary>
        ///  Permettre un ajout en zigzag d'un mot dans une matrice (remontee)
        /// </summary>
        /// <param name="motAdd">Mot a ajouter (toujours divisible par deux)</param>
        /// <param name="mat"></param>
        /// <param name="xDebut"></param>
        /// <param name="yDebut"></param>
        /// <returns></returns>
        public static int[,] PatternZigZagMontee(string motAdd, int[,] mat, int xDebut, int yDebut) // motDebut divisible par 2
        {

            int n = 2;
            int[,] arr = new int[motAdd.Length / 2, 2];
            int[] motchiff = new int[motAdd.Length];
            int cpt = 0;
            for (int i = 0; i < motAdd.Length; i++)
            {
                if (motAdd[i] == '0')
                {
                    motchiff[i] = 0;
                }
                else
                {
                    motchiff[i] = 1;
                }
            }
            for (int k = 0; k < arr.GetLength(0); k++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i % 2 == 0)
                    {
                        mat[xDebut - k, yDebut] = motchiff[cpt];
                        cpt++;

                    }
                    else
                    {
                        mat[xDebut - k, -1 + yDebut] = motchiff[cpt];
                        cpt++;
                    }

                }
            }

            return mat;
        }
        /// <summary>
        /// Placer le mot encodee en binaire dans la matrice du qrcode
        /// </summary>
        public void DataBitsPlacement()
        {
            int[,] mat_placement2 = new int[mat_placement.GetLength(0), mat_placement.GetLength(1)]; //creation d'une copie de matrice a modifier
            for (int i = 0; i < mat_placement.GetLength(0); i++)
            {
                for (int j = 0; j < mat_placement.GetLength(1); j++)
                {
                    mat_placement2[i, j] = mat_placement[i, j];
                }
            }
            int cptlig = mat_placement2.GetLength(0) - 1; //ligne debut
            int cptcol = mat_placement2.GetLength(1) - 1; //colone debut
            bool montee = true; //verifie si on est en remontee ou en descente
            int cptmot = 0; //compteur des lettres du mots pour renger le mot dans l'ordre voulu
            bool fin = false;
            while (fin == false)
            {

                if (cptmot >= messageEncoder.Length) //condition de sortie quand le mot est entierement remplis
                {
                    fin = true;
                }
                else
                {
                    bool fait = false;
                    if (version < 7) //versions inferieures a 7
                    {
                        if (montee)
                        {
                            while (fait == false) //faire une montee jusqu'a arriver au stade de la descente
                            {
                                string MotAdd = PartieARajouter(mat_placement2, cptlig, cptcol, cptmot, montee); //calcul de la taille du mot a rajoutee avant le changement
                                cptmot += MotAdd.Length;
                                if (MotAdd.Length != 0)
                                {
                                    PatternZigZagMontee(MotAdd, mat_placement2, cptlig, cptcol); //placement dans la matrice
                                    cptlig -= MotAdd.Length / 2; // car on remplis 2 col a la foit
                                    cptlig++; // toujours un de trop donc a rajouter
                                    cptcol--;
                                }
                                if (cptlig - 1 < 0) //passage en descente car on a atteint le max en montee
                                {
                                    fait = true;
                                    cptcol--; // reculer une colonne pour permettre la descente
                                }
                                else
                                {
                                    if (mat_placement2[cptlig - 1, cptcol] == 6 && mat_placement2[cptlig - 1, cptcol + 1] == 6) //on passe a la descente car on atteint un modul qui nous fait changer de sens
                                    {

                                        fait = true;
                                        cptcol--; // reculer une colonne pour permettre la descente
                                    }
                                    if (mat_placement2[cptlig - 1, cptcol] == 5 && mat_placement2[cptlig - 1, cptcol + 1] == 5) //on saute le module d'alignment patern
                                    {

                                        cptlig -= 6;
                                        cptcol++;
                                    }
                                    if (mat_placement2[cptlig - 1, cptcol] == 0 && mat_placement2[cptlig - 1, cptcol + 1] == 5) //on place sur la partie de la remontee en diagonale qui n'est pas l'allinment patern
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            mat_placement2[cptlig - 1 - i, cptcol] = Convert.ToInt32(Convert.ToString(messageEncoder[cptmot]));
                                            cptmot++;
                                        }
                                        cptlig -= 6;
                                        cptcol++;
                                    }
                                    if (mat_placement2[cptlig - 1, cptcol] == 7 && mat_placement2[cptlig - 1, cptcol + 1] == 7) //on saute la ligne du timing patern
                                    {


                                        cptlig -= 2;
                                        cptcol++;
                                    }
                                    if (cptcol == 8 && cptlig == mat_placement2.GetLength(0) - 1) // on arrive a cote du finder pater en bas a gauche dont on monte jusqu'a arriver a des valeurs a edit
                                    {
                                        cptlig -= 8;
                                    }
                                    if (cptcol == 6) //on saute le timing patern
                                    {
                                        cptcol--;
                                    }
                                }
                            }
                            montee = false;
                        }
                        else
                        {
                            while (fait == false) //de meme pour la descente
                            {
                                string MotAdd = PartieARajouter(mat_placement2, cptlig, cptcol, cptmot, montee);
                                cptmot += MotAdd.Length;
                                if (MotAdd.Length != 0)
                                {
                                    PatternZigZagDescente(MotAdd, mat_placement2, cptlig, cptcol);
                                    cptlig += MotAdd.Length / 2; // car on remplis 2 col a la foit
                                    cptlig--; // toujours un de trop donc a rajouter
                                    cptcol--;
                                }
                                if (cptlig + 1 >= mat_placement2.GetLength(0))
                                {
                                    fait = true;
                                    cptcol--; // reculer une colonne pour permettre la descente
                                }
                                else
                                {
                                    if (mat_placement2[cptlig + 1, cptcol] == 5 && mat_placement2[cptlig + 1, cptcol + 1] == 5)
                                    {

                                        cptlig += 6;
                                        cptcol++;
                                    }
                                    if (mat_placement2[cptlig + 1, cptcol] == 7 && mat_placement2[cptlig + 1, cptcol + 1] == 7)
                                    {


                                        cptlig += 2;
                                        cptcol++;
                                    }
                                    if (mat_placement2[cptlig + 1, cptcol] == 4 && mat_placement2[cptlig + 1, cptcol + 1] == 4)
                                    {

                                        fait = true;
                                        cptcol--; // reculer une colonne pour permettre la descente
                                    }
                                }
                            }
                            montee = true;
                            mat_placement = mat_placement2;
                        }
                    }
                    else //version supperieure a 7
                    {

                        int cpt = 0;
                        for (int i = 0; i < (taille - 1) / 2; i++) //on parcour la matrice et l'on place des lettre lorsque c'est possible de le placer
                        {
                            if (i % 2 == 0)
                            {

                                cpt = ZigzagM(cpt, cptlig, cptcol - i * 2); //on recupere le compteur du mot pour par la suite pouvoir ajiuter des lettres a partir de ce compteur
                                cptlig = 0; //on se positionne en haut pour la descente
                            }
                            else
                            {

                                cpt = ZigzagD(cpt, cptlig, cptcol - i * 2); //on recupere le compteur du mot pour par la suite pouvoir ajiuter des lettres a partir de ce compteur
                                cptlig = mat_placement2.GetLength(1) - 1; //on se positionne en bas pour la remontee

                            }




                        }
                        fin = true;
                    }
                }

            }

        }
        /// <summary>
        /// Remplir une matrice en diagolane montee quand c'est possible a partir d'un mot et d'un compteur
        /// </summary>
        /// <param name="cpt"></param>
        /// <param name="ligne"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int ZigzagM(int cpt, int ligne, int col)
        {
            int n = 2;
            for (int k = 0; k < mat_placement2.GetLength(0); k++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (mat_placement2[ligne - k, col] == 0)
                        {

                            mat_placement2[ligne - k, col] = int.Parse(Convert.ToString(msg[cpt]));

                            cpt++;
                        }
                    }
                    else
                    {
                        if (mat_placement2[ligne - k, col - 1] == 0)
                        {


                            mat_placement2[ligne - k, col - 1] = int.Parse(Convert.ToString(msg[cpt]));
                            cpt++;

                        }
                    }

                }
            }
            return cpt;
        }
        /// <summary>
        /// Remplir une matrice en diagolane descente quand c'est possible a partir d'un mot et d'un compteur
        /// </summary>
        /// <param name="cpt"></param>
        /// <param name="ligne"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int ZigzagD(int cpt, int ligne, int col)
        {

            int n = 2;
            for (int k = 0; k < mat_placement2.GetLength(0); k++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (mat_placement2[ligne + k, col] == 0)
                        {

                            if (cpt < msg.Length)
                            {
                                mat_placement2[ligne + k, col] = int.Parse(Convert.ToString(msg[cpt]));
                                cpt++;

                            }

                        }
                    }
                    else
                    {
                        if (mat_placement2[ligne + k, col - 1] == 0)
                        {

                            if (cpt < msg.Length)
                            {
                                mat_placement2[ligne + k, col] = int.Parse(Convert.ToString(msg[cpt]));
                                cpt++;

                            }

                        }
                    }

                }
            }
            return cpt;
        }
        /// <summary>
        /// calcul de la partie du mot a rajouter jusqu'a trouver un patern a ne pas modifier
        /// </summary>
        /// <param name="mat_placement2"></param>
        /// <param name="cptlig"></param>
        /// <param name="cptcol"></param>
        /// <param name="cptmot"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public string PartieARajouter(int[,] mat_placement2, int cptlig, int cptcol, int cptmot, bool direction) // true = montee / false = descente
        {
            string partiemot = "";
            int cptpartiemot = 0;
            int cptlig2 = 0;
            cptlig2 = cptlig;
            bool fait = false;
            if (direction)
            {
                while (fait == false)
                {
                    partiemot = "";
                    if (mat_placement2[cptlig2, cptcol] == 0 && mat_placement2[cptlig2, cptcol - 1] == 0)
                    {
                        cptlig2--;
                        cptpartiemot++;
                    }
                    if (cptlig2 < 0)
                    {
                        fait = true;
                    }
                    else
                    {
                        if (mat_placement2[cptlig2, cptcol] != 0 || mat_placement2[cptlig2, cptcol - 1] != 0)
                        {
                            fait = true;
                        }

                    }
                }
                if (cptpartiemot > 0)
                {

                    for (int i = 0; i < 2 * cptpartiemot; i++)
                    {
                        if (cptmot < messageEncoder.Length)
                        {
                            partiemot += messageEncoder[cptmot];
                            cptmot++;
                        }
                    }

                }

            }
            else
            {
                while (fait == false)
                {
                    partiemot = "";
                    if (mat_placement2[cptlig2, cptcol] == 0 && mat_placement2[cptlig2, cptcol - 1] == 0)
                    {
                        cptlig2++;
                        cptpartiemot++;
                    }
                    if (cptlig2 >= mat_placement2.GetLength(0))
                    {
                        fait = true;
                    }
                    else
                    {
                        if (mat_placement2[cptlig2, cptcol] != 0 || mat_placement2[cptlig2, cptcol - 1] != 0)
                        {
                            fait = true;
                        }
                    }

                }
                if (cptpartiemot >= 0)
                {

                    for (int i = 0; i < 2 * cptpartiemot; i++)
                    {
                        if (cptmot < messageEncoder.Length)
                        {
                            partiemot += messageEncoder[cptmot];
                            cptmot++;
                        }
                    }

                }


            }

            return partiemot;
        }

        /// <summary>
        /// Calculs des differents masques
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        #region AllMasks 
        public static int[,] Mask0(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = 13;
                }

            }
            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {



                    {
                        if ((i + j) % 2 == 0)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {

                            b[i, j] = 0;

                        }
                    }

                }

            }

            b = Mirror(b);

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }

        public static int[,] Mask1(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = 13;
                }

            }


            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {

                    {
                        if (i % 2 == 0)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {

                            b[i, j] = 0;

                        }
                    }

                }
            }

            b = Mirror(b);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }
        public static int[,] Mask2(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = 13;
                }

            }

            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {


                    {
                        if (j % 3 == 0)
                        {

                            b[i, j] = 1;
                        }
                        else
                        {
                            b[i, j] = 0;
                        }

                    }

                }

            }

            b = Mirror(b);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }
        public static int[,] Mask3(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = 13;
                }

            }

            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {

                    {
                        if ((i + j) % 3 == 0)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {
                            b[i, j] = 0;
                        }
                    }

                }
            }

            b = Mirror(b);

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }
        public static int[,] Mask5(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = a[i, j];
                }

            }

            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {

                    {
                        if (((i * j) % 2) + ((i * j) % 3) == 0)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {
                            b[i, j] = 0;
                        }
                    }

                }
            }

            b = Mirror(b);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }
        public static int[,] Mask6(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = a[i, j];
                }

            }

            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {

                    {
                        if ((((i * j) % 2) + ((i * j) % 3)) % 2 == 0)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {
                            b[i, j] = 0;
                        }
                    }
                }
            }

            b = Mirror(b);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }
        public static int[,] Mask7(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = a[i, j];
                }

            }

            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {


                    {
                        if ((((i + j) % 2) + ((i * j) % 3)) % 2 == 0)
                        {
                            b[i, j] = 1;
                        }
                        else
                        {
                            b[i, j] = 0;
                        }
                    }
                }
            }

            b = Mirror(b);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {

                    if (a[i, j] == 0 || a[i, j] == 1)
                    {
                        if (a[i, j] == b[i, j])
                        {
                            a[i, j] = 0;
                        }
                        else
                        {
                            a[i, j] = 1;
                        }
                    }

                }
            }
            return a;
        }
        #endregion
        /// <summary>
        /// Determiner le meilleur masque
        /// </summary>
        /// <returns></returns>
        public int BestMask()
        {

            int VT = -1;

            int[,] VT0 = Mask0(Mirror(mat_placement)); //Matrices possible a renvoyer ( AVEC LA PARTIE NON TOUCHEE POUR L'ETAPE SUIVANTE )
            VT0 = RetablirPatterns(Mirror(VT0));
            int[,] VT1 = Mask1(Mirror(mat_placement));
            VT1 = RetablirPatterns(Mirror(VT1));
            int[,] VT2 = Mask2(Mirror(mat_placement));
            VT2 = RetablirPatterns(Mirror(VT2));
            int[,] VT3 = Mask3(Mirror(mat_placement));
            VT3 = RetablirPatterns(Mirror(VT3));
            int[,] VT5 = Mask5(Mirror(mat_placement));
            VT5 = RetablirPatterns(Mirror(VT5));
            int[,] VT6 = Mask6(Mirror(mat_placement));
            VT6 = RetablirPatterns(Mirror(VT6));
            int[,] VT7 = Mask7(Mirror(mat_placement));
            VT7 = RetablirPatterns(Mirror(VT7));

            int score0 = CalcErreur1(VT0) + CalcErreur2(VT0) + CalcErreur3(VT0) + CalcErreur4(VT0);
            int score1 = CalcErreur1(VT1) + CalcErreur2(VT1) + CalcErreur3(VT1) + CalcErreur4(VT1);
            int score2 = CalcErreur1(VT2) + CalcErreur2(VT2) + CalcErreur3(VT2) + CalcErreur4(VT2);
            int score3 = CalcErreur1(VT3) + CalcErreur2(VT3) + CalcErreur3(VT3) + CalcErreur4(VT3);
            int score5 = CalcErreur1(VT5) + CalcErreur2(VT5) + CalcErreur3(VT5) + CalcErreur4(VT5);
            int score6 = CalcErreur1(VT6) + CalcErreur2(VT6) + CalcErreur3(VT6) + CalcErreur4(VT6);
            int score7 = CalcErreur1(VT7) + CalcErreur2(VT7) + CalcErreur3(VT7) + CalcErreur4(VT7);

            int scoremin = Math.Min(score7, Math.Min(score6, Math.Min(score5, Math.Min(score3, Math.Min(score2, Math.Min(score0, score1))))));
            if (scoremin == score0)
            {
                mat_placement = VT0;
                VT = 0;
            }
            if (scoremin == score1)
            {
                mat_placement = VT1;
                VT = 1;
            }
            if (scoremin == score2)
            {
                mat_placement = VT2;
                VT = 2;
            }
            if (scoremin == score3)
            {
                mat_placement = VT3;
                VT = 3;
            }
            if (scoremin == score5)
            {
                mat_placement = VT5;
                VT = 4;
            }
            if (scoremin == score6)
            {
                mat_placement = VT6;
                VT = 5;
            }
            if (scoremin == score7)
            {
                mat_placement = VT7;
                VT = 6;
            }
            if (VT != 0)
            {
                mat_placement = VT0;
                VT = 0;
            }
            return VT;
        }
        /// <summary>
        /// calcul des 4 penalitees pour obtenir le score du meilleur masque
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int CalcErreur1(int[,] a)
        {
            int result = 0;
            for (int i = 0; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] == 6)
                    {
                        a[i, j] = 1;
                    }
                }
            }

            for (int i = 0; i < a.GetLength(0) - 1; i++)
            {
                int col = 0;
                while (col < a.GetLength(1))
                {
                    int nb = a[i, col];
                    int cpt = 0;
                    bool stop = false;
                    while (stop == false)
                    {
                        if (col + cpt >= a.GetLength(1))
                        {
                            stop = true;
                        }
                        else
                        {
                            if (a[i, col + cpt] != nb)
                            {
                                stop = true;
                            }
                            else
                            {
                                cpt++;
                            }
                        }
                    }
                    if (cpt >= 4)
                    {
                        result += 3;
                    }
                    if (cpt > 4)
                    {
                        result += cpt - 4;
                    }
                    col += cpt + 1;

                }
            }
            for (int i = 0; i < a.GetLength(1) - 1; i++)
            {
                int col = 0;
                while (col < a.GetLength(0))
                {
                    int nb = a[col, i];
                    int cpt = 0;
                    bool stop = false;
                    while (stop == false)
                    {
                        if (col + cpt >= a.GetLength(1))
                        {
                            stop = true;
                        }
                        else
                        {
                            if (a[col + cpt, i] != nb)
                            {
                                stop = true;
                            }
                            else
                            {
                                cpt++;
                            }
                        }
                    }
                    if (cpt >= 4)
                    {
                        result += 3;
                    }
                    if (cpt > 4)
                    {
                        result += cpt - 4;
                    }
                    col += cpt + 1;

                }
            }

            return result;
        }
        public static int CalcErreur2(int[,] a)
        {
            int score = 0;
            for (int i = 0; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < a.GetLength(1) - 1; j++)
                {
                    if (a[i, j] == 6)
                    {
                        a[i, j] = 1;
                    }
                    int num = a[i, j];
                    if (a[i, j] == num && a[i + 1, j] == num && a[i, j + 1] == num && a[i + 1, j + 1] == num)
                    {
                        score += 3;
                    }
                }
            }
            return score;
        }
        public static int CalcErreur3(int[,] a)
        {
            int score = 0;
            int cpt = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                int col = 0;
                while (col < a.GetLength(0) - 1 - 9)
                {
                    if (a[i, col] == 1 && a[i, col + 1] == 0 && a[i, col + 2] == 1 && a[i, col + 3] == 1 && a[i, col + 4] == 1 && a[i, col + 5] == 0 && a[i, col + 6] == 1 && a[i, col + 7] == 0 && a[i, col + 8] == 0 && a[i, col + 9] == 0 && a[i, col + 10] == 0)
                    {
                        cpt++;
                    }
                    if (a[i, col] == 0 && a[i, col + 1] == 0 && a[i, col + 2] == 0 && a[i, col + 3] == 0 && a[i, col + 4] == 1 && a[i, col + 5] == 0 && a[i, col + 6] == 1 && a[i, col + 7] == 1 && a[i, col + 8] == 1 && a[i, col + 9] == 0 && a[i, col + 10] == 1)
                    {
                        cpt++;
                    }
                    col++;
                }
                int lig = 0;
                while (lig < a.GetLength(0) - 1 - 9)
                {
                    if (a[lig, i] == 1 && a[lig + 1, i] == 0 && a[lig + 2, i] == 1 && a[lig + 3, i] == 1 && a[lig + 4, i] == 1 && a[lig + 5, i] == 0 && a[lig + 6, i] == 1 && a[lig + 7, i] == 0 && a[lig + 8, i] == 0 && a[lig + 9, i] == 0 && a[lig + 10, i] == 0)
                    {
                        cpt++;
                    }
                    if (a[lig, i] == 0 && a[lig + 1, i] == 0 && a[lig + 2, i] == 0 && a[lig + 3, i] == 0 && a[lig + 4, i] == 1 && a[lig + 5, i] == 0 && a[lig + 6, i] == 1 && a[lig + 7, i] == 1 && a[lig + 8, i] == 1 && a[lig + 9, i] == 0 && a[lig + 10, i] == 1)
                    {
                        cpt++;
                    }
                    lig++;
                }


            }
            score = cpt * 40;
            return score;
        }
        public static int CalcErreur4(int[,] a)
        {
            int score = 0;
            int cptNoir = 0;
            int cptcases = 0;
            for (int i = 0; i < a.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < a.GetLength(1) - 1; j++)
                {
                    cptcases++;
                    if (a[i, j] == 6)
                    {
                        a[i, j] = 1;
                    }
                    if (a[i, j] == 1)
                    {
                        cptNoir++;
                    }
                }
            }
            int pourcent = (cptNoir / cptcases) * 100;

            int av = (pourcent / 5) * 5;
            int ap = ((pourcent / 5) + 1) * 5;
            av = Math.Abs(av - 50) / 5;
            ap = Math.Abs(ap - 50) / 5;
            av = av * 10;
            ap = ap * 10;
            score = Math.Max(av, ap);
            return score;
        }
        /// <summary>
        /// Retablir les paterns avec leurs valeurs 0 ou 1
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int[,] RetablirPatterns(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 4) //partie dessus carre
                    {
                        a[i, j] = 0;
                    }
                    if (a[i, j] == 9) //carre noir en bas a gauche
                    {
                        a[i, j] = 1;
                    }
                    if (i == 6) //alterner ligne 6 noir blanc
                    {
                        int cpt = 0;
                        for (int k = 0; k < a.GetLength(1); k++)
                        {
                            if (a[i, k] == 7)
                            {
                                if (cpt % 2 == 0)
                                {
                                    a[i, k] = 1;
                                }
                                else
                                {
                                    a[i, k] = 0;
                                }
                                cpt++;
                            }
                        }
                    }
                    if (j == 6) //alterner col 6 noir blanc
                    {
                        int cpt = 0;
                        for (int k = 0; k < a.GetLength(0); k++)
                        {
                            if (a[k, j] == 7)
                            {
                                if (cpt % 2 == 0)
                                {
                                    a[k, j] = 1;
                                }
                                else
                                {
                                    a[k, j] = 0;
                                }
                                cpt++;
                            }
                        }
                    }
                    if (a[i, j] == 3)
                    {
                        for (int s = 0; s < 7; s++)
                        {
                            for (int p = 0; p < 7; p++)
                            {
                                a[i + s, j + p] = 1;
                            }
                        }
                        for (int s = 1; s < 6; s++)
                        {
                            for (int p = 1; p < 6; p++)
                            {
                                a[i + s, j + p] = 0;
                            }
                        }
                        for (int s = 2; s < 5; s++)
                        {
                            for (int p = 2; p < 5; p++)
                            {
                                a[i + s, j + p] = 1;
                            }
                        }
                    }
                    if (a[i, j] == 5)
                    {
                        for (int s = 0; s < 5; s++)
                        {
                            for (int p = 0; p < 5; p++)
                            {
                                a[i + s, j + p] = 1;
                            }
                        }
                        for (int s = 1; s < 4; s++)
                        {
                            for (int p = 1; p < 4; p++)
                            {
                                a[i + s, j + p] = 0;
                            }
                        }
                        a[i + 2, j + 2] = 1;
                    }

                }
            }
            return a;
        }
        public static string BonFormat(string encodage_type, string message)
        {
            if (encodage_type == "H" && message.Length > 50)
            {
                encodage_type = "M";
            }
            return encodage_type;
        }
        /// <summary>
        /// Ajouter les informations sur le QRCode dans les zones prevus a cet effet
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int[,] AddInformationStrings(int a, int[,] b)
        {
            string[] tabstring = { };
            if (versionn < 7)
            {
                if (typeencodage == "L")
                {
                    tabstring = Information_StringsL;
                }
                if (typeencodage == "M")
                {
                    tabstring = Information_StringsM;
                }
                if (typeencodage == "Q")
                {
                    tabstring = Information_StringsQ;
                }
                if (typeencodage == "H")
                {
                    tabstring = Information_StringsH;
                }
                string MOT = tabstring[0];

                int cpt = 0;
                int cpt2 = 0;
                int cpt3 = 0;
                int cpt4 = 0;
                for (int i = 0; i < MOT.Length; i++)
                {
                    if (cpt < 7)
                    {
                        if (i == 6)
                        {
                            cpt++;
                        }
                        //if(MOT[i] == '0' || MOT[i] == '1')
                        {
                            b[8, 0 + cpt] = Convert.ToInt32(Convert.ToString(MOT[i]));
                            b[b.GetLength(0) - 1 - cpt3, 8] = int.Parse(Convert.ToString(MOT[i]));
                            cpt++;
                            cpt3++;
                        }

                    }
                    else
                    {
                        if (cpt2 == 2)
                        {
                            cpt2++;
                        }
                        //if (MOT[i] == '0' || MOT[i] == '1')
                        {
                            b[8 - cpt2, 8] = int.Parse(Convert.ToString(MOT[i]));
                            b[8, b.GetLength(1) - 1 - 7 + cpt4] = int.Parse(Convert.ToString(MOT[i]));
                            cpt2++;
                            cpt4++;
                        }

                    }
                }
            }
            else
            {
                string values = "000111110010010100 001000010110111100 001001101010011001 001010010011010011 001011101111110110 001100011101100010 001101100001000111 001110011000001101 001111100100101000 010000101101111000 010001010001011101 010010101000010111 010011010100110010 010100100110100110 010101011010000011 010110100011001001 010111011111101100 011000111011000100 011001000111100001 011010111110101011 011011000010001110 011100110000011010 011101001100111111 011110110101110101 011111001001010000 100000100111010101 100001011011110000 100010100010111010 100011011110011111 100100101100001011 100101010000101110 100110101001100100 100111010101000001 101000110001101001";
                string[] vals = values.Split(' ');
                string mot = vals[versionn - 7];
                int cpt = 0;
                for (int i = 0; i <= 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        b[i, b.GetLength(1) - 1 - 10 + j] = int.Parse(Convert.ToString(mot[mot.Length - 1 - cpt]));
                        cpt++;

                    }

                }

                cpt = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        b[b.GetLength(0) - 1 - 10 + i, j] = int.Parse(Convert.ToString(mot[mot.Length - 1 - cpt]));
                        cpt++;

                    }

                }

            }
            return b;
        }

        /// <summary>
        /// Creation d'une matrice sans les patterns pour avoir uniquements les infos encodees pour proceder au decodage
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static int[,] MatForDecod(Pixel[,] image)
        {
            int taille = image.GetLength(0);
            int version = (taille - 21) / 4 + 1;
            int[,] Patterns = new int[taille, taille];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Patterns[i, j] = 3;
                    Patterns[Patterns.GetLength(0) - 1 - i, j] = 3;
                    Patterns[i, Patterns.GetLength(0) - 1 - j] = 3;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // en haut a gauche
                    Patterns[i, 7] = 4;
                    Patterns[7, j] = 4;
                    //en bas a gauche 
                    Patterns[Patterns.GetLength(0) - 1 - i, 7] = 4;
                    Patterns[Patterns.GetLength(0) - 1 - 7, j] = 4;
                    Patterns[7, Patterns.GetLength(0) - 1 - j] = 4;
                    Patterns[i, Patterns.GetLength(0) - 1 - 7] = 4;
                }
            }
            for (int i = 8; i < Patterns.GetLength(1) - 1 - 7; i++)
            {
                Patterns[i, 6] = 7;
                Patterns[6, i] = 7;
            }


            if (version > 1)
            {
                List<int> Coords = new List<int>();
                for (int i = 0; i < 7; i++)
                {
                    if (Alignment_Pattern_Locations[version - 1, i] != -1)
                    {
                        Coords.Add(Alignment_Pattern_Locations[version - 1, i]); // ajouter a une liste toutes les coordonnes a partir de la liste des valeurs en fonctions des versions Alignment_Pattern_Locations (on utilise une liste pour ne pas prensre en compte les -1 qui servaient juste a avoir une taille de 7 partout)
                    }
                }
                int min = Coords[0];
                int max = Coords[0];
                for (int i = 0; i < Coords.Count; i++) //determiner le min et le max de la liste (utile pour placer les patterns)
                {
                    if (Coords[i] <= min)
                    {
                        min = Coords[i];
                    }
                    if (Coords[i] >= min)
                    {
                        max = Coords[i];
                    }
                }
                List<int[]> AjoutPatterns = new List<int[]>();
                int[] value1 = { min, min }; // en bas a gauche
                int[] value2 = { max, min };  // en haut a gauche
                int[] value3 = { max, max }; // en haut a droite 

                for (int i = 0; i < Coords.Count; i++)
                {
                    for (int j = 0; j < Coords.Count; j++)
                    {
                        int[] localisation = { Coords[i], Coords[j] };
                        bool faire = true;
                        if (localisation[0] == value1[0] && localisation[1] == value1[1]) // faire en sorte de ne rien ajouter si on se trouve sur le patern en bas a gauche
                        {
                            faire = false;
                        }
                        if (localisation[0] == value2[0] && localisation[1] == value2[1]) // faire en sorte de ne rien ajouter si on se trouve sur le patern en haut a gauche
                        {
                            faire = false;
                        }
                        if (localisation[0] == value3[0] && localisation[1] == value3[1])// faire en sorte de ne rien ajouter si on se trouve sur le patern en haut a droite
                        {
                            faire = false;
                        }
                        if (faire == true)
                        {
                            AjoutPatterns.Add(localisation);

                        }

                    }
                }
                for (int i = 0; i < AjoutPatterns.Count; i++) //ajouter les paterns
                {
                    int[] aled = AjoutPatterns[i];
                    int x = aled[0];
                    int y = aled[1];

                    // Mat_placement (5) : 
                    for (int s = x - 2; s <= x + 2; s++)
                    {
                        for (int t = y - 2; t <= y + 2; t++)
                        {
                            Patterns[Patterns.GetLength(0) - 1 - s, t] = 5;
                        }
                    }
                }
                Patterns[Patterns.GetLength(0) - 1 - 7, 8] = 9;  //ajout du pixel noir en bas a gauche sur la matrice de placement

            }


            if (version < 7)
            {
                for (int i = 0; i < 7; i++) // Ligne coin en bas a gauche
                {
                    Patterns[Patterns.GetLength(0) - 1 - i, 8] = 6;
                }
                for (int i = 0; i < 9; i++)  // contour en haut a gauche
                {
                    if (i != 6)
                    {
                        Patterns[8, i] = 6;
                    }
                    if (i != 2)
                    {
                        Patterns[8 - i, 8] = 6;
                    }
                }
                for (int i = 0; i < 8; i++) // ligne en haut a droite
                {

                    Patterns[8, Patterns.GetLength(0) - 1 - i] = 6;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        //mat placement 
                        Patterns[Patterns.GetLength(0) - 1 - 8 - i, j] = 6;
                        Patterns[j, Patterns.GetLength(1) - 1 - 8 - i] = 6;
                    }
                }

            }
            return Mirror(Patterns);
        }

        /// <summary>
        /// Decodage du QrCode
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        public static string DecodeQR(MyImage QRCode)
        {
            Pixel[,] Image = QRCode.Mat_pixel;
            int taille = Image.GetLength(0);
            int version = (taille - 21) / 4 + 1;
            int[,] Patterns = new int[Image.GetLength(0), Image.GetLength(1)];

            Patterns = MatForDecod(Image); //Creation d'une matrice de 0 et de 1 a partir de la matrice Pixel
            for (int i = 0; i < Patterns.GetLength(0); i++)
            {
                for (int j = 0; j < Patterns.GetLength(1); j++)
                {
                    if (Patterns[i, j] == 0)
                    {
                        if (Image[i, j].Blue == 255 && Image[i, j].Red == 255 && Image[i, j].Green == 255)
                        {
                            Patterns[i, j] = 0;
                        }
                        if (Image[i, j].Blue == 0 && Image[i, j].Red == 0 && Image[i, j].Green == 0)
                        {
                            Patterns[i, j] = 1;
                        }
                    }
                    if (Patterns[i, j] == 6)
                    {
                        if (Image[i, j].Blue == 0)
                        {
                            Patterns[i, j] = 6; //case noir (value = 1)
                        }
                        else
                        {
                            Patterns[i, j] = 2; //case blanche (value = 0)
                        }
                    }

                }
            }




            Patterns = Mirror(Patterns); //Mirroring pour pouvoir lire dans le sens facile

            string DeterminerLettreEncod = "";
            string Determinertypemask = "";
            string LettreEncode = "";
            int[,] Mask = new int[Patterns.GetLength(0), Patterns.GetLength(1)];
            if (version < 7)
            {
                DeterminerLettreEncod += Patterns[8, 0]; //determiner les infos du QRCode pour decoder 
                DeterminerLettreEncod += Patterns[8, 1];
                Determinertypemask += Patterns[8, 2];
                Determinertypemask += Patterns[8, 3];
                Determinertypemask += Patterns[8, 4];

            }
            else
            {

            }
            //Determiner le type d'encodage L M H ou Q 
            #region Trouver Lettre encodage
            if (DeterminerLettreEncod == "66")
            {
                LettreEncode = "L";
            }
            if (DeterminerLettreEncod == "62")
            {
                LettreEncode = "M";
            }
            if (DeterminerLettreEncod == "26")
            {
                LettreEncode = "Q";
            }
            if (DeterminerLettreEncod == "22")
            {
                LettreEncode = "H";
            }
            #endregion 
            //Trouver le bon masquage dans les infos du QR pour pouvoir demasquer
            #region Trouver type mask
            if (Determinertypemask == "626")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }

                        }

                    }
                }
            }
            if (Determinertypemask == "622")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if (i % 2 == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }
            if (Determinertypemask == "666")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if (j % 3 == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }
            if (Determinertypemask == "662")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if ((i + j) % 3 == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }
            if (Determinertypemask == "222")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if (((i * j) % 2) + ((i * j) % 3) == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }
            if (Determinertypemask == "266")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if ((((i * j) % 2) + ((i * j) % 3)) % 2 == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }
            if (Determinertypemask == "262")
            {
                for (int i = 0; i < Mask.GetLength(0); i++)
                {
                    for (int j = 0; j < Mask.GetLength(1); j++)
                    {
                        if ((((i + j) % 2) + ((i * j) % 3)) % 2 == 0)
                        {
                            if (Patterns[i, j] == 0 || Patterns[i, j] == 1)
                            {
                                if (Patterns[i, j] == 0)
                                {
                                    Patterns[i, j] = 1;
                                }
                                else
                                {
                                    Patterns[i, j] = 0;
                                }
                            }
                        }

                    }
                }
            }

            #endregion



            string Mot = ""; //Recuperer les valeures encodeee du QRCode
            int x = Patterns.GetLength(0) - 1;
            int y = Patterns.GetLength(1) - 1;
            for (int i = 0; i < (Patterns.GetLength(1) - 1) / 2; i++) //parcourir toute la matrice divisee par 2 car on parcours 2 col par 2 col
            {
                //if(y==6)
                //{
                //    y--;
                //}
                if (i % 2 == 0)
                {
                    Mot += RecupMontee(Patterns, x, y);
                    x = 0;
                    y -= 2;
                }
                else
                {
                    Mot += RecupDescente(Patterns, x, y);
                    x = Patterns.GetLength(0) - 1;
                    y -= 2;
                }
            }

            int[,] val = { { } };   //choix des bonnes valeurs en fonction du type d'encodage
            if (LettreEncode == "Q")
            {
                val = ErrorCorrectionCode_Q;
            }
            if (LettreEncode == "H")
            {
                val = ErrorCorrectionCode_H;
            }
            if (LettreEncode == "L")
            {
                val = ErrorCorrectionCode_L;
            }
            if (LettreEncode == "M")
            {
                val = ErrorCorrectionCode_M;
            }
            int solomon = val[version - 1, 1]; //Recuperations des valeurs pour l'entremellage des valeures et de Reed Solomon
            int a = val[version - 1, 2];
            int b = val[version - 1, 3];
            int c = val[version - 1, 4];
            int d = val[version - 1, 5];
            int cpt = 0;
            string motSansSolomon = "";
            for (int i = 0; i < a; i++) //Remplissage du premier block sans reed solomon
            {
                for (int j = 0; j < b * 8; j++)
                {
                    motSansSolomon += Mot[cpt];
                    cpt++;
                }
                cpt += solomon * 8;
            }
            for (int i = 0; i < c; i++)//Remplissage du second block sans reed solomon
            {
                for (int j = 0; j < d * 8; j++)
                {
                    motSansSolomon += Mot[cpt];
                    cpt++;
                }
                cpt += solomon * 8;
            }


            string taillestring = "";
            for (int i = 0; i < 9; i++) //recup taille string en binaire
            {
                taillestring += motSansSolomon[4 + i];
            }

            int taillemot = Convert.ToInt32(Methods.FromBaseXToY(taillestring, 2, 10)); //taille mot final
            int cptbin = 0;
            bool impaire = false;
            if (taillemot % 2 == 1) //Si Impaire notre derniere lettre etait encodee sur 6 bits
            {
                cptbin += 6;
                impaire = true;
            }
            int padding = 0;

            if (version <= 9) padding = 11; //choix du padding en fonction des versions
            if (version >= 10 && version <= 26) padding = 11;
            if (version >= 27 && version <= 40) padding = 13;
            cptbin += (taillemot / 2) * padding; //recuperation du nbr de paires de lettres pour le decodage
            string lettres = "";
            for (int i = 0; i < cptbin; i++) //recup toutes les paires
            {
                lettres += motSansSolomon[13 + i];
            }
            string MotFinal = "";

            for (int i = 0; i < (taillemot / 2); i++) //parcourir chaque paire
            {
                string A_convertir = "";
                for (int j = 0; j < padding; j++) //recup de la chaine de binaire des deu caracteres
                {
                    A_convertir += lettres[i * padding + j];

                }
                int char1 = Convert.ToInt32(Methods.FromBaseXToY(A_convertir, 2, 10)); //conversion de la val en decimal
                int char11 = char1 / 45; //diviser par 45 pour avoir la lettre forte 
                MotFinal += ListeLettres[char11]; //recup la lettre forte dans le tableau des alpha num
                MotFinal += ListeLettres[char1 - 45 * char11]; //recup la lettre faible dans le tableau des alpha num
            }

            if (impaire) //meme procede pour la derniere lettre puisque la chaine est impaire
            {
                string A_convertir = "";
                for (int i = 0; i < 6; i++)
                {
                    A_convertir += lettres[(taillemot / 2) * padding + i];
                }
                int char1 = Convert.ToInt32(Methods.FromBaseXToY(A_convertir, 2, 10));
                MotFinal += ListeLettres[char1];
            }




            return MotFinal;
        }

        public static string RecupMontee(int[,] mat, int xDebut, int yDebut) //Recuperer une chaine de caratcere de 0 ou de 1 en remonte zigzag
        {
            string mot = "";
            for (int k = 0; k < mat.GetLength(0); k++)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (mat[xDebut - k, yDebut] == 0 || mat[xDebut - k, yDebut] == 1)
                        {
                            mot += mat[xDebut - k, yDebut];
                        }
                    }
                    else
                    {
                        if (mat[xDebut - k, yDebut - 1] == 0 || mat[xDebut - k, yDebut - 1] == 1)
                        {
                            mot += mat[xDebut - k, -1 + yDebut];
                        }
                    }

                }
            }
            return mot;
        }
        public static string RecupDescente(int[,] mat, int xDebut, int yDebut) //Recuperer une chaine de caratcere de 0 ou de 1 en descente zigzag
        {
            string mot = "";
            for (int k = 0; k < mat.GetLength(0); k++)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (mat[xDebut + k, yDebut] == 0 || mat[xDebut + k, yDebut] == 1)
                        {
                            mot += mat[xDebut + k, yDebut];
                        }
                    }
                    else
                    {
                        if (mat[xDebut + k, yDebut - 1] == 0 || mat[xDebut + k, yDebut - 1] == 1)
                        {
                            mot += mat[xDebut + k, -1 + yDebut];
                        }
                    }

                }
            }
            return mot;
        }
        #endregion
    }
}

