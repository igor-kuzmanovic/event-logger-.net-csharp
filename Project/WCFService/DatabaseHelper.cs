using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServiceCommon;

namespace WCFService
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
