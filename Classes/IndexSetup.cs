using System;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Microsoft.VisualBasic;
using System.IO.IsolatedStorage;

namespace SearchO
{
	/// <summary>
	/// Summary description for IndexSetup.
	/// </summary>
	public class IndexSetup
	{
		string indexDirPath;
		public IndexSetup()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public string IndexDirPath
		{
			get
			{
				return indexDirPath;
			}
			set
			{
				indexDirPath = value;
			}
		}
		public bool verfiyIndex()
		{
			try
			{
				EventLogFile log = new EventLogFile();
				//log.DeleteLog();
				//log.RemoveSource();
				if(!indexExist())
				{
					if(!log.VerifyLog("indexLog"))
					{
						log.CreateLog();
					}
					return true;
				}
				else
				{
					string message = "There was Another Indexing Operation performed Before. Do you want to Delete current Index files and perform a new indexing Operation?";
					string caption = "Index Files Exist";
					MessageBoxButtons buttons = MessageBoxButtons.YesNo;
					DialogResult result;

					result = MessageBox.Show( message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

					if(result == DialogResult.Yes)
					{
						if(Directory.Exists(oldIndexDirPath()))
							Directory.Delete(oldIndexDirPath(),true);
						return true;
					}
					return false;
				}
			}
			catch(Exception ee)
			{
				
				MessageBox.Show(ee.Message);
				return false;
			}
		}
		private bool indexExist()
		{
			EventLogFile log = new EventLogFile();
			if(log.VerifyLog("indexLog"))
			{
				if(log.EntryCount > 0)
				{
					if(!log.ReadLastEntry().Equals("deleted"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;                    
				}
			}
			else
			{
				return false;
			}
		}
	
		public bool verfiyStorageSpace(IEnumerator IE)
		{
			IE.Reset();
			long totalFileLength = 0;
			while(IE.MoveNext())
			{
				totalFileLength += (new FileInfo(IE.Current.ToString())).Length;
			}
			foreach(string strDrive in Directory.GetLogicalDrives())
			{
				if(!strDrive.Equals(Directory.GetDirectoryRoot(Environment.SystemDirectory)))
				{
					if((totalFileLength + 100000000) < (new StorageSpace()).GetDiskFreeSpace(strDrive).FreeBytesAvailable)
					{
						//MessageBox.Show((totalFileLength + 100000000).ToString());
						IndexDirPath = strDrive+"\\INDEX";
						if(Directory.Exists(IndexDirPath))
							Directory.Delete(oldIndexDirPath(),true);
						Directory.CreateDirectory(IndexDirPath);
						break;
					}
				}
			}

			return true;
		}

		private string oldIndexDirPath()
		{
			EventLogFile log = new EventLogFile();
			System.Diagnostics.EventLogEntryCollection ELEC = log.ReadAllEntry();
			for(int i = ELEC.Count-1; i>=0 ; i--)
			{
				if((ELEC[i].Message).StartsWith("created@"))
				{
					return (ELEC[i].Message).Replace("created@",string.Empty);
				}
			}
			return string.Empty;
		}
	}
}
