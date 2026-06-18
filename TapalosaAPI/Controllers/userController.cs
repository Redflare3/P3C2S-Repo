using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TapalosaAPI.Models;

namespace TapalosaAPI.Controllers
{
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly Repository<User> _userRepo = new Repository<User>("Database/user.json");

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var userlist = _userRepo.GetAll();
            var userLogin = userlist.Find(u => u.username.Equals(loginRequest.username) && u.password == loginRequest.password);
            
            if (userLogin == null)
            {
                return Unauthorized(new {message = "Username atau Password yang dimasukkan salah"});
            }

            if (userLogin.status.Equals("Nonaktif"))
            {
                return Unauthorized("Akun anda Nonaktif. Silahkan menghubungi admin");
            }

            return Ok($"User {userLogin.username} berhasil login!");
        }

    }

    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
