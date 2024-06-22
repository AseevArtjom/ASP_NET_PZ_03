using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


/*
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
*/

builder.Services.AddScoped<LocalUploadedFileStorage>(x =>
{
    var env = x.GetRequiredService<IWebHostEnvironment>();
    return new LocalUploadedFileStorage(Path.Combine(env.WebRootPath,"uploads","images"));
});


builder.Services.AddDbContext<SiteContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<SiteContext>()
    .AddDefaultTokenProviders();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

app.MapControllerRoute(
    name: "InfoAndInfoSkill",
    pattern: "{controller}/{infoId}/{infoSkillId}/{action}"
);

