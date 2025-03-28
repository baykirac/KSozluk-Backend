using System.Security.Cryptography;

namespace UserApi.Extensions
{
   public static class AppSettingsDecrypt
{
 
     /// <summary>
     /// Verilen nesne içindeki şifreli değerleri çözerek orijinal metinlerine dönüştürür.
     /// </summary>
     /// <param name="_root">
     /// Şifre çözme işleminin uygulanacağı nesne.
     /// </param>
     /// <param name="_path">
     /// Şifre çözme anahtarını içeren byte dizisi.
     /// </param>
     /// <param name="_vector">
     /// Şifre çözme için kullanılan byte dizisi.
     /// </param>
     /// <param name="cipherPrefix">
     /// Şifrelenmiş değerlerin tanınması için kullanılan ön ek.
     /// </param>
     /// <returns>
     /// İşlemin sonucunda şifrelenmiş değerleri çözülmüş root nesnesini döner.
     /// </returns>
     public static IConfigurationRoot Decrypt(this IConfigurationRoot _root, byte[] _path, byte[] _vector, string cipherPrefix)
     {
         DecryptInChildren(_root);
 
         return _root;
         void DecryptInChildren(IConfiguration _parent)
         {  using (Aes myAes = Aes.Create())
             {

                 foreach (var _child in _parent.GetChildren())
                 {

                     if (_child.Value?.StartsWith(cipherPrefix) == true)
                     {

                         var cipherText = _child.Value.Substring(cipherPrefix.Length);
 
                         byte[] CipherTextBytes = Convert.FromBase64String(cipherText);
 
                         // Encrypt the string to an array of bytes.
                         //byte[] encrypted = EncryptStringToBytes_Aes(cipherText, myAes.Key, myAes.IV);
 
                         // Decrypt the bytes to a string.
                         string roundtrip = DecryptStringFromBytes_Aes(CipherTextBytes, _path, _vector);
 
                         //String.Join(",", IV)
                         _parent[_child.Key] = roundtrip;

                     }
 
                     DecryptInChildren(_child);
                 }
             }
         }
     }
 
     /// <summary>
     /// Verilen düz metni, AES algoritmasını kullanarak şifreler ve byte dizisi olarak döner.
     /// </summary>
     /// <param name="plainText">
     /// Şifrelenecek düz metin.
     /// </param>
     /// <param name="Key">
     /// Şifreleme için kullanılan anahtar byte dizisi.
     /// </param>
     /// <param name="IV">
     /// Şifreleme için kullanılan byte dizisi.
     /// </param>
     /// <returns>
     /// Şifrelenmiş metni içeren bir byte dizisi döner.
     /// </returns>
     static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
     {

         // Check arguments.
         if (plainText == null || plainText.Length <= 0)
             throw new ArgumentNullException("plainText");

         if (Key == null || Key.Length <= 0)
             throw new ArgumentNullException("Key");

         if (IV == null || IV.Length <= 0)
             throw new ArgumentNullException("IV");

         byte[] encrypted;
 
         // Create an Aes object
         // with the specified key and IV.
         using (Aes aesAlg = Aes.Create())
         {
             aesAlg.Key = Key;

             aesAlg.IV = IV;
 
             // Create an encryptor to perform the stream transform.
             ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
 
             // Create the streams used for encryption.
             using (MemoryStream msEncrypt = new MemoryStream())
             {
                 using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                 {
                     using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                     {

                         //Write all data to the stream.
                         swEncrypt.Write(plainText);

                     }

                     encrypted = msEncrypt.ToArray();

                 }
             }
         }
 
         // Return the encrypted bytes from the memory stream.
         return encrypted;
     }
 
     /// <summary>
     /// Verilen şifreli byte dizisini AES algoritması kullanarak çözer ve düz metin olarak döner.
     /// </summary>
     /// <param name="cipherText">
     /// Çözülecek şifreli metni içeren byte dizisi.
     /// </param>
     /// <param name="Key">
     /// Şifre çözme için kullanılan anahtar byte dizisi.
     /// </param>
     /// <param name="IV">
     /// Şifre çözme için kullanılan IV byte dizisi.
     /// </param>
     /// <returns>
     /// Çözülmüş düz metin olarak bir string döner.
     /// </returns>
     static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
     {
         // Check arguments.
         if (cipherText == null || cipherText.Length <= 0)
             throw new ArgumentNullException("cipherText");

         if (Key == null || Key.Length <= 0)
             throw new ArgumentNullException("Key");

         if (IV == null || IV.Length <= 0)
             throw new ArgumentNullException("IV");
 
         // Declare the string used to hold
         // the decrypted text.
         string plaintext = null;
 
         // Create an Aes object
         // with the specified key and IV.
         using (Aes aesAlg = Aes.Create())
         {
             aesAlg.Key = Key;

             aesAlg.IV = IV;
 
             // Create a decryptor to perform the stream transform.
             ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
 
             // Create the streams used for decryption.
             using (MemoryStream msDecrypt = new MemoryStream(cipherText))
             {
                 using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                 {
                     using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                     {
 
                         // Read the decrypted bytes from the decrypting stream
                         // and place them in a string.
                         plaintext = srDecrypt.ReadToEnd();
                         
                     }
                 }
             }
         }
 
         return plaintext;
     }
 
}
}
