using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sifh.ReportGenerator.DTO;
using Sifh.ReportGenerator.Repository;
using System.Diagnostics;
using System.Configuration;
using Sifh.ReportGenerator.Model;
using Sifh.ReportGenerator.Core;
using System.Collections.Generic;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using System.Globalization;

namespace Sifh.ReportGenerator
{
    public partial class FrmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private string conductorID;
        private string truckID;
        FrmTransportReport form = new FrmTransportReport();
        private RepositoryHelper _repositoryHelper = new RepositoryHelper();
        private Core.ReportGenerator _reportGenerator = new Core.ReportGenerator();
        private Core.ExcelToPDF _exelToPDF = new Core.ExcelToPDF();
        public string fileName;
        public byte[] fileContent;
        public int fontSize = 3;
        public string cn = "data source=mis.sifishhouse.com;initial catalog=sifhmis;user id=bgreene;password=@Kw5408bi;MultipleActiveResultSets=True;App=EntityFramework";

        private RepositoryItemComboBox riEditComboBox = new RepositoryItemComboBox();
        private Dictionary<string, int> _boxAssignmentTracker = new Dictionary<string, int>();

        public FrmMain()
        {
            InitializeComponent();
        }

        private async void simpleButtonExecute_Click(object sender, EventArgs e)
        {
            this.simpleButtonGenerateReports.Enabled = false;
            riEditComboBox.Items.Clear();
            _boxAssignmentTracker.Clear();

            Cursor.Current = Cursors.WaitCursor;

            if (textBoxAirwayBillNumber.Text == string.Empty)
            {
                MessageBox.Show("Please enter Airway Bill Number");
                return;
            } else this.simpleButtonSave.Enabled = true;

            if (dateEditStartDate.Text == string.Empty && dateEditEndDate.Text == string.Empty)
            {
                MessageBox.Show("Please enter start date and end date");
                return;
            }
            else if (dateEditStartDate.Text == string.Empty)
            {
                MessageBox.Show("Please enter start date");
                return;
            }
            else if (dateEditEndDate.Text == string.Empty)
            {
                MessageBox.Show("Please enter end date");
                return;
            }


            if (comboBoxCustomer.Text.ToString() == "Marumi llc")
            {
                form.DataReady += Form3_DataReady;
                form.TruckData += Form3_TruckData;
                form.ShowDialog();
            }

            var conductorID = Convert.ToInt32(this.conductorID);
            var truckID = Convert.ToInt32(this.truckID);

            var conductor = await _repositoryHelper.GetConductorByIDAsync(conductorID);
            var truck = await _repositoryHelper.GetTruckByIDAsync(truckID);
            var results = _repositoryHelper.GetReceivingNotesByDateRange(dateEditStartDate.DateTime, dateEditEndDate.DateTime).Where(k => k.ReceivingNoteDetails.Count() > 0);

            foreach (var result in results)
            {
                result.AirwayBillNumber = textBoxAirwayBillNumber.Text;
                result.CustomerName = comboBoxCustomer.Text.Trim();
                result.CustomerID = comboBoxCustomer.SelectedValue.ToString();
                result.ProductionDate = dateTimePicker.Value.ToString("MMMM dd yyyy");
                if (comboBoxCustomer.Text.ToString() == "Marumi llc")
                {
                    result.ConductorName = conductor.Name;
                    result.ConductorLicense = conductor.LicenseNumber;
                    result.TruckLicense = truck.License;
                }
            }
            Cursor.Current = Cursors.Default;
            gridControl1.DataSource = results.ToList();

            gridView1.Columns["ReceivingNoteID"].Visible = true;
            gridView1.Columns["InvoiceDate"].Visible = true;
            gridView1.Columns["ReferenceNumber"].Visible = true;
            gridView1.Columns["OrderDate"].Visible = true;
            gridView1.Columns["VesselID"].Visible = true;
            gridView1.Columns["TotalPayments"].Visible = true;
            gridView1.Columns["VesselName"].Visible = true;
            gridView1.Columns["Quantity"].Visible = true;
            gridView1.Columns["NetQuantity"].Visible = true;
            gridView1.Columns["LineItems"].Visible = true;
            gridView1.Columns["ProductName"].Visible = true;
            gridView1.Columns["ProductionDate"].Visible = true;
            gridView1.Columns["ReceivingLotIdentifierMRC"].Visible = true;

            gridView1.Columns["PackingListID"].Visible = false;

            gridView1.Columns["VesselName"].Group();

            gridView1.ExpandAllGroups();
        }

        private void Form3_DataReady(object sender, string ConductorID)
        {
            conductorID = ConductorID;
        }

