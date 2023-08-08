using DevExpress.LookAndFeel.Design;
using DevExpress.Utils.Menu;
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
    public partial class FrmBoxPicker : Form
    {
        public DXPopupMenu PopupMenu;

        public FrmBoxPicker()
        {
            InitializeComponent();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ((IDXDropDownControl)PopupMenu).Show(new SkinMenuManager(UserLookAndFeelDefault.Default), this, e.Location);
            }
        }
    }
}
