using System;
using System.Collections.Generic;

namespace ManageKPRLib
{
    public class InfoBankKPR
    {
        public string NamaBank { get; set; } = string.Empty;
        public double SukuBunga { get; set; }
        public string[] Tahapan { get; set; } = Array.Empty<string>();
    }

    public sealed class KprManager
    {
        private static readonly Lazy<KprManager> _instance = new Lazy<KprManager>(() => new KprManager());
        public static KprManager Instance => _instance.Value;

        private readonly Dictionary<string, InfoBankKPR> _tabelBank;

        private KprManager()
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
                throw new ArgumentException("Nama bank tidak boleh kosong.");
            }

            if (!_tabelBank.TryGetValue(namaBank, out InfoBankKPR? info) || info == null)
            {
                throw new KeyNotFoundException($"Bank '{namaBank}' tidak terdaftar di sistem.");
            }

            return info;
        }
    }
}
