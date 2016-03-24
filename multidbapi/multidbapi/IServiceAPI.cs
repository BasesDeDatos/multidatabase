using System.ServiceModel;

namespace multidbapi
{
    [ServiceContract]
    public interface ServiceInterface
    {
        [OperationContract]
        Person GetData(string id);

        [OperationContract]
        Instance CreateInstance(string pDatabase_type, string pUser, string pPass, string pServer, string pAlias);
    }
}