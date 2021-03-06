namespace CookBook.Infrastructure
{
    public interface IBusinessRule
    {
        bool IsViolated { get; }
        string Message { get; }
        string? PropertyName => string.Empty;
    }
}