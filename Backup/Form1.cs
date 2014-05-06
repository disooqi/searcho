using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Samples.DirectorySearcher;
using System.Threading;
using System.IO;

namespace SearchO
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private DirectorySearcher directorySearcher;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.StatusBarPanel statusBarPanel3;
		private System.Windows.Forms.StatusBarPanel statusBarPanel4;
		private BuildingIndex myControl;
		private System.Windows.Forms.Button button7;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button7 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.directorySearcher = new Microsoft.Samples.DirectorySearcher.DirectorySearcher();
			this.myControl = new SearchO.BuildingIndex();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button6 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel3 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel4 = new System.Windows.Forms.StatusBarPanel();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel4)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button7);
			this.groupBox1.Controls.Add(this.button5);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Location = new System.Drawing.Point(48, 168);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 376);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Documents";
			// 
			// button7
			// 
			this.button7.Enabled = false;
			this.button7.Location = new System.Drawing.Point(16, 320);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(144, 48);
			this.button7.TabIndex = 4;
			this.button7.Text = "Cancel";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// button5
			// 
			this.button5.Enabled = false;
			this.button5.Location = new System.Drawing.Point(16, 104);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(144, 48);
			this.button5.TabIndex = 3;
			this.button5.Text = "Add More";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button3
			// 
			this.button3.Enabled = false;
			this.button3.Location = new System.Drawing.Point(16, 264);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(144, 48);
			this.button3.TabIndex = 2;
			this.button3.Text = "Remove Checks";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(16, 208);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 48);
			this.button2.TabIndex = 1;
			this.button2.Text = "Check All";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 40);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(144, 48);
			this.button1.TabIndex = 0;
			this.button1.Text = "Determine Document Directory";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// directorySearcher
			// 
			this.directorySearcher.Location = new System.Drawing.Point(320, 184);
			this.directorySearcher.Name = "directorySearcher";
			this.directorySearcher.SearchCriteria = null;
			this.directorySearcher.Size = new System.Drawing.Size(808, 368);
			this.directorySearcher.TabIndex = 6;
			this.directorySearcher.SearchComplete += new System.EventHandler(this.directorySearcher_SearchComplete);
			// 
			// myControl
			// 
			this.myControl.Location = new System.Drawing.Point(320, 552);
			this.myControl.Name = "myControl";
			this.myControl.Size = new System.Drawing.Size(808, 152);
			this.myControl.TabIndex = 8;
			this.myControl.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button6);
			this.groupBox2.Controls.Add(this.button4);
			this.groupBox2.Location = new System.Drawing.Point(48, 552);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(176, 152);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Indexing";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(16, 88);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(144, 48);
			this.button6.TabIndex = 1;
			this.button6.Text = "Cancel";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button4
			// 
			this.button4.Enabled = false;
			this.button4.Location = new System.Drawing.Point(16, 24);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(144, 48);
			this.button4.TabIndex = 0;
			this.button4.Text = "Index";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(320, 168);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(480, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Documents:";
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 756);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1,
																						  this.statusBarPanel2,
																						  this.statusBarPanel3,
																						  this.statusBarPanel4});
			this.statusBar1.Size = new System.Drawing.Size(1200, 22);
			this.statusBar1.TabIndex = 7;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			// 
			// statusBarPanel3
			// 
			this.statusBarPanel3.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			// 
			// statusBarPanel4
			// 
			this.statusBarPanel4.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1200, 778);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.directorySearcher);
			this.Controls.Add(this.myControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "SearchO Indexer";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel4)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(new Form1());
			//Application.EnableVisualStyles();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			//CreateMyStatusBar();
            statusBarPanel1.Text = "Ready";
			statusBar1.ShowPanels = true;
			
		}
		private void directorySearcher_SearchComplete(object sender, System.EventArgs e)
		{
			this.button1.Enabled = true;
			//label1.Text = directorySearcher.Count.ToString()+" Document(s)";
			if(directorySearcher.Count == 0)
			{
				button2.Enabled = false;
				button3.Enabled = false;
				button4.Enabled = false;
				button5.Enabled = false;			
			}
			else				
			{
				button2.Enabled = true;
				button3.Enabled = true;
				button4.Enabled = true;
				button5.Enabled = true;				
			}
			button7.Enabled = false;
			statusBarPanel1.Text = "Search Complete";	
			statusBarPanel2.Text = directorySearcher.Count.ToString()+" Document(s)";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			
			string folderName;
			DialogResult result = folderBrowserDialog1.ShowDialog();
			folderName = folderBrowserDialog1.SelectedPath;
			directorySearcher.NewSearch = true;
			directorySearcher.SearchCriteria = folderName + "\\*.txt";
	
		    if(result == DialogResult.OK) 
	        {
				directorySearcher.BeginSearch();
				button2.Enabled = false;
				button3.Enabled = false;
				button4.Enabled = false;
				button5.Enabled = false;
				button7.Enabled = true;
				statusBarPanel1.Text = "Searching...";
            }			
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			for(int i = 0; i < directorySearcher.Count ; i++)
			{
				directorySearcher.SetItemChecked(i,true);
			}
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			string folderName;
			DialogResult result = folderBrowserDialog1.ShowDialog();
			folderName = folderBrowserDialog1.SelectedPath;
			directorySearcher.NewSearch = false;
			directorySearcher.SearchCriteria = folderName + "\\*.txt";			

			if(result == DialogResult.OK) 
			{
				directorySearcher.BeginSearch();
				button2.Enabled = false;
				button3.Enabled = false;
				button4.Enabled = false;
				button5.Enabled = false;
				button7.Enabled = true;
			}			
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			IndexSetup setup = new IndexSetup();
			if(setup.verfiyIndex())
			{
				IEnumerator IE = directorySearcher.CheckedItems.GetEnumerator();
				if(setup.verfiyStorageSpace(IE))
				{				
					myControl.BeginIndexing(IE, directorySearcher.Count, setup.IndexDirPath);
					(new EventLogFile()).writeEntry("created@"+setup.IndexDirPath);
				}
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			for(int i = 0; i < directorySearcher.Count ; i++)
			{
				directorySearcher.SetItemChecked(i,false);
			}
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			try
			{
				//myControl.StopIndexing();
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}
		private void CreateMyStatusBar()
		{
			// Create a StatusBar control.
			//StatusBar statusBar1 = new StatusBar();
			// Create two StatusBarPanel objects to display in the StatusBar.
			StatusBarPanel panel1 = new StatusBarPanel();
			StatusBarPanel panel2 = new StatusBarPanel();

			// Display the first panel with a sunken border style.
			panel1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			// Initialize the text of the panel.
			panel1.Text = "Ready...";
			// Set the AutoSize property to use all remaining space on the StatusBar.
			panel1.AutoSize = StatusBarPanelAutoSize.Spring;
    
			// Display the second panel with a raised border style.
			panel2.BorderStyle = StatusBarPanelBorderStyle.Sunken;
    
			// Create ToolTip text that displays time the application was 
			//started.
			panel2.ToolTipText = "Started: " + System.DateTime.Now.ToShortTimeString();
			// Set the text of the panel to the current date.
			panel2.Text = System.DateTime.Today.ToLongDateString();
			// Set the AutoSize property to size the panel to the size of the contents.
			panel2.AutoSize = StatusBarPanelAutoSize.Contents;
                
			// Display panels in the StatusBar control.
			statusBar1.ShowPanels = true;

			// Add both panels to the StatusBarPanelCollection of the StatusBar.            
			statusBar1.Panels.Add(panel1);
			statusBar1.Panels.Add(panel2);

			// Add the StatusBar to the form.
			this.Controls.Add(statusBar1);
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			try
			{
				directorySearcher.StopSearch();
				button7.Enabled = false;
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//if indxing running
			try
			{
				//myControl.StopIndexing();
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
			//if searching running
			try
			{
				directorySearcher.StopSearch();
				button7.Enabled = false;
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		
		}

	}
}
