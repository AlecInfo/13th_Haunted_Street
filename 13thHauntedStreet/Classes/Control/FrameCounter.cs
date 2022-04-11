using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _13thHauntedStreet
{
    class FrameCounter
    {
        #region Variables
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 120;

        private Queue<float> _sampleBuffer = new Queue<float>();
        #endregion

        public void Update(float gameTime)
        {
            CurrentFramesPerSecond = 1f / gameTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += gameTime;
        }
    }
}
