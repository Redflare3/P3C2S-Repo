using System;

namespace TapalosaApp
{
    public class InfoBankKPR
    {
        public string NamaBank { get; set; }
        public double SukuBunga { get; set; }
        public string[] Tahapan { get; set; }
    }

    public class KprManager
    {
        private readonly Dictionary<string, InfoBankKPR> _tabelBank;

        public KprManager()
        {
            _tabelBank = new Dictionary<string, InfoBankKPR>(StringComparer.OrdinalIgnoreCase)
            {
                { "BCA", new InfoBankKPR {
                    NamaBank = "BCA",
                    SukuBunga = 4.5,
                    Tahapan = new[] { "1. Isi Form", "2. BI Checking", "3. Appraisal", "4. Akad" }
                }},
                { "Mandiri", new InfoBankKPR {
                    NamaBank = "Mandiri",
                    SukuBunga = 5.1,
                    Tahapan = new[] { "1. Syarat Dokumen", "2. Verifikasi Data", "3. Wawancara", "4. Akad" }
                }},
                { "BNI", new InfoBankKPR {
                    NamaBank = "BNI",
                    SukuBunga = 4.7,
                    Tahapan = new[] { "1. Pengajuan", "2. Survei Lokasi", "3. Persetujuan", "4. Pencairan" }
                }}
            };
        }

        public InfoBankKPR CariTahapanBank(string namaBank)
        {
            if (string.IsNullOrWhiteSpace(namaBank))
            {
                throw new ArgumentException("Pre-condition failed: Nama bank tidak boleh kosong.");
            }

            bool dataDitemukan = _tabelBank.TryGetValue(namaBank, out InfoBankKPR info);

            if (!dataDitemukan)
            {
                throw new KeyNotFoundException($"Post-condition failed: Bank '{namaBank}' tidak terdaftar.");
            }

            return info;
        }
    }
}
