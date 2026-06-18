using System;
using System.Collections.Generic;

namespace KatalogRumahLib
{
    public class KatalogRumah
    {
        private static KatalogRumah? _instance;
        public static KatalogRumah Instance => _instance ??= new KatalogRumah();
        private static readonly Dictionary<string, string> TabelKatalog = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Gold", "Tipe 36/72 - Harga: Rp500jt - Spesifikasi: 2 Kamar Tidur, 1 Kamar Mandi" },
            { "Platinum", "Tipe 45/90 - Harga: Rp750jt - Spesifikasi: 3 Kamar Tidur, 1 Kamar Mandi" },
            { "Diamond", "Tipe 60/120 - Harga: Rp1.2M - Spesifikasi: 4 Kamar Tidur, 2 Kamar Mandi" }
        };

        public string CariInformasiRumah(string kategori)
        {
            if (string.IsNullOrEmpty(kategori))
            {
                return "Kategori rumah tidak boleh kosong.";
            }

            if (TabelKatalog.TryGetValue(kategori, out string? info) && info != null)
                return info;

            return "Kategori rumah tidak ditemukan.";
        }
    }
}
