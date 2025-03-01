using Microsoft.Xna.Framework;

namespace cr_mono.Core.GameLogic
{
    internal class WorldTime
    {
        private float realTime;
        internal int minutes;// set to internal for debugging
        internal int hours;  // set to internal for debugging
        internal int days;   // set to internal for debugging
        private int months;
        private int years;
        private enum TimeSpeed { Slow, Standard, Fast }
        private TimeSpeed speed = TimeSpeed.Fast;

        internal WorldTime() 
        {
            this.realTime = 0.0f;
            this.minutes = 0;
            this.hours = 12;
            this.days = 1;
            this.months = 1;
            this.years = 1;
        }

        internal void Update(GameTime gameTime) 
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * GetSpeed();
            realTime += elapsed;

            if (realTime < 1000.0) return;
            
            float rem = realTime - 1000.0f;
            realTime = rem;
            ProcessTime();
        }

        internal void ProcessTime() 
        {
            minutes++;

            if (minutes >= 60) 
            {
                hours++;
                minutes = 0;
            }

            if (hours >= 24) 
            {
                days++;
                hours = 0;
            }

            if (days >= 31) 
            {
                months++;
                days = 1;
            }

            if (months >= 13) 
            {
                years++;
                months = 1;
            }
        }

        internal void SetSpeed(int num)
        {
            switch (num)
            {
                case 1:
                    speed = TimeSpeed.Slow;
                    break;
                case 2:
                    speed = TimeSpeed.Standard;
                    break;
                case 3:
                    speed = TimeSpeed.Fast;
                    break;
            }
        }

        internal float GetSpeed() 
        {
            switch (speed) 
            {
                case TimeSpeed.Slow:
                    return 0.5f;
                case TimeSpeed.Standard:
                    return 1.0f;
                case TimeSpeed.Fast:
                    return 2.0f;
                default:
                    speed = TimeSpeed.Standard;
                    return 1.0f;
            }
        }
    }
}
