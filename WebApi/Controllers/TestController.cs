﻿using Base.Exceptions;
using Data.Entities;
using Data.Services;
using Security.Services;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiControllerBase
    {
        private readonly ITestObjectService _testObjectService;
        public TestController(ITestObjectService testObjectService)
        {
            _testObjectService = testObjectService;
        }
        [HttpGet]
        [Route("action")]
        public async Task<IHttpActionResult> Test()
        {
            using(var uofw = CreateUnitOfWork)
            {
                var obj = _testObjectService.Create(uofw, new TestObject() { Title = "Old title", Number = 5 });
                var dto = new { Title = "DTO Text", Number = -7 };
                uofw.GetRepository<TestObject>().SetFromObject(obj.ID, dto);

                await uofw.SaveChangesAsync();
                return Ok(new { obj, dto });
            }
            
        }
    }
}