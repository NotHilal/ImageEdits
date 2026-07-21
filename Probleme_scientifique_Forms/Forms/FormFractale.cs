using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Problème_scientifique___Image;

namespace Problème_scientifique___Forms.Forms
{
    public partial class FormFractale : Form
    {
        private Stopwatch Stopwatch = new Stopwatch();
        private int compteur = 0;
        public FormFractale()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }
        public int zoom => (int)sizeNumericUpDown.Value;
        public int iteration => (int)iterationNumericUpDown.Value;
        public byte pos1 => (byte)numericUpDown1.Value;
        public byte pos2 => (byte)numericUpDown2.Value;
        public byte pos3 => (byte)numericUpDown3.Value;
        public string ChoixFractales => (string)domainUpDown1.Text == "Choix fractale" ? "mandelbrot" : (string)domainUpDown1.Text;
        public string Colormap => (string)domainUpDown2.Text == "Colormap" ? "jet" : (string)domainUpDown2.Text;
        public string MethodeCouleur => (string)dropDown.Text == "Méthode couleur" ? "gradientMono" : (string)dropDown.Text;

        private void goButton_Click(object sender, EventArgs e)
        {
            try
            {

                //this.Stopwatch.Reset();
                //this.Stopwatch.Start();
                //this.picCanvas.Refresh();
                //string time = DateTime.Now.ToString("HHmmss");
                //MyImage.Fractale(zoom, iteration, ChoixFractales, MethodeCouleur, Colormap, pos1, pos2, pos3, time);
                //string dir = Directory.GetCurrentDirectory();
                //Bitmap image = new Bitmap($@"{dir}\Fichiers\\fractale_{ChoixFractales}_{MethodeCouleur}_{Colormap}_{pos1}{pos2}{pos3}{time}.bmp");
                //this.picCanvas.Image = image;
                //this.Stopwatch.Stop();

                label5.Visible = true;
                label5.Text = "Calcul... Veuillez patienter";
                label5.Update();
                this.Stopwatch.Reset();
                this.Stopwatch.Start();
                timer1.Enabled = true;
                timer1.Start();
                this.picCanvas.Refresh();
                string time = DateTime.Now.ToString("HHmmss");
                MyImage.Fractale(zoom, iteration, ChoixFractales, MethodeCouleur, Colormap, pos1, pos2, pos3, time);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\fractale_{ChoixFractales}_{MethodeCouleur}_{Colormap}_{pos1}{pos2}{pos3}{time}.bmp");
                this.picCanvas.Image = image;
                this.Stopwatch.Stop();
                label5.Text = "Calcul complété!";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            this.compteur++;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                worker.ReportProgress(compteur);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label7.Text = String.Format("{0:hh\\:mm\\:ss}", Stopwatch.Elapsed);
            label7.Update();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
        }

    }
}
