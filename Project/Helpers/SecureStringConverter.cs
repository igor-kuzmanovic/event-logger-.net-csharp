﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class SecureStringConverter
    {
        public static SecureString ToSecureString(byte[] valueData)
        {
            string value = Encoding.ASCII.GetString(valueData);
            Array.Clear(valueData, 0, valueData.Length);

            SecureString secureValue = ToSecureString(value);
            value = string.Empty;

            return secureValue;
        }

        public static SecureString ToSecureString(string value)
        {
            SecureString secureValue = new SecureString();
            Array.ForEach(value.ToCharArray(), (c) => secureValue.AppendChar(c));
            value = string.Empty;

            return secureValue;
        }

        public static string ToString(byte[] valueData)
        {
            string value = Encoding.ASCII.GetString(valueData);
            Array.Clear(valueData, 0, valueData.Length);

            return value;
        }

        public static string ToString(SecureString secureValue)
        {
            IntPtr valuePointer = Marshal.SecureStringToGlobalAllocAnsi(secureValue);
            string value = Marshal.PtrToStringUni(valuePointer);
            Marshal.ZeroFreeGlobalAllocAnsi(valuePointer);

            return value;
        }

        public static byte[] ToBytes(string value)
        {
            byte[] valueData = Encoding.ASCII.GetBytes(value);
            value = string.Empty;

            return valueData;
        }

        public static byte[] ToBytes(SecureString secureValue)
        {
            string value = ToString(secureValue);
            byte[] valueData = Encoding.ASCII.GetBytes(value);
            value = string.Empty;
           
            return valueData;
        }
    }
}