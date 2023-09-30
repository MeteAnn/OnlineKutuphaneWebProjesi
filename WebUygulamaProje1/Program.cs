using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UygulamaDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); //Bu kod, Entity Framework Core kullanarak veritaban�na eri�im sa�layan DbContext s�n�f�n� yap�land�r�r ve hizmetlere ekler. Bu sayede uygulama, UygulamaDbContext s�n�f�n� kullanarak veritaban� i�lemleri yapabilir. Veritaban� ba�lant�s� ve yap�land�rma, uygulama yap�land�rma dosyas�ndan al�n�r, bu da veritaban�n�n hedefini kolayca de�i�tirmenizi sa�lar.

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UygulamaDbContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();

//Dikkat : Yeni bir repository s�n�f olu�turuldu�unda mutlaka burada serviceslere ekmeleliyiz.

builder.Services.AddScoped<IKitapTuruRepository, KitapTuruRepository>(); //bu kod par�as�, IK�tapTuruRepository aray�z�n� KitapTuruRepository s�n�f� ile ili�kilendiren ve bu hizmetin Dependency Injection taraf�ndan y�netilmesini sa�layan bir konfig�rasyonu temsil eder. Bu, uygulama i�inde IK�tapTuruRepository t�r�ndeki ba��ml�l�klar�n, KitapTuruRepository'nin bir �rne�i ile ��z�mlenebilece�i anlam�na gelir.
builder.Services.AddScoped<IKitapRepository, KitapRepository>();
builder.Services.AddScoped<IKiralamaRepository, KiralamaRepository>();
builder.Services.AddScoped<IEmailSender,EmailSender>();



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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //Burada mutlaka action ad� vermek zorunday�z burayo bir mekanizma gibi d���nmek gerek. Buradaki kulland���m�z isimle Controller daki isim ayn� olmal�... burada var olan action sa controller i�ierisindeki metotlara gider.�lk a��l��ta ne kar��las�n istiyorsak bunu buraya yazaa��z index yerine privacy yazm�� olsayd�k bu sefer privacy sayfas� kar��m�za gelecekti.

app.Run();
