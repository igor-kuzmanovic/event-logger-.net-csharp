using IDSServiceCommon;
using System;

namespace IDSService
{
    internal class IDSService : IIDSService
    {
        public void Alarm(string message)
        {
            Console.WriteLine("[ALARM] {0}", message);
        }
    }
}
