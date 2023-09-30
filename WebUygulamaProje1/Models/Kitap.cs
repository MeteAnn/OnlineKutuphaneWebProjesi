using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUygulamaProje1.Models
{
    public class Kitap
    {

        [Key]
        public int Id { get; set; }

        [Required] //Null olamaz 
        public string KitapAdi { get; set; }

        public string Tanim { get; set; }

        [Required]
        public string Yazar { get; set; }

        [Required]
        [Range(10,5000)] //saçma değerler almasın diye bu aralıkta fiyat girilsin
        public double Fiyat { get; set; }



        [ValidateNever]
        public int KitapTuruId { get; set; } //Kitaplar tablosunaa KitapTürleri tablosundan foreign key koyacaksak bunları eklememiz lazım
        [ForeignKey("KitapTuruId")] // Bu özellik, Entity Framework Core'a ilişkiyi açıklar. KitapTuruId özelliği, KitapTuru nesnesiyle ilişkilendirilir. [ForeignKey] özelliği, bu ilişkinin hangi anahtar alanını kullanarak yapıldığını belirtir. Yani, KitapTuru nesnesinin hangi özelliğinin KitapTuruId ile eşleştiğini gösterir.
        [ValidateNever]
        public KitapTuru KitapTuru { get; set; }

        [ValidateNever]
        public string ResimUrl { get; set; }

    }
}
