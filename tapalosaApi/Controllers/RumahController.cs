using Microsoft.AspNetCore.Mvc;
using tapalosa.models;

namespace tapalosa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RumahController : ControllerBase
    {
        public static List<rumah> rumahlist = new List<rumah>
        {
            new rumah {idRumah = 1, blokNomor ="A1", harga = 175000000, statusKetersediaan = true},
            new rumah {idRumah = 2, blokNomor ="A2", harga = 175000000, statusKetersediaan = true}
        };

        [HttpGet]
        public ActionResult<List<rumah>> Get()
        {
            return rumahlist;
        }

        [HttpGet("{i}")]
        public ActionResult<rumah> GetbyidRumah(int i)
        {
            if (i < 0 || i >= rumahlist.Count())
                return NotFound();

            return rumahlist[i];
        }

        [HttpPost]
        public ActionResult<rumah> addrumah([FromBody] rumah h)
        {
            rumahlist.Add(h);
            return Ok(h);
        }

        [HttpDelete("{i}")]
        public ActionResult deleteRumah(int i)
        {
            if (i < 0 || i >= rumahlist.Count())
                return NotFound();

            rumahlist.RemoveAt(i);
            return Ok();
        }
    }
}
