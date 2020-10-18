using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FindstrUI.Core;
using FindstrUI.WPF.Util;

namespace FindstrUI.WPF.ViewModels
{
    class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            CommandResult = new ObservableCollection<string>();
        }

        public ObservableCollection<string> CommandResult
        {
            get => _commandResult;
            set
            {
                _commandResult = value;
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

        public void ExecuteCommand()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                CommandResult = new ObservableCollection<string> { "The search string cannot be empty." };
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

            worker.RunWorkerAsync(@$"findstr /p /i /n ""{SearchString}"" ""E:\Share\sm_reports\*""");
        }

        private void UpdateHits(bool hasResult)
        {
            Hits = hasResult ? CommandResult?.Count() : null;
        }

        private ObservableCollection<string> _commandResult;
        private int? _hits;
    }
}
