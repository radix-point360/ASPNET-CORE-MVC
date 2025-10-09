using Login_Using_Middleware.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<LoginMiddleware>();

var app = builder.Build();

app.UseLoginMiddleware();

app.Run();
