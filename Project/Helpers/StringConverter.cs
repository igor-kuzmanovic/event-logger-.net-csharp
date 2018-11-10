using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Helpers
{
    public static class StringConverter
    {
        public static SecureString ToSecureString(byte[] textBytes)
        {
            return ToSecureString(Encoding.ASCII.GetString(textBytes));
        }

        public static SecureString ToSecureString(string text)
        {
            SecureString secureText = new SecureString();
            Array.ForEach(text.ToCharArray(), (c) => secureText.AppendChar(c));  
            return secureText;
        }

        public static string ToString(byte[] textBytes)
        {
            return Encoding.ASCII.GetString(textBytes);
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
            return Encoding.ASCII.GetBytes(text);
        }

        public static byte[] ToBytes(SecureString secureText)
        {
            return Encoding.ASCII.GetBytes(ToString(secureText));
        }
    }
}
