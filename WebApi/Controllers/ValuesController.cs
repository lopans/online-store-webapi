using Data.Entities;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ValuesController : ApiControllerBase
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            using(var uofw = UnitOfWork)
            {
                uofw.GetRepository<TestObject>().Create(new TestObject() { TestField = 1 });
                uofw.SaveChanges();
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
