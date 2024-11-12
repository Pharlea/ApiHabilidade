using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace RPG_API
{
    public class Settings
    {
        public static string secret = "lUX(ihHls(c?g1aQpof5HhaE;m/BzG;BL;2YcuzKr.0RTKm;R?:V:uwaaDY(1OgL";

        public static string generateHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes("9463905759264");

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA256, iterationCount: 10000, numBytesRequested: 32));

            return hashed;
        }
    }
}
