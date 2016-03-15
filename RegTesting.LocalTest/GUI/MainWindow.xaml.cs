using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using RegTesting.LocalTest.Logic;
using CheckBox = System.Windows.Controls.CheckBox;
using Label = System.Windows.Controls.Label;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.Forms.MessageBox;

namespace RegTesting.LocalTest.GUI
{



	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private DispatcherTimer _timer;

		private readonly LocalTestLogic _localTestLogic;
		private BackgroundWorker _localTestBackgroundWorker;

		private readonly List<string> _testcases;
		private string currentScreenshotBase;


		/// <summary>
		/// Create and Initialize MainWindow
		/// </summary>
		public MainWindow()
		{

			InitializeComponent();
			_localTestLogic = new LocalTestLogic();
			lstBrowser.MouseLeftButtonUp += _SelectionChanged;
			lstBrowser.SelectionChanged += _SelectionChanged;
			languages.MouseLeftButtonUp += _SelectionChanged;
			languages.SelectionChanged += _SelectionChanged;

			string testsystem = _localTestLogic.GetAppSetting("Testsystem");
			if (string.IsNullOrEmpty(testsystem)) testsystem = "dev";
			txtTestsystem.Text = testsystem;

			string language = _localTestLogic.GetAppSetting("Language");
			if (string.IsNullOrEmpty(language)) language = "";
			

			string browser = _localTestLogic.GetAppSetting("Browser");
			if (string.IsNullOrEmpty(browser)) browser = "";
			
			languages.Items.Add(GetCheckBoxRow("DE"));
			languages.Items.Add(GetCheckBoxRow("EN"));
			languages.Items.Add(GetCheckBoxRow("ES"));
			languages.Items.Add(GetCheckBoxRow("FR"));
			languages.Items.Add(GetCheckBoxRow("IT"));
			languages.Items.Add(GetCheckBoxRow("NL"));
			SelectItems(languages,language.Split('|'), true);

			_testcases = new List<string>();
			string testcaseFile = _localTestLogic.GetAppSetting("TestcaseFile");
			if (!String.IsNullOrEmpty(testcaseFile))
				LoadTestcaseFile(testcaseFile);
			
			string filterTestcases = _localTestLogic.GetAppSetting("TestcaseFilter");
			txtFilter.Text = filterTestcases;

			AddLocalBrowserCapabilities("firefox");
			AddLocalBrowserCapabilities("chrome");
			AddLocalBrowserCapabilities("internet explorer");
			AddLocalBrowserCapabilities("phantomjs");
			SelectItems(lstBrowser,browser.Split('|'), true);

			objRunningTestGrid.Visibility = Visibility.Collapsed;
			objTestGrid.Visibility = Visibility.Visible;		
		}


		private void AddLocalBrowserCapabilities(string description)
		{
			CheckBox checkBox = GetCheckBoxRow(description);
			checkBox.Click += browserCheckBoxRow_Click;
			lstBrowser.Items.Add(checkBox);
		}
		void _SelectionChanged(object sender, EventArgs e)
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

		private void SelectItems(ListBox listBox, IEnumerable<string> itemsToSelect, bool findCheckbox)
		{
			foreach (string itemToSelect in itemsToSelect)
			{

				if (findCheckbox)
				{
					foreach (object item in listBox.Items)
					{
						CheckBox checkBox = item as CheckBox;
						if (checkBox != null && checkBox.Tag.ToString().Equals(itemToSelect))
						{
							listBox.SelectedItems.Add(item);
						}
					}
				}
				else
				{
					listBox.SelectedItems.Add(itemToSelect);
				}

			}
		}

		// Completed Method
		void LocalTestBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			_timer.Stop();
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

