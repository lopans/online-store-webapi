using Base.Services;
using Data.Entities.Store;
using Security.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/subcategories")]
    public class SubCategoriesController : ApiControllerBase
    {
        private readonly IBaseService<SubCategory> _subCategoryService;
        private readonly IAccessService _accessService;

        public SubCategoriesController(IBaseService<SubCategory> subCategoryService, IAccessService accessService)
        {
            _accessService = accessService;
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAll(int categoryID)
        {
            using (var uofw = CreateUnitOfWork)
            {

                var data = await _subCategoryService.GetAll(uofw)
                    .Where(x => x.CategoryID == categoryID)
                    .Select(x => new
                {
                    ID = x.ID,
                    Title = x.Title,
                    ImageID = x.Image != null ? (Guid?)x.Image.FileID : null,
                    FileName = x.Image != null ? x.Image.FileName + x.Image.Extension : null,
                    x.Description,
                    x.RowVersion
                }).ToListAsync();
                return await WrapListViewResult(data, typeof(Category), uofw, _accessService);
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> Get(int id)
        {
            using (var uofw = CreateUnitOfWork)
            {

                var data = await _subCategoryService.GetAll(uofw)
                    .Where(x => x.ID == id)
                    .Select(x => new
                    {
                        ID = x.ID,
                        Title = x.Title,
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
        public async Task<IHttpActionResult> Create(SubCategory model)
        {
            using (var uofw = CreateUnitOfWork)
            {
                var ret = _subCategoryService.Create(uofw,
                    new SubCategory()
                    {
                        CategoryID = model.CategoryID,
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
        public async Task<IHttpActionResult> Update(SubCategory model)
        {
            using (var uofw = CreateUnitOfWork)
            {
                var imgID = await _subCategoryService.GetAll(uofw)
                    .Where(x => x.ID == model.ID)
                    .Select(x => x.ImageID).SingleAsync();
                var ret = _subCategoryService.Update(uofw, new SubCategory()
                {
                    CategoryID = model.CategoryID,
                    Description = model.Description,
                    ID = model.ID,
                    Title = model.Title,
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
                _subCategoryService.Delete(uofw, id);
                return Ok();
            }
        }

    }
}