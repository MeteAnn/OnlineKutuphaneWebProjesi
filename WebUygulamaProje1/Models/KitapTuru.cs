using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {


        [Key] //Buradaki Id özelliği [Key] ile işaretlenmiştir. Bu Id özelliğinin bu sınıfın veritabanı tablosundaki birincil anahtar olduğunu belirtir.
        public int Id { get; set; }

        [Required(ErrorMessage ="Kitap Tür Adı Boş Bırakılamaz")] //Bu Not Null anlamına gelir bu null olamaz demektir. Ayrıca bu alanda ErrorMessage ile türkçe hata mesajı verdirebiliriz.


        [MaxLength(25)] //Bu kontrol 25 karakteri geçmesin demektir.Yani girilen karakter sayısını 25i geçmeyecektir.


        [DisplayName("Kitap Türü Adı:")] //Bu örnekte, DisplayName özniteliği "KitapTuru Türü Adı" adını taşıyor. Bu, belirli bir model sınıfındaki bir özelliğin görünür adını temsil eder. Özellikle ASP.NET MVC veya Razor Pages gibi web uygulamalarında kullanılır. Bu sayede, ilgili özellik kullanıldığında veya görüntülendiğinde, kullanıcıya daha açıklayıcı bir başlık veya etiket sunulur.
        public string Ad { get; set; }
        

    



    }
}
