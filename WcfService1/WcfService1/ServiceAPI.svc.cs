using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "GetData/{id}")]
        public Person GetData(string id)
        {
            // lookup person with the requested id 
            return new Person()
            {
                Id = Convert.ToInt32(id),
                Name = "Leo Messi"
            };
        }

        //[Serializable]
        [WebInvoke(Method = "POST", 
                    ResponseFormat = WebMessageFormat.Json, 
                    RequestFormat = WebMessageFormat.Json,
                    UriTemplate = "addDatabase")]
        public parametrosAddDatabase JSONparametrosAddDatabase(parametrosAddDatabase addDatabase)
        {
            return new parametrosAddDatabase()
            {
                database_type = addDatabase.database_type,
                user = addDatabase.user,
                pass = addDatabase.pass,
                server = addDatabase.server,
                protocol = addDatabase.protocol,
                port = addDatabase.port,
                alias = addDatabase.alias
            };
            /*return new parametrosAddDatabase()
            {
                database_type = "database_type",
                user = "user",
                pass = "pass",
                server = "server",
                protocol = "protocol",
                port = "port",
                alias = "alias"
            };*/
            //return addDatabase.user;
        }
        
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

 
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    public class parametrosAddDatabase
    {
        public string database_type { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string server { get; set; }
        public string protocol { get; set; }
        public string port { get; set; }
        public string alias { get; set; }
    }

}
