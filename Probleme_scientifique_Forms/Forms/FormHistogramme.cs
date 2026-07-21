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
    public partial class FormHistogramme : Form
    {
        public FormHistogramme()
        {
            InitializeComponent();
        }
        public string NomImage => (string)tBoxNomImage.Text;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox1.Refresh();
                MyImage picture = new MyImage(NomImage);
                picture.Histogramme();
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_histogramme.bmp");
                this.pictureBox1.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
