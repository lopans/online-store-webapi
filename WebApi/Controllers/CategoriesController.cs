using Base.Services;
using Data.Entities.Store;
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
        public CategoriesController(IBaseService<Category> categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> Get()
        {
            using (var uofw = CreateUnitOfWork)
            {
                var ret = await _categoryService.GetAll(uofw).Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    FileID = x.Image.FileID
                }).ToListAsync();
                return Ok(ret);
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
                        Title = model.Title,
                        ImageID = model.ImageID
                    });
                return Ok(ret);
            }
        }

    }
}