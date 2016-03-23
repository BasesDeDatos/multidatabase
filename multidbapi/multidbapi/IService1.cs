using System.ServiceModel;

namespace multidbapi
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        Person GetData(string id);

        [OperationContract]
        Numero GetInt(string n);
    }
}