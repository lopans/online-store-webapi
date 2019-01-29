using Base.Services;
using Data.Entities.Store;
using Security.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models.Category;

namespace WebApi.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiControllerBase
    {
        private readonly IBaseService<Category> _categoryService;
        private readonly IAccessService _accessService;

        public CategoriesController(IBaseService<Category> categoryService, IAccessService accessService)
        {
            _accessService = accessService;
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> Get()
        {
            using (var uofw = CreateUnitOfWork)
            {

                var data = await _categoryService.GetAll(uofw).Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    x.Color,
                    FileID = x.Image != null ? (Guid?)x.Image.FileID : null,
                    FileName = x.Image != null ? x.Image.FileName + x.Image.Extension : null,
                    x.RowVersion
                }).ToListAsync();
                return await WrapListViewResult(data, typeof(Category), uofw, _accessService);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateModel model)
        {
            using (var uofw = CreateUnitOfWork)
            {
                var ret = _categoryService.Create(uofw,
                    new Category()
                    {
                        Color = model.Color,
                        Title = model.Title,
                        ImageID = model.ImageID
                    });

                return Ok(new
                {
                    Color = ret.Color,
                    ID = ret.ID,
                    FileID = ret.Image != null ? (Guid?)ret.Image.FileID : null
                });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("update")]
        public async Task<IHttpActionResult> Update(UpdateModel model)
        {
            using (var uofw = CreateUnitOfWork)
            {
                var ret = _categoryService.Update(uofw,
                    new Category()
                    {
                        Color = model.Color,
                        Title = model.Title,
                        ImageID = model.ImageID,
                        ID = model.ID,
                        RowVersion = model.RowVersion
                    });
                return Ok(ret);
            }
        }

    }
}