namespace TapalosaCommon.Models
{
    public class Rumah
    {
        public int idRumah { get; set; }
        public string blok { get; set; } = string.Empty;
        public double harga { get; set; }
        public bool statusKetersediaan { get; set; }
    }
}
