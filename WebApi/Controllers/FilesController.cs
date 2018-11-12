using Base.Services;
using Base.Services.Media;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;

namespace WebApi.Controllers
{
    [RoutePrefix("api/file")]
    public class FilesController : ApiControllerBase
    {
        private readonly IFileSystemService _fileSystemService;
        public FilesController(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> All()
        {
            return Ok(new {a = "All" });
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetFile(Guid fileid)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            using (var uofw = CreateUnitOfWork)
            {
                Stream stream;
                try
                {
                    stream = await _fileSystemService.GetFile(fileid, uofw);
                }
                catch(FileNotFoundException fex)
                {
                    stream = File.Open(_fileSystemService.DefaultImagePath, FileMode.Open);
                }
                var content = new StreamContent(stream);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileid.ToString()
                };
                response.Content = content;
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }
    }
}
