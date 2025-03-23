using image_designer;
using MaxssauLibraries;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using System.ComponentModel;
using System.Threading;

namespace RAW_Edit
{

    enum image_processing_result
    {
        process_ready=0,
        process_fail=1,
        process_busy= 2
    }

    struct _SettingsStatus
    {
        public bool CM_Loaded;
        public bool DCP_Loaded;
    }

    public struct Processing_Image
    {

        public RGB_MinMaxValues image_input_minmax_rgb;
        public string file_name;
        public RGB_Pixel[,] image_input_rgb;
        public SixLabors.ImageSharp.Image<Rgba64> input_image;
        public XYZ_Pixel[,] image_input_xyz;
        public HSV_Pixel[,] image_input_hsv;
        public int height;
        public int width;

    }


    public partial class Form1 : Form
    {
        private classLogger logger = new classLogger();
        private classApplication app = new classApplication();

        private LibRawProcessor raw_data_file;

        private InputRAWImage raw_image;

        private RawData raw_data = new RawData();

        private Processing_Image image_edit;

        private image_processing_result process_result = new image_processing_result();

        private SettingsManager Settings;

        private OperationStatus Status;

        private classXMLCMReader DCP_CM_Settings;

        private _SettingsStatus SettingsStatus;

        public Form1()
        {
            InitializeComponent();

        }

