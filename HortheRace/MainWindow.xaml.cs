using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
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
        //private Thread _thread;
        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _token;
        private bool _isRunning;

        public MainWindow()
        {
            InitializeComponent();
            _isRunning = false;
            FillHorses();
            _cancelTokenSource = new CancellationTokenSource();
            _token = _cancelTokenSource.Token;
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
            //_thread = new(GoRace);
            //_thread.Start();
            Task task = new Task(() =>
            {
                GoRace(_token);
            });
            task.Start();
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

        private void GoRace(CancellationToken token)
        {
            while(!CheckHorthes())
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                foreach (var it in _horses)
                {
                    it.HortheRun();
                }

                try
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.DataBind,
                  new DoRace(RefreshView));
                }
                catch(Exception e)
                {
                    //Nothing to do with it....
                }
                
                Thread.Sleep(100);
            }

            _isRunning = false;

            try
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                  new DoRace(ShowResults));
            }
            catch (Exception e)
            {
                //Nothing to do with it...
            }
        }

        private void ShowResults()
        {
            RaseResults raseResults = new(_horses, int.Parse(BidCount.Text) - 1);
            raseResults.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _cancelTokenSource.Cancel();
        }
    }
}
