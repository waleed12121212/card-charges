namespace BlazingPizza;

public class CarrierStoreUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "user";
}