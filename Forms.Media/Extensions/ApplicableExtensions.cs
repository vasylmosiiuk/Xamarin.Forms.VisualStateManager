using System;

namespace Forms.Media.Extensions
{
    public static class ApplicableExtensions
    {
        public static void ThrowIfApplied(this IApplicable applicable)
        {
            if (applicable.IsApplied)
                throw new InvalidOperationException($"Applicable ({applicable.GetType()}) is applied and readonly for now");
        }
        public static void ApplySafety(this IApplicable applicable)
        {
            if (!applicable.IsApplied)
                applicable.Apply();
        }
    }
}