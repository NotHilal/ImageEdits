using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using static Problème_scientifique___Image.LINQ_MultithreadingClass; //Permet d'utiliser directement les méthodes de la class sans faire LINQ_MultithreadingClass.NomDeLaFonction

namespace Problème_scientifique___Image
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Multithreading
            //int threadCount = Environment.ProcessorCount;
            //Stopwatch stopwatchNoThread = new Stopwatch();
            //stopwatchNoThread.Start();
            //int[][,] hits = new int[threadCount][,];
            //for (int i = 0; i < threadCount; i++)
            //{
            //    hits[i] = new int[10, 10]; //pour des arrays plus grand ou égal à 5000 on a une OutOfMemory exeception. à voir.
            //}
            //for (int i = 0; i < threadCount; i++)
            //{
            //    LINQ_MultithreadingClass.Run(hits[i], new Random(i));
            //}
            //int[,] new_mat_nothread = LINQ_MultithreadingClass.AddMatrix(hits, threadCount);
            //Console.WriteLine(stopwatchNoThread.Elapsed);
            ////Exemple de calcul de matrice avec du multithreading. On ajoute parralèllement des valeurs à 32 matrices de tailles 1000x1000
            ////puis on additionne parallèlelement ces valeurs dans une seule et même matrices.
            //Stopwatch stopwatchThreading = new Stopwatch();
            //stopwatchThreading.Start();
            //int[][,] hitsThreads = new int[threadCount][,];
            //for (int i = 0; i < threadCount; i++)
            //{
            //    hitsThreads[i] = new int[10, 10];
            //}
            //var tasks = Enumerable.Range(0, threadCount)
            //    .Select(thread => Task.Run(() => LINQ_MultithreadingClass.Run(hitsThreads[thread], new Random(thread))))
            //    .ToArray();
            //Task.WaitAll(tasks);
            //int[,] new_mat = LINQ_MultithreadingClass.AddMatrix(hitsThreads, threadCount);
            //Console.WriteLine(stopwatchThreading.Elapsed);
            #endregion

            Console.ReadKey();
        }
        
    }
}
