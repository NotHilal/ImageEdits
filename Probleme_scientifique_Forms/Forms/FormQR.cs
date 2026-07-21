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
    public partial class FormQR : Form
    {
        public FormQR()
        {
            InitializeComponent();
        }
        public string NomImage => (string)textBox1.Text;
        public string message => (string)richTextBox1.Text;
        public string NiveauEncodage => dropDown.Text;
        private void button1_Click(object sender, EventArgs e)
        {    
            try
            {
                this.pictureBox1.Refresh();
                QRCode qr = new QRCode(message, NiveauEncodage);
                MyImage qrAgrandit = new MyImage(qr.Filename);
                qrAgrandit.Agrandir(10, true);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{qr.Filename}_agrandit_coeff.bmp");
                this.pictureBox1.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur ! Ne pas mettre des caractères n'appartenant pas au type alphanumérique!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.pictureBox2.Refresh();
                MyImage picture = new MyImage(NomImage);
                string dir = Directory.GetCurrentDirectory();
                Bitmap image = new Bitmap($@"{dir}\Fichiers\\{NomImage}_agrandit_coeff.bmp");
                textBox1.Text = QRCode.DecodeQR(picture);
                this.pictureBox2.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur:\n\n" + ex.Message, "Système", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
