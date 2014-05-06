using System;
using System.Collections;
using System.IO;
using splitingFile;
using System.Threading;
using System.Windows.Forms;
using System.Text;

namespace SearchO
{
	/// <summary>
	/// Summary description for BuildingIndex.
	/// </summary>
	public class BuildingIndex : Control
	{
		#region Data section

		private GroupBox Group;
		private ProgressBar Total;
		private ProgressBar singleProc;
		private Label TotalLabel;
		private Label sinLabel;
		private string indexDirPath;
		frmStatus f = new frmStatus();

		private Access dataAccess = new Access();
		private ArrayList file = new ArrayList();
		private MEMORYSTATUSEX stat = new MEMORYSTATUSEX();
		private int runCount = 0, fileCount;
		private Thread indexThread;

		//private bool indexing = false;

		private delegate void AdvProgsDelegate(int totalValue, int sinValue, string processName);
		private AdvProgsDelegate APD;

		private IEnumerator fileIE;
		private long numberOfLinesInFile = 0;

		#endregion
		#region Constructor
		public BuildingIndex()
		{
			Group = new GroupBox();
			Total = new ProgressBar();
			singleProc = new ProgressBar();
			TotalLabel = new Label();
			sinLabel = new Label();

            APD = new AdvProgsDelegate(AdvanceProgress);
			
			Controls.Add(Group);
			Group.Controls.Add(Total);
			Group.Controls.Add(singleProc);
			Group.Controls.Add(TotalLabel);
			Group.Controls.Add(sinLabel);

			Group.Dock = DockStyle.Fill;

			Group.Text = "Indexing Progress";

			this.singleProc.Location = new System.Drawing.Point(16, 90);
			this.singleProc.Name = "singleProc";
			this.singleProc.Size = new System.Drawing.Size(752, 24);
			singleProc.Value = 0;

			this.Total.Location = new System.Drawing.Point(16, 38);
			this.Total.Name = "Total";
			this.Total.Size = new System.Drawing.Size(752, 24);

			this.sinLabel.Location = new System.Drawing.Point(16, 76);
			this.sinLabel.Name = "sinLabel";
			this.sinLabel.Size = new System.Drawing.Size(760, 24);
			this.sinLabel.TabIndex = 1;
			this.sinLabel.Text = "Current Process";

			this.TotalLabel.Location = new System.Drawing.Point(16, 24);
			this.TotalLabel.Name = "TotalLabel";
			this.TotalLabel.Size = new System.Drawing.Size(760, 16);
			this.TotalLabel.TabIndex = 0;
			this.TotalLabel.Text = "Whole Process";	
		}
		#endregion
		#region Steps to build inverted file

		private void collectDocument()
		{
		}
		public void Tokenization(IEnumerator IE, int count)
		{
			try
			{
				IE.Reset();
				int docID = 1;
				while(IE.MoveNext())
				{
					try
					{
						BeginInvoke(APD,new object[] {((int)Math.Ceiling(100*docID/count))/4,(int)Math.Ceiling(100*docID/count)," Tokenizing .... : "+IE.Current.ToString()});
						StreamReader DocStream = new StreamReader(IE.Current.ToString());
						dataAccess.givingIdForDoc(indexDirPath ,docID,IE.Current.ToString());
						DocTokenization(DocStream,docID);				
						docID++;
					}
					catch(Exception ee)
					{
						MessageBox.Show("error# 1.1: "+ee.Message);
					}			
				}
				createARun(--docID, true);
			}
			catch(Exception ee)
			{
				MessageBox.Show("error# 1.2 : "+ee.Message);
			}
		}
		private void linguisticProcessing(ArrayList tokens, int docID)
		{
			try
			{
				ArrayList DocTokenswithCaseFolding = CaseFolding(tokens);
				ArrayList DocTokenswithoutStopList = StopList(DocTokenswithCaseFolding);
				Stemming stemObj = new Stemming(DocTokenswithoutStopList);

				foreach(string tok in stemObj.Stem())
				{
					file.Add("\""+tok+"\","+docID.ToString());
				}
				createARun(docID, false);			
			}
			catch(Exception ee)
			{
				MessageBox.Show("error# 14"+ee.Message);
			}
		}
		public void createIndex()
		{
			try
			{
				//sorting terms
				Step1(runCount);
			
				//Agregate terms in same Documents
				Step2(indexDirPath+"\\step2.txt");

				//Agregate terms in the whole corpus
				Step3(indexDirPath+"\\step3.txt");
			}
			catch(Exception ee)
			{
				MessageBox.Show("error# 15"+ee.Message);
			}
		}

