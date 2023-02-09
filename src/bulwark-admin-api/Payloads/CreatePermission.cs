namespace Bulwark.Admin.Api.Payloads;

public class CreatePermissionPayload
{
    public string Name { get; set; }

    public CreatePermissionPayload()
    {
        Name = string.Empty;
    }
}