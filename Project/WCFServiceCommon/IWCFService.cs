using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceCommon
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        byte[] CheckIn();

        [OperationContract]
        void Add(string entry);

        [OperationContract]
        void Update(int entryId, string entry);

        [OperationContract]
        void Delete(int entryId);

        [OperationContract]
        object Read(int entryId);

        [OperationContract]
        HashSet<object> ReadAll();
    }
}
