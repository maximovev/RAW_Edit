/*
 * Created by SharpDevelop.
 * User: maxss
 * Date: 01.05.2024
 * Time: 18:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace image_designer
{
	/// <summary>
	/// Description of classApplication.
	/// </summary>
	public class classApplication
	{
		public classLogger log;
		
		public string Log_module_name="App";
		
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
