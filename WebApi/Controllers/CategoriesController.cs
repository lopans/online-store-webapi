using Base.Services;
using Data.Entities.Store;
using Security.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models.Category;
using WebApi.SignalR;

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
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            using (var uofw = CreateUnitOfWork)
            {

                var data = await _categoryService.GetAll(uofw).Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    x.Color,
                    ImageID = x.Image != null ? (Guid?)x.Image.FileID : null,
                    FileName = x.Image != null ? x.Image.FileName + x.Image.Extension : null,
                    x.Description,
                    x.RowVersion
                }).ToListAsync();
                StoreHub.SayHello();
                return await WrapListViewResult(data, typeof(Category), uofw, _accessService);
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> Get(int id)
        {
            using (var uofw = CreateUnitOfWork)
            {

                var data = await _categoryService.GetAll(uofw)
                    .Where(x => x.ID == id)
                    .Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    x.Color,
                    x.Description,
                    ImageID = x.Image != null ? (Guid?)x.Image.FileID : null,
                    FileName = x.Image != null ? x.Image.FileName + x.Image.Extension : null,
                    x.RowVersion
                }).SingleOrDefaultAsync();
                if (data == null)
                    return NotFound();
                return Ok(data);
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
                        Description = model.Description,
                        Title = model.Title,
                        ImageID = model.ImageID
                    });

                return Ok(ret);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("update")]
        public async Task<IHttpActionResult> Update(Category model)
        {
            using (var uofw = CreateUnitOfWork)
            {
                var imgID = await _categoryService.GetAll(uofw)
                    .Where(x => x.ID == model.ID)
                    .Select(x => x.ImageID).SingleAsync();
                    
                var ret = _categoryService.Update(uofw, new Category()
                {
                    Color = model.Color,
                    Description = model.Description,
                    Title = model.Title,
                    ID = model.ID,
                    RowVersion = model.RowVersion,
                    ImageID = model.ImageID ?? imgID
                });
                return Ok(ret);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("delete")]
        public async Task<IHttpActionResult> Delete([FromUri]int id)
        {
            using (var uofw = CreateUnitOfWork)
            {
                _categoryService.Delete(uofw, id);
                return Ok();
            }
        }

    }
}