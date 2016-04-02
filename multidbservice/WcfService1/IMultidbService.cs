﻿using System;
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
        Person GetData(string id);
        
        [OperationContract]
        parametrosAddDatabase JSONparametrosAddDatabase(parametrosAddDatabase addDatabase);

        /*
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        */

        [OperationContract]
        string connection_maria();

        [OperationContract]
        string connection_mongo();

        [OperationContract]
        string connection_sqlserver();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

}
