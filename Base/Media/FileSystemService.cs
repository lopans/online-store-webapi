using Base.DAL;
using Base.Utils;
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
        FileData SaveFile(Stream stream, string fileName, string extension);
        FileData SaveFile(Stream file, IUnitOfWork uofw, string fileName, string extension);
        Task<Stream> GetFile(Guid guid, IUnitOfWork uofw);
        Task<FileData> SaveFileAsync(Stream file, IUnitOfWork uofw, string fileName, string extension);

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

        public FileData SaveFile(Stream stream, string fileName, string extension)
        {
            var fileDir = GetFolder();
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            var guid = Guid.NewGuid();
            var filePath = Path.Combine(fileDir, guid.ToString());
            using(var str = File.Create(filePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(str);
            }
            var fi = new FileInfo(filePath);
            return new FileData()
            {
                FileID = guid,
                FileName =  fileName.OrIfNullOrEmtry(guid.ToString()),
                CreateDate = DateTime.Now,
                Extension = extension.OrIfNullOrEmtry(fi.Extension)
            };
        }

        public FileData SaveFile(Stream file, IUnitOfWork uofw, string fileName, string extension)
        {
            var ret = SaveFile(file, fileName, extension);
            var res = uofw.GetRepository<FileData>().Create(ret);
            uofw.SaveChanges();
            return res;
        }

        public async Task<FileData> SaveFileAsync(Stream file, IUnitOfWork uofw, string fileName, string extension)
        {
            var ret = SaveFile(file, fileName, extension);
            var res = uofw.GetRepository<FileData>().Create(ret);
            await uofw.SaveChangesAsync();
            return res;
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