		private string GetLog()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append(_localTestLogic.TestHeader + "\n\n");
			foreach (string logEntry in _localTestLogic.LogEntries.ToList())
			{
				stringBuilder.Append(logEntry + "\n");

			}
			return stringBuilder.Replace("<br>","\n").ToString();
		}

		void browserCheckBoxRow_Click(object sender, RoutedEventArgs e)
		{
			SetSelectedItems(lstBrowser);
		}

		void languageCheckBoxRow_Click(object sender, RoutedEventArgs e)
		{
			SetSelectedItems(languages);
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


		private void BtnStartClick(object sender, RoutedEventArgs e)
		{
			if (lstViewTestcases.SelectedItems.Count == 0 || languages.SelectedItems.Count == 0 ||
				lstBrowser.SelectedItems.Count == 0)
			{
				ShowErrorMessage("You must select at least one test, one language and one browser. At least one of them is missing.", "Missing arguments!");
				return;
			}
				
			SetSelectedItems(languages);
			SetSelectedItems(lstBrowser);

			List<string> selectedBrowsersLocal = lstBrowser.SelectedItems.OfType<CheckBox>().Select(checkBox => checkBox.Tag.ToString()).ToList();
			List<string> selectedLanguages = languages.SelectedItems.Cast<CheckBox>().Where(checkBox => checkBox.IsChecked.HasValue && checkBox.IsChecked.Value).Select(checkBox => checkBox.Tag.ToString()).ToList();
			List<string> selectedTestcases = lstViewTestcases.SelectedItems.Cast<string>().ToList();

			_localTestLogic.SetAppSetting("TestcaseFilter", txtFilter.Text);
			_localTestLogic.SetAppSetting("TestcaseFile", txtFile.Text);
			_localTestLogic.SetAppSetting("Testsystem", txtTestsystem.Text);
			_localTestLogic.SetAppSetting("Language", string.Join("|", selectedLanguages));
			_localTestLogic.SetAppSetting("Browser", string.Join("|", lstBrowser.SelectedItems.Cast<CheckBox>().Where(checkBox => checkBox.IsChecked.HasValue && checkBox.IsChecked.Value).Select(checkBox => checkBox.Tag.ToString())));
			_localTestLogic.SetAppSetting("Testcase", string.Join("|", lstViewTestcases.SelectedItems.Cast<string>()));

			string testsystem = txtTestsystem.Text;

			LoadTestcaseFile(txtFile.Text);

			if(selectedBrowsersLocal.Any())
				TestLocal(testsystem,selectedBrowsersLocal,selectedTestcases,selectedLanguages);
		}
		

		private void TestLocal(string testsystem, List<string> lstBrowsers, List<string> testcases, List<string> languages)
		{

			objTestGrid.Visibility = Visibility.Collapsed;
			objRunningTestGrid.Visibility = Visibility.Visible;
			txtTestStatus.Text = "Starting local tests ...";
			StartTimer();

			try
			{
				_localTestBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
				_localTestBackgroundWorker.DoWork +=
					(objS, objDoWorkEventArgs) =>
					_localTestLogic.TestLocal(testsystem, lstBrowsers, testcases, languages);
				_localTestBackgroundWorker.RunWorkerCompleted += LocalTestBackgroundWorkerRunWorkerCompleted;
				_localTestBackgroundWorker.RunWorkerAsync();
			}
			catch (Exception objException)
			{
				txtTestStatus.Text = "Uncatched error:  " + objException;
			}
		}



		private void StartTimer()
		{
			// Create a Timer with a Normal Priority
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);

			// Set the callback to just show the time ticking away
			// NOTE: We are using a control so this has to run on 
			// the UI thread
			_timer.Tick += new EventHandler(delegate(object s, EventArgs a)
				{
					txtTestStatus.Text = GetLog();
					txtTestStatus.ScrollToEnd();
					var screen = _localTestLogic.CurrentScreenshot;

					if (string.IsNullOrEmpty(screen))
						return;

					var binaryData = Convert.FromBase64String(screen);
	
					BitmapImage bi = new BitmapImage();
					bi.BeginInit();
					bi.StreamSource = new MemoryStream(binaryData);
					bi.EndInit();
					currentScreenshot.Source = bi;
					currentScreenshotBase = screen;

				});

			// Start the timer
			_timer.Start();
		}

		private void BtnBackClick(object sender, RoutedEventArgs e)
		{
			_localTestLogic.CancelTests();
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
			_testcases.Clear();
			try
			{
				_localTestLogic.LoadTestFile(filename);
				txtFile.Text = filename;
				_testcases.AddRange(_localTestLogic.GetTestcases());
				string strTestcase = _localTestLogic.GetAppSetting("Testcase");
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



		private void TxtFilter_OnTextChanged(object sender, TextChangedEventArgs objE)
		{
			UpdateTextcaseSearch();
		}


		private void UpdateTextcaseSearch()
		{
			lstViewTestcases.ItemsSource = _testcases.Where(objTestcase => objTestcase.ToLower().Contains(txtFilter.Text.ToLower()));
		}

		private void ShowErrorMessage(string message, string heading)
		{
			MessageBox.Show(message,
					heading,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
		}

		private void BtnStartTestsuiteClick(object sender, RoutedEventArgs e)
		{
			string testsystem = txtTestsystem.Text;
			string fileName = txtFile.Text;

			using (WcfClient wcfClient = new WcfClient())
			{
				wcfClient.TestRemote(fileName, testsystem);
			}
		}

		private void BtnSaveImage(object sender, RoutedEventArgs e)
		{
			var bytes = Convert.FromBase64String(currentScreenshotBase);
			using (var imageFile = new FileStream(@"C:\RegTesting-" + DateTime.Now.Ticks + ".png", FileMode.Create))
			{
				imageFile.Write(bytes, 0, bytes.Length);
				imageFile.Flush();
			}
		}
	}

}
