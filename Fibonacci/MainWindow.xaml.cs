using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Fibonacci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void ShowNext();
        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _token;
        private bool _InProcess; 
        int first;
        int second;
        int max;

        public MainWindow()
        {
            InitializeComponent();
            first = second = 0;
            _cancelTokenSource = new();
            _token = _cancelTokenSource.Token;
            _InProcess = false;
        }

        private int CheckMaxNumber()
        {
            int.TryParse(MaxNumber.Text, out int result);
            return result;
        }

        private void MaxNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CheckMaxNumber() == 0)
            {
                MaxNumber.Text = "";
            }
        }

        private void MaxNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CheckMaxNumber() == 0)
            {
                MaxNumber.Text = "Введите верхнюю границу";
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_InProcess)
                return;
            max = CheckMaxNumber();

            if (max <= 0)
            {
                MessageBox.Show("Введена неверная верхняя граница!", "Ошибка");
                return;
            }

            _InProcess = true;

            first = second = 0;

            ResultBlock.Text = "";

            Task task = new(() =>
           {
               Calculate(_token);
           });

            task.Start();
        }
        
        private void Calculate(CancellationToken token)
        {
            while (max > second)
            {
                GenerateNext();
                if (token.IsCancellationRequested || max < second)
                {
                    _InProcess = false;
                    return;
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                   new ShowNext(ChangeText));
                }
                Thread.Sleep(500);
            }
            _InProcess = false;
        }

        private void ChangeText()
        {
            ResultBlock.Text += (second.ToString() + " ");
        }

        private void GenerateNext()
        {
            if (second == 0 && first == 0)
            {
                second = 1;
                return;
            }

            if (second == 1 && first == 0)
            {
                first = 1;
                return;
            }

            int temp = second + first;
            first = second;
            second = temp;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _cancelTokenSource.Cancel();
        }
    }
}
