using System;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;

namespace SearchO
{
	/// <summary>
	/// Summary description for EventLogFile.
	/// </summary>
	public class EventLogFile
	{
		#region Class Data
		private System.Diagnostics.EventLog eventLog1;
		#endregion
		#region Construction		
		public EventLogFile()
		{
			this.eventLog1 = new System.Diagnostics.EventLog();
			((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
			//this.eventLog1.SynchronizingObject = this;
			if(System.Diagnostics.EventLog.Exists("indexLog") & System.Diagnostics.EventLog.SourceExists("indexSource"))
			{
				eventLog1.Log = "indexLog";
				eventLog1.Source = "indexSource";
			}
		}
		#endregion
		public void CreateLog()
		{
			try
			{
				// Source cannot already exist before creating the log.
				if(!System.Diagnostics.EventLog.Exists("indexLog") & !System.Diagnostics.EventLog.SourceExists("indexSource"))
				{
					System.Diagnostics.EventLog.CreateEventSource("indexSource", "indexLog");
				}
				// Associate the EventLog component with the new log.
				eventLog1.Log = "indexLog";
				eventLog1.Source = "indexSource";
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}
		public void DeleteLog()
		{
			try
			{
				if (System.Diagnostics.EventLog.Exists(eventLog1.Log))
				{
					System.Diagnostics.EventLog.Delete(eventLog1.Log);
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}
		public void RemoveSource()
		{
			try
			{
				if (System.Diagnostics.EventLog.SourceExists("Source1"))
				{
					System.Diagnostics.EventLog.DeleteEventSource("Source1");
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}


		public int EntryCount
		{
			get
			{
				return eventLog1.Entries.Count;
			}
		}
		public void writeEntry(string message)
		{
			try
			{
				eventLog1.WriteEntry(message);
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}
		public System.Diagnostics.EventLogEntryCollection ReadAllEntry()
		{
			try
			{
				return eventLog1.Entries;
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
				return null;
			}
		}
		public string ReadLastEntry()
		{
			try
			{
				return eventLog1.Entries[eventLog1.Entries.Count-1].Message;
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
				return string.Empty;
			}
		}
		public void ClearLog()
		{
			try
			{
				eventLog1.Clear();
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}
		public bool VerifyLog(string logName)
		{
			try
			{
				return System.Diagnostics.EventLog.Exists(logName);
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
				return false;
			}
		}
		public bool VerifySource(string sourceName)
		{
			try
			{
				return System.Diagnostics.EventLog.SourceExists(sourceName);
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
				return false;
			}
		}
	}
}
