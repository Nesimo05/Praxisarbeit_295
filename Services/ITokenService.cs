namespace Ski_Service_Backend.Services
{
    public interface ITokenService
    {
        string CreateToken(string username);
    }
}