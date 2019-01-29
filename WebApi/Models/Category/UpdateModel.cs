namespace WebApi.Models.Category
{
    public class UpdateModel: CreateModel
    {
        public int ID { get; set; }
        public byte[] RowVersion { get; set; }
    }
}