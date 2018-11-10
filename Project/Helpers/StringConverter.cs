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
            SecureString secureText = new SecureString();

            // Convert the text bytes into characters
            char[] textChars = Encoding.Unicode.GetChars(textBytes);

            // Insert all the characters into the secure string
            Array.ForEach(textChars, charByte => secureText.AppendChar(charByte));

            // Clear the character array for security reasons
            Array.Clear(textChars, 0, textChars.Length);

            return secureText;
        }

        public static SecureString ToSecureString(string text)
        {
            SecureString secureText = new SecureString();

            // Convert the text into characters
            char[] textChars = text.ToCharArray();

            // Insert all the characters into the secure string
            Array.ForEach(textChars, charByte => secureText.AppendChar(charByte));

            // Clear the character array for security reasons
            Array.Clear(textChars, 0, textChars.Length);

            return secureText;
        }

        public static string ToString(byte[] textBytes)
        {
            string text = string.Empty;

            // Turn the text bytes into a string 
            text = Encoding.Unicode.GetString(textBytes);

            return text;
        }

        public static string ToString(SecureString secureText)
        {
            string text = string.Empty;

            // Copies the contents of the secure text into unsafe memory
            IntPtr secureStringPointer = Marshal.SecureStringToGlobalAllocUnicode(secureText);

            // Converts the content into a string
            text = Marshal.PtrToStringUni(secureStringPointer);

            // Zero out the unsafe memory and free the pointer
            Marshal.ZeroFreeGlobalAllocUnicode(secureStringPointer);

            return text;
        }

        public static byte[] ToBytes(string text)
        {
            byte[] textBytes = null;

            // Convert the string into character bytes
            textBytes = Encoding.Unicode.GetBytes(text);

            return textBytes;
        }

        public static byte[] ToBytes(SecureString secureText)
        {
            byte[] textBytes = null;

            // Copies the contents of the secure text into unsafe memory
            IntPtr secureStringPointer = Marshal.SecureStringToGlobalAllocUnicode(secureText);

            // Converts the content into a string
            string text = Marshal.PtrToStringUni(secureStringPointer);

            // Zero out the unsafe memory and free the pointer
            Marshal.ZeroFreeGlobalAllocUnicode(secureStringPointer);

            // Convert the string into character bytes
            textBytes = Encoding.Unicode.GetBytes(text);

            // Clear the string for security reasons
            text = string.Empty;

            return textBytes;
        }
    }
}
