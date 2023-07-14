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

        private void buttonGetLicence_Click(object sender, EventArgs e)
        {
            byte[] fileContent;
            var vesselID = Convert.ToInt32(comboBoxVesselName1.SelectedValue);
            var vesselName = Convert.ToString(comboBoxVesselName1.Text);

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
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF Files (*.pdf)|*.pdf";
                DialogResult result = saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;

                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        Document document = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                        document.Open();
                        PdfContentByte cb = writer.DirectContent;
                        PdfReader reader = new PdfReader(ms);
                        for (int pageNum = 1; pageNum <= reader.NumberOfPages; pageNum++)
                        {
                            document.NewPage();
                            PdfImportedPage page = writer.GetImportedPage(reader, pageNum);
                            cb.AddTemplate(page, 0, 0);
                        }
                        document.Close();
                        reader.Close();
                    }

                    MessageBox.Show($"{vesselName}'s licence was retrieved");
                }
            }
            else
            {
                MessageBox.Show($"{vesselName}'s licence NOT found");
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
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
