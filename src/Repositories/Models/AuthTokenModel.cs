﻿namespace Bulwark.Admin.Repositories.Models;
public class AuthTokenModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	[BsonElement("userId")] public string UserId { get; set; }
	[BsonElement("deviceId")] public string DeviceId { get; set; }
	[BsonElement("accessToken")] public string AccessToken { get; set; }
	[BsonElement("refreshToken")] public string RefreshToken { get; set; }
	[BsonElement("created")] public DateTime Created { get; set; }
	[BsonElement("modified")] public DateTime Modified { get; set; }
	
	public AuthTokenModel()
	{
		Id = ObjectId.GenerateNewId().ToString();
		UserId = string.Empty;
		DeviceId = string.Empty;
		AccessToken = string.Empty;
		RefreshToken = string.Empty;
	}
}


