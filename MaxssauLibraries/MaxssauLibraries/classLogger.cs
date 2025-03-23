/*
 * Created by SharpDevelop.
 * User: maxss
 * Date: 01.05.2024
 * Time: 17:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.IO;

namespace image_designer
{
	/// <summary>
	/// Description of classLogger.
	/// </summary>
	public class classLogger
	{
		public classLogger()
		{
		}
		
		~classLogger()
		{
			if(file!=null)
			{
				
			}
		}
		
		public enum STATUS: int
		{
			CLOSE=0,
			OPEN=1,
			FAIL=2,
			READONLY=3
		}
		
		public enum FILE_MODE:int
		{
			NEW=0,
			APPEND=1
		}
		
		private StreamWriter file;
		
		public string file_name;
		public string app_path;
		
		public STATUS status;
		
		public STATUS open_log(FILE_MODE mode)
		{
			string log_path=app_path+"\\logs";
			string _file_name="";
			try
			{
				if(Directory.Exists(app_path))
				{
					if(!Directory.Exists(log_path))
					{
						Directory.CreateDirectory(log_path);
					}
					
					_file_name=log_path+"\\"+file_name;
				}
			
				FileInfo fInfo = new FileInfo(_file_name);
				if(fInfo.IsReadOnly)
				{
					status=STATUS.READONLY;
					//return STATUS.READONLY;
				}
				
				switch(mode)
				{
					case FILE_MODE.NEW:
						{
							file=new StreamWriter(_file_name,false);
							status=STATUS.OPEN;
							return STATUS.OPEN;
						};
					case FILE_MODE.APPEND:
						{
							file=new StreamWriter(_file_name,true);
							status=STATUS.OPEN;
							return STATUS.OPEN;
						};
					default:
						{
							status=STATUS.CLOSE;
							return STATUS.CLOSE;
						};
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: "+ex.Message+Environment.NewLine+ex.StackTrace,"Log Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				status=STATUS.FAIL;
				return STATUS.FAIL;
			}
			
		}
		
		public int add_to_log(string module, string value)
		{
			string item="";
			try
			{
				item=GetTimeStamp() + "{"+module+"} "+value;
				file.WriteLine(item);
				file.Flush();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: "+ex.Message+Environment.NewLine+ex.StackTrace,"Log Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				status=STATUS.FAIL;
				return -1;
			}
			return 0;
		}
		
		private string GetTimeStamp()
		{
			
			string result=string.Format("[{0,4:t} {0,9:d}] ",DateTime.Now,DateTime.Now);
			return result;
		}
		
	}
}
