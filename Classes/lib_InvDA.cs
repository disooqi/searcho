using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace SearchO
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Access
	{
		private string errorReason = "";
		public Access()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public void	givingIdForDoc(string indexDirPath,int docID,string docPath)
		{
			try
			{
				string path = indexDirPath+"\\documents.txt";
				if(!File.Exists(path))
				{
					using (StreamWriter sw = new StreamWriter(path))
					{
						sw.Write(docID.ToString());
						sw.Write(",");
						sw.Write("\"");
						sw.Write(docPath);
						sw.Write("\"");
						sw.Write("\r\n");			
					}
				}
				else
				{
					using (StreamWriter sw = File.AppendText(path))
					{
						sw.Write(docID.ToString());
						sw.Write(",");
						sw.Write("\"");
						sw.Write(docPath);
						sw.Write("\"");
						sw.Write("\r\n");
					}
				}
			}
			catch(Exception es)
			{
				MessageBox.Show("givingIdForDoc: "+es.Message);
			}
		}

		
		public void writingOnFile(ArrayList Tokens,string filePath)
		{
			try
			{				
				if(!File.Exists(filePath))
				{
					
					using (StreamWriter sw = new StreamWriter(filePath ,false ,Encoding.Unicode))
					{
						//sw.Encoding = uni;
						foreach(string token in Tokens)
						{
							sw.WriteLine(token);
						}
					}
				}
				else
				{
					using (StreamWriter sw = File.AppendText(filePath))
					{
						//sw.Encoding = uni;
						foreach(string token in Tokens)
						{
							sw.WriteLine(token);
						}
					}
				}//else
			}
			catch(Exception es)
			{
				MessageBox.Show("writingOnFile: "+es.Message);
				Application.Exit();
			}		
		}

		private System.Boolean executeSqlCommand(string myExecuteQuery, string dbConnection) 
		{
			try
			{
				SqlConnection oConn = new SqlConnection(dbConnection);
				SqlCommand myCommand = new SqlCommand(myExecuteQuery, oConn);
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				oConn.Close();
				return true;
			}
			catch(Exception e)
			{
				e.ToString();
				return false;
			}
		}
		
		private string getDBConnString() 
		{
			return 	 " integrated security=SSPI;" +
				"persist security info=False;initial catalog=Index";
			//return 	 "workstation id=DOS;packet size=4096;integrated security=SSPI;data source=DOS;" +
			//		 "persist security info=False;initial catalog=Lib_Inv";
							
		}

		private System.Boolean getData(System.Data.DataSet ResultSet, string sourcetable,   string sqlString, string dbConnection) 
		{

			try 
			{
				///setup our connection to the database
				System.Data.SqlClient.SqlConnection oConn = new System.Data.SqlClient.SqlConnection(dbConnection);
				oConn.Open();

				///setup the data adapter
				SqlDataAdapter myDataAdapter = new SqlDataAdapter();
				myDataAdapter.SelectCommand = new SqlCommand(sqlString, oConn);
        
				myDataAdapter.Fill(ResultSet,sourcetable);

				///close the connection
				oConn.Close();
				return true;
			} 
			catch (Exception e) 
			{
				this.errorReason = e.ToString();
				return false;
			}
		}

		

	}
}
