using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Problème_scientifique___Image;

namespace Problème_scientifique___Image
{
    [TestClass]
    public class TestUnitaire
    {
        [TestMethod]
        public void TestHSV_RGB()
        {
            Pixel pixel = new Pixel(13, 1, 2);
            Tuple<double, double, double> HSVvalues = Pixel.RGBversHSV(pixel.Red, pixel.Green, pixel.Blue);
            Tuple<double, double, double> HSVvalues_expected = Tuple.Create(355.0, 0.922, 0.051);
            Tuple<int, int, int> RGBvalues = Pixel.HSVversRGB(HSVvalues.Item1, HSVvalues.Item2, HSVvalues.Item3);
            Tuple<int, int, int> RGBvalues_expected = Pixel.HSVversRGB(355.0, 0.922, 0.051);
            Assert.AreEqual(HSVvalues_expected, HSVvalues);
            Assert.AreEqual(RGBvalues_expected, RGBvalues);
        }
        [TestMethod]
        public void Test_DecimalToBase()
        {
            string cinqBinaire = Methods.DecimalToBase(5, 2);
            string cinqBinaireAttendu = "101";
            Assert.AreEqual(cinqBinaire, cinqBinaireAttendu);
        }
        [TestMethod]
        public void Test_ChoixVersion()
        {
            int a = QRCode.ChoixNiveauEncodage("HELLO WOLRD", "L");
            int aa = QRCode.ChoixNiveauEncodage("HELLO POLE LEONARD DE VINCI", "L");
            Assert.AreEqual(a, 1);
            Assert.AreEqual(aa, 2);
        }
        [TestMethod]
        public void TestConversionBases()
        {
            string a = Methods.FromBaseXToY("22", 10, 2);
            Assert.AreEqual(a, "10110");
            string b = Methods.FromBaseXToY("143", 10, 6);
            Assert.AreEqual(b, "355");
        }
        [TestMethod]
        public void TestEncoderQRCodeMSG()
        {
            string oldPath = Directory.GetCurrentDirectory();
            QRCode L = new QRCode("HELLO WOLRD", "L");
            string a = L.EncoderQRCode(L.mot, "L");
            Assert.AreEqual(a, "0010000001011011000010110111100011010001011100101101110000111100110000110100000011101100000100011110110000010001111011000001000111101100000100011110110011000111010101111011010001011100101111010001011011110111");
            QRCode M = new QRCode("HELLO WOLRD", "M");
            string aa = M.EncoderQRCode(M.mot, "M");
            Assert.AreEqual(aa, "0010000001011011000010110111100011010001011100101101110000111100110000110100000011101100000100011110110000010001111011000001000101101101001010110100001101010010110000011011011000011111101100000001101110100001");
            QRCode Q = new QRCode("HELLO WOLRD", "Q");
            string aaa = Q.EncoderQRCode(Q.mot, "Q");
            Assert.AreEqual(aaa, "0010000001011011000010110111100011010001011100101101110000111100110000110100000011101100000100011110110000100001000111111010000100010100110111101001010110000101001010011110111100101000010110110011101100010010");
            QRCode H = new QRCode("HELLO WOLRD", "H");
            string aaaa = H.EncoderQRCode(H.mot, "H");
            Assert.AreEqual(aaaa, "00100000010110110000101101111000110100010111001011011100001111001100001101000000111011000001000111101100000100011110110000010001010101100100110000001101111001101101111011011001101000010001111111010101010010010011110110110011011100110000111101010111011111111001001001101001100101001001101110010010000101101000010011101100100110111100100110000000111100010000000");
        }

    }
}