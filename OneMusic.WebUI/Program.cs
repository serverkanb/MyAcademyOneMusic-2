using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
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

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<OneMusicContext>().AddErrorDescriber<CustomErrrorDescriber>();// kayýt olurken ki hatalarý validatorda ezmiþtik onu buraya ekledik

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

builder.Services.AddValidatorsFromAssemblyContaining<SingerValidator>();


builder.Services.AddDbContext<OneMusicContext> ();
builder.Services.AddControllersWithViews(option =>
{
    //Kullanýcý giriþi yapmamýþsa, sayfalara eriþimi engeller.
    //Yetkilendirme gerektiren tüm istekleri otomatik olarak kontrol eder.
//ASP.NET Core uygulamalarýnda global yetkilendirme filtresi olarak kullanýlýr.
    var authorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    //option.Filters.Add(new AuthorizeFilter(authorizePolicy)); 
} );

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index";//giriþyapma yolu
    options.AccessDeniedPath = "/ErrorPage/AccessDenied";//hata sayfasýnýn yolunu yazdýk 
    options.LogoutPath = "/Login/Logout"; //çýkýþ yapma
 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404/", "?code{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


//areadan kopyaladýk 
//buradailk açýlan sayfayý ayarlayabiliriz Home du Default index girdik
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