using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using tapalosa.Models;

namespace tapalosa.Controllers
{
    [ApiController]
    [Route("rumah")]
    public class Controllers : ControllerBase
    {
        private static readonly List<rumah> rumahlist = new List<rumah>
        {
            new rumah {idRumah = 1, blokNomor = "A1", harga = 175000000, statusKetersediaan = true},
            new rumah {idRumah = 2, blokNomor = "A2", harga = 175000000, statusKetersediaan = true}
        };

        [HttpGet]
        public ActionResult<List<rumah>> Get()
        {
            return Ok(rumahlist);
        }

        [HttpGet("{i}")]
        public ActionResult<rumah> GetbyidRumah(int i)
        {
            if (i < 0 || i >= rumahlist.Count)
                return NotFound();

            return Ok(rumahlist[i]);
        }

        [HttpPost]
        public ActionResult<rumah> AddRumah([FromBody] rumah h)
        {
            rumahlist.Add(h);
            return Ok(h);
        }

        [HttpDelete("{i}")]
        public IActionResult DeleteRumah(int i)
        {
            if (i < 0 || i >= rumahlist.Count)
                return NotFound();

            rumahlist.RemoveAt(i);
            return Ok();
        }
    }
}
