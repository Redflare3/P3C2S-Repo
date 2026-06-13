using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using tapalosa.Models;

namespace tapalosa.Controllers
{
    [ApiController]
    public class KprControllers : ControllerBase
    {
        private static Repository<rumah> rumahRepo = new Repository<rumah>("rumah.json");
        private static Repository<PengajuanKpr> pengajuanKprRepo = new Repository<PengajuanKpr>("pengajuanKpr.json");

        private static List<User> userlist = new()
        {
            new User { idUser = 1, nama = "Raissa", nomortelp = "081234567890", role = "admin" },
            new User { idUser = 2, nama = "Dina", nomortelp = "081234567891", role = "customer" }
        };

        [HttpPost("pengajuankpr/{role}/{nama}")]
        public ActionResult<PengajuanKpr> AddPengajuanKpr(string role, string nama, [FromBody] PengajuanKpr pengajuan)
        {
            if (role == "admin")
            {
                pengajuanKprRepo.Add(pengajuan);
                return Ok(pengajuan);
            }
            else
            {
                return BadRequest("Hanya admin yang dapat menambahkan pengajuan KPR.");
            }
        }

        [HttpGet("rumah/{role}/{nama}")]
        public ActionResult<List<rumah>> Get(string role, string nama)
        {
            if (role == "admin")
            {
                return Ok(rumahRepo.GetAll());
            }
            else if (role == "customer")
            {
                var user = userlist.Find(u => u.nama == nama && u.role == "customer");
                if (user == null)
                {
                    return NotFound("User tidak ditemukan atau bukan customer.");
                }

                var pengajuanKpr = pengajuanKprRepo.GetAll().Where(p => p.idUser == user.idUser).ToList();
                var rumah = rumahRepo.GetAll().Where(r => pengajuanKpr.Any(p => p.idRumah == r.idRumah)).ToList();
                return Ok(rumah);
            }
            else
            {
                return BadRequest("Role tidak valid.");
            }
        }

        [HttpGet("pengajuankpr/{role}/{nama}")]
        public ActionResult<List<PengajuanKpr>> GetPengajuanKpr(string role, string nama)
        {
            if (role == "admin")
            {
                return Ok(pengajuanKprRepo.GetAll());
            }
            else if (role == "customer")
            {
                var user = userlist.Find(u => u.nama == nama && u.role == "customer");
                if (user == null)
                {
                    return NotFound("User tidak ditemukan atau bukan customer.");
                }

                var pengajuanKpr = pengajuanKprRepo.GetAll().Where(p => p.idUser == user.idUser).ToList();
                return Ok(pengajuanKpr);
            }
            else
            {
                return BadRequest("Role tidak valid.");
            }
        }     

        [HttpDelete("pengajuankpr/{role}/{nama}/{idPengajuan}")]
        public ActionResult DeletePengajuanKpr(string role, string nama, int idPengajuan)
        {
            if (role == "admin")
            {
                var pengajuan = pengajuanKprRepo.GetAll().Find(p => p.idPengajuan == idPengajuan);
                if (pengajuan == null)
                {
                    return NotFound("Pengajuan KPR tidak ditemukan.");
                }

                pengajuanKprRepo.Remove(pengajuan);
                return Ok();
            }
            else
            {
                return BadRequest("Hanya admin yang dapat menghapus pengajuan KPR.");
            }
        }
    }
}
