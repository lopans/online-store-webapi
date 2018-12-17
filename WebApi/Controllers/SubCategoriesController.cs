using Base.Services;
using Data.Entities.Store;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models.Category;

namespace WebApi.Controllers
{
    [RoutePrefix("api/subcategories")]
    public class SubCategoriesController : ApiControllerBase
    {
        private readonly IBaseService<SubCategory> _subCategoryService;
        public SubCategoriesController(IBaseService<SubCategory> subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }
        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> Get(int? categoryID)
        {
            using (var uofw = CreateUnitOfWork)
            {
                var ret = await _subCategoryService.GetAll(uofw)
                .Where(x => x.CategoryID == categoryID)
                .Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    //x.Color,
                    //FileID = x.Image != null ? (Guid?)x.Image.FileID : null
                }).ToListAsync();
                return Ok(ret);
            }
        }
    }
}