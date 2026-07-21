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
    public partial class FormEditionImage : Form
    {
        public FormEditionImage()
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
        public int agrandirCoeff => (int)numericUpDown1.Value;
        public int retrecirCoeff => (int)numericUpDown2.Value;
        public int rotationCoeff => (int)numericUpDown3.Value;
        private void tBoxNomImage_TextChanged(object sender, EventArgs e)
        {
            string input = tBoxNomImage.Text;
            Console.WriteLine(input);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Agrandir(agrandirCoeff);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_agrandit_coeff{agrandirCoeff}.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_agrandit_coeff{agrandirCoeff}");
            }           
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {      
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Retrectir(retrecirCoeff);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_retrecissement_coeff{retrecirCoeff}.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_retrecissement_coeff{retrecirCoeff}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Rotation(rotationCoeff);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_rotation{rotationCoeff}.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_rotation{rotationCoeff}");
            }        
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEditionImageNuance_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Nuancier_gris_noir();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_nuanceDeGris.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_nuanceDeGris");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonEditionImageMiroir_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Miroir();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_miroir.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_miroir");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.NoirBlanc();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_noirEtBlanc.bmp");
                this.pictureBox1.Image = image;
                MyImage.Affichage_image($"{NomImage}_noirEtBlanc");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
