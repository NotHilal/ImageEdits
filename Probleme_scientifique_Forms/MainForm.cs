using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Problème_scientifique___Forms
{
    public partial class MainForm : Form
    {
        #region Variables
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        #endregion
        //Constructeurs
        public MainForm()
        {
            InitializeComponent();
            random = new Random();
            btnCloseChildForm.Visible = false;

        }
        #region Méthodes
        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index) //Permet de choisir une couleur différente si on tombe sur la même
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitleBar.BackColor = color;
                    panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    ThemeColor.PrimaryColor = color;
                    ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    btnCloseChildForm.Visible = true;

                }
            }
        }
        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
        public void OpenChildForm(Form childForm, object btnSender)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktopPanel.Controls.Add(childForm);
            this.panelDesktopPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitre.Text = childForm.Text;
        }
        #endregion

        private void buttonEditionImage_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormEditionImage(), sender);
        }

        private void buttonFiltres_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormFiltres(), sender);
        }

        private void buttonHistogramme_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormHistogramme(), sender);
        }

        private void buttonQRCode_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormQR(), sender);
        }
        private void buttonInnovation_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormInnovation(), sender);
        }
        private void buttonStenographie_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormStenographie(), sender);
        }
        private void buttonFractale_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormFractale(), sender);
        }
        private void buttonInnovation_Click_2(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormInnovation(), sender);
        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if(activeForm != null)
            {
                activeForm.Close();
                Reset();
            }
        }
        private void Reset()
        {
            DisableButton();
            lblTitre.Text = "ACCUEIL";
            panelTitleBar.BackColor = Color.FromArgb(207, 16, 82);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            btnCloseChildForm.Visible = false;
        }

    }
}
