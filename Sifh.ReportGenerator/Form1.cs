using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;
using System.Diagnostics;


namespace Sifh.ReportGenerator
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private string conductorID;
        private string truckID;
        private string textBoxPath;
        Form3 form = new Form3();
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();
        public string fileName;
        public byte[] fileContent;
        public string cn = "data source=mis.sifishhouse.com;initial catalog=sifhmis;user id=bgreene;password=@Kw5408bi;MultipleActiveResultSets=True;App=EntityFramework";
        public Form1()
        {
            InitializeComponent();
        }

        private async void simpleButtonExecute_Click(object sender, EventArgs e)
        {
            
            if (comboBoxCustomer.Text.ToString() == "Great Ocean LLC")
            {
                form.DataReady += Form3_DataReady;
                form.TruckData += Form3_TruckData;
                form.ShowDialog();
            }

            var conductorID = Convert.ToInt32(this.conductorID);
            var truckID = Convert.ToInt32(this.truckID);

            var conductor = await _repositoryHelper.GetConductorByIDAsync(conductorID);
            var truck = await _repositoryHelper.GetTruckByIDAsync(truckID);
            var results = _repositoryHelper.GetReceivingNotesByDateRange(dateEditStartDate.DateTime, dateEditEndDate.DateTime);

            foreach (var result in results)
            {
                result.AirwayBillNumber = textBoxAirwayBillNumber.Text;
                result.CustomerName = comboBoxCustomer.Text.Trim();
                result.ProductionDate = dateTimePicker.Value.ToString("MMMM dd yyyy");
                result.BoxNumber = Int32.Parse(textBoxNumberOfBoxes.Text);
                result.ConductorName = conductor.Name;
                result.ConductorLicense = conductor.LicenseNumber;
                result.TruckLicense = truck.License;

               
            }
            gridControl1.DataSource = results.ToList();
        }

        private void Form3_DataReady(object sender, string ConductorID)
        {
            conductorID = ConductorID;
        }

        private void Form3_TruckData(object sender, string TruckID)
        {
            truckID = TruckID;
        }

        private void simpleButtonGenerateReports_Click(object sender, EventArgs e)
        {
            var reportName = comboBoxCustomer.Text.ToString();
           
            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as ReportDataView;
                var vesselID = row.VesselID;
                var vesselName = row.VesselName;
                var reportRNId = row.ReceivingNoteID;
                var fileNameDate = dateTimePicker.Value.ToString("MMMM_dd_yyy");

                var productionDate = DateTime.Parse(row.ProductionDate);
                var productionDateMonth = productionDate.ToString("MMMM");




                var archivePath = $"{productionDate.Year}\\{productionDateMonth}\\{productionDate.Day}\\{row.CustomerName}\\{row.VesselName}";
                var textBoxPath = $"{productionDate.Year}\\{productionDateMonth}\\{productionDate.Day}";
                textBoxArchiveFolder.Text = textBoxPath;
                var newFile = new FileInfo(fileNameDate + "_" + reportName + "_" + reportRNId + ".xlsx");



                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }
                var filePath = Path.Combine(archivePath,newFile.Name);


                _reportGenerator.GenerateExcelReport(Core.ReportGenerator.ReportType.All, newFile,row, filePath);

                //_reportGenerator.TemplateFile = "";
                if (reportName == "Great Ocean LLC")
                {
                    newFile = new FileInfo("Required_" + reportName + "_" + vesselName + "_" + reportRNId + ".xlsx");
                    filePath = Path.Combine(archivePath, newFile.Name);
                    _reportGenerator.GenerateExcelReportCustomer(Core.ReportGenerator.ReportType.All, newFile, row, filePath);
                }


                byte[] fileContent;

                using (SqlConnection connection = new SqlConnection(cn))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT VesselDocument FROM VesselCertificate WHERE VesselID = {vesselID}";
                        fileContent = (byte[])command.ExecuteScalar();
                    }
                }

                if (fileContent != null)
                {
                   var licensePath = $"{archivePath}\\{vesselName}_licence.pdf";
                    File.WriteAllBytes(licensePath, fileContent);
                }
                else
                {
                    MessageBox.Show($"{vesselName}'s licence NOT found");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker.Value = DateTime.Now;
            var customers = _repositoryHelper.GetCustomers().Select(x => new CustomerView()
            {
                CustomerID = x.CustomerID,
                CustomerName = x.CustomerName
            });

            this.comboBoxCustomer.DataSource = customers.ToList();
            this.comboBoxCustomer.DisplayMember = "CustomerName";
            this.comboBoxCustomer.ValueMember = "CustomerId";
        }

        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form2 form = new Form2();
            form.Show();
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form2 form = new Form2();
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            string folderPath = textBoxArchiveFolder.Text;

            if (Directory.Exists(folderPath))
            {
                Process.Start("explorer.exe", folderPath);
            }
            else
            {
                MessageBox.Show("The specified folder path does not exist.");
            }
        }
    }
}
