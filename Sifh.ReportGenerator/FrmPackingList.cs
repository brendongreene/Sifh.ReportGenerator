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

        public bool ExecuteButtonClicked { get; private set; }

        private RepositoryItemComboBox riEditComboBox = new RepositoryItemComboBox();
        private Dictionary<string, int> _boxAssignmentTracker = new Dictionary<string, int>();
        public FrmPackingList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedRows = gridView1.GetSelectedRows();
            if (gridView1.SelectedRowsCount == 0)
            {
                MessageBox.Show("No row(s) selected");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to create packing list?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (var selectedRow in selectedRows)
                    {
                        var row = gridView1.GetRow(selectedRow) as PackingListReportView;
                        _repositoryHelper.AddPacingList(row);
                    }
                    MessageBox.Show("Packing List saved");
                    ExecuteButtonClicked = true;

                    this.Close();
                }
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {

            ComboBoxNumber.SelectedIndexChanged -= ComboBoxNumber_EditValueChanged;
            ComboBoxNumber.SelectedIndexChanged += ComboBoxNumber_EditValueChanged;

            gridControl1.DataSource = PackingList;

            riEditComboBox.Items.Clear();
            _boxAssignmentTracker.Clear();

            for (int boxCount = 1; boxCount <= PackingList.Count; boxCount++)
            {
                var key = "Box-" + boxCount.ToString();
                riEditComboBox.Items.Add(key);
                ComboBoxNumber.Items.Add(key);
                _boxAssignmentTracker.Add(key, 0);
            }
        }

        private void gridView1_PopupMenuShowing_1(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            var menu = new DXPopupMenu();
            
            if(gridView1.SelectedRowsCount > 0)
            {
                menu.Caption = "Assign Box Number for Selected Rows";
            }
            else
            {
                menu.Caption = " Assign Box Number to" + ((PackingListReportView)gridView1.GetFocusedRow()).BoatName ;
            }

            var assignBoxNumber = new DXMenuItem("Assign Box Number");

            assignBoxNumber.Click += (object s, EventArgs args) =>
            {
                if (gridView1.SelectedRowsCount == 0)
                {
                    MessageBox.Show("Select Row(s)");
                    return;
                } 
                else
                {
                    popupMenu.Show(Control.MousePosition);                 
                }
            };    
            

            //menu.Items.Add(new DXEditMenuItem("Box Number", riEditComboBox, null, null, null, 100, -1));


            var clearBoxAssignment = new DXMenuItem("Clear Box Assignment");

            clearBoxAssignment.Click += (object s, EventArgs args) =>
            {
                //var row = ((PackingListReportView)gridView1.GetFocusedRow());
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
                            gridView1.UnselectRow(selectedRow);
                        }
                        else
                        {
                            if (_boxAssignmentTracker[key] == 2)
                            {
                                ComboBoxNumber.Items.Add("Box-" + row.BoxNumber);

                                List<string> sortedItems = ComboBoxNumber.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
                                ComboBoxNumber.Items.Clear();
                                ComboBoxNumber.Items.AddRange(sortedItems.ToArray());

                                _boxAssignmentTracker[key]--;
                            }
                            else if (_boxAssignmentTracker[key] < 2)
                            {
                                _boxAssignmentTracker[key]--;
                            }
                            row.BoxNumber = 0;
                        }
                        gridView1.UnselectRow(selectedRow);
                    }
                }
            };

            menu.Items.Add(assignBoxNumber);
            menu.Items.Add(clearBoxAssignment);
            e.Menu.Items.Add(menu);
        }

        private void ComboBoxNumber_EditValueChanged(object sender, EventArgs e)
        {
            ToolStripComboBox ComboBoxNumber = (ToolStripComboBox)sender;

            if (ComboBoxNumber.SelectedItem != null)
            {
                var key = ComboBoxNumber.SelectedItem.ToString();

                foreach (var selectedRow in gridView1.GetSelectedRows())
                {
                    var row = gridView1.GetRow(selectedRow) as PackingListReportView;

                    var oldKey = "Box-" + row.BoxNumber.ToString();

                    if (row.BoxNumber == Convert.ToInt32(key.Replace("Box-", string.Empty)))
                    {
                        MessageBox.Show("Asiigned box number already" + row.BoxNumber);
                        return;
                    }
                    else
                    {
                        row.BoxNumber = Convert.ToInt32(key.Replace("Box-", string.Empty));

                        if (_boxAssignmentTracker.ContainsKey(oldKey) && _boxAssignmentTracker[oldKey] > 0)
                        {
                            _boxAssignmentTracker[oldKey]--;

                            if (!ComboBoxNumber.Items.Contains(oldKey))
                            {
                                ComboBoxNumber.Items.Add(oldKey);

                                List<string> sortedItems = ComboBoxNumber.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
                                ComboBoxNumber.Items.Clear();
                                ComboBoxNumber.Items.AddRange(sortedItems.ToArray());
                            }
                        }

                        _boxAssignmentTracker[key]++;

                        if (_boxAssignmentTracker[key] >= 2)
                        {
                            ComboBoxNumber.Items.Remove(key);

                        }
                    }
                }

                popupMenu.Close();

            }
        }

        //private void RiEditComboBox_EditValueChanged(object sender, EventArgs e)
        //{
        //    var comboBoxEdit = sender as ComboBoxEdit;

        //    //var row = ((ReportDataView)gridView1.GetFocusedRow());

        //    var key = comboBoxEdit.EditValue.ToString();

        //    if (gridView1.SelectedRowsCount == 0)
        //    {
        //        MessageBox.Show("No row(s) selected");
        //        return;
        //    }
        //    else
        //    {
        //        foreach (var selectedRow in gridView1.GetSelectedRows())
        //        {
        //            var row = gridView1.GetRow(selectedRow) as PackingListReportView;

        //            var oldKey = "Box-" + row.BoxNumber.ToString();

        //            if (row.BoxNumber == Convert.ToInt32(key.Replace("Box-", string.Empty)))
        //            {
        //                MessageBox.Show("Choose a different box to replace.");
        //                return;
        //            }
        //            else
        //            {
        //                row.BoxNumber = Convert.ToInt32(key.Replace("Box-", string.Empty));

        //                if (_boxAssignmentTracker.ContainsKey(oldKey) && _boxAssignmentTracker[oldKey] > 0)
        //                {
        //                    _boxAssignmentTracker[oldKey]--;

        //                    if (_boxAssignmentTracker[oldKey] <= 0)
        //                    {
        //                        riEditComboBox.Items.Add(oldKey);

        //                        List<string> sortedItems = riEditComboBox.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
        //                        riEditComboBox.Items.Clear();
        //                        riEditComboBox.Items.AddRange(sortedItems);
        //                    }
        //                }

        //                _boxAssignmentTracker[key]++;

        //                if (_boxAssignmentTracker[key] >= 2)
        //                {
        //                    riEditComboBox.Items.RemoveAt(comboBoxEdit.SelectedIndex);

        //                }
        //            }
        //        }
        //    }
        //}
    }
}
