namespace Sifh.ReportGenerator
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.textBoxConductorLicense = new System.Windows.Forms.TextBox();
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonAddConductor = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxTruckLicense = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonAddTruck = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRemoveLicence = new System.Windows.Forms.Button();
            this.comboBoxVesselName1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxVesselName = new System.Windows.Forms.ComboBox();
            this.buttonAddLicence = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxConductor = new System.Windows.Forms.ComboBox();
            this.buttonRemoveConductor = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 64);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(390, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "ADMIN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.buttonBack);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(0, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1129, 534);
            this.panel2.TabIndex = 1;
            // 
            // buttonBack
            // 
            this.buttonBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBack.Location = new System.Drawing.Point(892, 489);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 11;
            this.buttonBack.Text = "CLOSE";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(577, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(287, 51);
            this.label11.TabIndex = 10;
            this.label11.Text = "TRANSPORT";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(120, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(202, 51);
            this.label10.TabIndex = 4;
            this.label10.Text = "VESSEL ";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.buttonRemoveConductor);
            this.panel6.Controls.Add(this.comboBoxConductor);
            this.panel6.Controls.Add(this.textBoxConductorLicense);
            this.panel6.Controls.Add(this.textBoxLastName);
            this.panel6.Controls.Add(this.textBoxFirstName);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.buttonAddConductor);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Location = new System.Drawing.Point(499, 224);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(468, 259);
            this.panel6.TabIndex = 9;
            // 
            // textBoxConductorLicense
            // 
            this.textBoxConductorLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConductorLicense.Location = new System.Drawing.Point(11, 198);
            this.textBoxConductorLicense.Name = "textBoxConductorLicense";
            this.textBoxConductorLicense.Size = new System.Drawing.Size(158, 26);
            this.textBoxConductorLicense.TabIndex = 8;
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLastName.Location = new System.Drawing.Point(11, 139);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.Size = new System.Drawing.Size(158, 26);
            this.textBoxLastName.TabIndex = 7;
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFirstName.Location = new System.Drawing.Point(11, 83);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.Size = new System.Drawing.Size(158, 26);
            this.textBoxFirstName.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 182);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "LICENSE NUMBER";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "LAST NAME:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(101, 12);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(261, 42);
            this.label8.TabIndex = 3;
            this.label8.Text = "CONDUCTOR";
            // 
            // buttonAddConductor
            // 
            this.buttonAddConductor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonAddConductor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddConductor.Location = new System.Drawing.Point(41, 230);
            this.buttonAddConductor.Name = "buttonAddConductor";
            this.buttonAddConductor.Size = new System.Drawing.Size(99, 24);
            this.buttonAddConductor.TabIndex = 1;
            this.buttonAddConductor.Text = "ADD";
            this.buttonAddConductor.UseVisualStyleBackColor = true;
            this.buttonAddConductor.Click += new System.EventHandler(this.buttonAddConductor_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "FIRST NAME:";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.textBoxTruckLicense);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.buttonAddTruck);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Location = new System.Drawing.Point(499, 57);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(468, 161);
            this.panel5.TabIndex = 9;
            // 
            // textBoxTruckLicense
            // 
            this.textBoxTruckLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTruckLicense.Location = new System.Drawing.Point(58, 67);
            this.textBoxTruckLicense.Name = "textBoxTruckLicense";
            this.textBoxTruckLicense.Size = new System.Drawing.Size(171, 26);
            this.textBoxTruckLicense.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(153, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 42);
            this.label6.TabIndex = 3;
            this.label6.Text = "TRUCK";
            // 
            // buttonAddTruck
            // 
            this.buttonAddTruck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonAddTruck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddTruck.Location = new System.Drawing.Point(265, 67);
            this.buttonAddTruck.Name = "buttonAddTruck";
            this.buttonAddTruck.Size = new System.Drawing.Size(99, 26);
            this.buttonAddTruck.TabIndex = 1;
            this.buttonAddTruck.Text = "ADD";
            this.buttonAddTruck.UseVisualStyleBackColor = true;
            this.buttonAddTruck.Click += new System.EventHandler(this.buttonAddTruck_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(55, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "TRUCK LICENSE:";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.buttonRemoveLicence);
            this.panel3.Controls.Add(this.comboBoxVesselName1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.comboBoxVesselName);
            this.panel3.Controls.Add(this.buttonAddLicence);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(28, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(380, 175);
            this.panel3.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(93, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 42);
            this.label3.TabIndex = 3;
            this.label3.Text = "LICENCE";
            // 
            // buttonRemoveLicence
            // 
            this.buttonRemoveLicence.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRemoveLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveLicence.Location = new System.Drawing.Point(217, 120);
            this.buttonRemoveLicence.Name = "buttonRemoveLicence";
            this.buttonRemoveLicence.Size = new System.Drawing.Size(99, 26);
            this.buttonRemoveLicence.TabIndex = 7;
            this.buttonRemoveLicence.Text = "REMOVE";
            this.buttonRemoveLicence.UseVisualStyleBackColor = true;
            this.buttonRemoveLicence.Click += new System.EventHandler(this.buttonRemoveLicence_Click);
            // 
            // comboBoxVesselName1
            // 
            this.comboBoxVesselName1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxVesselName1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVesselName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxVesselName1.FormattingEnabled = true;
            this.comboBoxVesselName1.Location = new System.Drawing.Point(35, 120);
            this.comboBoxVesselName1.Name = "comboBoxVesselName1";
            this.comboBoxVesselName1.Size = new System.Drawing.Size(175, 26);
            this.comboBoxVesselName1.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "CHOOSE VESSEL:";
            // 
            // comboBoxVesselName
            // 
            this.comboBoxVesselName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxVesselName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVesselName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxVesselName.FormattingEnabled = true;
            this.comboBoxVesselName.Location = new System.Drawing.Point(35, 67);
            this.comboBoxVesselName.Name = "comboBoxVesselName";
            this.comboBoxVesselName.Size = new System.Drawing.Size(176, 26);
            this.comboBoxVesselName.TabIndex = 0;
            // 
            // buttonAddLicence
            // 
            this.buttonAddLicence.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonAddLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddLicence.Location = new System.Drawing.Point(217, 66);
            this.buttonAddLicence.Name = "buttonAddLicence";
            this.buttonAddLicence.Size = new System.Drawing.Size(99, 26);
            this.buttonAddLicence.TabIndex = 1;
            this.buttonAddLicence.Text = "ADD";
            this.buttonAddLicence.UseVisualStyleBackColor = true;
            this.buttonAddLicence.Click += new System.EventHandler(this.buttonAddLicence_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "CHOOSE VESSEL:";
            // 
            // comboBoxConductor
            // 
            this.comboBoxConductor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxConductor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConductor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxConductor.FormattingEnabled = true;
            this.comboBoxConductor.Location = new System.Drawing.Point(202, 83);
            this.comboBoxConductor.Name = "comboBoxConductor";
            this.comboBoxConductor.Size = new System.Drawing.Size(176, 26);
            this.comboBoxConductor.TabIndex = 8;
            this.comboBoxConductor.SelectedIndexChanged += new System.EventHandler(this.comboBoxConductor_SelectedIndexChanged);
            // 
            // buttonRemoveConductor
            // 
            this.buttonRemoveConductor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRemoveConductor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveConductor.Location = new System.Drawing.Point(201, 117);
            this.buttonRemoveConductor.Name = "buttonRemoveConductor";
            this.buttonRemoveConductor.Size = new System.Drawing.Size(99, 24);
            this.buttonRemoveConductor.TabIndex = 10;
            this.buttonRemoveConductor.Text = "REMOVE";
            this.buttonRemoveConductor.UseVisualStyleBackColor = true;
            this.buttonRemoveConductor.Click += new System.EventHandler(this.buttonRemoveConductor_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "CONDUCTOR";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 579);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADMIN ";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonRemoveLicence;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxVesselName1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAddLicence;
        private System.Windows.Forms.ComboBox comboBoxVesselName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonAddConductor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonAddTruck;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.TextBox textBoxTruckLicense;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxConductorLicense;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.ComboBox comboBoxConductor;
        private System.Windows.Forms.Button buttonRemoveConductor;
        private System.Windows.Forms.Label label4;
    }
}