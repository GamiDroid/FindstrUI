using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using FindstrUI.Core;
using FindstrUI.WPF.Util;

namespace FindstrUI.WPF.ViewModels
{
    class MainWindowViewModel : NotifyPropertyChanged
    {
        #region Constructor
        public MainWindowViewModel()
        {
            CommandResult = new ObservableCollection<string>();
        } 
        #endregion

        #region Properties
        public ObservableCollection<string> CommandResult
        {
            get => _commandResult;
            set
            {
                _commandResult = value;
                OnPropertyChanged();
            }
        }
        public string FolderSelection
        {
            get => _folderSelection;
            set
            {
                if (_folderSelection == value)
                    return; // Value is not changed.
                _folderSelection = value;
                OnPropertyChanged();
            }
        }
        public int? Hits
        {
            get => _hits;
            set
            {
                if (_hits == value)
                    return; // Value is not changed.
                _hits = value;
                OnPropertyChanged();
            }
        }
        public string SearchString { get; set; } 
        #endregion

        public void ExecuteCommand()
        {
            if (string.IsNullOrEmpty(FolderSelection))
            {
                MessageBox.Show("There must be a folder selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommandResult = new ObservableCollection<string>();
                UpdateHits(false);
                return;
            }

            if (string.IsNullOrEmpty(SearchString))
            {
                MessageBox.Show("The search string cannot be empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommandResult = new ObservableCollection<string>();
                UpdateHits(false);
                return;
            }

            CommandResult = new ObservableCollection<string> { "Wait for result..." };

            var worker = new BackgroundWorker();
            worker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                var strResult = TerminalCommand.GetResult((string)e.Argument);
                var lines = strResult.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                e.Result = lines.ToList();
            };

            worker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                CommandResult = new ObservableCollection<string>(e.Result as List<string>);
                UpdateHits(true);
            };

            string path = FolderSelection + "\\*";

            worker.RunWorkerAsync(@$"findstr /i /n ""{SearchString}"" ""{path}""");
        }

        public void Browse()
        {
            var dlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FolderSelection = dlg.SelectedPath;
            }
        }

        private void UpdateHits(bool hasResult)
        {
            Hits = hasResult ? CommandResult?.Count() : null;
        }

        #region Private fields
        private ObservableCollection<string> _commandResult;
        private string _folderSelection;
        private int? _hits; 
        #endregion
    }
}
