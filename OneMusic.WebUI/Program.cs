using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using OneMsuic.BusinessLayer.Abstract;
using OneMsuic.BusinessLayer.Concrete;
using OneMsuic.BusinessLayer.Validators;
using OneMusic.BusinessLayer.Concrete;
using OneMusic.DataAccessLayer.Abstract;
using OneMusic.DataAccessLayer.Concrete;
using OneMusic.DataAccessLayer.Context;
using OneMusic.EntityLayer.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Bağlantı cümlesi appsettings.json -> ConnectionStrings:DefaultConnection
builder.Services.AddDbContext<OneMusicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<OneMusicContext>().AddErrorDescriber<CustomErrrorDescriber>();// kayıt olurken ki hataları validatorda ezmiştik onu buraya ekledik

builder.Services.AddScoped<IAboutDal, EfAboutDal> ();
builder.Services.AddScoped<IAboutService, AboutManager> ();

builder.Services.AddScoped<IBannerDal, EfBannerDal>();
builder.Services.AddScoped<IBannerService, BannerManager>();

builder.Services.AddScoped<IAlbumDal, EfAlbumDal>();
builder.Services.AddScoped<IAlbumService, AlbumManager>();

builder.Services.AddScoped<ISingerDal, EfSingerDal>();
builder.Services.AddScoped<ISingerService, SingerManager>();

builder.Services.AddScoped<ISongDal, EfSongDal>();
builder.Services.AddScoped<ISongService, SongManager>();


builder.Services.AddScoped<IContactDal, EfContactDal>();
builder.Services.AddScoped<IContactService, ContactManager>();

builder.Services.AddScoped<IMessageDal, EfMessageDal>();
builder.Services.AddScoped<IMessageService, MessageManager>();

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<IFavoriteSongDal, EfFavoriteSongDal>();
builder.Services.AddScoped<IFavoriteSongService, FavoriteSongManager>();

builder.Services.AddScoped<IPlaylistDal, EfPlaylistDal>();
builder.Services.AddScoped<IPlaylistService, PlaylistManager>();

// Favori kalp butonları için (view'larda giriş yapan kullanıcının favorilerini okur)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<OneMusic.WebUI.Services.FavoriteState>();

builder.Services.AddValidatorsFromAssemblyContaining<SingerValidator>();


builder.Services.AddControllersWithViews(option =>
{
    //Kullanıcı girişi yapmamışsa, sayfalara erişimi engeller.
    //Yetkilendirme gerektiren tüm istekleri otomatik olarak kontrol eder.
//ASP.NET Core uygulamalarında global yetkilendirme filtresi olarak kullanılır.
    var authorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    //option.Filters.Add(new AuthorizeFilter(authorizePolicy));
} );

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index";//girişyapma yolu
    options.AccessDeniedPath = "/ErrorPage/AccessDenied";//hata sayfasının yolunu yazdık
    options.LogoutPath = "/Login/Logout"; //çıkış yapma

});

var app = builder.Build();

// Uygulama açılırken bekleyen migration'ları uygular ve temel rolleri oluşturur.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OneMusicContext>();
    context.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    foreach (var roleName in new[] { "Admin", "Artist", "Visitor" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new AppRole { Name = roleName });
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404/", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


//areadan kopyaladık
//buradailk açılan sayfayı ayarlayabiliriz Home du Default index girdik
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );


    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Default}/{action=Index}/{id?}");
});

app.Run();
