using Microsoft.AspNetCore.Mvc;
using TapalosaAPI.Models;

namespace TapalosaAPI.Controllers
{
    [ApiController]
    public class pengajuanKprController : ControllerBase
    {
        private readonly Repository<User> _userRepo = new Repository<User>("database/user.json");
        private readonly Repository<PengajuanKpr> _pengajuanRepo = new Repository<PengajuanKpr>("database/pengajuanKpr.json");

        [HttpGet("pengajuanKpr/{role}/{nama}")]
        public ActionResult<List<PengajuanKpr>> Get(string role, string nama)
        {
            var userList = _userRepo.GetAll();
            var pengajuanList = _pengajuanRepo.GetAll();

            if (role == "admin")
            {
                var userAdmin = userList.Find(x => x.nama.Equals(nama, StringComparison.OrdinalIgnoreCase) && x.role == "admin");

                if (userAdmin == null)
                    return NotFound("Admin tidak ditemukan");

                return Ok(pengajuanList);
            }

            else if (role == "customer")
            {
                var userx = userList.Find(x => x.nama == nama && x.role == "customer");
                if (userx == null)
                {
                    return NotFound("Customer tidak ditemukan");
                }

                var pengajuanuser = pengajuanList.Where(p => p.idUser == userx.idUser).ToList();

                return Ok(pengajuanuser);
            }

            else
            {
                return BadRequest("Role tidak valid");
            }
        }

        [HttpGet("pengajuanKpr/{idPengajuan}")]
        public ActionResult<PengajuanKpr> GetByIndex(int idPengajuan)
        {
            var pengajuanList = _pengajuanRepo.GetAll();
            var pengajuan = pengajuanList.Find(p => p.idPengajuan == idPengajuan);

            if (pengajuan == null)
                return NotFound("Pengajuan KPR tidak ditemukan");

            else
                return Ok(pengajuan);
        }

        [HttpPost("pengajuanKpr/{admin}")]
        public ActionResult<PengajuanKpr> AddPengajuanKpr(string admin, [FromBody] PengajuanKpr addPengajuan)
        {
            if (addPengajuan == null)
            {
                return BadRequest("Data pengajuan yang ditambahkan tidak valid");
            }

            var pengajuanList = _pengajuanRepo.GetAll();
            if (pengajuanList.Any(p => p.idPengajuan == addPengajuan.idPengajuan))
            {
                return Conflict("ID pengajuan KPR sudah ada!");
            }

            _pengajuanRepo.Add(addPengajuan);
            return Ok(addPengajuan);

        }

        [HttpPut("pengajuanKpr/{admin}/{idPengajuan}")]
        public ActionResult<PengajuanKpr> UpdatePengajuanKpr(string admin, int idPengajuan, [FromBody] PengajuanKpr updatePengajuan)
        {
            var pengajuanList = _pengajuanRepo.GetAll();
            var pengajuan = pengajuanList.Find(p => p.idPengajuan == idPengajuan);

            if (pengajuan == null)
                return NotFound("Pengajuan KPR tidak ditemukan");

            if (updatePengajuan == null)
                return BadRequest("Data pengajuan yang diperbarui tidak valid");

            var userAdmin = _userRepo.GetAll().Find(x => x.nama.Equals(admin, StringComparison.OrdinalIgnoreCase) && x.role == "admin");
            if (userAdmin == null)
                return NotFound("Admin tidak ditemukan");

            pengajuan.tanggalUpdate = updatePengajuan.tanggalUpdate;
            pengajuan.statusPengajuan = updatePengajuan.statusPengajuan;

            _pengajuanRepo.Save(pengajuanList);
            return Ok(pengajuan);
        }

        [HttpDelete("pengajuanKpr/{admin}/{idPengajuan}")]
        public ActionResult DeletePengajuanKpr(string admin, int idPengajuan)
        {
            var pengajuanList = _pengajuanRepo.GetAll();
            var pengajuan = pengajuanList.Find(p => p.idPengajuan == idPengajuan);

            if (pengajuan == null)
                return NotFound("Pengajuan KPR tidak ditemukan");

            var userAdmin = _userRepo.GetAll().Find(x => x.nama.Equals(admin, StringComparison.OrdinalIgnoreCase) && x.role == "admin");
            if (userAdmin == null)
                return NotFound("Admin tidak ditemukan");

            pengajuanList.Remove(pengajuan);
            _pengajuanRepo.Save(pengajuanList);
            return Ok("Pengajuan KPR berhasil dihapus");
        }
    }
}