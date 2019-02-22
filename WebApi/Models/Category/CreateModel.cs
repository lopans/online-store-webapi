namespace WebApi.Models.Category
{
    public class CreateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ImageID { get; set; }
        public string Color { get; set; }

    }
}