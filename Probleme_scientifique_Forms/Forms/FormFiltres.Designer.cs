namespace Problème_scientifique___Forms.Forms
{
    partial class FormFiltres
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitre = new System.Windows.Forms.Label();
            this.buttonFiltresDetectionDeContour = new System.Windows.Forms.Button();
            this.tBoxNomImage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFiltresFlou = new System.Windows.Forms.Button();
            this.buttonFiltresRepoussage = new System.Windows.Forms.Button();
            this.buttonFiltresNettete = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitre
            // 
            this.labelTitre.AutoSize = true;
            this.labelTitre.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelTitre.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitre.Location = new System.Drawing.Point(411, 38);
            this.labelTitre.Name = "labelTitre";
            this.labelTitre.Size = new System.Drawing.Size(57, 20);
            this.labelTitre.TabIndex = 22;
            this.labelTitre.Text = "Image";
            // 
            // buttonFiltresDetectionDeContour
            // 
            this.buttonFiltresDetectionDeContour.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonFiltresDetectionDeContour.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFiltresDetectionDeContour.Location = new System.Drawing.Point(55, 114);
            this.buttonFiltresDetectionDeContour.Name = "buttonFiltresDetectionDeContour";
            this.buttonFiltresDetectionDeContour.Size = new System.Drawing.Size(291, 59);
            this.buttonFiltresDetectionDeContour.TabIndex = 19;
            this.buttonFiltresDetectionDeContour.Text = "Detection de contour";
            this.buttonFiltresDetectionDeContour.UseVisualStyleBackColor = true;
            this.buttonFiltresDetectionDeContour.Click += new System.EventHandler(this.buttonFiltresDetectionDeContour_Click);
            // 
            // tBoxNomImage
            // 
            this.tBoxNomImage.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBoxNomImage.Location = new System.Drawing.Point(171, 49);
            this.tBoxNomImage.Name = "tBoxNomImage";
            this.tBoxNomImage.Size = new System.Drawing.Size(160, 20);
            this.tBoxNomImage.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Nom de l\'image : ";
            // 
            // buttonFiltresFlou
            // 
            this.buttonFiltresFlou.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonFiltresFlou.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFiltresFlou.Location = new System.Drawing.Point(55, 194);
            this.buttonFiltresFlou.Name = "buttonFiltresFlou";
            this.buttonFiltresFlou.Size = new System.Drawing.Size(291, 59);
            this.buttonFiltresFlou.TabIndex = 23;
            this.buttonFiltresFlou.Text = "Flou";
            this.buttonFiltresFlou.UseVisualStyleBackColor = true;
            this.buttonFiltresFlou.Click += new System.EventHandler(this.buttonFiltresFlou_Click);
            // 
            // buttonFiltresRepoussage
            // 
            this.buttonFiltresRepoussage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonFiltresRepoussage.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFiltresRepoussage.Location = new System.Drawing.Point(55, 268);
            this.buttonFiltresRepoussage.Name = "buttonFiltresRepoussage";
            this.buttonFiltresRepoussage.Size = new System.Drawing.Size(291, 59);
            this.buttonFiltresRepoussage.TabIndex = 24;
            this.buttonFiltresRepoussage.Text = "Repoussage";
            this.buttonFiltresRepoussage.UseVisualStyleBackColor = true;
            this.buttonFiltresRepoussage.Click += new System.EventHandler(this.buttonFiltresRepoussage_Click);
            // 
            // buttonFiltresNettete
            // 
            this.buttonFiltresNettete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonFiltresNettete.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFiltresNettete.Location = new System.Drawing.Point(55, 343);
            this.buttonFiltresNettete.Name = "buttonFiltresNettete";
            this.buttonFiltresNettete.Size = new System.Drawing.Size(291, 59);
            this.buttonFiltresNettete.TabIndex = 25;
            this.buttonFiltresNettete.Text = "Netteté";
            this.buttonFiltresNettete.UseVisualStyleBackColor = true;
            this.buttonFiltresNettete.Click += new System.EventHandler(this.buttonFiltresNettete_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(381, 79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(397, 339);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // FormFiltres
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonFiltresNettete);
            this.Controls.Add(this.buttonFiltresRepoussage);
            this.Controls.Add(this.buttonFiltresFlou);
            this.Controls.Add(this.labelTitre);
            this.Controls.Add(this.buttonFiltresDetectionDeContour);
            this.Controls.Add(this.tBoxNomImage);
            this.Controls.Add(this.label1);
            this.Name = "FormFiltres";
            this.Text = "Filtres";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelTitre;
        private System.Windows.Forms.Button buttonFiltresDetectionDeContour;
        private System.Windows.Forms.TextBox tBoxNomImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonFiltresFlou;
        private System.Windows.Forms.Button buttonFiltresRepoussage;
        private System.Windows.Forms.Button buttonFiltresNettete;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}