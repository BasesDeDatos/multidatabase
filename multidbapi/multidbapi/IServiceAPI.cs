using System.ServiceModel;

namespace multidbapi
{
    [ServiceContract]
    public interface ServiceInterface
    {
        [OperationContract]
        Person GetData(string id);

        [OperationContract]
        Numero GetInt(string n);
    }
}