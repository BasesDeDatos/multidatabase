using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace nsMultiDBService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMultiDBService" in both code and config file together.
    [ServiceContract]
    public interface IMultiDBService
    {
        [OperationContract]
        string addDatabase(parametrosAddDatabase addDatabase);
        [OperationContract]
        string createDatabase(parametrosCreateDatabase createDatabase);
        [OperationContract]
        string createTable(parametrosCreateTable createTable);
        
        [OperationContract]
        string getDBConnections();

        [OperationContract]
        string getDatabases();

        [OperationContract]
        string getTables();

        [OperationContract]
        string connection_mongo();


    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

}