        private void LoadCMSettings(string setting_name, ref SettingsManager Settings, ref classXMLCMReader DCP_CM_Settings, ref _SettingsStatus SettingsStatus, ref classLogger logger, ref OperationStatus Status)
        {
            if (Settings != null)
            {
                if (Settings.GetSetting(setting_name) != "")
                {
                    try
                    {
                        DCP_CM_Settings = new(Settings.GetSetting(setting_name));
                        if (DCP_CM_Settings != null)
                        {
                            if (DCP_CM_Settings.Status == OperationStatus.STATUS_OK)
                            {
                                SettingsStatus.CM_Loaded = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                        {
                            logger.add_to_log("Load CM file: conversion", ex.Message);
                            if (ex.StackTrace != null)
                            {
                                logger.add_to_log("Load CM file: conversion", ex.StackTrace);
                            }
                        }
                        Status = OperationStatus.STATUS_FAIL;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger.app_path = app.GetCurrentFolder();
            logger.file_name = app.GetTimeStamp() + ".log";
            logger.open_log(classLogger.FILE_MODE.APPEND);
            app.log = logger;

            Settings = new("config.xml", ref logger);
            Settings.LoadSettings();

            LoadCMSettings("ColorMatrixFile", ref Settings, ref DCP_CM_Settings, ref SettingsStatus, ref logger, ref Status);

            cmbBitDepth.SelectedIndex = 0;

            for (int i = 0; i < DCP_CM_Settings.values.Count; i++)
            {
                cmbCMProfile.Items.Add(DCP_CM_Settings.values[i].name);
            }
            cmbCMProfile.SelectedIndex = 0;
        }

        private void toolButtonOpen_Click(object sender, EventArgs e)
        {
            dlgOpenFile.Filter = "RAW Files|*.jpg;*.tif;*.tiff";
            dlgOpenFile.ShowDialog();
            if (dlgOpenFile.FileName != "")
            {
                image_edit.file_name = dlgOpenFile.FileName;
                tlbStatus.Text = "Open file...";
                bwOpenFile.RunWorkerAsync();


            }
        }

        void ThreadOpenProcess()
        {

        }

        private void bwOpenFile_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                process_result = image_processing_result.process_busy;

                raw_image = new InputRAWImage();

                image_edit.input_image = SixLabors.ImageSharp.Image.Load<Rgba64>(image_edit.file_name);
                //image_edit.image_input_rgb = new RGB_Pixel[image_edit.input_image.Width, image_edit.input_image.Height];

                raw_image.Image_Input_RAW_RGB = new RGB_Pixel[image_edit.input_image.Width, image_edit.input_image.Height];

                raw_image.ImageWidth = image_edit.input_image.Width;
                raw_image.ImageHeight = image_edit.input_image.Height;

                raw_image.Image_Input_MinMaxLevels.R.reset();
                raw_image.Image_Input_MinMaxLevels.G.reset();
                raw_image.Image_Input_MinMaxLevels.B.reset();

                image_edit.height = image_edit.input_image.Height;
                image_edit.width = image_edit.input_image.Width;

                image_edit.image_input_minmax_rgb.R.reset();
                image_edit.image_input_minmax_rgb.G.reset();
                image_edit.image_input_minmax_rgb.B.reset();

                for (int x = 0; x < image_edit.input_image.Width; x++)
                {
                    for (int y = 0; y < image_edit.input_image.Height; y++)
                    {
                        /*image_edit.image_input_rgb[x, y].R = image_edit.input_image[x, y].R;
                        image_edit.image_input_rgb[x, y].G = image_edit.input_image[x, y].G;
                        image_edit.image_input_rgb[x, y].B = image_edit.input_image[x, y].B;*/

                        raw_image.Image_Input_RAW_RGB[x, y].R = image_edit.input_image[x, y].R;
                        raw_image.Image_Input_RAW_RGB[x, y].G = image_edit.input_image[x, y].G;
                        raw_image.Image_Input_RAW_RGB[x, y].B = image_edit.input_image[x, y].B;

                        raw_image.Image_Input_MinMaxLevels.R.calc(raw_image.Image_Input_RAW_RGB[x, y].R);
                        raw_image.Image_Input_MinMaxLevels.G.calc(raw_image.Image_Input_RAW_RGB[x, y].G);
                        raw_image.Image_Input_MinMaxLevels.B.calc(raw_image.Image_Input_RAW_RGB[x, y].B);

                        image_edit.image_input_minmax_rgb.R.calc((double)image_edit.input_image[x, y].R);
                        image_edit.image_input_minmax_rgb.G.calc((double)image_edit.input_image[x, y].G);
                        image_edit.image_input_minmax_rgb.B.calc((double)image_edit.input_image[x, y].B);


                    }
                }

                process_result = image_processing_result.process_ready;
            }
            catch (Exception ex)
            {
                logger.add_to_log("Open File", ex.Message);
                if (ex.StackTrace != null)
                {
                    logger.add_to_log("Open File", ex.StackTrace);
                }
                process_result = image_processing_result.process_fail;
            }
        }

        private void bwOpenFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (process_result)
            {
                case image_processing_result.process_ready:
                    {
                        tlbStatus.Text = "Open file complete";
                        frmEdit form = new frmEdit();
                        form.image_edit = image_edit;
                        form.raw_image = raw_image;
                        form.Settings = Settings;
                        form.Logger = logger;
                        form.MdiParent = this;
                        form.DCP_CM_Settings = DCP_CM_Settings;
                        form.CM_Selected_Profile = cmbCMProfile.SelectedIndex;

                        switch (cmbBitDepth.SelectedIndex)
                        {
                            case 0:
                                {
                                    form.RAW_BitDepthCoeff = BitDepthCoeff.RAW_12Bit;
                                }
                                break;
                            case 1:
                                {
                                    form.RAW_BitDepthCoeff = BitDepthCoeff.RAW_14Bit;
                                }
                                break;
                        }
                        form.Show();
                    }
                    break;
                case image_processing_result.process_fail:
                    {
                        tlbStatus.Text = "Open file fail";
                    }
                    break;
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings frmSettings = new frmSettings();
            frmSettings.Logger = logger;
            frmSettings.Settings = Settings;
            frmSettings.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tlbBtnSaveImage_Click(object sender, EventArgs e)
        {
            if (HasChildren == true)
            {
                frmEdit activeChild = (frmEdit)this.ActiveMdiChild;
                if (activeChild != null)
                {
                    activeChild.SaveImage();
                }
            }
        }
    }
}
