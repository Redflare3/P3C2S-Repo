using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using static Microsoft.AspNetCore.Http.Results;
using TapalosaCommon;
using TapalosaCommon.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();

var userRepo = Repository<User>.GetInstance(GetDatabasePath("User", "database/user.json"));
var rumahRepo = Repository<Rumah>.GetInstance(GetDatabasePath("Rumah", "database/rumah.json"));
var pengajuanRepo = Repository<PengajuanKpr>.GetInstance(GetDatabasePath("PengajuanKpr", "database/pengajuanKpr.json"));

string GetDatabasePath(string key, string fallback)
{
    return app.Configuration[$"DatabasePaths:{key}"] ?? fallback;
}

app.MapPost("/tapalosaapi/user/login", (LoginRequest request) =>
{
    if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.password))
    {
        return BadRequest(new { message = "Username dan password wajib diisi" });
    }

    var users = userRepo.GetAll();
    var user = users.FirstOrDefault(u =>
        u.username.Equals(request.username, StringComparison.OrdinalIgnoreCase) &&
        u.password == request.password);

    if (user == null)
    {
        return Json(new { message = "Username atau password salah" }, statusCode: 401);
    }

    if (user.status.Equals("Nonaktif", StringComparison.OrdinalIgnoreCase))
    {
        return Json(new { message = "Akun Anda dinonaktifkan. Silakan hubungi admin." }, statusCode: 401);
    }

    return Ok(new
    {
        message = "Login Berhasil",
        idUser = user.idUser,
        nama = user.nama,
        role = user.role,
        status = user.status,
        username = user.username
    });
});

app.MapGet("/rumah/rumah", () =>
{
    return Ok(rumahRepo.GetAll());
});

app.MapGet("/rumah/rumah/{idRumah:int}", (int idRumah) =>
{
    var rumahList = rumahRepo.GetAll();
    var rumah = rumahList.FirstOrDefault(r => r.idRumah == idRumah);

    if (rumah == null)
    {
        return NotFound("Rumah tidak ditemukan");
    }
    return Ok(rumah);
});

app.MapPost("/rumah/rumah", (Rumah addRumah) =>
{
    if (addRumah == null)
    {
        return BadRequest("Data rumah yang dimasukkan tidak valid");
    }

    var rumahList = rumahRepo.GetAll();
    if (rumahList.Any(r => r.idRumah == addRumah.idRumah))
    {
        return Conflict("Sudah ada rumah dengan ID yang sama");
    }

    rumahRepo.Add(addRumah);
    return Ok(addRumah);
});

app.MapPut("/rumah/rumah/{idRumah:int}", (int idRumah, Rumah updateRumah) =>
{
    var rumahList = rumahRepo.GetAll();
    var rumah = rumahList.FirstOrDefault(r => r.idRumah == idRumah);

    if (rumah == null)
    {
        return NotFound("Rumah tidak ditemukan");
    }

    rumah.statusKetersediaan = updateRumah.statusKetersediaan;
    rumahRepo.Save(rumahList);
    return Ok(rumah);
});

app.MapDelete("/rumah/rumah/{idRumah:int}", (int idRumah) =>
{
    var rumahList = rumahRepo.GetAll();
    var rumah = rumahList.FirstOrDefault(r => r.idRumah == idRumah);

    if (rumah == null)
    {
        return NotFound("Rumah tidak ditemukan");
    }

    rumahList.Remove(rumah);
    rumahRepo.Save(rumahList);
    return Ok($"Rumah dengan id {idRumah} berhasil dihapus");
});

app.MapGet("/pengajuanKpr", () =>
{
    return Ok(pengajuanRepo.GetAll());
});

app.MapGet("/pengajuanKpr/{idPengajuan:int}", (int idPengajuan) =>
{
    var pengajuanList = pengajuanRepo.GetAll();
    var pengajuan = pengajuanList.FirstOrDefault(p => p.idPengajuan == idPengajuan);

    if (pengajuan == null)
    {
        return NotFound("Pengajuan KPR tidak ditemukan");
    }
    return Ok(pengajuan);
});

