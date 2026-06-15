using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using TapalosaAPI.Models;

namespace TapalosaAPI.Controllers
{
    [Route("tapalosaapi")]
    [ApiController]
    public class TapalosaController:ControllerBase
    {
        public static List<rumah> rumahlist = new List<rumah>
        {
            new rumah { idRumah = 101, blok = "Blok A-1", harga = "500000000", statusKetersediaan = 1 },
            new rumah { idRumah = 102, blok = "Blok A-2", harga = "450000000", statusKetersediaan = 1 },
            new rumah { idRumah = 103, blok = "Blok B-1", harga = "420000000", statusKetersediaan = 0 },
            new rumah { idRumah = 104, blok = "Blok C-1", harga = "600000000", statusKetersediaan = 1 },
            new rumah { idRumah = 105, blok = "Blok D-1", harga = "350000000", statusKetersediaan = 0 }
        };

        public static List<user> userList = new List<user>
        {
            new user { idUser = 1, nama = "Andi", role = "admin", status = "Aktif" },
            new user { idUser = 2, nama = "Budi", role = "admin", status = "Aktif" },
            new user { idUser = 3, nama = "Citra", role = "customer", status = "Nonaktif" },
            new user { idUser = 4, nama = "Dinda", role = "customer", status = "Aktif" },
            new user { idUser = 5, nama = "Eko", role = "customer", status = "Aktif" }
        };

        public static List<pengajuanKpr> pengajuanList = new List<pengajuanKpr>
        {
            new pengajuanKpr { idPengajuan = 1001, idUser = 3, idRumah = 101, tanggalPengajuan = DateTime.Parse("2026-06-01"), tanggalUpdate = DateTime.Parse("2026-06-04"), statusPengajuan = "Diproses" },
            new pengajuanKpr { idPengajuan = 1002, idUser = 4, idRumah = 102, tanggalPengajuan = DateTime.Parse("2026-06-02"), tanggalUpdate = DateTime.Parse("2026-06-05"), statusPengajuan = "Pending" },
            new pengajuanKpr { idPengajuan = 1003, idUser = 5, idRumah = 103, tanggalPengajuan = DateTime.Parse("2026-06-03"), tanggalUpdate = DateTime.Parse("2026-06-06"), statusPengajuan = "Disetujui" },
            new pengajuanKpr { idPengajuan = 1004, idUser = 4, idRumah = 104, tanggalPengajuan = DateTime.Parse("2026-06-04"), tanggalUpdate = DateTime.Parse("2026-06-07"), statusPengajuan = "Ditolak" },
            new pengajuanKpr { idPengajuan = 1005, idUser = 5, idRumah = 105, tanggalPengajuan = DateTime.Parse("2026-06-05"), tanggalUpdate = DateTime.Parse("2026-06-08"), statusPengajuan = "Diproses" }
        };

        [HttpGet("rumah/{role}/{nama}")]
        public ActionResult<List<rumah>> Get(string role, string nama)
        {
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
                    return NotFound("User tidak ditemukan");
                }

                var pengajuanuser = pengajuanList.Where(p => p.idUser == userx.idUser); 

                var rumahuser = rumahlist.Where(r => pengajuanuser.Any(p => p.idRumah == r.idRumah)); 
            
                return Ok(rumahuser);  
            }

            else
            {
                return BadRequest("Role tidak valid");
            }
               

        }

        [HttpGet("rumah/{idRumah}")]
        public ActionResult<rumah> GetByIndex(int idRumah)
        {
            var rumah = rumahlist.Find(r => r.idRumah == idRumah);

            if (rumah == null)
                return NotFound("Rumah tidak ditemukan");

            else 
                return Ok(rumah);
            
        }

        [HttpPost("rumah")]
        public ActionResult<rumah> Post(rumah addRumah)
        {
            if (addRumah == null)
            {
                return BadRequest("Data rumah yang dimasukkan tidak valid");
            }

            if (rumahlist.Any(r => r.idRumah == addRumah.idRumah))
            {
                return Conflict("Sudah ada rumah dengan ID yang sama");
            }
                
            rumahlist.Add(addRumah);
            return Ok(addRumah);
        }

        [HttpPut("rumah/{idRumah}")]
        public ActionResult<rumah> Put(int idRumah, rumah updateRumah)
        {
            var rumah = rumahlist.Find(r => r.idRumah == idRumah);

            if (rumah == null)
            {
                return NotFound("Rumah tidak ditemukan");
            }

            rumah.statusKetersediaan = updateRumah.statusKetersediaan;
            return Ok(rumah);
        }

        [HttpDelete("rumah/{idRumah}")]
        public ActionResult Delete(int idRumah)
        {
            var rumah = rumahlist.Find(r => r.idRumah == idRumah);

            if (rumah == null)
            {
                return NotFound("Rumah tidak ditemukan");
            }

            rumahlist.Remove(rumah);
            return Ok($"rumah dengan id {idRumah} berhasil dihapus");
        }
    }
}
