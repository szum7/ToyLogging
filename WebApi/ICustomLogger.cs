namespace WebApi
{
    public interface ICustomLogger<out TCategoryName>
    {
        void LogWarning(string? message, params object?[] args);
    }
}
