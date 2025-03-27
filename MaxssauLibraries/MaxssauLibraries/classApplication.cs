/*
 * Created by SharpDevelop.
 * User: maxss
 * Date: 01.05.2024
 * Time: 18:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
//using System.Buffers;
using System.Runtime.Intrinsics.Arm;
using image_designer;
using MaxssauLibraries;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace image_designer
{
	/// <summary>
	/// Description of classApplication.
	/// </summary>
	/// 
	public class classRAWData
	{
		public classDCPXMLReader[] DCP_data;
        public int DCP_SelectedProfile;
        public RAW_ConversionStages ConversionStageSetup = new RAW_ConversionStages();
		public RAW_ConversionSetup ConversionSetup = new RAW_ConversionSetup();
		public RAW_Converter_Output_Type OutputType;
		private classLogger Logger;
		public InputRAWImage RawImage = new InputRAWImage();
		public ImagePack ImageOutput = new ImagePack();
		public WhiteBalanceRGBAvg WB_rgb = new WhiteBalanceRGBAvg();
		public classXMLCMReader[] CM_data;
		public BitDepthCoeff RAW_bitdepth_coeff = new BitDepthCoeff();
		public int CM_SelectedProfile;
		public int BlackLevel_User = 0;
		public RGB_Histogram rgb_histogram_output;
		public HSV_Histogram hsv_histogram_output;

        public classRAWData()
        {

        }
    }

	public class classApplication
	{
		public classLogger log;
		
		public string Log_module_name="App";

        private string modulename = "DCP Loader";

        public OperationStatus LoadDCPFiles(ref string[] dcp_files)
		{
            try
            {
				if(!Directory.Exists("DCP"))
				{
					Directory.CreateDirectory("DCP");
				}

				dcp_files = Directory.GetFiles("DCP", "*.dcp_xml");

				return OperationStatus.STATUS_OK;
            }
            catch(Exception ex)
			{
                if (log != null)
                {
					
                    if (log.status == classLogger.STATUS.OPEN)
                    {
                        log.add_to_log(modulename, ex.Message);
                        if (ex.StackTrace != null)
                        {
                            log.add_to_log(modulename, ex.StackTrace.ToString());
                        }
                    }
                }
				return OperationStatus.STATUS_FAIL;
            }
		}

		public classApplication()
		{
		}
		
		public string GetCurrentFolder()
		{
			return Environment.CurrentDirectory;
		}
		
		public string GetTimeStamp()
		{
			return DateTime.Now.Year.ToString()+GetLeaderZero(DateTime.Now.Month)+GetLeaderZero(DateTime.Now.Day);
		}
		
		public string GetLeaderZero(decimal value)
		{
			if(value<10 && value>-1)
			{
				return "0"+value.ToString();
			}
			else
			{
				return value.ToString();
			}
		}
		
		public string GetLeaderDoubleZero(decimal value)
		{
			if(value<9 && value>-1)
			{
				return "00"+value.ToString();
			}
			else
			{
				if(value>9 && value<100)
				{
					return "0"+value.ToString();
				}
				else
				{
					return value.ToString();
				}
			}
		}
	}
}
