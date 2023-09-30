using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly UygulamaDbContext _uygulamaDbContext;

        internal DbSet<T> dbSet; // Bu sınıf içindeki bir üye değişken, T türündeki veritabanı tablosuna erişimi temsil eder. Bu değişken, sorguların ve işlemlerin bu tablo üzerinde gerçekleştirilmesine izin verir.

        public Repository(UygulamaDbContext uygulamaDbContext) //Bu nesne otomatik olarak oluşturulup bize verilir.
        {
            _uygulamaDbContext = uygulamaDbContext; //Buraya geliyor bu nesne program ilk ayağa kalkarken biz burada atamasını yapıyoruz. 

            this.dbSet = _uygulamaDbContext.Set<T>(); //dbSet aslında _uygulamaDbContext.KitapTurleri yani artık uzun uzun bunu yazmak yerine dbSet yazacağız.
            _uygulamaDbContext.Kitaplar.Include(k => k.KitapTuru).Include(k => k.KitapTuruId);


        }

        public void Ekle(T entity)
        {
            dbSet.Add(entity); //Ekleme işlemi bu şekilde artık
        }

        public T Get(Expression<Func<T, bool>> filtre, string? includeProps = null)
        {


            IQueryable<T> sorgu = dbSet;
            sorgu = sorgu.Where(filtre); //şu kısım bize birden fazla sorgu getirebilir.
            if (!string.IsNullOrEmpty(includeProps))
            {

                foreach (var includeProp in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    sorgu = sorgu.Include(includeProp);
                }

            }
            return sorgu.FirstOrDefault();


        }

        public IEnumerable<T> GetAll(string? includeProps=null)
        {
            
            IQueryable<T> sorgu = dbSet;
            if (!string.IsNullOrEmpty(includeProps))
            {

                foreach (var includeProp in includeProps.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    sorgu = sorgu.Include(includeProp);
                }

            }
            return sorgu.ToList(); //Tamamını getirecektir.


        }

        public void Sil(T entity)
        {

            dbSet.Remove(entity); //bunu dediğimiz an tek kaydı siler.

        }

        public void SilAralik(IEnumerable<T> entities)
        {

            dbSet.RemoveRange(entities); //Bu da birden fazla siler

        }
    }
}
