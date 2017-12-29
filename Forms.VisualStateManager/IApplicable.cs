namespace Forms.VisualStateManager
{
    public interface IApplicable
    {
        bool IsApplied { get; }
        void Apply();
    }
}