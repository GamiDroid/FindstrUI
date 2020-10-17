using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FindstrUI.Core;
using FindstrUI.WPF.Util;

namespace FindstrUI.WPF.ViewModels
{
    class MainWindowViewModel : NotifyPropertyChanged
    {
        public string CommandResult 
        { 
            get => _commandResult;
            set
            {
                if (_commandResult == value)
                    return;

                _commandResult = value;
                OnPropertyChanged();
            }
        }
        public string SearchString { get; set; }

        public void ExecuteCommand()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                CommandResult = "The search string cannot be empty.";
                return;
            }

            CommandResult = "Wait for result...";

            var worker = new BackgroundWorker();
            worker.DoWork += (object sender, DoWorkEventArgs e) => { e.Result = TerminalCommand.GetResult((string)e.Argument); };
            worker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => { CommandResult = (string)e.Result; };
            worker.RunWorkerAsync(@$"findstr /p /i /n ""{SearchString}"" ""E:\Share\sm_reports\*""");
        }

        private string _commandResult;
    }
}
