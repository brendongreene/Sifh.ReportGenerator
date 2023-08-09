using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sifh.ReportGenerator
{
    public partial class FrmAddCustomer : Form
    {
        public FrmAddCustomer()
        {
            InitializeComponent();
        }
        public string cn = "data source=mis.sifishhouse.com;initial catalog=sifhmis;user id=bgreene;password=@Kw5408bi;MultipleActiveResultSets=True;App=EntityFramework";

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAddCustomer_Click(object sender, EventArgs e)
        {
            var customerName = textBoxAddCustomer.Text; 

            if (customerName == null)
            {
                MessageBox.Show("Enter customer name!");
            }

            using (SqlConnection connection = new SqlConnection(cn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Customer (CustomerName) VALUES ('{customerName}')";
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show($"{customerName} was added.");
        }
    }
}
