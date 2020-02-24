namespace ExitGames.Client.DemoParticle
{
    using System;
    using System.Runtime.CompilerServices;

    public class TimeKeeper
    {
        private int lastExecutionTime = Environment.TickCount;
        private bool shouldExecute;

        public TimeKeeper(int interval)
        {
            this.IsEnabled = true;
            this.Interval = interval;
        }

        public void Reset()
        {
            this.shouldExecute = false;
            this.lastExecutionTime = Environment.TickCount;
        }

        public int Interval { get; set; }

        public bool IsEnabled { get; set; }

        public bool ShouldExecute
        {
            get
            {
                return (this.IsEnabled && (this.shouldExecute || ((Environment.TickCount - this.lastExecutionTime) > this.Interval)));
            }
            set
            {
                this.shouldExecute = value;
            }
        }
    }
}

