using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{

    
    public class KitapController : Controller
    {



        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;


        //private readonly UygulamaDbContext _uygulamaDbContext; //her seferinde nesne oluşturmak yerine bundan bir tane yapıyoruz ve projemizin başından sonuna kadar kullanıyoruz. DESİGN PATTERN : SİGLETON

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {

            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin, Ogrenci")]
        public IActionResult Index()
        {

            List<Kitap> objKitapList=_kitapRepository.GetAll(includeProps: "KitapTuru").ToList();
           

            return View(objKitapList);
        }

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
            {

                Text = k.Ad,
                Value = k.Id.ToString()

            }); //kitapControllerında kitapTürüRepository kullanıyoruz.

            ViewBag.KitapTuruList = KitapTuruList; //Controllerdan View veri aktarır.

            if (id==null || id==0)
            {
                //Ekleme
                return View();
            }
            else
            {
                //Guncelleme
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Benim buraya eşit olan id getir demektir. buradaki u aslında şuna eşittir. Expression<Func<T, bool>> filtre 


                if (kitapVt == null) //Bu satırda, eğer kitapVt null ise (yani belirtilen id ile kayıt bulunamamışsa), yine NotFound() metodunu kullanarak bir HTTP 404 hata yanıtı döndürülür.
                {

                    return NotFound();

                }
                return View(kitapVt);
            }


        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        [HttpPost] 
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file) //Bu şekilde nesne buraya geldi ama bu nesneyi EntityFramework kullanarak veritabanına kaydediyoruz.
        {

            

            if (ModelState.IsValid) //KitapControllerın Model içindeki alanlar doğru şekilde gelmiş mi bunu kontrol eder.
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if (file!=null)
                {

              


                using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName),FileMode.Create))
                {

                    file.CopyTo(fileStream);
                }
                kitap.ResimUrl = @"\img\" + file.FileName;

                }


                if (kitap.Id==0)
                {


                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni Kitap Başarıyla Oluşturuldu."; //Bunları post kısmına yazıyoruz sadece

                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Yeni Kitap Güncelleme Başarıyla Oluşturuldu."; 
                }

            _kitapRepository.Kaydet(); //burayı görünce de veritabanına atıyor. Eğer bunu yapmazsak bilgiler veri tabanına eklenmez.

            

            return RedirectToAction("Index", "Kitap"); //Bu şekilde yazdığımız zaman veritabanına yeni kayıt ekledikten sonra yukarıdaki Index actionu çağıracaktır.

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
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {

            if (id == null || id == 0) 
            {


                return NotFound();

            }
            Kitap? kitapVt = _kitapRepository.Get(u=>u.Id==id); 

            if (kitapVt == null) 
            {

                return NotFound();

            }


            return View(kitapVt); 


        }

        [Authorize(Roles = UserRoles.Role_Admin)]

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {

            Kitap? kitap = _kitapRepository.Get(u=>u.Id==id);

            if (kitap==null)
            {


                return NotFound();


            }

            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["basarili"] = "Kitap Başarıyla Silindi.";
            return RedirectToAction("Index", "Kitap");



        }




    }


    //Controller ve Model sınıfı adları aynı olmak zorunda değildir, ancak bu ad benzerliği uygulamanızın kodunu daha anlaşılır hale getirebilir. Önemli olan, Controller sınıflarının doğru şekilde Model sınıflarıyla etkileşimde bulunması ve işlevlerini yerine getirmesidir.
}
