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
using System.IO;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TextSerchInDir
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
        private string _currentFile;

        public MainWindow()
        {
            InitializeComponent();
            _cancelTokenSource = new();
            _token = _cancelTokenSource.Token;
            _InProcess = false;
            _founded = 0;
            _path = "";
        }

        private void WordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (WordTextBox.Text == "Выберите слово")
            {
                WordTextBox.Text = "";
                WordTextBox.Foreground = Brushes.Black;
            }
        }

        private void WordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (WordTextBox.Text == "")
            {
                WordTextBox.Text = "Выберите слово";
                WordTextBox.Foreground = Brushes.Gray;
            }
        }

        private void DirDialogButton_Click(object sender, RoutedEventArgs e)
        {
             
            var openFolderDialog = new CommonOpenFileDialog();
            openFolderDialog.Title = "Выберите каталог";
            openFolderDialog.IsFolderPicker = true;
            openFolderDialog.EnsurePathExists = true;
            openFolderDialog.Multiselect = false;
            openFolderDialog.AllowNonFileSystemItems = false;

            if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirPathTextBlock.Text = _path = openFolderDialog.FileName;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_InProcess)
            {
                MessageBox.Show("Уже идет поиск!", "Ошибка");
                return;
            }
            
            if (WordTextBox.Text == "Выберите слово" || WordTextBox.Text == "")
            {
                MessageBox.Show("Не выбрано слово для поиска!", "Ошибка");
            }

            _InProcess = true;
            ResultTextBlock.Text = "Идет поиск";
            _targetWord = WordTextBox.Text;

            Task task = new(() => {
                asyncSearch(_token);
            });

            task.Start();
        }

        private void asyncSearch(CancellationToken token)
        {
            string[] allfiles = Directory.GetFiles(_path, "*.txt", SearchOption.AllDirectories);
            foreach (var it in allfiles)
            {
                try
                {
                    _currentFile = it;
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                            new SearchText(ShowFoudedFiles));
                    }
                    StreamReader sr = new(it);
                    while (!sr.EndOfStream)
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
            }
            
            if (token.IsCancellationRequested)
            {
                return;
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    new SearchText(ShowResult));
            }
        }

        private void ShowFoudedFiles()
        {
            if (ResultTextBlock.Text == "Идет поиск")
                ResultTextBlock.Text = "Найдены файлы:\n";
            ResultTextBlock.Text += $"{_currentFile}\n";
            _InProcess = false;
        }

        private void ShowResult()
        {
            ResultTextBlock.Text += $"Найдено {_founded} слов";
            _InProcess = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _cancelTokenSource.Cancel();
        }
    }
}
