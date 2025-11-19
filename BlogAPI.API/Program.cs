using BlogAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Infrastructure.Repository;
using BlogAPI.Core.Interfaces;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddNewtonsoftJson();

//Register Repositories
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

//Register DBContext
builder.Services.AddDbContext<BlogDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
