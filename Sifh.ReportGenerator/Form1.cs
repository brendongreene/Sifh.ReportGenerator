using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;

namespace Sifh.ReportGenerator
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();
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

        private void simpleButtonGenerateMCCReports_Click(object sender, EventArgs e)
        {
            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as ReportDataView;
                var reportRNId = row.ReceivingNoteID;
                var reportName = comboBoxCustomer.Text.Trim();
                var fileNameDate = dateTimePicker.Value.ToString("MMMM_dd_yyy");
                var newFile = new FileInfo(fileNameDate + "_" + reportName + "_" + reportRNId + ".xlsx");


                _reportGenerator.GenerateExcelReport(Core.ReportGenerator.ReportType.All, newFile,row);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var customers = _repositoryHelper.GetCustomers().Select(x => new CustomerView()
            {
                CustomerID = x.CustomerID,
                CustomerName = x.CustomerName
            });

            this.comboBoxCustomer.DataSource = customers.ToList();
            this.comboBoxCustomer.DisplayMember = "CustomerName";
            this.comboBoxCustomer.ValueMember = "CustomerId";
        }

    }
}
