using System;
using System.Timers;

namespace GameOfLife.ViewModel
{
    public class TimerImpl : ITimer
    {
        private readonly Timer timer;
        private int updateOnceIn = 200;

        public TimerImpl()
        {
            timer = new Timer(updateOnceIn);
            timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            GenerateTick(null);
        }

        public void Start()
        {
//      timer.Change(0, UpdateOnceIn);
            timer.Enabled = true;
        }

        public void Stop()
        {
//      timer.Change(-1, UpdateOnceIn);
            timer.Enabled = false;
        }

        public int UpdateOnceIn
        {
            get { return updateOnceIn; }
            set
            {
                updateOnceIn = value;
//        timer.Change(updateOnceIn, updateOnceIn);
                timer.Interval = updateOnceIn;
            }
        }

        private void GenerateTick(object state)
        {
            RaiseTick();
        }

        public event Action Tick;

        protected virtual void RaiseTick()
        {
            var handler = Tick;
            if (handler != null)
            {
                handler();
            }
        }
    }
}