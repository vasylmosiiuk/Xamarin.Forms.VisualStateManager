using System;

namespace Forms.Media.Animations.Abstractions
{
    public interface IKeyFrame<TValue> : IApplicable
    {
        TimeSpan KeyTime { get; set; }
        TValue Value { get; set; }
    }
}