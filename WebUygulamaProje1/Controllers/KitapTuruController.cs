using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{


    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
    {



        private readonly IKitapTuruRepository _kitapTuruRepository;




        //private readonly UygulamaDbContext _uygulamaDbContext; //her seferinde nesne oluşturmak yerine bundan bir tane yapıyoruz ve projemizin başından sonuna kadar kullanıyoruz. DESİGN PATTERN : SİGLETON

        public KitapTuruController(IKitapTuruRepository context)
        {

           _kitapTuruRepository = context;
            

        }

        public IActionResult Index()
        {

            List<KitapTuru> objKitapTuruList=_kitapTuruRepository.GetAll().ToList();

            objKitapTuruList = _kitapTuruRepository.GetAll().ToList();

            return View(objKitapTuruList);
        }


        public IActionResult Ekle()
        {


            return View();


        }

        [HttpPost] 
        public IActionResult Ekle(KitapTuru kitapTuru) //Bu şekilde nesne buraya geldi ama bu nesneyi EntityFramework kullanarak veritabanına kaydediyoruz.
        {

            if (ModelState.IsValid) //KitapTuruControllerın Model içindeki alanlar doğru şekilde gelmiş mi bunu kontrol eder.
            {

            

            _kitapTuruRepository.Ekle(kitapTuru); //Burada EntityFramework yeni bir kayıt atacağını anlıyor.
            _kitapTuruRepository.Kaydet(); //burayı görünce de veritabanına atıyor. Eğer bunu yapmazsak bilgiler veri tabanına eklenmez.

             TempData["basarili"] = "Yeni Kitap Türü Başarıyla Oluşturuldu."; //Bunları post kısmına yazıyoruz sadece

            return RedirectToAction("Index", "KitapTuru"); //Bu şekilde yazdığımız zaman veritabanına yeni kayıt ekledikten sonra yukarıdaki Index actionu çağıracaktır.

            }
            return View(); //Bu kısımda kuralları ihlal ettiğinde ekle sayfasını döndürür tekrar
        }




        public IActionResult Guncelle(int? id)//eğer id parametresi null olarak gelirse, programın çökmemesini sağlar.
        {

            if (id==null || id==0) // Bu satırda, id parametresinin değeri null veya 0 ise, işlemin devam edemeyeceği anlamına gelir. Bu nedenle, NotFound() metodunu kullanarak bir HTTP 404 hata yanıtı döndürülür. Bu, belirtilen id değerine sahip bir "KitapTuru" kaydının bulunamadığını gösterir.
            {


                return NotFound();

            }
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u=>u.Id==id); //Benim buraya eşit olan id getir demektir. buradaki u aslında şuna eşittir. Expression<Func<T, bool>> filtre 


            if (kitapTuruVt==null) //Bu satırda, eğer kitapTuruVt null ise (yani belirtilen id ile kayıt bulunamamışsa), yine NotFound() metodunu kullanarak bir HTTP 404 hata yanıtı döndürülür.
            {

                return NotFound();  

            }


            return View(kitapTuruVt); // Eğer yukarıdaki koşullar sağlanmazsa, kitapTuruVt değişkeni bir "KitapTuru" kaydını temsil eder ve bu kayıt, bir görünüme (View) iletilir. Bu, kullanıcının bu "KitapTuru" kaydını güncellemek için bir form göreceği sayfanın görünümünün oluşturulmasını başlatır.


        }








        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru) 
        {

            if (ModelState.IsValid)
            {

                _kitapTuruRepository.Guncelle(kitapTuru);
                _kitapTuruRepository.Kaydet();

                TempData["basarili"] = "Kitap Türü Başarıyla Güncelleştirildi.";

                return RedirectToAction("Index", "KitapTuru");




            }

            return View();
        }


        public IActionResult Sil(int? id)
        {

            if (id == null || id == 0) 
            {


                return NotFound();

            }
            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u=>u.Id==id); 

            if (kitapTuruVt == null) 
            {

                return NotFound();

            }


            return View(kitapTuruVt); 


        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {

            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u=>u.Id==id);

            if (kitapTuru==null)
            {


                return NotFound();


            }

            _kitapTuruRepository.Sil(kitapTuru);
            _kitapTuruRepository.Kaydet();
            TempData["basarili"] = "Kitap Türü Başarıyla Silindi.";
            return RedirectToAction("Index", "KitapTuru");



        }




    }


    //Controller ve Model sınıfı adları aynı olmak zorunda değildir, ancak bu ad benzerliği uygulamanızın kodunu daha anlaşılır hale getirebilir. Önemli olan, Controller sınıflarının doğru şekilde Model sınıflarıyla etkileşimde bulunması ve işlevlerini yerine getirmesidir.
}
