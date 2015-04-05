using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Threading;
using RegTesting.LocalTest.Logic;
using CheckBox = System.Windows.Controls.CheckBox;
using Label = System.Windows.Controls.Label;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;

namespace RegTesting.LocalTest.GUI
{



	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

        private bool remoteAvailable = false;
		private DispatcherTimer _objTimer;

		private readonly LocalTestLogic _objLocalTestLogic;
		private BackgroundWorker _objLocalTestBackgroundWorker = null;
		private BackgroundWorker _objGetRemoteCapabilityBackgroundWorker = null;

		private readonly List<string> _lstTestcases;
		List<string> _lstBrowsers = new List<string>();
		private List<string> _lstLanguages = new List<string>();

		private const string StrLocalPrefix = "LOCAL: ";
		private const string StrRemotePrefix = "REMOTE: ";

		 


		/// <summary>
		/// Create and Initialize MainWindow
		/// </summary>
		public MainWindow()
		{

			InitializeComponent();
			_objLocalTestLogic = new LocalTestLogic();
			lstBrowser.MouseLeftButtonUp += lst_SelectionChanged;
			lstBrowser.SelectionChanged += lst_SelectionChanged;
			lstLanguage.MouseLeftButtonUp += lst_SelectionChanged;
			lstLanguage.SelectionChanged += lst_SelectionChanged;

			string strTestsystem = _objLocalTestLogic.GetAppSetting("Testsystem");
			if (String.IsNullOrEmpty(strTestsystem)) strTestsystem = "dev";
			txtTestsystem.Text = strTestsystem;

			string strLanguage = _objLocalTestLogic.GetAppSetting("Language");
			if (String.IsNullOrEmpty(strLanguage)) strLanguage = "";
			

			string strBrowser = _objLocalTestLogic.GetAppSetting("Browser");
			if (String.IsNullOrEmpty(strBrowser)) strBrowser = "";
			
			lstLanguage.Items.Add(GetCheckBoxRow("DE"));
			SelectItems(lstLanguage,strLanguage.Split('|'), true);

            _lstTestcases = new List<string>();
            string testcaseFile = _objLocalTestLogic.GetAppSetting("TestcaseFile");
            if (!String.IsNullOrEmpty(testcaseFile))
                LoadTestcaseFile(testcaseFile);
            
			string filterTestcases = _objLocalTestLogic.GetAppSetting("TestcaseFilter");
			txtFilter.Text = filterTestcases;


			AddLocalBrowserCapabilities(StrLocalPrefix + "firefox");
			AddLocalBrowserCapabilities(StrLocalPrefix + "chrome");
			AddLocalBrowserCapabilities(StrLocalPrefix + "internet explorer");

			SelectItems(lstBrowser,strBrowser.Split('|'), true);
			AddRemoteTestingVariants();
		}


