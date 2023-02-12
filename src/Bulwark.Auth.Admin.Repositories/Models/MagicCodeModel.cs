﻿namespace Bulwark.Admin.Repositories.Models;
public class MagicCodeModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("userId")] public string UserId { get; set; }
    [BsonElement("code")] public string Code { get; set; }
    [BsonElement("expires")] public DateTime Expires { get; set; }
    [BsonElement("created")] public DateTime Created { get; set; }

    public MagicCodeModel()
    {
        Id = ObjectId.GenerateNewId().ToString();
        UserId = string.Empty;
        Code = string.Empty;
    }
}


