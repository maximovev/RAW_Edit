using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using image_designer;
using MaxssauLibraries;

namespace RAW_Edit
{
    public enum RotateMode
    {
        RotateNone, RotateLeft, RotateRight
    }

    

    public partial class frmEdit : Form
    {


        public Processing_Image image_edit;

        public InputRAWImage     raw_image;

        public SettingsManager Settings;
        public classLogger Logger;
        public OperationStatus Status;

        public classDCPXMLReader DCP_data;
        public classXMLCMReader DCP_CM_Settings;
        public classRAWConverter raw_processor;

        public int CM_Selected_Profile = 0;

        private delegate void WorkerComplete();

        private System.Drawing.Image image;

        private BackgroundWorker workerThread;

        public BitDepthCoeff RAW_BitDepthCoeff;

        void Worker_fn(object sender, DoWorkEventArgs e)
        {
            try
            {
                raw_processor.RAW_Process();
            }
            catch (Exception ex)
            {
                if(Logger!=null)
                {
                    if(Logger.status==classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("RAW Converter", ex.Message);
                        if(ex.StackTrace!=null)
                        {
                            Logger.add_to_log("RAW Converter", ex.StackTrace);
                        }
                    }
                }
            }
        }

        void Worker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Bitmap bitmap = new Bitmap(raw_processor.RawImage.ImageWidth, raw_processor.RawImage.ImageHeight);

                RGB_Pixel pixel = new RGB_Pixel();

                for (int x = 0; x < raw_processor.RawImage.ImageWidth; x++)
                {
                    for (int y = 0; y < raw_processor.RawImage.ImageHeight; y++)
                    {
                        pixel.R = Math.Min(255, raw_processor.ImageOutput.Image_RGB[x, y].R * 255);
                        pixel.G = Math.Min(255, raw_processor.ImageOutput.Image_RGB[x, y].G * 255);
                        pixel.B = Math.Min(255, raw_processor.ImageOutput.Image_RGB[x, y].B * 255);
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb((int)pixel.R, (int)pixel.G, (int)pixel.B));
                    }
                    image = bitmap;
                }

