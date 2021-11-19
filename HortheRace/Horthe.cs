using System;
using System.Windows.Media;

namespace HortheRace
{
    public class Horthe
    {
        public string Title { get; set; }
        public SolidColorBrush BackColor { get; set; }
        public int Progress { get; set; }
        public int Iresult { get; set; }
        public string Result { get; set; }
        private int _stage;
        private int _step;
        private DateTime _startTime;
        

        public Horthe(string title)
        {
            var rand = new Random();
            Title = title;
            BackColor = new(Color.FromArgb(Convert.ToByte(rand.Next(0, 255)), Convert.ToByte(rand.Next(0, 255)),
                Convert.ToByte(rand.Next(0, 255)), 255));
            Clear();
        }

        public void Start()
        {
            _startTime = DateTime.Now;
        }

        public void Clear()
        {
            Progress = 0;
            Result = "";
            _stage = 0;
            Iresult = 0;
            NewStep();
        }

        private void NewStep()
        {
            var rand = new Random();
            _step = rand.Next(8, 20);
        }

        public void HortheRun()
        {
            if (Result != "")
                return;
            Progress += _step;
            if(Progress > 300 && _stage == 0)
            {
                _stage++;
                NewStep();
            }
            if(Progress > 600 && _stage == 1)
            {
                _stage++;
                NewStep();
            }
            if(Progress > 900 && _stage == 2)
            {
                _stage++;
                NewStep();
            }
            if(Progress > 1000)
            {
                Result = (DateTime.Now - _startTime).ToString("T").Remove(0,6).Remove(6);
                Iresult = int.Parse(Result.Replace(".", string.Empty));
            }
        }
    }
}
