using Microsoft.AspNetCore.Mvc;
using TapalosaAPI.Models;

namespace TapalosaAPI.Controllers
{
    [ApiController]
    public class rumahController : ControllerBase
    {
        private readonly Repository<Rumah> _rumahRepo = new Repository<Rumah>("database/rumah.json");
        private readonly Repository<User> _userRepo = new Repository<User>("database/user.json");
        private readonly Repository<PengajuanKpr> _pengajuanRepo = new Repository<PengajuanKpr>("database/pengajuanKpr.json");

        [HttpGet("rumah/{role}/{nama}")]
        public ActionResult<List<Rumah>> Get(string role, string nama)
        {
            var userList = _userRepo.GetAll();
            var rumahlist = _rumahRepo.GetAll();
            var pengajuanList = _pengajuanRepo.GetAll();


            if (role == "admin")
            {
                var userAdmin = userList.Find(x => x.nama.Equals(nama, StringComparison.OrdinalIgnoreCase) && x.role == "admin");

                if (userAdmin == null)
                    return NotFound("Admin tidak ditemukan");

                return Ok(rumahlist);
            }

            else if (role == "customer")
            {
                var userx = userList.Find(x => x.nama == nama && x.role == "customer");
                if (userx == null)
                {
                    return NotFound("Customer tidak ditemukan");
                }

                var pengajuanuser = pengajuanList.Where(p => p.idUser == userx.idUser);

                var rumahuser = rumahlist.Where(r => pengajuanuser.Any(p => p.idRumah == r.idRumah)).ToList();

                return Ok(rumahuser);
            }

            else
            {
                return BadRequest("Role tidak valid");
            }
        }

        [HttpGet("rumah/{idRumah}")]
        public ActionResult<Rumah> GetByIndex(int idRumah)
        {
            var rumahlist = _rumahRepo.GetAll();
            var rumah = rumahlist.Find(r => r.idRumah == idRumah);

            if (rumah == null)
                return NotFound("Rumah tidak ditemukan");

            else
                return Ok(rumah);
        }

        [HttpPost("rumah")]
        public ActionResult<Rumah> Post(Rumah addRumah)
        {
            if (addRumah == null)
            {
                return BadRequest("Data rumah yang dimasukkan tidak valid");
            }

            var rumahlist = _rumahRepo.GetAll();
            if (rumahlist.Any(r => r.idRumah == addRumah.idRumah))
            {
                return Conflict("Sudah ada rumah dengan ID yang sama");
            }

            _rumahRepo.Add(addRumah);
            return Ok(addRumah);
        }

        [HttpPut("rumah/{idRumah}")]
        public ActionResult<Rumah> Put(int idRumah, Rumah updateRumah)
        {
            var rumahlist = _rumahRepo.GetAll();
            var rumah = rumahlist.Find(r => r.idRumah == idRumah);

            if (rumah == null)
            {
                return NotFound("Rumah tidak ditemukan");
            }

            rumah.statusKetersediaan = updateRumah.statusKetersediaan;
            _rumahRepo.Save(rumahlist);
            return Ok(rumah);
        }

        [HttpDelete("rumah/{idRumah}")]
        public ActionResult Delete(int idRumah)
        {
            var rumahlist = _rumahRepo.GetAll();
            var rumah = rumahlist.Find(r => r.idRumah == idRumah);

            if (rumah == null)
            {
                return NotFound("Rumah tidak ditemukan");
            }

            rumahlist.Remove(rumah);
            _rumahRepo.Save(rumahlist);
            return Ok($"rumah dengan id {idRumah} berhasil dihapus");
        }
    }
}
