using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;


namespace WebUygulamaProje1.Utility
{

    // Veritabanında EF tablo oluşturulması için ilgili model sınıflarımızı buraya eklemeliyiz.
    public class UygulamaDbContext:IdentityDbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) //ASP.NET işlemi bu şekilde çalışır. BU bir mekanizma ve bu şekilde çalışır. Bu işlemi sadece bir kere yapıyoruz veritabanı ile entityler arasındaki köprüdür bu
        
        {
        
        }

        public DbSet<KitapTuru>KitapTurleri { get; set; } //Burada yaptığımız işlemler veritabanındaki tablonun adını oluşturmaktır.

        //Migration : Kod tarafında işlemi veritabanında karşılığını oluşturmak için yaptığımız işlemdir.Migration, veritabanının yapısını güncellemek veya değiştirmek için kod tabanlı bir yaklaşım sunar ve şu işlemleri içerebilir.

        public DbSet<Kitap>Kitaplar { get; set; }


        public DbSet<Kiralama>Kiralamalar { get; set; }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }    

    }
}
