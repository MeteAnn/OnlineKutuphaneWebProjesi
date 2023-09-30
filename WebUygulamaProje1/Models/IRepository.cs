using System.Linq.Expressions;

namespace WebUygulamaProje1.Models
{



    public interface IRepository<T> where T : class //Bu satırda, IRepository adında bir generic (genel) arayüz tanımlanır. T, bu arayüzün parametresi olarak kabul edilen bir tipi temsil eder. where T : class, T'nin bir referans tipi (class) olması gerektiğini belirtir. Yani, yalnızca sınıf türündeki veri türleri bu arayüzü uygulayabilir.
    {


        //T -> KitapTuru

        IEnumerable<T> GetAll(string? includeProps = null);//GetAll tümünü getir demektir.

        T Get(Expression<Func<T, bool>> filtre, string? includeProps = null); //Bu yöntem, belirli bir filtreleme kriterini karşılayan öğeyi almak için kullanılır. Genellikle bir lambda ifadesiyle birlikte kullanılır ve bu sayede özel sorgular oluşturulabilir.

        void Ekle(T entity); //: Bu yöntem, yeni bir öğeyi (örneğin, yeni bir Kitap Türü) veritabanına eklemek için kullanılır.
        void Sil(T entity); //Bu yöntem, belirtilen öğeyi veritabanından silmek için kullanılır.    
        void SilAralik(IEnumerable<T> entities); //Bu yöntem, bir koleksiyon içindeki öğeleri toplu olarak silmek için kullanılır.



    }




}