        private void Form3_TruckData(object sender, string TruckID)
        {
            truckID = TruckID;
        }

        private void simpleButtonGenerateReports_Click(object sender, EventArgs e)
        {
            if(textBoxAirwayBillNumber.Text == String.Empty)
            {
                MessageBox.Show("Enter Airway Bill Number");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            if (gridView1.SelectedRowsCount == 0)
            {
                MessageBox.Show("Make sure the grid was generated and at least 1 row was selected");
                return;
            }

            var reportName = comboBoxCustomer.Text.ToString();

            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                var row = gridView1.GetRow(rowHandle) as PackingListReportView;
                var recievingNoteItems = _repositoryHelper.getReceivingNoteItems(row.PackingListID);
                var packingListQuantity = recievingNoteItems.Sum(x => x.Quantity);
                var product = _repositoryHelper.getProductName(recievingNoteItems.First().ProductID);
                var receivingNotes = _repositoryHelper.GetReceivingNotesDetails(row.PackingListID);
                var totalNumberOfBoxes = _repositoryHelper.getTotalNumberOfBoxes(row.PackingListID);


                foreach (var receivingNote in receivingNotes)
                {
                    var receivingNoteItemsTotal = _repositoryHelper.getReceivingNoteItemsByReceivingNoteID(receivingNote.ReceivingNoteID);
                    var grossQuantity = receivingNoteItemsTotal.Sum(x => x.Quantity);
                    var vessel = _repositoryHelper.GetVesselByID(receivingNote.VesselID);
                    ReportFromPackingListView receivingNoteInUse = new ReportFromPackingListView
                    {
                        ReceivingNoteID = receivingNote.ReceivingNoteID,
                        InvoiceDate = receivingNote.InvoiceDate,
                        ReferenceNumber = receivingNote.ReferenceNumber,
                        OrderDate = receivingNote.OrderDate,
                        VesselID = vessel.VesselID,
                        VesselName = vessel.VesselName,
                        Quantity = packingListQuantity,
                        GrossQuantity = grossQuantity,
                        ProductName = product.ProductName,
                        CustomerName = comboBoxCustomer.SelectedText,
                        AirwayBillNumber = textBoxAirwayBillNumber.Text,
                        RegistrationNumber = vessel.RegistrationNumber,
                        BoxNumber = totalNumberOfBoxes,
                        ProductionDate = row.ProductionDate.Replace("_","/"),
                        ReceivingLotIdentifierMRC = receivingNote.ReceivingNoteID.ToString() + "/" + receivingNote.ReferenceNumber.ToString(),
                        StatusClassID = receivingNote.StatusClassID,
                        TotalPayments = receivingNote.TotalPayments,
                        CheckNumber1 = receivingNote.CheckNumber1,
                        CheckNumber2 = receivingNote.CheckNumber2,
                        DateCreated = receivingNote.DateCreated,
                        FormattedDateCreated = receivingNote.DateCreated.ToString("MMMM dd yyyy", CultureInfo.InvariantCulture)
                };

                    
                    var fileNameDate = dateTimePicker.Value.ToString("MMMM_dd_yyy");

                    DateTime productionDate = DateTime.ParseExact(row.ProductionDate, "dd_MM_yyyy", CultureInfo.InvariantCulture);
                    var productionDateMonth = productionDate.ToString("MMMM");




                    var archivePath = $"{ConfigurationManager.AppSettings["ArchivePath"].ToString()}\\{productionDate.Year}\\{productionDateMonth}\\{productionDate.Day}\\{row.CustomerName}\\{vessel.VesselName}";
                    var textBoxPath = $"{ConfigurationManager.AppSettings["ArchivePath"].ToString()}\\{productionDate.Year}\\{productionDateMonth}\\{productionDate.Day}";
                    textBoxArchiveFolder.Text = textBoxPath;
                    var newFile = new FileInfo(fileNameDate + "_" + reportName + "_" + receivingNote.ReceivingNoteID + ".xlsx");



                    if (!Directory.Exists(archivePath))
                    {
                        Directory.CreateDirectory(archivePath);
                    }
                    var filePath = Path.Combine(archivePath, newFile.Name);

                    var fileType = Int32.Parse(comboBoxFileType.SelectedValue.ToString());

                    _reportGenerator.GenerateExcelReport(Core.ReportGenerator.ReportType.All, newFile, receivingNoteInUse, filePath, fileType);

                    if (fileType == 2)
                    {
                        string pdfFilePath = Path.ChangeExtension(filePath, ".pdf");
                        _exelToPDF.ConvertExcelToPdfUsingAspose(filePath, pdfFilePath);
                        MessageBox.Show("PDF report have been generated");
                    }
                    else
                    {
                        MessageBox.Show("Report(s) have been generated");
                    }

                    //_reportGenerator.TemplateFile = "";
                    if (reportName == "Marumi llc")
                    {
                        newFile = new FileInfo("Required_" + reportName + "_" + vessel.VesselName + "_" + receivingNote.ReceivingNoteID + ".xlsx");
                        filePath = Path.Combine(archivePath, newFile.Name);
                        _reportGenerator.GenerateExcelReportCustomer(Core.ReportGenerator.ReportType.All, newFile, receivingNoteInUse, filePath);

                        if (fileType == 2)
                        {
                            string pdfFilePath = Path.ChangeExtension(filePath, ".pdf");
                            _exelToPDF.ConvertExcelToPdfUsingAspose(filePath, pdfFilePath);

                            MessageBox.Show("PDF for additional files have been generated");
                        }
                        else
                        {
                            MessageBox.Show($"Additional files for {reportName}");
                        }



                        byte[] fileContent;

                        using (SqlConnection connection = new SqlConnection(cn))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = $"SELECT VesselDocument FROM VesselCertificate WHERE VesselID = {vessel.VesselID}";
                                fileContent = (byte[])command.ExecuteScalar();
                            }
                        }

                        if (fileContent != null)
                        {
                            var licensePath = $"{archivePath}\\{vessel.VesselName}_licence.pdf";
                            File.WriteAllBytes(licensePath, fileContent);
                        }
                        else
                        {
                            MessageBox.Show($"{vessel.VesselName}'s licence NOT found");
                        }
                    }
                }
            }  
            Cursor.Current = Cursors.Default;
            gridView1.ClearSelection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.simpleButtonSave.Enabled = false;
            this.simpleButtonGenerateReports.Enabled = false;
            this.textBoxArchiveFolder.Text = ConfigurationManager.AppSettings["ArchivePath"].ToString();

            dateTimePicker.Value = DateTime.Now;
            var customers = _repositoryHelper.GetCustomers().Select(x => new CustomerView()
            {
                CustomerID = x.CustomerID,
                CustomerName = x.CustomerName
            });

            this.comboBoxCustomer.DataSource = customers.ToList();
            this.comboBoxCustomer.DisplayMember = "CustomerName";
            this.comboBoxCustomer.ValueMember = "CustomerId";

            List<FileType> fileType = new List<FileType>();
            fileType.Add(new FileType { Type = ".xlsx", ID = 1 });
            fileType.Add(new FileType { Type = ".pdf", ID = 2 });

            comboBoxFileType.DataSource = fileType;
            comboBoxFileType.DisplayMember = "Type";
            comboBoxFileType.ValueMember = "ID";

            //riEditComboBox.EditValueChanged -= RiEditComboBox_EditValueChanged;
            //riEditComboBox.EditValueChanged += RiEditComboBox_EditValueChanged;

            gridView1.Columns["FormattedDateCreated"].Visible = false;
            gridView1.Columns["CheckNumber1"].Visible = false;
            gridView1.Columns["CheckNumber2"].Visible = false;
            gridView1.Columns["BoxNumber"].Visible = false;
            gridView1.Columns["TotalBoxes"].Visible = false;
            gridView1.Columns["ConductorName"].Visible = false;
            gridView1.Columns["ConductorLicense"].Visible = false;
            gridView1.Columns["TruckLicense"].Visible = false;
            gridView1.Columns["VesselIDForLicence"].Visible = false;
            gridView1.Columns["AirwayBillNumber"].Visible = false;
            gridView1.Columns["CustomerName"].Visible = false;
            gridView1.Columns["RegistrationNumber"].Visible = false;
            gridView1.Columns["CustomerID"].Visible = false;
            gridView1.Columns["PackingListID"].Visible = false;
            //gridView1.Columns["StatusClassID"].Visible = false;

            gridView1.Appearance.Row.FontSizeDelta = fontSize;
        }

        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            string folderPath = textBoxArchiveFolder.Text;

            if (Directory.Exists(folderPath))
            {
                Process.Start("explorer.exe", folderPath);
            }
            else
            {
                MessageBox.Show("The specified folder path does not exist.");
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.dateEditStartDate.EditValue = dateTimePicker.Value;
            this.dateEditEndDate.EditValue = dateTimePicker.Value;
            gridView1.GridControl.DataSource = new List<ReportDataView>();
        }

        private void addCustomer_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            FrmAddCustomer form = new FrmAddCustomer();
            form.FormClosed += Form5_FormClosed;
            form.Show();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        private void comboBoxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.simpleButtonGenerateReports.Enabled = false;
            gridView1.GridControl.DataSource = new List<ReportDataView>();
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            var vessels = new List<string>();
            var productionDate = new DateTime();
            var CustomerName = "";

            if (gridView1.SelectedRowsCount == 0)
            {
                MessageBox.Show("Select recieving notes from grid");
                return;
            }

            if(gridView1.Columns["PackingListID"].Visible == true)
            {
                MessageBox.Show("Generate receiving notes (PRESS GET)");
                return;
            }
            var customerId = Int32.Parse(comboBoxCustomer.SelectedValue.ToString());
            var packingList = new List<PackingListReportView>();
            var receivingNoteItemsList = new List<ReceivingNoteItemView>();
            var packingListNumber = _repositoryHelper.GetLastPackingList() + 1;

            foreach (var rowHandle in gridView1.GetSelectedRows())
            {
                if (rowHandle < 0)
                    continue;
                var row = gridView1.GetRow(rowHandle) as ReportDataView;
                vessels.Add(row.VesselName);
                CustomerName = row.CustomerName;
                productionDate = DateTime.Parse(row.FormattedDateCreated);

                foreach (var receivingNote in row.ReceivingNoteDetails)
                {
                    //receivingNoteItemsList.Add(new ReceivingNoteItemView(item));
                    var box = new PackingListReportView();
                    box.DateCreated = DateTime.Now;
                    box.CustomerID = customerId;
                    box.StatusClassID = receivingNote.GradeClassID;
                    box.Weight = receivingNote.Quantity;
                    box.BoatName = _repositoryHelper.GetVesselName(receivingNote.ReceivingNoteID);
                    box.ReceivingNoteItemID = receivingNote.ReceivingNoteItemID;
                    box.PackingListNumber = packingListNumber;
                    box.CustomerName = comboBoxCustomer.Text;
                    box.AirwayBillNumber = textBoxAirwayBillNumber.Text;
                    box.ProductionDate = dateTimePicker.Value.ToString("MMMM dd yyyy");
                    box.ReceivingNoteID = receivingNote.ReceivingNoteID;

                    packingList.Add(box);
                }
            }
            

            FrmPackingList formPackingList = new FrmPackingList();
            formPackingList.packingList = packingList;
            formPackingList.productionDate = productionDate;
            formPackingList.productionDateMonth = productionDate.ToString("MMMM");
            formPackingList.CustomerName = CustomerName;
            formPackingList.CustomerId = customerId;
            formPackingList.Vessels = vessels;
            formPackingList.FontSize = fontSize;

            formPackingList.FormClosed += Form6_FormClosed;

            formPackingList.ShowDialog();

        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is FrmPackingList form6)
            {
                form6.FormClosed -= Form6_FormClosed;

                if (form6.ExecuteButtonClicked)
                {
                    simpleButtonExecute_Click(sender, e);
                }
            }
        }

        private void gridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            var menu = new DXPopupMenu();

            if(gridView1.GetFocusedRow() == null)
            {
                return;
            }

            if (gridView1.GetFocusedRow().GetType() != typeof(ReportDataView))
            {
                menu.Caption = ((PackingListReportView)gridView1.GetFocusedRow()).PackingListID + " Packing List ";
            } 
            else
            {
                return;
            }


            //menu.Items.Add(new DXEditMenuItem("Box Number", riEditComboBox, null, null, null, 100, -1));


            var cancelPackingList = new DXMenuItem("Cancel This Packing List");

            cancelPackingList.Click += (object s, EventArgs args) =>
            {

                var packingListID = ((PackingListReportView)gridView1.GetFocusedRow()).PackingListID;
                _repositoryHelper.CancelPackingList(packingListID);
                var packingLists = _repositoryHelper.GetOpenPackingLists();

                gridView1.GridControl.DataSource = packingLists;
                ////var row = ((ReportDataView)gridView1.GetFocusedRow());
                //var selectedRows = gridView1.GetSelectedRows();

                //if(gridView1.SelectedRowsCount == 0)
                //{
                //    MessageBox.Show("No row(s) selected");
                //}
                //else
                //{
                //    foreach (var selectedRow in selectedRows)
                //    {
                //        var row = gridView1.GetRow(selectedRow) as ReportDataView;

                //            var key = "Box-" + row.BoxNumber.ToString();

                //            if (row.BoxNumber == 0)
                //            {
                //                MessageBox.Show("No box number is assigned");
                //            }
                //            else
                //            {
                //                if (_boxAssignmentTracker[key] == 2)
                //                {
                //                    riEditComboBox.Items.Add("Box-" + row.BoxNumber);

                //                    List<string> sortedItems = riEditComboBox.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
                //                    riEditComboBox.Items.Clear();
                //                    riEditComboBox.Items.AddRange(sortedItems);
                //                }
                //                else if (_boxAssignmentTracker[key] < 2)
                //                {
                //                    _boxAssignmentTracker[key]--;
                //                }
                //                row.BoxNumber = 0;
                //            }


                //    }
                //}         
            };
            menu.Items.Add(cancelPackingList);
            if (e.Menu != null)
            {
                e.Menu.Items.Add(menu);
            }
            else return;
        }

        //private void RiEditComboBox_EditValueChanged(object sender, EventArgs e)
        //{
        //    var comboBoxEdit = sender as ComboBoxEdit;

        //    //var row = ((ReportDataView)gridView1.GetFocusedRow());

        //    var key = comboBoxEdit.EditValue.ToString();

        //    if(gridView1.SelectedRowsCount == 0)
        //    {
        //        MessageBox.Show("No row(s) selected");
        //        return; 
        //    } else
        //    {
        //        foreach (var selectedRow in gridView1.GetSelectedRows())
        //        {
        //            var row = gridView1.GetRow(selectedRow) as ReportDataView;
                    
        //                var oldKey = "Box-" + row.BoxNumber.ToString();

        //                if (row.BoxNumber == Convert.ToInt32(key.Replace("Box-", string.Empty)))
        //                {
        //                    MessageBox.Show("Choose a different box to replace.");
        //                    return;
        //                }
        //                else
        //                {
        //                    row.BoxNumber = Convert.ToInt32(key.Replace("Box-", string.Empty));

        //                    if (_boxAssignmentTracker.ContainsKey(oldKey) && _boxAssignmentTracker[oldKey] > 0)
        //                    {
        //                        _boxAssignmentTracker[oldKey]--;
                                
        //                        if (_boxAssignmentTracker[oldKey] <= 0)
        //                        {
        //                            riEditComboBox.Items.Add(oldKey);

        //                            List<string> sortedItems = riEditComboBox.Items.Cast<string>().OrderBy(item => Convert.ToInt32(item.Replace("Box-", ""))).ToList();
        //                            riEditComboBox.Items.Clear();
        //                            riEditComboBox.Items.AddRange(sortedItems);
        //                        }
        //                    }

        //                    _boxAssignmentTracker[key]++;

        //                    if (_boxAssignmentTracker[key] >= 2)
        //                    {
        //                        riEditComboBox.Items.RemoveAt(comboBoxEdit.SelectedIndex);

        //                    }
        //                } 
        //        }
        //    }
        //}

        private void AddToBoxMenu_Click(object sender, EventArgs e)
        {

        }

        private void textBoxArchiveFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxFileType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonViewPackingList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.simpleButtonSave.Enabled = false;
            var packingLists = _repositoryHelper.GetOpenPackingLists();

            gridView1.GridControl.DataSource = packingLists;

            gridView1.Columns["ReceivingNoteID"].Visible = false;
            gridView1.Columns["InvoiceDate"].Visible = false;
            gridView1.Columns["ReferenceNumber"].Visible = false;
            gridView1.Columns["OrderDate"].Visible = false;
            gridView1.Columns["VesselID"].Visible = false;
            gridView1.Columns["TotalPayments"].Visible = false;
            gridView1.Columns["VesselName"].Visible = false;
            gridView1.Columns["Quantity"].Visible = false;
            gridView1.Columns["NetQuantity"].Visible = false;
            gridView1.Columns["LineItems"].Visible = false;
            gridView1.Columns["ProductName"].Visible = false;
            gridView1.Columns["ReceivingLotIdentifierMRC"].Visible = false;

            gridView1.Columns["PackingListID"].Visible = true;
            gridView1.Columns["PackingListID"].VisibleIndex = 1;
            gridView1.Columns["DateCreated"].VisibleIndex = 2;
            gridView1.Columns["ProductionDate"].Visible = true;

            this.simpleButtonGenerateReports.Enabled = true;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            FrmAdmin form = new FrmAdmin();
            form.Show();
        }

        private void buttonIncreaseFontSize_Click(object sender, EventArgs e)
        {
            fontSize++;
            gridView1.Appearance.Row.FontSizeDelta = fontSize;
            gridView1.RefreshData();
        }

        private void buttonDecreaseFontSize_Click(object sender, EventArgs e)
        {
            fontSize--;
            gridView1.Appearance.Row.FontSizeDelta = fontSize;
            gridView1.RefreshData();
        }
    }
}
