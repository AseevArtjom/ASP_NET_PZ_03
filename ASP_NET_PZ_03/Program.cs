using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IObjectCollectionStorage<List<Info>>, FileStorage<List<Info>>>(o =>
{
    return new FileStorage<List<Info>>("info.json");
});

builder.Services.AddSingleton<IObjectCollectionStorage<List<Profession>>, FileStorage<List<Profession>>>(o =>
{
    return new FileStorage<List<Profession>>("profession.json");
});

builder.Services.AddSingleton<IObjectCollectionStorage<List<Skill>>, FileStorage<List<Skill>>>(o =>
{
    return new FileStorage<List<Skill>>("skill.json");
});

builder.Services.AddScoped<LocalUploadedFileStorage>(x =>
{
    var env = x.GetRequiredService<IWebHostEnvironment>();
    return new LocalUploadedFileStorage(Path.Combine(env.WebRootPath,"uploads","images"));
});

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



