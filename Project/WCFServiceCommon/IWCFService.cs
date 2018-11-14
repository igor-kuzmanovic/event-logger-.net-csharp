using System.ServiceModel;

namespace WCFServiceCommon
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        byte[] CheckIn();

        [OperationContract]
        bool Add(string content);

        [OperationContract]
        bool Update(int entryID, string content);

        [OperationContract]
        bool Delete(int entryID);

        [OperationContract]
        byte[] ReadFile();
    }
}
