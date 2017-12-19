using System;

namespace Forms.VisualStateManager
{
    public struct Duration
    {
        private enum DurationType
        {
            Auto,
            TimeSpan,
            Forever
        }

        private readonly DurationType _durationType;
        private readonly TimeSpan _timeSpan;
        public bool HasTimeSpan => _durationType == DurationType.TimeSpan;

        public TimeSpan TimeSpan
        {
            get
            {
                if (HasTimeSpan)
                    return _timeSpan;
                throw new InvalidOperationException("Not explicit TimeSpan duration");
            }
        }

        public Duration(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            _durationType = DurationType.TimeSpan;
        }

        private Duration(DurationType type)
        {
            _durationType = type;
        }

        public static readonly Duration Automatic = new Duration(DurationType.Auto);
        public static readonly Duration Forever = new Duration(DurationType.Forever);

        public override string ToString()
        {
            if (HasTimeSpan)
                return _timeSpan.ToString();
            return _durationType == DurationType.Forever ? "Forever" : "Automatic";
        }

        public override bool Equals(object obj)
        {
            if (obj is Duration d)
                return Equals(d);
            return base.Equals(obj);
        }

        public bool Equals(Duration other)
        {
            return _durationType == other._durationType && (_durationType != DurationType.TimeSpan || _timeSpan.Equals(other._timeSpan));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) _durationType * 397) ^ _timeSpan.GetHashCode();
            }
        }

        public static bool operator ==(Duration d1, Duration d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(Duration d1, Duration d2)
        {
            return !(d1 == d2);
        }
    }
}