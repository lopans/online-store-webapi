using Data.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ValuesController : ApiControllerBase
    {
        private readonly ITestObjectService _testObjectService;
        public ValuesController(ITestObjectService testObjectService)
        {
            _testObjectService = testObjectService;
        }
        // GET api/values
        public async Task<IHttpActionResult> Get()
        {
            using(var uofw = UnitOfWork)
            {
                var ret = _testObjectService.GetAll(uofw).Select(x => new { x.TestField, x.ID });
                //uofw.GetRepository<TestObject>().Create(new TestObject() { TestField = 1 });
                //uofw.SaveChanges();
                return Ok(ret.ToList());
            }
            //return new string[] { "value1", "value2" };
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
