using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{

    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {



        private readonly IKiralamaRepository _kiralamaRepository;

        private readonly IKitapRepository _kitapRepository;
        
        private readonly IWebHostEnvironment _webHostEnvironment;


        //private readonly UygulamaDbContext _uygulamaDbContext; //her seferinde nesne oluşturmak yerine bundan bir tane yapıyoruz ve projemizin başından sonuna kadar kullanıyoruz. DESİGN PATTERN : SİGLETON

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
        {

            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            List<Kiralama> objKiralamaList= _kiralamaRepository.GetAll(includeProps: "Kitap").ToList();
           

            return View(objKiralamaList);
        }


        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {

                Text = k.KitapAdi,
                Value = k.Id.ToString()

            }); 

            ViewBag.KitapList = KitapList; //Controllerdan View veri aktarır.

            if (id==null || id==0)
            {
                //Ekleme
                return View();
            }
            else
            {
                //Guncelleme
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); //Benim buraya eşit olan id getir demektir. buradaki u aslında şuna eşittir. Expression<Func<T, bool>> filtre 


                if (kiralamaVt == null) //Bu satırda, eğer kitapVt null ise (yani belirtilen id ile kayıt bulunamamışsa), yine NotFound() metodunu kullanarak bir HTTP 404 hata yanıtı döndürülür.
                {

                    return NotFound();

                }
                return View(kiralamaVt);
            }


        }

        [HttpPost] 
        public IActionResult EkleGuncelle(Kiralama kiralama) //Bu şekilde nesne buraya geldi ama bu nesneyi EntityFramework kullanarak veritabanına kaydediyoruz.
        {

            

            if (ModelState.IsValid) //KitapControllerın Model içindeki alanlar doğru şekilde gelmiş mi bunu kontrol eder.
            {

           

                if (kiralama.Id==0)
                {


                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni Kiralama Başarıyla Oluşturuldu."; //Bunları post kısmına yazıyoruz sadece

                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Yeni Kiralama Güncelleme Başarıyla Oluşturuldu."; 
                }

            _kiralamaRepository.Kaydet(); //burayı görünce de veritabanına atıyor. Eğer bunu yapmazsak bilgiler veri tabanına eklenmez.

            

            return RedirectToAction("Index", "Kiralama"); //Bu şekilde yazdığımız zaman veritabanına yeni kayıt ekledikten sonra yukarıdaki Index actionu çağıracaktır.

            }
            return View(); //Bu kısımda kuralları ihlal ettiğinde ekle sayfasını döndürür tekrar
        }




        //public IActionResult Guncelle(int? id)//eğer id parametresi null olarak gelirse, programın çökmemesini sağlar.
        //{

        //    if (id == null || id == 0) // Bu satırda, id parametresinin değeri null veya 0 ise, işlemin devam edemeyeceği anlamına gelir. Bu nedenle, NotFound() metodunu kullanarak bir HTTP 404 hata yanıtı döndürülür. Bu, belirtilen id değerine sahip bir "Kitap" kaydının bulunamadığını gösterir.
        //    {


        //        return NotFound();

        //    }
        //    Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Benim buraya eşit olan id getir demektir. buradaki u aslında şuna eşittir. Expression<Func<T, bool>> filtre 


        //    if (kitapVt == null) //Bu satırda, eğer kitapVt null ise (yani belirtilen id ile kayıt bulunamamışsa), yine NotFound() metodunu kullanarak bir HTTP 404 hata yanıtı döndürülür.
        //    {

        //        return NotFound();

        //    }


        //    return View(kitapVt); // Eğer yukarıdaki koşullar sağlanmazsa, kitapVt değişkeni bir "Kitap" kaydını temsil eder ve bu kayıt, bir görünüme (View) iletilir. Bu, kullanıcının bu "Kitap" kaydını güncellemek için bir form göreceği sayfanın görünümünün oluşturulmasını başlatır.


        //}







        /*
        [HttpPost]
        public IActionResult Guncelle(Kitap kitap) 
        {

            if (ModelState.IsValid)
            {

                _kitapRepository.Guncelle(kitap);
                _kitapRepository.Kaydet();

                TempData["basarili"] = "Kitap Başarıyla Güncelleştirildi.";

                return RedirectToAction("Index", "Kitap");




            }

            return View();
        }
        */

        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {

                Text = k.KitapAdi,
                Value = k.Id.ToString()

            });

            ViewBag.KitapList = KitapList; //Controllerdan View veri aktarır.

            if (id == null || id == 0) 
            {


                return NotFound();

            }
            Kiralama? kiralamaVt = _kiralamaRepository.Get(u=>u.Id==id); 

            if (kiralamaVt == null) 
            {

                return NotFound();

            }


            return View(kiralamaVt); 


        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {

            Kiralama? kiralama = _kiralamaRepository.Get(u=>u.Id==id);

            if (kiralama==null)
            {


                return NotFound();


            }

            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = "Kiralama Başarıyla Silindi.";
            return RedirectToAction("Index", "Kiralama");



        }




    }


    //Controller ve Model sınıfı adları aynı olmak zorunda değildir, ancak bu ad benzerliği uygulamanızın kodunu daha anlaşılır hale getirebilir. Önemli olan, Controller sınıflarının doğru şekilde Model sınıflarıyla etkileşimde bulunması ve işlevlerini yerine getirmesidir.
}
