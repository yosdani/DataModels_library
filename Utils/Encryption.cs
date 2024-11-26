using Datamodels.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Datamodels.Utils
{
    public static class Encryption
    {
        private static readonly byte[] pruebaKey_1 = Encoding.Unicode.GetBytes("¶ỴÊ₢1¨Ñ¿ṍʤꟄﷻF4®̿ɯﺝñR¶YΏzbA۞ǳ№bY£¥Ꝏﺾ☼-خϞȄÇﺻﻻ");
        private static readonly byte[] pruebaIV_1 = Encoding.Unicode.GetBytes("ꜺꞠ*9*ʤꟄﷻF4®̿ɯﺝñR¶YΏzbA۞ǳ№bY£¥Ꝏﺾ☼EDWARDRAMﻻ۞ὦHORUS+Ơ♂☺"._Reverse());
        private static readonly byte[] pruebaKey_2 = Encoding.Unicode.GetBytes("Æu¢ԯࣰῇꭥˡǉѫḞ♂ﬅ900805FrankՏʪþﻺἘḜԘ͌210311C0tecitoŎ¥Ὃ▲"._Reverse());
        private static readonly byte[] pruebaIV_2 = Encoding.Unicode.GetBytes("▓♠ﺻRamsés۩͒ŕcҒࣱ‡۵╛YosdaniښﺽAᴅⱥꭁﷺFrankỔꞶۣЗڮ~Ariel¾;@éñYordanɀꟐ♣");
        private static readonly byte[] pruebaKey_FrontEnd = (new int[] { 11, 785452079, 24469, 89561, 65465513, 312121, 37, 48649 })._ToByteArr();
        private static readonly byte[] pruebaIV_FrontEnd = (new int[] { 4231, 7418669, 73, 2083 })._ToByteArr();
        private const char dataSeparator = '-';
        private const char a1 = '+', v1 = '-';
        private const char a2 = '/', v2 = '_';

        public static string Encrypt_DoubleBound_1(string data) => _Encrypt_DoubleBound(data, Encrypt_1);

        public static string Encrypt_DoubleBound_2(string data) => _Encrypt_DoubleBound(data, Encrypt_2);

        public static string Encrypt_DoubleBound_4(string data) => _Encrypt_DoubleBound(data, Encrypt_4);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string _Encrypt_DoubleBound(string data, Func<string, string> encryptor)
        {
            Random random = new Random();
            string toEncript = $"{random.Next()}{dataSeparator}{encryptor(data)}{dataSeparator}{random.Next()}";
            return encryptor(toEncript);
        }

        public static string Decript_DoubleBound_1(string data) => _Decript_DoubleBound(data, Decrypt_1);

        public static string Decript_DoubleBound_2(string data) => _Decript_DoubleBound(data, Decrypt_2);

      //  public static string Decript_DoubleBound_4(string data) => _Decript_DoubleBound(data, Decrypt_4);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string _Decript_DoubleBound(string data, Func<string, string> decryptor)
        {
            string decryptedText = decryptor(data);
            string[] arr = decryptedText.Split(dataSeparator);
            return decryptor(arr[1]);
        }

        public static string Encrypt_1(string text) => Convert.ToBase64String(_Encrypt(Encoding.Default.GetBytes(text), pruebaKey_1, pruebaIV_1));

        public static string Encrypt_2(string text) => Convert.ToBase64String(_Encrypt(Encoding.Default.GetBytes(text), pruebaKey_2, pruebaIV_2));

        public static string Encrypt_3(string text) => Convert.ToBase64String(_Encrypt(Encoding.Default.GetBytes(text), pruebaKey_2, pruebaIV_2));

        public static string Encrypt_4(string text) => Convert.ToBase64String(_Encrypt(Encoding.Default.GetBytes(text), pruebaKey_2, pruebaIV_2));

        public static string Encrypt_FrontEnd(string text) => Convert.ToBase64String(_Encrypt(Encoding.UTF8.GetBytes(text), pruebaKey_FrontEnd, pruebaIV_FrontEnd));

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte[] _Encrypt(byte[] bytes, byte[] key, byte[] iv)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException("bytes");
            if (key == null || key.Length == 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length == 0)
                throw new ArgumentNullException("IV");
            using (Aes aesAlg = Aes.Create())
            {
                FixKey(ref key, aesAlg);
                FixIV(ref iv, aesAlg);
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                        swEncrypt.Write(bytes);
                    return msEncrypt.ToArray();
                }
            }
        }

        public static string Decrypt_1(string text) => Encoding.Default.GetString(_Decrypt(Convert.FromBase64String(text), pruebaKey_1, pruebaIV_1));

        public static string Decrypt_2(string text) => Encoding.Default.GetString(_Decrypt(Convert.FromBase64String(text), pruebaKey_2, pruebaIV_2));

       // public static string Decrypt_3(string text) => Encoding.Default.GetString(_Decrypt(Convert.FromBase64String(text), seamindKey_3, seamindIV_3));

        //public static string Decrypt_4(string text) => Encoding.Default.GetString(_Decrypt(Convert.FromBase64String(text), seamindKey_4, seamindIV_4));

        public static string Decrypt_FrontEnd(string text) => Encoding.UTF8.GetString(_Decrypt(Convert.FromBase64String(text), pruebaKey_FrontEnd, pruebaIV_FrontEnd));

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte[] _Decrypt(byte[] bytes, byte[] key, byte[] iv)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException("bytes");
            if (key == null || key.Length == 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length == 0)
                throw new ArgumentNullException("IV");
            using (Aes aesAlg = Aes.Create())
            {
                FixKey(ref key, aesAlg);
                FixIV(ref iv, aesAlg);
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (MemoryStream output = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];
                    int read = csDecrypt.Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        output.Write(buffer, 0, read);
                        read = csDecrypt.Read(buffer, 0, buffer.Length);
                    }
                    return output.ToArray();
                }
            }
        }

        private static void FixKey(ref byte[] key, Aes aes)
        {
            if (!aes.ValidKeySize(key.Length * 8))
                Array.Resize(ref key, aes.LegalKeySizes.Max(ks => ks.MaxSize) / 8);
        }

        private static void FixIV(ref byte[] iv, Aes aes)
        {
            int size = iv.Length * 8;
            if (!aes.LegalBlockSizes.Any(lbs =>
            {
                int min = lbs.MinSize;
                int max = lbs.MaxSize;
                return size == min || size == max || (lbs.SkipSize != 0 && size > min && size < max && size % lbs.SkipSize == 0);
            }))
                Array.Resize(ref iv, aes.LegalBlockSizes.Max(ks => ks.MaxSize) / 8);
        }

        public static string ToURLFix(string text) => text.Replace(a1, v1).Replace(a2, v2);

        public static string FromURLFix(string text) => text.Replace(v1, a1).Replace(v2, a2);
    }
}
