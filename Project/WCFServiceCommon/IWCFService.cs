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
        string CheckIn();

        [OperationContract]
        void Add();

        [OperationContract]
        void Update();

        [OperationContract]
        void Delete();

        [OperationContract]
        void Read();

        [OperationContract]
        void ReadAll();
    }
}
