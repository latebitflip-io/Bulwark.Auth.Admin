using Bulwark.Auth.Admin.Core;
using dotenv.net;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

DotEnv.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoClient = new MongoClient(Environment
   .GetEnvironmentVariable("DB_CONNECTION"));

builder.Services.AddSingleton<IMongoClient>(
    mongoClient);

builder.Services.AddSingleton<IMongoDatabase>(
    mongoClient.GetDatabase("bulwark"));

builder.Services.AddTransient<IAccountRepository, MongoDbAccount>();
builder.Services.AddTransient<IAuthTokenRepository, MongoDbAuthToken>();
builder.Services.AddTransient<IRoleRepository, MongoDbRole>();
builder.Services.AddTransient<IMagicCodeRepository, MongoDbMagicCode>();
builder.Services.AddTransient<IPermissionRepository, MongoDbPermission>();
builder.Services.AddTransient<IRoleRepository, MongoDbRole>();
builder.Services.AddTransient<IPermissionManagement, PermissionManagementService>();
builder.Services.AddTransient<IRoleManagement, RoleManagementService>();
builder.Services.AddTransient<IAccountManagement, AccountManagementService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

