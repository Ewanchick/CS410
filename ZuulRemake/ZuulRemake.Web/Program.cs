using Microsoft.EntityFrameworkCore;
using ZuulRemake.Models;
using ZuulRemake.Repos;
using ZuulRemake.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Adds controller services
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Register Repos
//builder.Services.AddScoped<RoomRepo>();
//builder.Services.AddScoped<MonsterRepo>();
//builder.Services.AddScoped<ItemRepo>();

//// Register Services
//builder.Services.AddScoped<RoomService>();
//builder.Services.AddScoped<MonsterService>();
//builder.Services.AddScoped<ItemService>();
//builder.Services.AddScoped<GameService>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
