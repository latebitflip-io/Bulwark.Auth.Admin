namespace Bulwark.Auth.Admin.Payloads;

public class UpdateRolePayload
{
    public string Id { get; set; }
    public string Name { get; set; }

    public UpdateRolePayload()
    {
        Name = string.Empty;
        Id = string.Empty;
    }
}