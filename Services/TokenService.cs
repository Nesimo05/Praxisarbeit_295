using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ski_Service_Backend.Services
{
    public class TokenService : ITokenService
    {
        // IConfiguration wird in diese Klasse injiziert, um den Token-Schlüssel aus der Konfigurationsdatei zu lesen
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            // Initialisierung des SymmetricSecurityKey mit dem aus der Konfigurationsdatei gelesenen Token-Schlüssel
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        }

        /// <summary>
        /// Erzeugt ein JWT (JSON Web Token) für den angegebenen Benutzernamen.
        /// </summary>
        /// <param name="username">Der Benutzername, für den das Token erstellt werden soll.</param>
        /// <returns>Das generierte JWT.</returns>
        public string CreateToken(string username)
        {
            // Erstellen von Ansprüchen. Es können weitere Informationen zu diesen Ansprüchen hinzugefügt werden, z.B. E-Mail-ID.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, username)
            };

            // Erstellen von Anmeldeinformationen. Festlegen des verwendeten Sicherheitsalgorithmus
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Erstellen des Token-Beschreibers
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Erstellen des Tokens
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Rückgabe des generierten Tokens
            return tokenHandler.WriteToken(token);
        }
    }
}
