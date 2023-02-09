namespace Bulwark.Admin.Repositories.Models;
public class RoleModel
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("name")] public string Name { get; set; }
    [BsonElement("permissions")] public List<string> Permissions { get; set; }
    [BsonElement("created")] public DateTime Created { get; set; }
    [BsonElement("modified")] public DateTime Modified { get; set; }
    
    public RoleModel()
    {
        Id = ObjectId.GenerateNewId().ToString();
        Permissions = new List<string>();
        Name = string.Empty;
    }
}