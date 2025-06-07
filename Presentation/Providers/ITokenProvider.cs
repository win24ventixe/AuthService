
namespace Presentation.Providers
{
    public interface ITokenProvider
    {
        Task<string> GetTokenAsync();
    }
}