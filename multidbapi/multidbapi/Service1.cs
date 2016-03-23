using System;
using System.ServiceModel.Web;

namespace multidbapi
{
    public class Service1 : IService1
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "api/addDatabase/{id}")]
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
                    UriTemplate = "api/getInt/{n}")]
        public Numero GetInt(string n)
        {
            int pN = Convert.ToInt32(n);
            return new Numero()
            {
                n = pN,
                multiplo = pN * 10
            };
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Numero
    {
        public int n { get; set; }
        public int multiplo { get; set; }
    }
}