
namespace Domain.Interfaces
{
    public interface ITokenManager
    {
        public string GenerateToken(string id, string email, string name);
    }
}