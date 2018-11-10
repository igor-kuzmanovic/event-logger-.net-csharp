using System;
using System.Security;

namespace Helpers
{
    public static class InputHelper
    {
        public static SecureString InputPrivateKey()
        {
            SecureString privateKey = new SecureString();
            ConsoleKeyInfo keyInfo;

            Console.Write("Enter the private key: ");

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Backspace && privateKey.Length > 0)
                {
                    privateKey.RemoveAt(privateKey.Length - 1);
                    Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    privateKey.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }
            } while (keyInfo.Key != ConsoleKey.Enter || privateKey.Length == 0);

            return privateKey;
        }
    }
}
