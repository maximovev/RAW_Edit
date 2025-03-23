using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using image_designer;
using MaxssauLibraries;

namespace RAW_Edit
{
    public partial class frmSettings : Form
    {

        public SettingsManager Settings;
        public classLogger Logger;
        public OperationStatus Status;


        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnSelectColorMatrixFile_Click(object sender, EventArgs e)
        {
            try
            {
                dlgOpenfile.Filter = "cm xml file|*.xml";
                dlgOpenfile.ShowDialog();
                if (dlgOpenfile.FileName != "")
                {
                    Settings.UpdateSetting("ColorMatrixFile", dlgOpenfile.FileName, true);
                    Settings.SaveSettings();
                    txtPathColorMatrixFile.Text = dlgOpenfile.FileName;
                    Status = OperationStatus.STATUS_OK;
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Form Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Form Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            if (Settings != null)
            {
                if (Settings.GetSetting("ColorMatrixFile") != "" || Settings.Status!=OperationStatus.STATUS_OK)
                {
                    txtPathColorMatrixFile.Text = Settings.GetSetting("ColorMatrixFile");
                }
                else
                {
                    txtPathColorMatrixFile.Text= "Not selected";
                }
            }
            else
            {
                txtPathColorMatrixFile.Text = "Settings load error";
            }
        }

        private void btnColorMatrixFileClear_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.UpdateSetting("ColorMatrixFile", "", true);
                txtPathColorMatrixFile.Text = "Not selected";
                Status = OperationStatus.STATUS_OK;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.add_to_log("Form Settings: conversion", ex.Message);
                    if (ex.StackTrace != null)
                    {
                        Logger.add_to_log("Form Settings: conversion", ex.StackTrace);
                    }
                }
                Status = OperationStatus.STATUS_FAIL;
            }
        }
    }
}
