using System;
using System.ServiceModel.Web;

namespace multidbapi
{
    public class ServiceModel : ServiceInterface
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

        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "addDatabase/{pDatabase_type}/{pUser}/{pPass}/{pServer}/{pAlias}")]
        public Instance CreateInstance(string pDatabase_type, string pUser, string pPass, string pServer, string pAlias)
        {
            // lookup person with the requested id 
            return new Instance()
            {
                database_type = pDatabase_type,
                user = pUser,
                pass = pPass,
                server = pServer,
                protocol = "tcp",
                port = "4100",
                alias = pAlias
            };
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Instance
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