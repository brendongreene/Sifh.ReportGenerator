using System;
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

            var trucks = _repositoryHelper.GetTrucks().OrderBy(x => x.License);
            this.comboBoxTruck.DataSource = trucks.ToList();
            this.comboBoxTruck.DisplayMember = "License";
            this.comboBoxTruck.ValueMember = "TruckID";
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAddTruck_Click(object sender, EventArgs e)
        {
            if (textBoxTruckLicense.Text == string.Empty)
            {
                MessageBox.Show("Enter license");
                return;
            }
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
            MessageBox.Show($"{truckLicense}'s added.");
            var trucks = _repositoryHelper.GetTrucks().OrderBy(x => x.License);
            this.comboBoxTruck.DataSource = trucks.ToList();
            this.comboBoxTruck.DisplayMember = "License";
            this.comboBoxTruck.ValueMember = "TruckID";
        }

        private void buttonAddConductor_Click(object sender, EventArgs e)
        {
            if(textBoxFirstName.Text == string.Empty && textBoxLastName.Text == string.Empty && textBoxConductorLicense.Text == string.Empty)
            {
                MessageBox.Show("Enter first name, last name and conductor's license number");
                return;
            } else if (textBoxFirstName.Text == string.Empty)
            {
                MessageBox.Show("Enter first name");
                return;
            } else if(textBoxLastName.Text == string.Empty)
            {
                MessageBox.Show("Enterlast name");
                return;
            } else if(textBoxConductorLicense.Text == string.Empty)
            {
                MessageBox.Show("Enter conductor's license");
                return;
            }

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

        private async void buttonRemoveConductor_Click(object sender, EventArgs e)
        {
            var conductorlID = Convert.ToInt32(comboBoxConductor.SelectedValue);
            var conductorName = await _repositoryHelper.GetConductorByIDAsync(conductorlID);

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Conductor WHERE ConductorID = {conductorlID}";
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show($"{conductorName.Name} removed.");
            var conductors = _repositoryHelper.GetConductors().OrderBy(x => x.FirstName);
            this.comboBoxConductor.DataSource = conductors.ToList();
            this.comboBoxConductor.DisplayMember = "Name";
            this.comboBoxConductor.ValueMember = "ConductorID";
        }

        private async void buttonRemoveTruck_ClickAsync(object sender, EventArgs e)
        {
            var truckID = Convert.ToInt32(comboBoxTruck.SelectedValue);
            var truck =  await _repositoryHelper.GetTruckByIDAsync(truckID);


            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Truck WHERE TruckID = {truckID}";
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show($"{truck.License} removed.");
            var trucks = _repositoryHelper.GetTrucks().OrderBy(x => x.License);
            this.comboBoxTruck.DataSource = trucks.ToList();
            this.comboBoxTruck.DisplayMember = "License";
            this.comboBoxTruck.ValueMember = "TruckID";
        }
    }
}
