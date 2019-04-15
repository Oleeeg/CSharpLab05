using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace CSharpLab05
{
    class TaskManagerViewModel : INotifyPropertyChanged
    {
        private ICommand _killProcess;
        private ICommand _openProcess;

        private ObservableCollection<ProcessClass> _allProcesses;
        private ProcessClass _selectedProcess;
        private Thread _moduleThread;
        private Thread _dataThread;

        internal TaskManagerViewModel()
        {
            Processes = new ObservableCollection<ProcessClass>();
            var processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++) _allProcesses.Add(new ProcessClass(processes[i]));
            _moduleThread = new Thread(UpdateModules);
            _moduleThread.Start();
            _dataThread = new Thread(UpdateTaskManager);
            _dataThread.Start();
        }

        public ICommand KillProcessCommand => _killProcess ??
                                                    (_killProcess = new RelayCommand<object>(KillProcess,
                                                        CanExecuteCommand));

        public ICommand OpenFolderCommand => _openProcess ??
                                             (_openProcess = new RelayCommand<object>(OpenFolder,
                                                 CanExecuteCommand));

        private void OpenFolder(object obj)
        {
            if (_selectedProcess.Path != "Access is forbidden.")
            {
                try
                {
                    Process.Start(Path.GetDirectoryName(_selectedProcess.Path));
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (PathTooLongException ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }
            else
                MessageBox.Show("Access is forbidden.");
        }

        private void KillProcess(object obj)
        {

            Process pr = Process.GetProcessById(_selectedProcess.Id);
            try
            {
                pr.Kill();
            }
            catch (System.ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public ObservableCollection<ProcessClass> Processes
        {
            get => _allProcesses;
            set
            {
                _allProcesses = value;
                OnPropertyChanged();
            }

        }

        public ProcessClass SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        private void ShowModulesImplementation(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.ShowInfo, SelectedProcess);
        }

        private void UpdateTaskManager()
        {
            while (StationManager.Stop)
            {
                Thread.Sleep(5000);
            }
            _dataThread.Join(2000);
            _dataThread.Abort();
            _dataThread = null;
        }

        private void UpdateModules()
        {
            while (StationManager.Stop)
            {
                foreach (ProcessClass pr in Processes)
                    pr.UpdateFields();
                Processes=new ObservableCollection<ProcessClass>(Processes);
                Thread.Sleep(2000);
            }
            _moduleThread.Join(2000);
            _moduleThread.Abort();
            _moduleThread = null;
        }

        private bool CanExecuteCommand(object obj)
        {
            return SelectedProcess != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
