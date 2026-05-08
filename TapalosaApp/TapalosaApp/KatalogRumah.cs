using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TapalosaApp
{
    internal class KatalogRumah
    {
        private static readonly Dictionary<string, string> TabelKatalog = new Dictionary<string, string>
        {
            { "Gold", "Tipe 36/72 - Harga: Rp500jt - Spesifikasi: 2 Kamar Tidur, 1 Kamar Mandi" },
            { "Platinum", "Tipe 45/90 - Harga: Rp750jt - Spesifikasi: 3 Kamar Tidur, 1 Kamar Mandi" },
            { "Diamond", "Tipe 60/120 - Harga: Rp1.2M - Spesifikasi: 4 Kamar Tidur, 2 Kamar Mandi" }
        };
        public string CariInformasiRumah(string kategori)
        {
            Debug.Assert(!string.IsNullOrEmpty(kategori), "Kategori rumah tidak boleh kosong");
            if (TabelKatalog.ContainsKey(kategori))
            {
                return TabelKatalog[kategori];
            }

            return "Kategori rumah tidak ditemukan.";
        }
        }
}
