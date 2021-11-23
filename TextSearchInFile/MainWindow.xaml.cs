using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Threading;
using Microsoft.Win32;

namespace TextSearchInFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void SearchText();
        private string _targetWord;
        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _token;
        private bool _InProcess;
        int _founded;
        private string _path;

        public MainWindow()
        {
            InitializeComponent();
            _cancelTokenSource = new();
            _token = _cancelTokenSource.Token;
            _InProcess = false;
            _founded = 0;
            _path = "";
        }

        private void FileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBlock.Text = openFileDialog.FileName;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if(_InProcess)
            {
                MessageBox.Show("Уже идет поиск!", "Ошибка");
                return;
            }
            if(!System.IO.File.Exists(FilePathTextBlock.Text))
            {
                MessageBox.Show("Неверное имя файла!", "Ошибка");
                return;
            }

            if(WordTextBox.Text == "Выберите слово" || WordTextBox.Text == "")
            {
                MessageBox.Show("Не выбрано слово для поиска!", "Ошибка");
            }

            _InProcess = true;
            ResultTextBlock.Text = "Идет поиск";
            _targetWord = WordTextBox.Text;
            _path = FilePathTextBlock.Text;

            Task task = new(() => {
                asyncSearch(_token);
            });

            task.Start();
        }

        private void asyncSearch(CancellationToken token)
        {
            try
            {
                StreamReader sr = new(_path);
                while(!sr.EndOfStream)
                {
                    string resultString = sr.ReadLine();
                    string[] stringArr = resultString.Split(' ');
                    for (int i = 0; i < stringArr.Length; i++)
                    {
                        if (_targetWord == stringArr[i])
                            _founded++;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Ошибка");
            }
            if(token.IsCancellationRequested)
            {
                return;
            }else
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, 
                    new SearchText(ShowResult));
            }
        }

        private void ShowResult()
        {
            ResultTextBlock.Text = $"Найдено {_founded} слов";
            _InProcess = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _cancelTokenSource.Cancel();
        }
    }
}
