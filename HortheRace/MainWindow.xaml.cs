using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace HortheRace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void DoRace();
        private List<Horthe> _horses;
        private Thread _thread;
        private bool _isRunning;

        public MainWindow()
        {
            InitializeComponent();
            _isRunning = false;
            FillHorses();
        }

        private void FillHorses()
        {
            _horses = new();
            _horses.Add(new("1: Зорька"));
            _horses.Add(new("2: Ветерок"));
            _horses.Add(new("3: Буян"));
            _horses.Add(new("4: Шалун"));
            _horses.Add(new("5: Рыжий"));
            RefreshView();
        }

        private void RefreshView()
        {
            MyProgressBars.Items.Clear();
            foreach(var it in _horses)
            {
                MyProgressBars.Items.Add(it);
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
                return;
            int progressBarCount = CheckCount();
            if (progressBarCount == 0)
            {
                MessageBox.Show("Некорректный номер лошади!", "Ошибка");
                return;
            }
            foreach (var it in _horses)
            {
                it.Clear();
                it.Start();
            }
            _isRunning = true;
            _thread = new(GoRace);
            _thread.Start();
        }
        
        private int CheckCount()
        {
            int.TryParse(BidCount.Text, out int result);
            return (result > 0 && result <= _horses.Count) ? result : 0;
        }

        private bool CheckHorthes()
        {
            bool result = true;
            foreach (var it in _horses)
            {
                if (it.Result == "")
                {
                    result = false;
                }
            }
            return result;
        }

        private void GoRace()
        {
            while(!CheckHorthes())
            {
                foreach (var it in _horses)
                {
                    it.HortheRun();
                }
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.DataBind,
                  new DoRace(RefreshView));
                Thread.Sleep(100);
            }
            _isRunning = false;
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                  new DoRace(ShowResults));
        }

        private void ShowResults()
        {
            RaseResults raseResults = new(_horses, int.Parse(BidCount.Text) - 1);
            raseResults.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isRunning = false;
        }
    }
}
