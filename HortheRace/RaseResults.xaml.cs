using System;
using System.Collections.Generic;
using System.Windows;


namespace HortheRace
{
    /// <summary>
    /// Interaction logic for RaseResults.xaml
    /// </summary>
    public partial class RaseResults : Window
    {
        private Horthe[] _horthes;
       

        public RaseResults(List<Horthe> horthes, int bid)
        {
            InitializeComponent();
            _horthes = new Horthe[horthes.Count];
            _horthes = horthes.ToArray();
            MyHorshes.ItemsSource = _horthes;
            MyHorse.Text = _horthes[bid].Title;
            HortheSort();
            MyPlace.Text = _horthes[bid].Result;
            GetPlace();
        }

        private void HortheSort()
        {
            Horthe buble = _horthes[0];
            for(int j = 0; j < _horthes.Length - 1; j++)
            {
                buble = _horthes[0];
                for (int i = 0; i < _horthes.Length - 1; i++)
                {
                    if (buble.Iresult > _horthes[i + 1].Iresult)
                    {
                        _horthes[i] = _horthes[i + 1];
                        _horthes[i + 1] = buble;
                    }
                    else
                    {
                        buble = _horthes[i + 1];
                    }
                }
            }
        }

        private void GetPlace()
        {
            for(int i = 0; i < _horthes.Length; i++)
            {
                if (MyHorse.Text == _horthes[i].Title)
                    MyPlace.Text = (i + 1).ToString();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
