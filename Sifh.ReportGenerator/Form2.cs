﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using iTextSharp.text.pdf;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;
using iTextSharp.text;


namespace Sifh.ReportGenerator
{
    public partial class Form2 : Form
    {
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();
        public Form2()
        {
            InitializeComponent();
        }
        public string cn = "data source=mis.sifishhouse.com;initial catalog=sifhmis;user id=bgreene;password=@Kw5408bi;MultipleActiveResultSets=True;App=EntityFramework";

        private void buttonAddLicence_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PDF Files (*.pdf)|*.pdf";
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                //string fileName = Path.GetFileName(filePath);
                var vesselID = Convert.ToInt32(comboBoxVesselName.SelectedValue);
                var vesselName = Convert.ToString(comboBoxVesselName.Text);

                byte[] fileContent = File.ReadAllBytes(filePath);

                using (SqlConnection connection = new SqlConnection(cn))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO VesselCertificate (VesselID, VesselDocument) VALUES ({vesselID}, @FileContent)";
                    command.Parameters.AddWithValue("@FileContent", fileContent);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show($"{vesselName}'s licence saved.");
            }
        }

        private void buttonRemoveLicence_Click(object sender, EventArgs e)
        {
            var vesselID = Convert.ToInt32(comboBoxVesselName1.SelectedValue);
            var vesselName = comboBoxVesselName.Text;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM VesselCertificate WHERE VesselID = {vesselID}";
                    command.ExecuteNonQuery();
                }
                MessageBox.Show($"{vesselName}'s licence removed.");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var vessels = _repositoryHelper.GetVessels().OrderBy(x => x.VesselName);

            this.comboBoxVesselName.DataSource = vessels.ToList();
            this.comboBoxVesselName.DisplayMember = "VesselName";
            this.comboBoxVesselName.ValueMember = "VesselID";

            this.comboBoxVesselName1.DataSource = vessels.ToList();
            this.comboBoxVesselName1.DisplayMember = "VesselName";
            this.comboBoxVesselName1.ValueMember = "VesselID";

            var conductors = _repositoryHelper.GetConductors().OrderBy(x => x.FirstName);
            this.comboBoxConductor.DataSource = conductors.ToList();
            this.comboBoxConductor.DisplayMember = "Name";
            this.comboBoxConductor.ValueMember = "ConductorID";
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAddTruck_Click(object sender, EventArgs e)
        {
            var truckLicense = textBoxTruckLicense.Text;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Truck (License) VALUES ('{truckLicense}')";
                    command.ExecuteNonQuery();
                }
            }
        }

        private void buttonAddConductor_Click(object sender, EventArgs e)
        {
            var FirstName = textBoxFirstName.Text;
            var LastNAme = textBoxLastName.Text;
            var name = FirstName + " " + LastNAme;
            var ConductorLicense = textBoxConductorLicense.Text;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Conductor (FirstName, LastName, LicenseNumber) VALUES ('{FirstName}', '{LastNAme}', '{ConductorLicense}')";
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show($"{name}'s added.");
            var conductors = _repositoryHelper.GetConductors().OrderBy(x => x.FirstName);
            this.comboBoxConductor.DataSource = conductors.ToList();
            this.comboBoxConductor.DisplayMember = "Name";
            this.comboBoxConductor.ValueMember = "ConductorID";
        }

        private void buttonRemoveConductor_Click(object sender, EventArgs e)
        {
            var conductorlID = Convert.ToInt32(comboBoxConductor.SelectedValue);
            var conductorName = _repositoryHelper.GetConductorByIDAsync(conductorlID);


            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Conductor WHERE ConductorID = {conductorlID}";
                    command.ExecuteNonQuery();
                }
                MessageBox.Show($"{conductorName.Result.Name} removed.");
                var conductors = _repositoryHelper.GetConductors().OrderBy(x => x.FirstName);
                this.comboBoxConductor.DataSource = conductors.ToList();
                this.comboBoxConductor.DisplayMember = "Name";
                this.comboBoxConductor.ValueMember = "ConductorID";
            }
        }
    }
}
