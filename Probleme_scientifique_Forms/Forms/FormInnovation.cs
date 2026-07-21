using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Problème_scientifique___Image;

namespace Problème_scientifique___Forms.Forms
{
    public partial class FormInnovation : Form
    {
        private Stopwatch Stopwatch; 
        public FormInnovation()
        {
            InitializeComponent();
            Stopwatch = new Stopwatch();
        }
        public int zoom => (int)sizeNumericUpDown.Value;
        public int iteration => (int)iterationNumericUpDown.Value;
        public int iterationRouge => (int)numericUpDown1.Value;
        public int iterationVert => (int)numericUpDown2.Value;
        public int iterationBleu => (int)numericUpDown3.Value;
        public int iterationMaximale => (int)pointsCountNumericUpDown.Value;
        private void goButton_Click_1(object sender, EventArgs e)
        { 
            try
            {
                label5.Visible = true;
                label5.Text = "Calcul... Veuillez patienter.";
                label5.Update();
                label7.Text = "00:00:00";
                label7.Update();
                this.Stopwatch.Reset();
                this.Stopwatch.Start();
                this.picCanvas.Refresh();
                string time = DateTime.Now.ToString("HHmmss");
                BuddhabrotMultiThreading buddha;
                if (iterationRouge == 0 && iterationVert == 0 && iterationBleu == 0)
                {
                    buddha = new BuddhabrotMultiThreading(zoom, iteration, iterationMaximale);
                }
                else buddha = new BuddhabrotMultiThreading(zoom, iteration, iterationMaximale, iterationRouge, iterationVert, iterationBleu);
                buddha.DrawBuddhabrot(time);
                MyImage buddhaRotation = new MyImage($"fractale_Buddhabrot{time}");
                buddhaRotation.Rotation(-90);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\fractale_Buddhabrot{time}_rotation-90.bmp");
                MyImage.Affichage_image($"fractale_Buddhabrot{time}_rotation-90");
                this.picCanvas.Image = image;
                this.Stopwatch.Stop();
                label7.Text = label7.Text = String.Format("{0:hh\\:mm\\:ss}", Stopwatch.Elapsed);
                label7.Update();
                label5.Text = "Calcul complété!";
                label5.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
