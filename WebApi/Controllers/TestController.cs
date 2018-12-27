using Base.Exceptions;
using Data.Entities;
using Data.Services;
using Security.Services;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Base.Utils;
using Base.Services;
using Data.Services.Core;

namespace WebApi.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiControllerBase
    {
        private readonly ITestObjectService _testObjectService;
        private readonly IMappedBaseEntityService _mappedBaseEntityService ;
        
        private readonly IAccessService _accessService;
        public TestController(ITestObjectService testObjectService, IAccessService accessService, IMappedBaseEntityService mappedBaseEntityService)
        {
            _mappedBaseEntityService = mappedBaseEntityService;
            _accessService = accessService;
            _testObjectService = testObjectService;
        }
        [HttpGet]
        [Route("action")]
        public async Task<IHttpActionResult> Test()
        {
            using(var uofw = CreateUnitOfWork)
            {
               var a =await _mappedBaseEntityService.GetEntitiesAsync();
                var obj = _testObjectService.Create(uofw, new TestObject() { Title = "Old title", Number = 5 });
                //var dto = new { Title = "DTO Text", Number = -7 };
                //uofw.GetRepository<TestObject>().SetFromObject(obj.ID, dto);
                //obj.ChangeProperty(uofw, x => x.Number, 10)
                //    .ChangeProperty(uofw, x => x.Title, "Eureka!");

                obj.Number = 15;
                obj.Title = "jopa";
                _testObjectService.Update(uofw, obj);
                await uofw.SaveChangesAsync();
                return Ok(new { obj });
            }
            
        }
    }
}