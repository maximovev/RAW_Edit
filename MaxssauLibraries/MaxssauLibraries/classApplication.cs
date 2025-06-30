/*

maxssau

Maximov Evgeny
9890175@mail.ru
Russia, Samara

2024/05/01 - first edit
2025/04/01 - refactoring

Logger

 */
using System;
using System.IO;
//using System.Buffers;
using System.Runtime.Intrinsics.Arm;
using image_designer;
using MaxssauLibraries;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Generic;

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
		private ClassLogger Logger;
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

	



	public class classApplication: ClassAddToLog
    {

        private string ModuleName = "Application";
        		
		public string Log_module_name="App";

        private string modulename = "DCP Loader";

		public classDCPLoader DCP_Data;

		public void LoadDCPData()
		{
			DCP_Data = new classDCPLoader(Logger);
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
