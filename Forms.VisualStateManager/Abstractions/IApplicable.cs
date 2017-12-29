namespace Forms.VisualStateManager.Abstractions
{
    public interface IApplicable
    {
        bool IsApplied { get; }
        void Apply();
    }
}