                PreviewWindowUpdateControls();
                ReDrawImage();
            }
            catch(Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("RAW Converter", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("RAW Converter", ex.StackTrace);
                        }
                    }
                }
            }
        }

        public frmEdit()
        {
            InitializeComponent();
        }

        public void SaveImage()
        {
            try
            {
                SaveFileDialog dlgSave = new SaveFileDialog();
                dlgSave.Filter = "Tif files (*.Tif,*.tiff)|*.Tif;*.tiff";
                dlgSave.AddExtension=true;
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    using (SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba64> image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba64>(raw_processor.RawImage.ImageWidth, raw_processor.RawImage.ImageHeight))
                    {
                        for (int x = 0; x < raw_processor.RawImage.ImageWidth; x++)
                        {
                            for (int y = 0; y < raw_processor.RawImage.ImageHeight; y++)
                            {
                                ushort red = (ushort)(raw_processor.ImageOutput.Image_RGB[x, y].R * 65535);
                                ushort green = (ushort)(raw_processor.ImageOutput.Image_RGB[x, y].G * 65535);
                                ushort blue = (ushort)(raw_processor.ImageOutput.Image_RGB[x, y].B * 65535);
                                SixLabors.ImageSharp.PixelFormats.Rgba64 pixel = new SixLabors.ImageSharp.PixelFormats.Rgba64(red, green, blue, ushort.MaxValue);
                                image[x, y] = pixel;
                            }
                        }
                        var tiffEncoder = new SixLabors.ImageSharp.Formats.Tiff.TiffEncoder
                        {
                            PhotometricInterpretation = SixLabors.ImageSharp.Formats.Tiff.Constants.TiffPhotometricInterpretation.Rgb,
                            BitsPerPixel = SixLabors.ImageSharp.Formats.Tiff.TiffBitsPerPixel.Bit48
                        };
                        using (var outputStream = new FileStream(dlgSave.FileName, FileMode.CreateNew))
                        {
                            image.Save(outputStream,tiffEncoder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    if (Logger.status == classLogger.STATUS.OPEN)
                    {
                        Logger.add_to_log("Save Image File", ex.Message);
                        if (ex.StackTrace != null)
                        {
                            Logger.add_to_log("Save Image File", ex.StackTrace);
                        }
                    }
                }
            }
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            
            Bitmap bitmap = new Bitmap(image_edit.width, image_edit.height);
            image = bitmap;

            raw_processor = new classRAWConverter(ref Logger);

            raw_processor.ConversionStageSetup.ClipImageData = true;
            raw_processor.ConversionStageSetup.LinearizeData = true;
            raw_processor.ConversionStageSetup.BlackSubstract = true;
            raw_processor.ConversionStageSetup.WhiteBalanceCorrection = true;
            raw_processor.ConversionStageSetup.ColorTransform = true;
            raw_processor.ConversionStageSetup.ApplyGamma = true;
            raw_processor.ConversionStageSetup.UserBlackLevel = false;
            raw_processor.ConversionStageSetup.UseHighLightReconstructuion = false;

            raw_processor.BlackLevel_User = 1;
            
            raw_processor.RAW_bitdepth_coeff = RAW_BitDepthCoeff;

            raw_processor.DCP_CM_Settings = DCP_CM_Settings;
            raw_processor.DCP_data = DCP_data;

            raw_processor.CM_SelectedProfile=CM_Selected_Profile;

            /*raw_processor.RawImage.ImageWidth = image_edit.width;
            raw_processor.RawImage.ImageHeight = image_edit.height;*/

            raw_processor.RawImage = raw_image;

            workerThread = new BackgroundWorker();

            workerThread.DoWork += Worker_fn;

            workerThread.RunWorkerCompleted += Worker_Complete;



            workerThread.RunWorkerAsync();



            PreviewWindowUpdateControls();
            ReDrawImage();
        }

        public void SetRotate(RotateMode Mode)
        {
            switch (Mode)
            {
                case RotateMode.RotateLeft:
                    {
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    break;
                case RotateMode.RotateRight:
                    {
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    break;
                case RotateMode.RotateNone:
                    {
                        image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    }
                    break;
            }



            PreviewWindowUpdateControls();
            ReDrawImage();

        }

        public bool ImageReady = false;

        void FormPreviewLoad(object sender, EventArgs e)
        {
            PreviewWindowUpdateControls();
        }

        public void SetRAWImage(Bitmap img)
        {
            image = img;
            ReDrawImage();
        }


        private void formPreview_Resize(object sender, EventArgs e)
        {
            // preview tab movement

            PreviewWindowUpdateControls();
            ReDrawImage();
        }

        private void ReDrawImage()
        {
            if (image != null)
            {
                hsbPreview.Maximum = image.Width - picPreview.Width;
                vsbPreview.Maximum = image.Height - picPreview.Height;

                Refresh();
            }
        }

        private void PreviewWindowUpdateControls()
        {
            tabPreview.Left = 0;
            tabPreview.Top = 0;

            if (this.ClientSize.Width > 200 && this.ClientSize.Height > 200)
            {
                picPreview.Top = 0;
                picPreview.Left = 0;

                picPreview.Width = tabRAW1.Width-vsbPreview.Width;
                picPreview.Height = tabRAW1.Height-hsbPreview.Height;


                tabPreview.Height = this.ClientSize.Height - hsbPreview.Height;
                tabPreview.Width = this.ClientSize.Width - vsbPreview.Width;

                hsbPreview.Left = 0;
                hsbPreview.Top = tabPreview.Height;
                hsbPreview.Width = tabPreview.Width;

                vsbPreview.Left = tabPreview.Width;
                vsbPreview.Top = 0;
                vsbPreview.Height = tabPreview.Height;

                ReDrawImage();
            }
        }

        private void vsbPreview_Scroll(object sender, ScrollEventArgs e)
        {
            ReDrawImage();
        }

        private void hsbPreview_Scroll(object sender, ScrollEventArgs e)
        {
            ReDrawImage();
        }

        private void picRAWPreview_Paint(object sender, PaintEventArgs e)
        {
            Point point = new Point(-hsbPreview.Value, -vsbPreview.Value);
            e.Graphics.DrawImage(image, point);
        }

        private void vsbPreview_Scroll(object sender, EventArgs e)
        {
            ReDrawImage();
        }

        private void hsbPreview_ValueChanged(object sender, EventArgs e)
        {
            ReDrawImage();
        }

        private void frmEdit_ResizeEnd(object sender, EventArgs e)
        {
            PreviewWindowUpdateControls();
            ReDrawImage();
        }

        private void frmEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picRAWPreview_Click(object sender, EventArgs e)
        {

        }

        private void frmEdit_Resize(object sender, EventArgs e)
        {
            PreviewWindowUpdateControls();
            ReDrawImage();
        }
    }
}
