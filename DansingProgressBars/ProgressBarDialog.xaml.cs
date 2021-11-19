using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        private Thread _thread;

        public ProgressBarDialog(int progressBarCount)
        {
            InitializeComponent();
            _items = new();
            FillListBox(progressBarCount);
            isRunning = false;
            //MyProgressBars.ItemsSource = items;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            Thread.Sleep(500);
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

        public void DoWork()
        {
            do
            {
                foreach (var it in _items)
                {
                    it.DoWork();
                }
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.DataBind,
                    new DoDanse(RefreshView));
                Thread.Sleep(100);
            } while (isRunning);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = true;
            _thread = new(DoWork);
            _thread.Start();
            //StartButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new DoDanse(DoWork));
        }
    }
}
