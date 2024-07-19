using RedditTrackerAPI.Data.DTOs;
using RedditTrackerAPI.Data.Interfaces.Repositories;
using RedditTrackerAPI.Data.Interfaces.Services;
using RedditTrackerAPI.Data.Sql.Repositories;
using RedditTrackerAPI.Data.Web.Repositories;
using RedditTrackerAPI.Middleware;
using RedditTrackerAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Register services
builder.Services.AddScoped<IRedditService, RedditService>();

// Register repositories
builder.Services.AddScoped<IRedditRepository, RedditRepository>();
builder.Services.AddScoped<ISubredditRepository, SubredditRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowReactApp");

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
