using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace DansingProgressBars
{
    /// <summary>
    /// Interaction logic for ProgressBarDialog.xaml
    /// </summary>

     public partial class ProgressBarDialog : Window
    {
        public delegate void DoDanse();
        private List<MyListBoxDataItem> _items;
        private bool isRunning;
        //private Thread _thread;
        private CancellationTokenSource _canselTokenSource;
        private CancellationToken _tocken;

        public ProgressBarDialog(int progressBarCount)
        {
            InitializeComponent();
            _items = new();
            FillListBox(progressBarCount);
            isRunning = false;
            //MyProgressBars.ItemsSource = items;
            _canselTokenSource = new();
            _tocken = _canselTokenSource.Token;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            Thread.Sleep(500);
            _canselTokenSource.Cancel();
            Close();
        }

        private void FillListBox(int progressBarCount)
        {
            for(int i = 0; i < progressBarCount; i++)
            {
                _items.Add(new MyListBoxDataItem("Progress Bar #" + i));
            }
            RefreshView();
        }

        private void RefreshView()
        {
            MyProgressBars.Items.Clear();
            foreach(var it in _items)
            {
                MyProgressBars.Items.Add(it);
            }
        }

        public void DoWork(CancellationToken token)
        {
            do
            {
                

                foreach (var it in _items)
                {
                    it.DoWork();
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }
                else
                {
                    try
                    {
                        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.DataBind,
                    new DoDanse(RefreshView));
                    }
                    catch (Exception e)
                    {
                        // do nothing with is... 
                    }
                    
                }
                Thread.Sleep(100);
            } while (isRunning);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
                return;
            isRunning = true;
            //_thread = new(DoWork);
            //_thread.Start();
            Task task = new(() =>
            {
                DoWork(_tocken);
            });

            task.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _canselTokenSource.Cancel();
        }
    }
}
