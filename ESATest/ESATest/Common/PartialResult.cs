using System;
using static ESATest.Common.Common;

namespace ESATest.Common
{
    internal class PartialResult
    {
        public TimeSpan AverageTime
        {
            get
            {
                if (SuccessfulIterations > 0)
                {
                    return TimeSpan.FromMilliseconds(Time / SuccessfulIterations);
                }

                return new TimeSpan();
            }
        }

        public string AverageTimeString
        {
            get
            {
                return AverageTime.ToString(TimeSpanFormat);
            }
        }

        public int Order { get; set; }

        public int SuccessfulIterations { get; set; } = 0;

        public string Technique { get; set; }

        public double Time { get; set; }

        public void AddProcessTime(double processTime)
        {
            if (processTime > 0)
            {
                Time = Time + processTime;
                SuccessfulIterations++;
            }
        }
    }
}
