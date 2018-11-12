using Base.DAL;
using System.Collections.Specialized;
using ImageResizer;
using System;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace Base.Services.Media
{
    public interface IFileSystemService
    {
        string FilesFolderPath { get; }
        string DefaultImagePath { get; }
        FileData SaveFile(Stream stream);
        FileData SaveFile(Stream file, IUnitOfWork uofw);
        Task<Stream> GetFile(Guid guid, IUnitOfWork uofw);

    }
    public class FileSystemService : IFileSystemService
    {
        private readonly IBaseService<FileData> _fileService;

        public FileSystemService(IBaseService<FileData> fileService)
        {
            _fileService = fileService;
        }


        public string FilesFolderPath { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files"); }
        public string DefaultImagePath { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "default.jpg"); }

        public FileData SaveFile(Stream stream)
        {
            var fileDir = GetFolder();
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            var guid = new Guid();
            var filePath = Path.Combine(fileDir, guid.ToString());
            using(var str = File.Create(filePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                str.CopyTo(str);
            }
            var fi = new FileInfo(filePath);
            return new FileData()
            {
                FileID = guid,
                FileName =  guid.ToString(),
                CreateDate = DateTime.Now,
                Extension = fi != null && fi.Name.Contains(".") ?
                    fi.Name.Substring(fi.Name.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() :
                    ""
            };
        }

        public FileData SaveFile(Stream file, IUnitOfWork uofw)
        {
            var ret = SaveFile(file);
            return uofw.GetRepository<FileData>().Create(ret);
        }

        public async Task<Stream> GetFile(Guid guid, IUnitOfWork uofw)
        {
            var file = await _fileService.GetAll(uofw).SingleOrDefaultAsync(x => x.FileID == guid);
            if (file == null)
                throw new FileNotFoundException();

            var filepath = Path.Combine(GetFolder(file), file.FileID.ToString()) + "." + file.Extension;
            if(!File.Exists(filepath))
                throw new FileNotFoundException();
            //using(var ms = new MemoryStream())
            //{
            //    System.Drawing.Image img = System.Drawing.Image.FromFile(filepath);
            //    var sizeParam = img.Width >= img.Height ? "width" : "heigth";
            //    var instructions = new NameValueCollection();
            //    instructions.Add(sizeParam, ResizeSettings)
            //    ImageJob job = new ImageJob(filepath, ms, new ResizeSettings()

            //}
            return File.Open(filepath, FileMode.Open);
        }

        private string GetFolder(FileData file = null)
        {
            var date = file?.CreateDate.Date ?? DateTime.Now.Date;
            return Path.Combine(FilesFolderPath,
                date.Year.ToString(),
                date.Month.ToString(),
                date.Day.ToString());
        }
    }
}
