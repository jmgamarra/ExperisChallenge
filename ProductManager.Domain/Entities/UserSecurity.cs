namespace ProductManager.Domain.Entities
{
    public class UserSecurity
    {
            public int UserId { get; set; }
            public int SecurityId { get; set; }
            public string PasswordHash { get; set; }
            public bool IsActive { get; set; }
    }
}
