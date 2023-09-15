using System.Security.Cryptography;
using System.Text;

namespace Defender.IdentityService.Application.Helpers;

public static class Security
{
    public static string Encrypt(string key, string toEncrypt, bool useHashing = true)
    {
        byte[] resultArray = null;
        try
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                using var hashmd5 = MD5.Create();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);


            using var tdes = TripleDES.Create();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }
        catch (Exception ex)
        {
            SimpleLogger.Log(ex);
        }
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string key, string cipherString, bool useHashing = true)
    {
        byte[] resultArray = null;
        try
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                using var hashmd5 = MD5.Create();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            using var tdes = TripleDES.Create();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);


        }
        catch (Exception ex)
        {
            SimpleLogger.Log(ex);
        }
        return Encoding.UTF8.GetString(resultArray);
    }
}
