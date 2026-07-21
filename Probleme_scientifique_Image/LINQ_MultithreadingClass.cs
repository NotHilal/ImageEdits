using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Problème_scientifique___Image
{
    public static class LINQ_MultithreadingClass
    {
        #region Lambda functions
        //Func<int, int> multiplyByFive = num => num * 5; //Le num à gauche du => est le paramètre. Et Func<int, int> réfère à un delegates qui prends un paramètre int et une sortie
                                                        //de sorte que Func<T1, TResult> T1 : parameter et TResult : sortie

        public static Func<int, int, int> AddTogether = (a, b) => a + b; //On voit qu'ici on a Func<T1, T2, TResult> qui prend deux paramètres a et b qui sont ajouter
        #endregion

        public static Action<int[,], Random> Run = (matrice, random) =>
        {
            for (int ligne = 0; ligne < matrice.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < matrice.GetLength(1); colonne++)
                {
                    matrice[ligne, colonne] = random.Next(0, 2);
                    //matrice[ligne, colonne] += 1;
                }
            }
        };
        public static int[,] AddMatrix(int[][,] matrices, int threadCount)
        {
            int[,] new_mat = new int[matrices[0].GetLength(0), matrices[0].GetLength(1)];
            for (int ligne = 0; ligne < new_mat.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < new_mat.GetLength(1); colonne++)
                {
                    Parallel.For(0, threadCount, i =>
                    {
                        Interlocked.Add(ref new_mat[ligne, colonne], matrices[i][ligne, colonne]);
                    });
                }
            }
            return new_mat;
        }
        #region Extension methods 
        //On doit mettre public static class LINQ_MultithreadingClass pour que ça fonctionne

        //public static string Wow(this int num) //Méthode d'extension d'un int 
        //{
        //    return $"{num}... Wow.";
        //}
        public static string Test2D(this Array source, int pad = 10)
        {
            var result = "";
            for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
            {
                for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
                    result += source.GetValue(i, j).ToString().PadLeft(pad);
                result += "\n";
            }
            return result;
        }
        #endregion
    }
}