		#endregion
		#region Tokenization Region
		public int RunCount
		{
			get
			{
				return runCount;
			}
		}

		private void DocTokenization(StreamReader DocStream, int docID)
		{
			try
			{
				string line;
				while((line = DocStream.ReadLine()) != null)
				{
					ArrayList tokens = lineTokenization(line);
					linguisticProcessing(tokens, docID);
				}
				DocStream.Close();			
			}
			catch(Exception ee)
			{
				MessageBox.Show("error# 16"+ee.Message);
			} 
			
		}

		private ArrayList lineTokenization(string line)
		{
			try
			{			
				ArrayList DocTokens = new ArrayList();
				char[] delimiter = new char[24];
				delimiter[0]=' ';
				delimiter[1]=',';
				delimiter[2]='\r';
				delimiter[3]='\n';
				delimiter[4]=';';
				delimiter[5]=':';
				delimiter[6]='.';//U.S.A
				delimiter[7]='(';
				delimiter[8]=')';
				delimiter[9]='|';
				delimiter[10]='[';
				delimiter[11]=']';
				delimiter[12]='+';
				delimiter[13]='-';
				delimiter[14]='*';
				delimiter[15]='=';
				delimiter[16]='{';
				delimiter[17]='<';
				delimiter[18]='>';
				delimiter[19]='\'';
				delimiter[20]='\"';
				delimiter[21]='}';
				delimiter[22]='	';
				delimiter[23]='@';


				//line = line.Replace(". "," ");
				//line = line.Replace(".\r"," ");
				DocTokens.AddRange(line.Split(delimiter));
				DocTokens.Sort();
				int n = DocTokens.Count;
				for(int i = n-1; i >= 0 ; i--)
				{
					if(DocTokens[i].Equals(""))
					{
						DocTokens.RemoveRange(0,i+1);
						break;
					}
				}			
				return DocTokens;
			}
			catch(Exception ee)
			{
				MessageBox.Show("error# 2 "+ee.Message);
				return null;
			}
		}
		private void createARun(int docID, bool isNoMoreDoc)
		{
			try
			{
				if(!isMemoryEmpty() || isNoMoreDoc)
				{
					BeginInvoke(APD,new object[] {50,100,"Creating Runs..."});
					IComparer termComp = new lineComparer();
					file.Sort(termComp);
					//foreach(string str in file)
					//f.Show(str);
					string path = indexDirPath+"\\run"+(++runCount).ToString()+".txt";
					dataAccess.writingOnFile(file,path);
					file.Clear();
					GarbageCollection();
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show("createARun: "+ee.Message);
			}
		}


		#endregion
		#region Linguistic Processing Region
		private ArrayList CaseFolding(ArrayList DocTokens)
		{
			try
			{
				ArrayList TokensAfterCaseFolding = new ArrayList();
				foreach (string s in DocTokens)
				{
					TokensAfterCaseFolding.Add(s.ToLower());
				}
				return TokensAfterCaseFolding;
			}
			catch(Exception ee)
			{
				MessageBox.Show("CaseFolding: "+ee.Message);
				return null;
			}			
		}

		private ArrayList StopList(ArrayList DocTokens)
		{
			try
			{
				ArrayList DocTokensWithoutStopList = new ArrayList();
				const int numOfStopList = 24;
				string[] stop_List = new string[numOfStopList];
				stop_List[0]="a";
				stop_List[1]="in";
				stop_List[2]="the";
				stop_List[3]="to";
				stop_List[4]="for";
				stop_List[5]="that";
				stop_List[6]="into";
				stop_List[7]="this";
				stop_List[8]="has";
				stop_List[9]="have";
				stop_List[10]="is";
				stop_List[11]="are";
				stop_List[12]="and";
				stop_List[13]="and";
				stop_List[14]="on";
				stop_List[15]="an";
				stop_List[16]="also";
				stop_List[17]="be";
				stop_List[18]="by";
				stop_List[19]="our";
				stop_List[20]="we";
				stop_List[21]="were";
				stop_List[22]="of";
				stop_List[23]="it";
				for(int i = 0; i<numOfStopList ; i++)
				{
					DocTokensWithoutStopList = removeToken(DocTokens,stop_List[i]);
				}
				return DocTokensWithoutStopList;
			}
			catch(Exception ee)
			{
				MessageBox.Show("StopList: "+ee.Message);
				return null;
			}
		}

		private	ArrayList removeToken(ArrayList DocTokens,string token)
		{
			try
			{
				int n = DocTokens.Count;
				int start = DocTokens.IndexOf(token);
				if(start != -1)
				{
					for(int i = DocTokens.Count-1; i >= 0 ; i--)
					{
						if(DocTokens[i].Equals(token))
						{
							DocTokens.RemoveRange(start,(i+1)-start);
							break;
						}
					}
				}
				return DocTokens;
			}
			catch(Exception ee)
			{
				MessageBox.Show("removeToken: "+ee.Message);
				return null;
			}
		}
		#endregion
		#region Memory Part
		private void GarbageCollection()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
		private bool isMemoryEmpty()
		{
			stat.Init();
			NativeMethods.GlobalMemoryStatusEx(ref stat);
			if(stat.ullAvailPhys>55000000)
				return true;
			else
				return false;
		}
		#endregion
		#region Index Section
			#region Step1 : of index
		private void Step1(int runFileNum)
		{
			try
			{
				string path = indexDirPath+"\\step2.txt";
				long totalLength = 0;
				long destFileLength = 0;
				int threshold = 1000;
				if(!File.Exists(path))
				{
					ArrayList listOfRuns = new ArrayList();
					for(int i = 1 ; i<= runFileNum ; i++)
					{				
						string str = string.Concat(indexDirPath,"\\run",(i),".txt");
						totalLength += (new FileInfo(str)).Length;
						listOfRuns.Add((new run(str)));
					}
					
					using(StreamWriter sw = new StreamWriter(path ,false ,Encoding.Unicode))
					{
						while(!isEmpty(listOfRuns))
						{
							run smallerObj = getSmallestRun(listOfRuns);
							sw.WriteLine(smallerObj.currentLine);		
							smallerObj.nextLine();						
							removeNullObj(listOfRuns);
						
							if(numberOfLinesInFile++ > threshold)
							{
								threshold += 8000;
								destFileLength = (new FileInfo(path)).Length;
								//singleProc.Value = (int)(100*destFileLength/totalLength);
								BeginInvoke(APD,new object[] {(int)((100*destFileLength/totalLength)/4)+25,(int)(100*destFileLength/totalLength), "sorting tokens ...."});
							}
						}
					}
				}//if file exist
				GarbageCollection();
			}//end of try

			catch(Exception ee)
			{
				MessageBox.Show("Step1: "+ee.Message);
			}
		}
		private bool isEmpty(ArrayList AL)
		{
			if(AL.Count == 0)
				return true;
			else
				return false;
		}
		private run getSmallestRun(ArrayList runs)
		{
			try
			{				
				run small = (run)runs[0];
				for(int i=1 ; i<runs.Count ; i++)
				{
					if(tokenToBeProcessed(small.currentLine).CompareTo(tokenToBeProcessed(((run)runs[i]).currentLine))>0)
					{
						small = (run)runs[i];
					}
				}
				return small;
			}
			catch(ThreadAbortException te)
			{return null;}
			catch(Exception ee)
			{
				MessageBox.Show("error# 8"+ee.Message);
				return null;
			}
		}
		private void removeNullObj(ArrayList AL)
		{
			foreach(run r in AL)
			{
				if(r.currentLine == null)
				{
					AL.Remove(r);
					break;
				}
			}
		}
			#endregion
			#region Step2 : of index
		private void Step2(string sourcePath)
		{
			try
			{
				string destPath = indexDirPath+"\\step3.txt";
				if(!File.Exists(destPath))
				{
					StreamReader reader = new StreamReader(sourcePath);
					string firstLine = string.Empty;
					string nextLine  = string.Empty;
					int termFrequency = 1;
					long countOfLines = 0 ,countOfAddedFile = 0;
					firstLine = reader.ReadLine();
					using(StreamWriter sw = new StreamWriter(destPath ,false ,Encoding.Unicode))
					{
						while(firstLine != null)
						{
							countOfLines++;
							if(firstLine.Equals(nextLine = reader.ReadLine()))
							{
								termFrequency++;
							}
							else
							{
								sw.WriteLine(firstLine+","+termFrequency.ToString());				
								termFrequency = 1;
								firstLine = nextLine;
								countOfAddedFile++;
								BeginInvoke(APD,new object[] {(int)((100*countOfLines/numberOfLinesInFile)/4)+50,(int)(100*countOfLines/numberOfLinesInFile), "Extracting Term Frequency ...."});

							}							
						}
						numberOfLinesInFile = countOfAddedFile;
						reader.Close();
						File.Delete(sourcePath);
					}
				}// if file exist end
				GarbageCollection();
			}// try end
			catch(Exception ee)
			{
				MessageBox.Show("Step2: "+ee.Message);
			}
		}
			#endregion
			#region Step3 : of index
		private void Step3(string sourcePath)
		{
			try
			{
				string destPath = indexDirPath+"\\dictionary.txt";
				if(!File.Exists(destPath))
				{
					StreamReader reader = new StreamReader(sourcePath);
					string firstToken = string.Empty;
					string firstLine = string.Empty;
					string nextLine = string.Empty;
					string formatedLine = string.Empty;
					
					firstLine = reader.ReadLine();
					long collectionFrequency = getTermFrequency(firstLine), countOfLines = 0;
					string postingList = getDocID(firstLine)+","+ getTermFrequency(firstLine)+";";
					using(StreamWriter sw = new StreamWriter(destPath ,false ,Encoding.Unicode))
					{
						while(firstLine != null)
						{
							firstToken = tokenToBeProcessed(firstLine);
							countOfLines++;
							if(firstToken.Equals(tokenToBeProcessed(nextLine = reader.ReadLine())))
							{
								postingList += getDocID(nextLine)+","+ getTermFrequency(nextLine)+";";
								collectionFrequency += getTermFrequency(nextLine);
							}
							else
							{
								formatedLine = "\""+firstToken+"\":"+collectionFrequency.ToString()+";"+postingList;
								sw.WriteLine(formatedLine);
								BeginInvoke(APD,new object[] {(int)((100*countOfLines/numberOfLinesInFile)/4)+75,(int)(100*countOfLines/numberOfLinesInFile), "Extracting Corpus Frequency and Posting Lists ...."});
								if(nextLine != null)
								{
									postingList = getDocID(nextLine)+","+ getTermFrequency(nextLine)+";";
									collectionFrequency = getTermFrequency(nextLine);
								}
								firstLine = nextLine;								
							}							
						}
						reader.Close();
						File.Delete(sourcePath);
					}
				}// if file exist end
				GarbageCollection();
			}// try end
			catch(Exception ee)
			{
				MessageBox.Show("Step3: "+ee.Message);
			}
		}
		
		private long getTermFrequency(string line)
		{
			try
			{
				return long.Parse(line.Substring(line.LastIndexOf(',')+1,line.Length-(line.LastIndexOf(',')+1)));
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
				Application.Exit();
				return 0;
			}
		}

		private string getDocID(string line)
		{
			return line.Substring(line.IndexOf(',')+1,line.LastIndexOf(',')-(line.IndexOf(',')+1));
		}

		private string tokenToBeProcessed(string line)
		{
			if(line == null)
				return line;
			else
				return line.Substring(line.IndexOf('\"')+1,line.LastIndexOf('\"')-(line.IndexOf('\"')+1));
		}

				#endregion
		#endregion	
		#region control stuff
		public void BeginIndexing(IEnumerator Enum, int n, string dirPath)
		{
			try
			{
				indexDirPath = dirPath;
				fileIE = Enum;
				fileCount = n;
				runCount = 0;
				indexThread = new Thread(new ThreadStart(ThreadProcedure));
				indexThread.Start();
			}
			catch(Exception ee)
			{
				MessageBox.Show("error# 11"+ee.Message);
			}
		}
		private void ThreadProcedure()
		{
			try
			{
				Tokenization(fileIE, fileCount);
				createIndex();
				//IAsyncResult r1 = BeginInvoke(tokDelegate,new Object[] {});
				//IAsyncResult r2 = BeginInvoke(invertDelegate);
			}
			catch(ThreadAbortException te)
			{}
			catch(Exception ee)
			{
				MessageBox.Show("error# 12 "+ee.Message);
			}
		}
		private void AdvanceProgress(int totalValue,int sinValue,string processName)
		{
			Total.Value = totalValue;
			singleProc.Value = sinValue;
			sinLabel.Text = sinValue.ToString()+ "%       of "+ processName;
			TotalLabel.Text = totalValue.ToString()+"%       of Indexing Process";
		}
//		public void StopIndexing()
//		{
//			if (!indexing)
//			{
//				return;
//			}
//
//			if (indexThread.IsAlive)
//			{
//				indexThread.Abort();
//				indexThread.Join();
//			}
//
//			indexThread = null;
//			indexing = false;
//		}
		#endregion
		//public bool IsIndexFilesExist()
		//{
			//directorySearcher.SearchCriteria = folderName + "\\document.txt";

		//}
	}
	#region Helper Class

	public class helperForPassingPara
	{
		private IEnumerator localIE;
		private int c;
		private BuildingIndex BI = new BuildingIndex();
		public helperForPassingPara(IEnumerator IE, int count)
		{
			localIE = IE;
			c= count;
		}

		public void helper()
		{
			BI.Tokenization(localIE, c);
			BI.createIndex();
		}
	}

	#endregion
}
