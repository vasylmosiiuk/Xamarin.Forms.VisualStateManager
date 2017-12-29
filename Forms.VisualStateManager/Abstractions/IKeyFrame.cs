using System;

namespace Forms.VisualStateManager.Abstractions
{
    public interface IKeyFrame<TValue> : IApplicable
    {
        TimeSpan KeyTime { get; set; }
        TValue Value { get; set; }
    }
}