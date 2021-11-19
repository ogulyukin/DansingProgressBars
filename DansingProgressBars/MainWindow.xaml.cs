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

namespace DansingProgressBars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            int progressBarCount = checkCount();
            if(progressBarCount == 0)
            {
                MessageBox.Show("Некорректное значение количества", "Ошибка");
                return;
            }
            ProgressBarDialog pgdialog = new(progressBarCount);
            pgdialog.ShowDialog();
        }

        private int checkCount()
        {
            return int.TryParse(count.Text, out int result) ? result : 0;
        }
    }
}
