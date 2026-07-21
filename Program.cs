using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace Problème_scientifique___Image
{
    class Program
    {
        private MyImage imageClass;
        static void Main(string[] args)
        {
            #region Traitement image 
            MyImage imageCat = new MyImage("cat");
            MyImage imageCoco = new MyImage("coco");
            MyImage imageLena = new MyImage("lena");
            //imageLena.Rotation180AntiClockWise();
            //imageLena.Rotation180ClockWise();
            //imageCoco.Rotation(37);
            //MyImage.EncoderImage(imageLena, imageCoco);           
            //MyImage imageLenaCoder = new MyImage("lena_encoder");
            //MyImage.DecoderImage(imageLenaCoder, imageCoco);
            //image.Retrectir(3);
            //for (int i = 0; i < 90; i++)
            //{
            //    image.Rotation(i);
            //}
            //Pixel pixel = new Pixel(2, 1, 13);
            //Tuple<double, double, double> HSVvalues = Pixel.RGBversHSV(pixel.Red, pixel.Green, pixel.Blue);
            //Console.WriteLine($"Voici les valeurs de HSVvalues : " +
            //    $"h = {HSVvalues.Item1} " +
            //    $"s = {HSVvalues.Item2} " +
            //    $"v = {HSVvalues.Item3} ");
            //Tuple<int, int, int> RGBvalues = Pixel.HSVversRGB(HSVvalues.Item1, HSVvalues.Item2, HSVvalues.Item3);
            //Console.WriteLine($"Voici les valeurs de RGBValues : " +
            //    $"r = {RGBvalues.Item1} " +
            //    $"g = {RGBvalues.Item2} " +
            //    $"b = {RGBvalues.Item3} ");
            //MyImage.Fractale(1000, 100, "mandelbrot", "gradientMono");
            //Buddhabrot buddha = new Buddhabrot(200, 200, 1000000, 2500, 4000, 8000);
            //buddha.DrawBuddhabrot();
            //string cinq = Methods.DecimalToBaseString(5, 2);
            //int cinqDecimal = Methods.BinaryStringToBase(cinq,2);
            //string test = Methods.DecimalToBaseString(cinqDecimal, 2);
            //int cc = Methods.BinaryStringToBase(test, 2);
            //Console.WriteLine(cinq);
            //Console.WriteLine(test);
            #endregion

            //#region Threading
            //// Create a secondary thread by passing a ThreadStart delegate  
            //Thread workerThread = new Thread(new ThreadStart(ThreadingClass.Print));
            //// Start secondary thread  
            //workerThread.Start();

            //if (workerThread.ThreadState == ThreadState.Running)
            //{
            //    Console.WriteLine("I'm working mom!");
            //}
            //// Main thread : Print 1 to 10 every 0.2 second.   
            //// Thread.Sleep method is responsible for making the current thread sleep  
            //// in milliseconds. During its sleep, a thread does nothing.  
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine($"Main thread: {i}");
            //    Thread.Sleep(200);
            //}


            //int workers, ports;
            //// Get available threads  
            //ThreadPool.GetAvailableThreads(out workers, out ports);
            //Console.WriteLine($"Availalbe worker threads: {workers} ");
            //Console.WriteLine($"Available completion port threads: {ports}");


            //#endregion
            string a = "ABCDE%";
            string b = "ABCDE?FHGT";

            // Console.WriteLine(Methods.InAlphaNum(a));
            // Console.WriteLine(Methods.InAlphaNum(b));

            //Console.WriteLine(Methods.FromBaseXToY("01100001011", 2,10));
            string mot = "HELLO WORLD";
            Console.WriteLine(mot.Length);
            
            string taille9bits =Methods.Nbrcaractere9bits(mot);
            Console.WriteLine(taille9bits);
            Console.WriteLine(Methods.FromBaseXToY(taille9bits,2,10));
            string motTobinary11 = Methods.FromStringToBinary11(mot.ToUpper());
            Console.WriteLine(motTobinary11);
            Console.WriteLine(Methods.FromBinary11ToString(motTobinary11));
            Console.WriteLine(Methods.CalculsNbrBits(motTobinary11));
            Console.ReadKey();
        }
    }
}
