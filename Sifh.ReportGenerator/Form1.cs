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
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Sifh.ReportGenerator
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();
        public string fileName;
        public byte[] fileContent;
        public string cn = "data source=mis.sifishhouse.com;initial catalog=sifhmis;user id=bgreene;password=@Kw5408bi;MultipleActiveResultSets=True;App=EntityFramework";
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButtonExecute_Click(object sender, EventArgs e)
        {
            var results = _repositoryHelper.GetReceivingNotesByDateRange(dateEditStartDate.DateTime, dateEditEndDate.DateTime);

            foreach (var result in results)
            {
                result.AirwayBillNumber = textBoxAirwayBillNumber.Text;
                result.CustomerName = comboBoxCustomer.Text.Trim();
                result.ProductionDate = dateTimePicker.Value.ToString("MMMM dd yyyy");
                result.BoxNumber = Int32.Parse(textBoxNumberOfBoxes.Text);
            }
            gridControl1.DataSource = results.ToList();
        }

        private void simpleButtonGenerateReports_Click(object sender, EventArgs e)
        {
            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as ReportDataView;
                var vesselID = row.VesselID;
                var vesselName = row.VesselName;
                var reportRNId = row.ReceivingNoteID;
                var reportName = comboBoxCustomer.Text.ToString();
                var fileNameDate = dateTimePicker.Value.ToString("MMMM_dd_yyy");

                var productionDate = DateTime.Parse(row.ProductionDate);
                var productionDateMonth = productionDate.ToString("MMMM");




                var archivePath = $"{productionDate.Year}\\{productionDateMonth}\\{productionDate.Day}\\{row.CustomerName}\\{row.VesselName}"; 
                var newFile = new FileInfo(fileNameDate + "_" + reportName + "_" + reportRNId + ".xlsx");



                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }
                var filePath = Path.Combine(archivePath,newFile.Name);


                _reportGenerator.GenerateExcelReport(Core.ReportGenerator.ReportType.All, newFile,row, filePath);

                //_reportGenerator.TemplateFile = "";
                if (reportName == "Maruni")
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
    }
}
