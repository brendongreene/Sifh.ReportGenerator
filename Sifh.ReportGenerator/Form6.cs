using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sifh.ReportGenerator
{
    public partial class Form6 : Form
    {
        public List<PackingListReportView> PackingList { get; set; }
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var item in PackingList)
            {
                _repositoryHelper.AddPacingList(item);
            }
            MessageBox.Show("Packing List saved");
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = PackingList;

            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridView1.Columns)
            {
                column.Visible = false;
            }

            // Show the desired columns
            gridView1.Columns["BoatName"].Visible = true;
            gridView1.Columns["Weight"].Visible = true;
            gridView1.Columns["BoxNumber"].Visible = true;
        }
    }
}
