namespace Security.DTO
{
    public class EntityPermissionSet
    {
        public string EntityType { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }

        public static EntityPermissionSet FromAll(string type)
        {
            return new EntityPermissionSet()
            {
                Create = true,
                Delete = true,
                Read = true,
                Update = true,
                EntityType = type
            };
        }

        public static EntityPermissionSet FromReadOnly(string type)
        {
            return new EntityPermissionSet()
            {
                Create = false,
                Delete = false,
                Read = true,
                Update = false,
                EntityType = type
            };
        }
    }

    
}
