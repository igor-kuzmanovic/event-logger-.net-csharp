using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class StringConverter
    {
        public static SecureString ToSecureString(byte[] textBytes)
        {
            string text = Encoding.ASCII.GetString(textBytes);
            SecureString secureText = ToSecureString(text);

            return secureText;
        }

        public static SecureString ToSecureString(string text)
        {
            SecureString secureText = new SecureString();
            Array.ForEach(text.ToCharArray(), (c) => secureText.AppendChar(c));

            return secureText;
        }

        public static string ToString(byte[] textBytes)
        {
            string text = Encoding.ASCII.GetString(textBytes);

            return text;
        }

        public static string ToString(SecureString secureText)
        {
            IntPtr pointer = Marshal.SecureStringToGlobalAllocAnsi(secureText);
            string text = Marshal.PtrToStringAnsi(pointer);
            Marshal.ZeroFreeGlobalAllocAnsi(pointer);

            return text;
        }

        public static byte[] ToBytes(string text)
        {
            byte[] textBytes = Encoding.ASCII.GetBytes(text);

            return textBytes;
        }

        public static byte[] ToBytes(SecureString secureText)
        {
            string text = ToString(secureText);
            byte[] textBytes = Encoding.ASCII.GetBytes(text);
           
            return textBytes;
        }
    }
}
