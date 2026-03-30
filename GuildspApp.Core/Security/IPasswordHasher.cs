namespace GuildsApp.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool VerifyPassword(string password, string hash);
    }
}
