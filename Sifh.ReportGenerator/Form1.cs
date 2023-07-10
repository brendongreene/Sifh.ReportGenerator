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
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;

namespace Sifh.ReportGenerator
{
    public partial class Form1 : Form
    {
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButtonExecute_Click(object sender, EventArgs e)
        {
            var results =_repositoryHelper.GetReceivingNotesByDateRange(dateEditStartDate.DateTime, dateEditEndDate.DateTime);

            gridControl1.DataSource = results.ToList();
        }

        private void simpleButtonGenerateMCCReports_Click(object sender, EventArgs e)
        {
            var newFile = new FileInfo("TestMCC.xlsx");

            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as ReceivingNoteView;

                _reportGenerator.GenerateMCCExcelReport(Core.ReportGenerator.ReportType.ModelCatch, newFile,row);

            }
        }

        private void simpleButtonGenerateMRCReports_Click(object sender, EventArgs e)
        {
            var newFile = new FileInfo("TestMRC.xlsx");

            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as ReceivingNoteView;

                _reportGenerator.GenerateMRCExcelReport(Core.ReportGenerator.ReportType.ModelReprocessing, newFile, row);

            }
        }

        private void simpleButtonGenerateMTCReports_Click(object sender, EventArgs e)
        {

            var newFile = new FileInfo("TestMTC.xlsx");

            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as ReceivingNoteView;

                _reportGenerator.GenerateMTCExcelReport(Core.ReportGenerator.ReportType.ModelTransshipping, newFile, row);

            }
        }
    }
}
