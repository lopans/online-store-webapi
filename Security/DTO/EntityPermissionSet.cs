namespace Security.DTO
{
    public class EntityPermissionSet
    {
        public string EntityType { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
