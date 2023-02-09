namespace Bulwark.Admin.Repositories.Models;
public class PermissionModel
{
	[BsonId]
	public string Id { get; set; }
	public DateTime Created { get; set; }
	
	public PermissionModel()
	{
		Id = string.Empty;
	}
}


