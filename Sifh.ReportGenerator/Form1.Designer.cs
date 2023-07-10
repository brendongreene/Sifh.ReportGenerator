namespace Sifh.ReportGenerator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateEditStartDate = new DevExpress.XtraEditors.DateEdit();
            this.dateEditEndDate = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.simpleButtonExecute = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButtonGenerateMCCReports = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonGenerateMTReports = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonGenerateMRCReports = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dateEditStartDate
            // 
            this.dateEditStartDate.EditValue = null;
            this.dateEditStartDate.Location = new System.Drawing.Point(120, 63);
            this.dateEditStartDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dateEditStartDate.Name = "dateEditStartDate";
            this.dateEditStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditStartDate.Size = new System.Drawing.Size(125, 20);
            this.dateEditStartDate.TabIndex = 0;
            // 
            // dateEditEndDate
            // 
            this.dateEditEndDate.EditValue = null;
            this.dateEditEndDate.Location = new System.Drawing.Point(121, 90);
            this.dateEditEndDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dateEditEndDate.Name = "dateEditEndDate";
            this.dateEditEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditEndDate.Size = new System.Drawing.Size(125, 20);
            this.dateEditEndDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Date";
            // 
            // simpleButtonExecute
            // 
            this.simpleButtonExecute.Location = new System.Drawing.Point(293, 79);
            this.simpleButtonExecute.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButtonExecute.Name = "simpleButtonExecute";
            this.simpleButtonExecute.Size = new System.Drawing.Size(50, 15);
            this.simpleButtonExecute.TabIndex = 4;
            this.simpleButtonExecute.Text = "Go";
            this.simpleButtonExecute.Click += new System.EventHandler(this.simpleButtonExecute_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gridControl1.Location = new System.Drawing.Point(8, 122);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(695, 298);
            this.gridControl1.TabIndex = 5;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsSelection.ShowCheckBoxSelectorInPrintExport = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // simpleButtonGenerateMCCReports
            // 
            this.simpleButtonGenerateMCCReports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonGenerateMCCReports.Location = new System.Drawing.Point(401, 436);
            this.simpleButtonGenerateMCCReports.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButtonGenerateMCCReports.Name = "simpleButtonGenerateMCCReports";
            this.simpleButtonGenerateMCCReports.Size = new System.Drawing.Size(98, 15);
            this.simpleButtonGenerateMCCReports.TabIndex = 6;
            this.simpleButtonGenerateMCCReports.Text = "Generate MCC";
            this.simpleButtonGenerateMCCReports.Click += new System.EventHandler(this.simpleButtonGenerateMCCReports_Click);
            // 
            // simpleButtonGenerateMTReports
            // 
            this.simpleButtonGenerateMTReports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonGenerateMTReports.Location = new System.Drawing.Point(605, 436);
            this.simpleButtonGenerateMTReports.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButtonGenerateMTReports.Name = "simpleButtonGenerateMTReports";
            this.simpleButtonGenerateMTReports.Size = new System.Drawing.Size(98, 15);
            this.simpleButtonGenerateMTReports.TabIndex = 7;
            this.simpleButtonGenerateMTReports.Text = "Generate MTC";
            this.simpleButtonGenerateMTReports.Click += new System.EventHandler(this.simpleButtonGenerateMTCReports_Click);
            // 
            // simpleButtonGenerateMRCReports
            // 
            this.simpleButtonGenerateMRCReports.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonGenerateMRCReports.Location = new System.Drawing.Point(503, 436);
            this.simpleButtonGenerateMRCReports.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButtonGenerateMRCReports.Name = "simpleButtonGenerateMRCReports";
            this.simpleButtonGenerateMRCReports.Size = new System.Drawing.Size(98, 15);
            this.simpleButtonGenerateMRCReports.TabIndex = 8;
            this.simpleButtonGenerateMRCReports.Text = "Generate MRC";
            this.simpleButtonGenerateMRCReports.Click += new System.EventHandler(this.simpleButtonGenerateMRCReports_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 459);
            this.Controls.Add(this.simpleButtonGenerateMRCReports);
            this.Controls.Add(this.simpleButtonGenerateMTReports);
            this.Controls.Add(this.simpleButtonGenerateMCCReports);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.simpleButtonExecute);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateEditEndDate);
            this.Controls.Add(this.dateEditStartDate);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dateEditStartDate;
        private DevExpress.XtraEditors.DateEdit dateEditEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton simpleButtonExecute;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonGenerateMCCReports;
        private DevExpress.XtraEditors.SimpleButton simpleButtonGenerateMTReports;
        private DevExpress.XtraEditors.SimpleButton simpleButtonGenerateMRCReports;
    }
}

