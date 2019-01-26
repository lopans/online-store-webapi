namespace Security.DTO
{
    public class EntityPermissionSet
    {
        public string EntityType { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }

        public static EntityPermissionSet FullAccess(string entityType)
        {
            return new EntityPermissionSet
            {
                Create = true,
                Delete = true,
                Read = true,
                Update = true,
                EntityType = entityType
            };
        }
        public static EntityPermissionSet ReadOnly(string entityType)
        {
            return new EntityPermissionSet
            {
                Create = false,
                Delete = false,
                Read = true,
                Update = false,
                EntityType = entityType
            };
        }
    }
}
