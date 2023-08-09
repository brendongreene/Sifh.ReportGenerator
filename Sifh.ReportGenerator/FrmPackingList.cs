using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;
using System.Collections.Generic;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;

namespace Sifh.ReportGenerator
{
    public partial class FrmPackingList : Form
    {
        public List<PackingListReportView> PackingList { get; set; }
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();

        public RepositoryItemComboBox RiEditComboBox { get; set; }
        public Dictionary<string, int> BoxAssignmentTracker { get; set; }
        public FrmPackingList()
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
        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {

            var menu = new DXPopupMenu();
            menu.Caption = ((PackingListReportView)gridView1.GetFocusedRow()).BoatName + " Assign Box Number";



            menu.Items.Add(new DXEditMenuItem("Box Number", RiEditComboBox, null, null, null, 100, -1));


            var clearBoxAssignment = new DXMenuItem("Clear Box Assignment");

            clearBoxAssignment.Click += (object s, EventArgs args) =>
            {
                //var row = ((ReportDataView)gridView1.GetFocusedRow());
                var selectedRows = gridView1.GetSelectedRows();

                if (gridView1.SelectedRowsCount == 0)
                {
                    MessageBox.Show("No row(s) selected");
                }
                else
                {
                    foreach (var selectedRow in selectedRows)
                    {
                        var row = gridView1.GetRow(selectedRow) as PackingListReportView;

                        var key = "Box-" + row.BoxNumber.ToString();

                        if (row.BoxNumber == 0)
                        {
                            MessageBox.Show("No box number is assigned");
                        }
                        else
                        {
                            if (BoxAssignmentTracker[key] == 2)
                            {
                                RiEditComboBox.Items.Add("Box-" + row.BoxNumber);

                                List<string> sortedItems = RiEditComboBox.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
                                RiEditComboBox.Items.Clear();
                                RiEditComboBox.Items.AddRange(sortedItems);
                            }
                            else if (BoxAssignmentTracker[key] < 2)
                            {
                                BoxAssignmentTracker[key]--;
                            }
                            row.BoxNumber = 0;
                        }


                    }
                }
            };
            menu.Items.Add(clearBoxAssignment);
            e.Menu.Items.Add(menu);
        }

        private void RiEditComboBox_EditValueChanged(object sender, EventArgs e)
        {
            var comboBoxEdit = sender as ComboBoxEdit;

            //var row = ((ReportDataView)gridView1.GetFocusedRow());

            var key = comboBoxEdit.EditValue.ToString();

            if (gridView1.SelectedRowsCount == 0)
            {
                MessageBox.Show("No row(s) selected");
                return;
            }
            else
            {
                foreach (var selectedRow in gridView1.GetSelectedRows())
                {
                    var row = gridView1.GetRow(selectedRow) as PackingListReportView;

                    var oldKey = "Box-" + row.BoxNumber.ToString();

                    if (row.BoxNumber == Convert.ToInt32(key.Replace("Box-", string.Empty)))
                    {
                        MessageBox.Show("Choose a different box to replace.");
                        return;
                    }                        
                    else
                    {
                        row.BoxNumber = Convert.ToInt32(key.Replace("Box-", string.Empty));

                        if (BoxAssignmentTracker.ContainsKey(oldKey) && BoxAssignmentTracker[oldKey] > 0)
                        {
                            BoxAssignmentTracker[oldKey]--;

                            if (BoxAssignmentTracker[oldKey] <= 0)
                            {
                                RiEditComboBox.Items.Add(oldKey);

                                List<string> sortedItems = RiEditComboBox.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
                                RiEditComboBox.Items.Clear();
                                RiEditComboBox.Items.AddRange(sortedItems);
                            }
                        }

                        BoxAssignmentTracker[key]++;

                        if (BoxAssignmentTracker[key] >= 2)
                        {
                            RiEditComboBox.Items.RemoveAt(comboBoxEdit.SelectedIndex);

                        }
                    }
                }
            }
        }
    }
}
