using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DansingProgressBars
{
    class MyListBoxDataItem
    {
        public SolidColorBrush BackColor { get; set; }
        public string Title { get; set; }
        public int Progress { get; set; }
        private int _step;
        private bool direction;

        public MyListBoxDataItem( string title )
        {
            var rand = new Random();
            Title = title;
            BackColor = new(Color.FromArgb(Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255))
                , Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255))));
            Progress = rand.Next(0, 99);
            _step = rand.Next(1, 10);
            direction = Convert.ToBoolean(rand.Next(0, 2));
        }

        public void DoWork()
        {
            if (direction)
            {
                Progress += _step;
            }
            else
            {
                Progress -= _step;
            }
            if (Progress > 100)
                direction = false;
            if (Progress < 0)
                direction = true;
        }
    }
}
