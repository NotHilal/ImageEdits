using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Problème_scientifique___Image
{
    class ThreadingClass
    {
        /// <summary>  
        ///  This code is executed by a secondary thread  
        /// </summary>  
        public static void Print()
        {
            for (int i = 11; i < 20; i++)
            {
                Console.WriteLine($"Worker thread: {i}");
                Thread.Sleep(1000);
            }
        }
    }
}
