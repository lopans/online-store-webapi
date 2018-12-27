using Base.DAL;
using Base.Services.Media;
using System;
using System.IO;
using System.Linq;
using System.Net;
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
        public async Task<HttpResponseMessage> GetFile(Guid? fileid)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            using (var suofw = CreateSystemUnitOfWork)
            {
                Stream stream;
                try
                {
                    if (!fileid.HasValue)
                        throw new FileNotFoundException();
                    stream = await _fileSystemService.GetFile(fileid.Value, suofw);
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

        [HttpPost]
        [Route("upload")]
        public async Task<HttpResponseMessage> UploadFile()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents.Any() ? provider.Contents[0] : null;
            if (file == null)
                return response; // TODO: message?
            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            var ext = "." + filename.Split('.').Last();
            var buffer = await file.ReadAsByteArrayAsync();
            Stream stream = new MemoryStream(buffer);
            FileData ret = new FileData();
            try
            {
                using (var uofw = CreateUnitOfWork)
                {
                    ret = await _fileSystemService.SaveFileAsync(stream, uofw, filename, ext);
                }
                response.Content = new StringContent(ret.ID.ToString());
            }
            catch (Exception e)
            {
                response.StatusCode = (HttpStatusCode)500;
            }
            return response;

        }
    }
}
