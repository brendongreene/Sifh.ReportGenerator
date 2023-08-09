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
    public partial class FrmTransportReport : Form
    {
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        public FrmTransportReport()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            var conductors = _repositoryHelper.GetConductors().OrderBy(x => x.FirstName);

            this.comboBoxConductor.DataSource = conductors.ToList();
            this.comboBoxConductor.DisplayMember = "Name";
            this.comboBoxConductor.ValueMember = "ConductorID";

            var trucks = _repositoryHelper.GetTrucks().OrderBy(x => x.License);
            
            this.comboBoxTruck.DataSource = trucks.ToList();
            this.comboBoxTruck.DisplayMember = "License";
            this.comboBoxTruck.ValueMember = "TruckID";
        }

        public event EventHandler<string> DataReady;

        public event EventHandler<string> TruckData;

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var ConductorID = this.comboBoxConductor.SelectedValue.ToString();
            var TruckID = this.comboBoxTruck.SelectedValue.ToString();
            DataReady?.Invoke(this, ConductorID);
            TruckData?.Invoke(this, TruckID);
            this.Close();
        }
    }
}
