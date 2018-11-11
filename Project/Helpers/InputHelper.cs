using System;
using System.Security;

namespace Helpers
{
    public static class InputHelper
    {
        public static SecureString GetKey()
        {
            SecureString key = new SecureString();
            ConsoleKeyInfo keyInfo;

            Console.Write("Enter the private key: ");

            do
            {
                // Read a key from the console without printing it
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Backspace && key.Length > 0)
                {
                    // If the pressed key is Backspace remove the last character from the key
                    key.RemoveAt(key.Length - 1);

                    // Remove the last star from the console
                    Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    // If the pressed key is a valid character append it to the key
                    key.AppendChar(keyInfo.KeyChar);

                    // Write a star to the console to reflect the key length
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter || key.Length == 0);

            Console.Clear();

            return key;
        }
    }
}
