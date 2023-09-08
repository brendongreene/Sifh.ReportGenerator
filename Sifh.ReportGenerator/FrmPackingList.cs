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
using System.IO;
using System.Configuration;
using DevExpress.Data.Filtering.Helpers;
using Sifh.ReportGenerator.Model;

namespace Sifh.ReportGenerator
{
    public partial class FrmPackingList : Form
    {
        public List<PackingListReportView> PackingList { get; set; }
        public DateTime productionDate { get; set; }
        public string productionDateMonth { get; set; }
        public string CustomerName { get; set; }

        public int CustomerId { get; set; }

        private int packingListCount = 0;

        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();

        public bool ExecuteButtonClicked { get; private set; }

        private RepositoryItemComboBox riEditComboBox = new RepositoryItemComboBox();
        private Dictionary<string, int> _boxAssignmentTracker = new Dictionary<string, int>();
        public FrmPackingList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            packingListCount++;
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
                   // var archivePath = "Packing Lists";
                    var archivePath = $"{ConfigurationManager.AppSettings["ArchivePath"].ToString()}\\{productionDate.Year}\\{productionDateMonth}\\{productionDate.Day}\\{CustomerName}";
                    var newFile = new FileInfo(productionDate.ToString("dd_MM_yyyy") + "_PackingList_" + packingListCount + ".xlsx");
                    if (!Directory.Exists(archivePath))
                    {
                        Directory.CreateDirectory(archivePath);
                    }
                    var filePath = Path.Combine(archivePath, newFile.Name);
                    var rows = new List<PackingListReportView>();
                    var packingList = new PackingListReportView();
                    packingList.DateCreated = DateTime.Now;
                    packingList.CustomerID = CustomerId;

                    foreach (var selectedRow in selectedRows)
                    {
                        var row = gridView1.GetRow(selectedRow) as PackingListReportView;
                        rows.Add(row);
                        packingList.ReceivingNoteItemIds.Add(row.ReceivingNoteItemID.Value);
                        var packingListDetail = new PackingListDetails()
                        {
                            ReceivingNoteItemID = row.ReceivingNoteItemID.Value,
                            BoxNumber = row.BoxNumber.Value
                        };

                        _repositoryHelper.createPackingListDetail(packingListDetail);
                    }
                    rows.OrderBy(x => x.BoxNumber);

                    _repositoryHelper.AddPackingList(packingList);
                    _reportGenerator.GenerateExcelPackingList(Core.ReportGenerator.ReportType.All, newFile, rows, filePath);
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

            gridView1.Columns["PackingListID"].Visible = false;
            gridView1.Columns["CustomerID"].Visible = false;
            gridView1.Columns["InvoiceNumber"].Visible = false;
            gridView1.Columns["StatusClassID"].Visible = false;
            gridView1.Columns["AirlineID"].Visible = false;
            gridView1.Columns["PackingListNumber"].Visible = false;

            gridView1.Columns["ReceivingNoteID"].Group();

            gridView1.ExpandAllGroups();



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
            
            menu.Caption = " Assign Box Number to" + " " + ((PackingListReportView)gridView1.GetFocusedRow()).BoatName ;
      

            var assignBoxNumber = new DXMenuItem("Assign Box Number");

            assignBoxNumber.Click += (object s, EventArgs args) =>
            {
                popupMenu.Show(Control.MousePosition);                 
            };    
            

            //menu.Items.Add(new DXEditMenuItem("Box Number", riEditComboBox, null, null, null, 100, -1));


            var clearBoxAssignment = new DXMenuItem("Clear Box Assignment");

            clearBoxAssignment.Click += (object s, EventArgs args) =>
            {
                //var row = ((PackingListReportView)gridView1.GetFocusedRow());
                var selectedRow = gridView1.GetFocusedRow() as PackingListReportView;

                        var key = "Box-" + selectedRow.BoxNumber.ToString();

                if (selectedRow.BoxNumber == 0 || selectedRow.BoxNumber == null)
                {
                    MessageBox.Show("No box was assigned to this item");
                    return;
                }

                            if (_boxAssignmentTracker[key] == 2)
                            {
                                ComboBoxNumber.Items.Add("Box-" + selectedRow.BoxNumber);

                                List<string> sortedItems = ComboBoxNumber.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
                                ComboBoxNumber.Items.Clear();
                                ComboBoxNumber.Items.AddRange(sortedItems.ToArray());

                                _boxAssignmentTracker[key]--;
                            }
                            else if (_boxAssignmentTracker[key] < 2)
                            {
                                _boxAssignmentTracker[key]--;
                            }
                            selectedRow.BoxNumber = 0;
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

                    var selectedRow = gridView1.GetFocusedRow() as PackingListReportView;

                    var oldKey = "Box-" + selectedRow.BoxNumber.ToString();

                    if (selectedRow.BoxNumber == Convert.ToInt32(key.Replace("Box-", string.Empty)))
                    {
                        MessageBox.Show("Asiigned box number already" + " " + selectedRow.BoxNumber);
                        return;
                    }
                    else
                    {
                        selectedRow.BoxNumber = Convert.ToInt32(key.Replace("Box-", string.Empty));

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