app.MapDelete("/pengajuanKpr/{idPengajuan:int}", (int idPengajuan) =>
{
    var pengajuanList = pengajuanRepo.GetAll();
    var pengajuan = pengajuanList.FirstOrDefault(p => p.idPengajuan == idPengajuan);

    if (pengajuan == null)
    {
        return NotFound("Pengajuan KPR tidak ditemukan");
    }

    pengajuanList.Remove(pengajuan);
    pengajuanRepo.Save(pengajuanList);
    return Ok(new { message = $"Pengajuan KPR dengan id {idPengajuan} berhasil dihapus." });
});

app.MapGet("/pengajuanKpr/{role}/{nama}", (string role, string nama) =>
{
    var user = userRepo.GetAll()
        .FirstOrDefault(u => u.role.Equals(role, StringComparison.OrdinalIgnoreCase)
                           && u.nama.Equals(nama, StringComparison.OrdinalIgnoreCase));

    if (user == null)
    {
        return NotFound("User tidak ditemukan");
    }

    var pengajuanList = pengajuanRepo.GetAll()
        .Where(p => p.idUser == user.idUser)
        .ToList();

    return Ok(pengajuanList);
});

app.MapGet("/rumah/pengajuan/{idPengajuan:int}", (int idPengajuan) =>
{
    var pengajuanList = pengajuanRepo.GetAll();
    var pengajuan = pengajuanList.FirstOrDefault(p => p.idPengajuan == idPengajuan);

    if (pengajuan == null)
    {
        return NotFound("Pengajuan KPR tidak ditemukan");
    }

    var rumahList = rumahRepo.GetAll();
    var rumah = rumahList.FirstOrDefault(r => r.idRumah == pengajuan.idRumah);

    if (rumah == null)
    {
        return NotFound("Rumah untuk pengajuan ini tidak ditemukan");
    }

    return Ok(rumah);
});

app.MapPost("/pengajuanKpr/{admin}", (string admin, PengajuanKpr addPengajuan) =>
{
    if (addPengajuan == null)
    {
        return BadRequest("Data pengajuan yang ditambahkan tidak valid");
    }

    var pengajuanList = pengajuanRepo.GetAll();
    if (pengajuanList.Any(p => p.idPengajuan == addPengajuan.idPengajuan))
    {
        return Conflict("ID pengajuan KPR sudah ada!");
    }

    pengajuanRepo.Add(addPengajuan);
    return Ok(addPengajuan);
});

app.MapPut("/pengajuanKpr/{admin}/{idPengajuan:int}", (string admin, int idPengajuan, string aksi) =>
{
    var pengajuanList = pengajuanRepo.GetAll();
    var pengajuan = pengajuanList.FirstOrDefault(p => p.idPengajuan == idPengajuan);

    if (pengajuan == null)
    {
        return NotFound("Pengajuan KPR tidak ditemukan");
    }

    var userAdmin = userRepo.GetAll().FirstOrDefault(x => x.nama.Equals(admin, StringComparison.OrdinalIgnoreCase) && x.role.Equals("admin", StringComparison.OrdinalIgnoreCase));
    if (userAdmin == null)
    {
        return NotFound("Admin tidak ditemukan");
    }

    string currentStateStr = pengajuan.statusPengajuan;

    if (!KprStateMachine.TryTransition(currentStateStr, aksi, out string nextStateStr))
    {
        return BadRequest($"Transisi status tidak valid! Anda tidak bisa melakukan aksi '{aksi}' pada status '{pengajuan.statusPengajuan}' saat ini.");
    }

    pengajuan.statusPengajuan = nextStateStr;
    pengajuan.tanggalUpdate = DateTime.Now;

    pengajuanRepo.Save(pengajuanList);
    return Ok(pengajuan);
});

app.Run("http://localhost:5064");

public record LoginRequest(string username, string password);
