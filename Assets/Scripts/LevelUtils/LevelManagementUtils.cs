using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class LevelManagementUtils {

    public static string Hash(string input) {
        using (SHA1Managed sha1 = new SHA1Managed()) {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash) {
                // "x2" for lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
