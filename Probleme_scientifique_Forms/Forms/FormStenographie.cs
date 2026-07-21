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
    public partial class FormStenographie : Form
    {
        public FormStenographie()
        {
            InitializeComponent();
        }
        private string NomMere => tBoxNomImage.Text;
        private string NomFille => tBoxNomImage2.Text;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MyImage imageMere = new MyImage($"{NomMere}");
                MyImage imageFille = new MyImage($"{NomFille}");
                MyImage.EncoderImage(imageMere, imageFille);
                MyImage imageEncoder = new MyImage($"{NomMere}_encoder");
                MyImage.DecoderImage(imageEncoder, imageFille);
                this.pictureBox1.Refresh();
                this.pictureBox2.Refresh();
                string dir = Directory.GetCurrentDirectory();
                Bitmap imageM = new Bitmap($@"{dir}\Fichiers\\{NomMere}_encoder.bmp");
                Bitmap imageF = new Bitmap($@"{dir}\Fichiers\\{NomFille}_décoder.bmp");
                this.pictureBox1.Image = imageM;
                this.pictureBox2.Image = imageF;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
