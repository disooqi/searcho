using System;
using System.Collections;
using System.IO;


namespace SearchO
{
	/// <summary>
	/// Summary description for CustomComparer.
	/// </summary>
	public class lineComparer : IComparer  
	{
		int IComparer.Compare( Object x, Object y )  
		{
			string strX = tokenToBeProcessed(x.ToString());
			string strY = tokenToBeProcessed(y.ToString());
			if((new CaseInsensitiveComparer()).Compare( strX, strY ) == 0)
			{
				return(docID(x.ToString())-docID(y.ToString()));
			}
			else
			{
				return((new CaseInsensitiveComparer()).Compare( strX, strY ));
			}
		}

		private string tokenToBeProcessed(string line)
		{
			return line.Substring(line.IndexOf('\"')+1,line.LastIndexOf('\"')-(line.IndexOf('\"')+1));
		}

		private int docID(string line)
		{
			return int.Parse(line.Substring(line.IndexOf(',')+1,line.Length-(line.IndexOf(',')+1)));
		}
	}

	public class runComparer : IComparer  
	{

		int IComparer.Compare( Object x, Object y )  
		{
			string strX = tokenToBeProcessed(((run)x).currentLine);
			string strY = tokenToBeProcessed(((run)y).currentLine);
			if((new CaseInsensitiveComparer()).Compare( strX, strY ) == 0)
			{
				return(docID(((run)x).currentLine.ToString())-docID(((run)y).currentLine.ToString()));
			}
			else
			{
				return((new CaseInsensitiveComparer()).Compare( strX, strY ));
			}
		}
        
		private string tokenToBeProcessed(string line)
		{
			return line.Substring(line.IndexOf('\"')+1,line.LastIndexOf('\"')-(line.IndexOf('\"')+1));
		}

		private int docID(string line)
		{
			return int.Parse(line.Substring(line.IndexOf(',')+1,line.Length-(line.IndexOf(',')+1)));
		}


	}

}
