using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Problème_scientifique___Image;

namespace Problème_scientifique___Forms.Forms
{
    public partial class FormFiltres : Form
    {
        public FormFiltres()
        {
            InitializeComponent();
            LoadTheme();
        }
        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btns.BackColor = ThemeColor.PrimaryColor;
                    btns.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
            labelTitre.ForeColor = ThemeColor.PrimaryColor;
        }
        public string NomImage => (string)tBoxNomImage.Text;

        private void buttonFiltresDetectionDeContour_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.DetectionDeContour();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_DetectionDeContour.bmp");
                this.pictureBox1.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFiltresFlou_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Flou();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_flouGaussien.bmp");
                this.pictureBox1.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFiltresRepoussage_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Repoussage();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_repoussage.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_repoussage");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFiltresNettete_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Nettete();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_Nettete.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_Nettete");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
