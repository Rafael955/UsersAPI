namespace UsersAPI.Domain.Entities
{
    public class Role 
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        #region Navegabilidades
        
        public List<User> Users { get; set; }

        #endregion
    }
}
