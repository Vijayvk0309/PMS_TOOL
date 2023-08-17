using Microsoft.EntityFrameworkCore;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using PMS_Data_Extraction_from_Machinery_Manual.Repository;
using PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbContextClass>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#region Configuration Repository
builder.Services.AddTransient<ILogin, Login>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IExcelValidateRepository, ExcelValidateRepository>();

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");

app.Run();
