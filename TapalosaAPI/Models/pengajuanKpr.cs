namespace TapalosaAPI.Models
{
    public class PengajuanKpr
    {
        public int idPengajuan { get; set; }
        public int idUser { get; set; }
        public int idRumah { get; set; }
        public DateTime tanggalPengajuan { get; set; }
        public DateTime tanggalUpdate { get; set; }
        public string statusPengajuan { get; set; } 
    }
}
