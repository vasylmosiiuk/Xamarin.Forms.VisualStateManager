namespace Forms.Media
{
    public interface IApplicable
    {
        bool IsApplied { get; }
        void Apply();
    }
}