using System;
using System.IO;

namespace Helpers
{
    internal class DatabaseHelper : IDisposable
    {
        public DatabaseHelper(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
