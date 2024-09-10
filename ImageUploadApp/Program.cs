var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Clear uploaded images on startup
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
if (Directory.Exists(uploadPath))
{
    var files = Directory.GetFiles(uploadPath);
    foreach (var file in files)
    {
        System.IO.File.Delete(file);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ImageUpload}/{action=Index}/{id?}");

app.Run();