		private void AddLocalBrowserCapabilities(string description)
		{
			CheckBox checkBox = GetCheckBoxRow(description);
			checkBox.Click += browserCheckBoxRow_Click;
			lstBrowser.Items.Add(checkBox);
		}
		void lst_SelectionChanged(object sender, EventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null)
			{

				IEnumerable<CheckBox> selectedCheckBoxes = listBox.SelectedItems.Cast<CheckBox>().ToArray();
				IEnumerable<CheckBox> checkBoxes = listBox.Items.Cast<CheckBox>();
				foreach (CheckBox selectedCheckBox in selectedCheckBoxes)
				{
					selectedCheckBox.IsChecked = true;
				}
				IEnumerable<CheckBox> unselectedCheckBoxs = checkBoxes.Except(selectedCheckBoxes);
				foreach (CheckBox unselectedCheckBox in unselectedCheckBoxs)
				{
					unselectedCheckBox.IsChecked = false;
				}
			}
		}
		
		private CheckBox GetCheckBoxRow(string description)
		{
			CheckBox result = new CheckBox { Tag = description };

			Label label = new Label {Content = description};
			result.Content = label;

			return result;
		}

		private void SelectItems(ListBox objListBox, IEnumerable<string> arrToSelect, bool findCheckbox)
		{
			foreach (string strToSelect in arrToSelect)
			{

				if (findCheckbox)
				{
					foreach (object item in objListBox.Items)
					{
						CheckBox checkBox = item as CheckBox;
						if (checkBox != null && checkBox.Tag.ToString().Equals(strToSelect))
						{
							objListBox.SelectedItems.Add(item);
						}
					}
				}
				else
				{
					objListBox.SelectedItems.Add(strToSelect);
				}

			}
		}

		// Completed Method
		void LocalTestBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			_objTimer.Stop();
			if (e.Cancelled)
			{
				txtTestStatus.Text = GetLog() + "\nCanceled.";
				
			}
			else if (e.Error != null)
			{
				txtTestStatus.Text =  GetLog() + "\nError: " + e.Error;
			}
			else
			{
				txtTestStatus.Text =GetLog() +  "\nSuccess." ;
			}
			txtTestStatus.ScrollToEnd();
		}

		// Completed Method
		void __objGetRemoteCapabilityBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

			if (e.Cancelled)
			{
			}
			else if(!remoteAvailable)
			{
				Title = "RegTesting LocalTest (Local)";

			}
			else
			{
				Title = "RegTesting LocalTest (Local+Remote)";

				string strLanguage = _objLocalTestLogic.GetAppSetting("Language");
				if (String.IsNullOrEmpty(strLanguage)) strLanguage = "";


				string strBrowser = _objLocalTestLogic.GetAppSetting("Browser");
				if (String.IsNullOrEmpty(strBrowser)) strBrowser = "";
				SelectItems(lstBrowser, strBrowser.Split('|'), true);
				SelectItems(lstLanguage, strLanguage.Split('|'), true);
			}
		}


		private string GetLog()
		{
			StringBuilder objStringBuilder = new StringBuilder();

			objStringBuilder.Append(_objLocalTestLogic.TestHeader + "\n\n");
			foreach (string strLogEntry in _objLocalTestLogic.LogEntries.ToList())
			{
				objStringBuilder.Append(strLogEntry + "\n");

			}
			return objStringBuilder.Replace("<br>","\n").ToString();
		}

		private void AddRemoteTestingVariants()
		{
			try
			{
				_objGetRemoteCapabilityBackgroundWorker = new BackgroundWorker();
				_objGetRemoteCapabilityBackgroundWorker.DoWork +=
					(objS, objDoWorkEventArgs) =>
						GetRemoteCapability();
				_objGetRemoteCapabilityBackgroundWorker.RunWorkerCompleted += __objGetRemoteCapabilityBackgroundWorker_RunWorkerCompleted;
				_objGetRemoteCapabilityBackgroundWorker.RunWorkerAsync();
			}
			catch (Exception objException)
			{
				txtTestStatus.Text = "Uncatched error:  " + objException;
			}
		}

		private void GetRemoteCapability()
		{

			List<string> lstRemoteLanguages;
			List<string> lstRemoteBrowsers;


            try {
                using (WcfClient objClient = new WcfClient())
			    {
				    lstRemoteLanguages = objClient.GetLanguages();
				    lstRemoteBrowsers = objClient.GetBrowsers();

			    }
			    Dispatcher.Invoke((() =>
			    {
				    lstLanguage.Items.Clear();
				    foreach (string strRemoteLanguage in lstRemoteLanguages)
				    {
					    CheckBox checkBoxRow = GetCheckBoxRow(strRemoteLanguage);
					    checkBoxRow.Click += languageCheckBoxRow_Click;
					    lstLanguage.Items.Add(checkBoxRow);
				    }
				    foreach (string strRemoteBrowser in lstRemoteBrowsers)
				    {
					    CheckBox checkBoxRow = GetCheckBoxRow(StrRemotePrefix + strRemoteBrowser);
					    checkBoxRow.Click += browserCheckBoxRow_Click;

					    lstBrowser.Items.Add(checkBoxRow);
				    }
				
				    lstBrowser.Items.Refresh();
				    lstLanguage.Items.Refresh();
                    remoteAvailable = true;

			    }));
            } catch {
                remoteAvailable = false;
            }
	


		}

		void browserCheckBoxRow_Click(object sender, RoutedEventArgs e)
		{
			SetSelectedItems(lstBrowser);
		}

		void languageCheckBoxRow_Click(object sender, RoutedEventArgs e)
		{
			SetSelectedItems(lstLanguage);
		}

		private void SetSelectedItems(ListBox listBox)
		{
			foreach (object item in listBox.Items)
			{
				CheckBox checkBox = (CheckBox)item;
				if (checkBox.IsChecked.HasValue && checkBox.IsChecked.Value && listBox.SelectedItems.IndexOf(item) < 0)
					listBox.SelectedItems.Add(item);
				else if(!(checkBox.IsChecked.HasValue && checkBox.IsChecked.Value) && listBox.SelectedItems.IndexOf(item) > -1)
					listBox.SelectedItems.Remove(item);
			}
		}


		private void BtnStartClick(object objSender, RoutedEventArgs e)
		{
			if (lstViewTestcases.SelectedItems.Count == 0 || lstLanguage.SelectedItems.Count == 0 ||
			    lstBrowser.SelectedItems.Count == 0)
			{
				ShowErrorMessage("You must select at least one test, one language and one browser. At least one of them is missing.", "Missing arguments!");
				return;
			}
				
			SetSelectedItems(lstLanguage);
			SetSelectedItems(lstBrowser);

			List<string> lstSelectedBrowsersLocal = new List<string>();
			List<string> lstSelectedBrowsersRemote = new List<string>();

			foreach (object selectedObject in lstBrowser.SelectedItems)
			{
				CheckBox checkBox = selectedObject as CheckBox;
				if (checkBox != null)
				{
					string strSelectedBrowser = checkBox.Tag.ToString();
					if(strSelectedBrowser.StartsWith(StrLocalPrefix))
						lstSelectedBrowsersLocal.Add(strSelectedBrowser.Substring(StrLocalPrefix.Length));
					if (strSelectedBrowser.StartsWith(StrRemotePrefix))
						lstSelectedBrowsersRemote.Add(strSelectedBrowser.Substring(StrRemotePrefix.Length));
				}
			}

			List<string> lstSelectedLanguages = lstLanguage.SelectedItems.Cast<CheckBox>().Where(checkBox => checkBox.IsChecked.HasValue && checkBox.IsChecked.Value).Select(checkBox => checkBox.Tag.ToString()).ToList();
			List<string> lstSelectedTestcases = lstViewTestcases.SelectedItems.Cast<string>().ToList();

			_objLocalTestLogic.SetAppSetting("TestcaseFilter", txtFilter.Text);
            _objLocalTestLogic.SetAppSetting("TestcaseFile", txtFile.Text);
			_objLocalTestLogic.SetAppSetting("Testsystem", txtTestsystem.Text);
			_objLocalTestLogic.SetAppSetting("Language", string.Join("|", lstSelectedLanguages));
			_objLocalTestLogic.SetAppSetting("Browser", string.Join("|", lstBrowser.SelectedItems.Cast<CheckBox>().Where(checkBox => checkBox.IsChecked.HasValue && checkBox.IsChecked.Value).Select(checkBox => checkBox.Tag.ToString())));
			_objLocalTestLogic.SetAppSetting("Testcase", string.Join("|", lstViewTestcases.SelectedItems.Cast<string>()));

			string strTestsystem = txtTestsystem.Text;


			if(lstSelectedBrowsersLocal.Any())
				TestLocal(strTestsystem,lstSelectedBrowsersLocal,lstSelectedTestcases,lstSelectedLanguages);

			if (lstSelectedBrowsersRemote.Any())
				TestRemote(strTestsystem, lstSelectedBrowsersRemote, lstSelectedTestcases, lstSelectedLanguages);


		}

		private void TestRemote(string strTestsystem, List<string> lstBrowsers, List<string> lstTestcases, List<string> lstLanguages)
		{
			using (WcfClient objClient = new WcfClient())
			{
				objClient.TestRemote(strTestsystem, lstBrowsers, lstTestcases, lstLanguages);
			}
		}

		private void TestLocal(string strTestsystem, List<string> lstBrowsers, List<string> lstTestcases, List<string> lstLanguages)
		{

			objTestGrid.Visibility = Visibility.Collapsed;
			objRunningTestGrid.Visibility = Visibility.Visible;
			txtTestStatus.Text = "Starting local tests ...";
			StartTimer();

			try
			{
				_objLocalTestBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
				_objLocalTestBackgroundWorker.DoWork +=
					(objS, objDoWorkEventArgs) =>
					_objLocalTestLogic.TestLocal(strTestsystem, lstBrowsers, lstTestcases, lstLanguages);
				_objLocalTestBackgroundWorker.RunWorkerCompleted += LocalTestBackgroundWorkerRunWorkerCompleted;
				_objLocalTestBackgroundWorker.RunWorkerAsync();
			}
			catch (Exception objException)
			{
				txtTestStatus.Text = "Uncatched error:  " + objException;
			}
		}



		private void StartTimer()
		{
			// Create a Timer with a Normal Priority
			_objTimer = new DispatcherTimer();
			_objTimer.Interval = TimeSpan.FromMilliseconds(500);

			// Set the callback to just show the time ticking away
			// NOTE: We are using a control so this has to run on 
			// the UI thread
			_objTimer.Tick += new EventHandler(delegate(object s, EventArgs a)
				{
					txtTestStatus.Text = GetLog();
					txtTestStatus.ScrollToEnd();
				});

			// Start the timer
			_objTimer.Start();
		}

		private void BtnBackClick(object objSender, RoutedEventArgs e)
		{
			_objLocalTestLogic.CancelTests();
			objRunningTestGrid.Visibility = Visibility.Collapsed;
			objTestGrid.Visibility = Visibility.Visible;
		}

        private void BtnPickFileClick(object objSender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Test Libraries (*.dll)|*.dll";
            openFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(openFileDialog.FileName))
                LoadTestcaseFile(openFileDialog.FileName);            
        }



        private void LoadTestcaseFile(String filename)
        {
            _lstTestcases.Clear();
            try
            {
                _objLocalTestLogic.LoadTestFile(filename);
                txtFile.Text = filename;
                _lstTestcases.AddRange(_objLocalTestLogic.GetTestcases());
                string strTestcase = _objLocalTestLogic.GetAppSetting("Testcase");
                if (String.IsNullOrEmpty(strTestcase)) strTestcase = "";
                UpdateTextcaseSearch();
                SelectItems(lstViewTestcases, strTestcase.Split('|'), false);
            }
            catch (FileNotFoundException)
            {
                //File not found... Don't show an error. Should only occur when initially starting app and
                //we try to reload the last loaded file... Instead of displaying an error, just don't load anything.
            }
            catch (TypeLoadException)
            {
                //File was found, but the file doesn't contain the typesloader we need to search for testclasses
                UpdateTextcaseSearch();
            }
        }



		private void TxtFilter_OnTextChanged(object objSender, TextChangedEventArgs objE)
		{
            UpdateTextcaseSearch();
		}


        private void UpdateTextcaseSearch()
        {
            lstViewTestcases.ItemsSource = _lstTestcases.Where(objTestcase => objTestcase.ToLower().Contains(txtFilter.Text.ToLower()));
        }

		private void ShowErrorMessage(string message, string heading)
		{
			MessageBox.Show(message,
					heading,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
		}
	}

}
