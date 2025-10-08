using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace alquilerCanchasDAL
{
    public class EncriptadorDAL
    {
        public static string EncriptarSHA256(string textoPlano)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(textoPlano);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool CompararHash(string textoPlano, string hashAlmecenado)
        {
            string hashGenerado = EncriptarSHA256(textoPlano);
            return hashGenerado.Equals(hashAlmecenado);
        }
    }
}