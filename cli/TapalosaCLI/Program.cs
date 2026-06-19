using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KatalogRumahLib;
using ManageKPRLib;
using TapalosaCommon.Models;

namespace TapalosaCLI
{
    class Program
    {
        private static readonly ApiClient _apiClient = new ApiClient();
        private static string _currentSessionUser = "Andi";
        private static string _currentSessionRole = "admin";

        static async Task Main(string[] args)
        {
            KatalogRumah katalog = new KatalogRumah();
            KprManager kprManager = KprManager.Instance;

            bool running = true;

            while (running)
            {
                Console.WriteLine("1. Detail rumah berdasarkan ID");
                Console.WriteLine("2. Tambah rumah baru");
                Console.WriteLine("3. Tampilkan seluruh pengajuan KPR");
                Console.WriteLine("4. Ajukan KPR Baru");
                Console.WriteLine("5. Update Status Pengajuan KPR");
                Console.WriteLine("6. Hapus pengajuan KPR berdasarkan ID");
                Console.WriteLine("7. Cari Kategori Rumah (Gold/Platinum/Diamond)");
                Console.WriteLine("8. Cari Info Tahapan KPR Bank");
                Console.WriteLine("0. Keluar");
                Console.WriteLine("----------------------------------------------------------");
                Console.Write("Pilih menu (0-8): ");

                string? pil = Console.ReadLine();
                Console.WriteLine();

                switch (pil)
                {
                    case "1":
                        await TampilkanDetailRumah();
                        break;
                    case "2":
                        await TambahRumahBaru();
                        break;
                    case "3":
                        await TampilkanSeluruhPengajuanKpr();
                        break;
                    case "4":
                        await AjukanKprBaru();
                        break;
                    case "5":
                        await ProcessKprStatus();
                        break;
                    case "6":
                        await HapusPengajuanKpr();
                        break;
                    case "7":
                        Console.WriteLine("Masukkan kategori rumah yang ingin dicari (Gold/Platinum/Diamond): ");
                        string inputKategori = Console.ReadLine();
                        Console.WriteLine("\n--- Hasil Pencarian ---");
                        string hasil = katalog.CariInformasiRumah(inputKategori ?? "");
                        Console.WriteLine(hasil);
                        break;
                    case "8":
                        TampilkanInfoBankKPR(kprManager);
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Keluar dari program. Terima kasih!");
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid!");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nTekan tombol apa saja untuk kembali ke menu utama...");
                    SafeReadKey();
                }
            }
        }

        private static void SafeClear()
        {
            try { Console.Clear(); } catch (System.IO.IOException) { }
        }

        private static void SafeReadKey()
        {
            try { Console.ReadKey(); }
            catch (InvalidOperationException)
            {
                Console.WriteLine("[Console redirect detected. Press Enter to continue...]");
                Console.ReadLine();
            }
        }

        private static async Task TampilkanDetailRumah()
        {
            Console.WriteLine("--- Detail Rumah dari API ---");
            Console.Write("Masukkan ID Rumah: ");
            if (int.TryParse(Console.ReadLine(), out int idRumah))
            {
                var rumah = await _apiClient.GetRumahByIdAsync(idRumah);
                if (rumah != null)
                {
                    Console.WriteLine($"ID Rumah    : {rumah.idRumah}");
                    Console.WriteLine($"Blok        : {rumah.blok}");
                    Console.WriteLine($"Harga       : {rumah.harga:C0}");
                    Console.WriteLine($"Ketersediaan: {(rumah.statusKetersediaan ? "Tersedia" : "Tidak Tersedia")}");
                }
                else
                {
                    Console.WriteLine("Rumah tidak ditemukan.");
                }
            }
            else
            {
                Console.WriteLine("ID Rumah tidak valid.");
            }
        }

        private static async Task TampilkanSeluruhPengajuanKpr()
        {
            Console.WriteLine("--- Daftar Seluruh Pengajuan KPR ---");
            var pengajuanList = await _apiClient.GetAllPengajuanKprAsync();
            if (pengajuanList == null || pengajuanList.Count == 0)
            {
                Console.WriteLine("Tidak ada pengajuan KPR atau gagal menghubungi API.");
                return;
            }

            Console.WriteLine(string.Format("{0,-15} | {1,-10} | {2,-10} | {3,-25} | {4,-20}", "ID Pengajuan", "ID User", "ID Rumah", "Status Pengajuan", "Tanggal Update"));
            Console.WriteLine(new string('-', 90));
            foreach (var p in pengajuanList)
            {
                Console.WriteLine(string.Format("{0,-15} | {1,-10} | {2,-10} | {3,-25} | {4,-20}", p.idPengajuan, p.idUser, p.idRumah, p.statusPengajuan, p.tanggalUpdate.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        private static async Task TambahRumahBaru()
        {
            Console.WriteLine("--- Tambah Rumah Baru ---");
            Console.Write("Masukkan ID Rumah baru (Angka): ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID Rumah harus angka!"); return; }

            Console.Write("Masukkan Blok Rumah (Contoh: Blok E-1): ");
            string? blok = Console.ReadLine();

            Console.Write("Masukkan Harga Rumah: ");
            if (!double.TryParse(Console.ReadLine(), out double harga)) { Console.WriteLine("Harga tidak valid!"); return; }

            var rumahBaru = new Rumah { idRumah = id, blok = blok ?? string.Empty, harga = harga, statusKetersediaan = true };

            bool success = await _apiClient.TambahRumahAsync(rumahBaru);
            if (success)
            {
                Console.WriteLine("Rumah baru berhasil ditambahkan via API!");
            }
            else
            {
                Console.WriteLine("Gagal menambahkan rumah (mungkin ID sudah terpakai).");
            }
        }

        private static async Task AjukanKprBaru()
        {
            Console.WriteLine("--- Ajukan KPR Baru ---");
            Console.Write("Masukkan ID Pengajuan Baru (Angka): ");
            if (!int.TryParse(Console.ReadLine(), out int idPengajuan)) return;

            Console.Write("Masukkan ID User Anda: ");
            if (!int.TryParse(Console.ReadLine(), out int idUser)) return;

            Console.Write("Masukkan ID Rumah yang diajukan: ");
            if (!int.TryParse(Console.ReadLine(), out int idRumah)) return;

            var pengajuan = new PengajuanKpr
            {
                idPengajuan = idPengajuan,
                idUser = idUser,
                idRumah = idRumah,
                tanggalPengajuan = DateTime.Now,
                tanggalUpdate = DateTime.Now,
                statusPengajuan = "MenungguPersetujuan"
            };

            bool success = await _apiClient.TambahPengajuanKprAsync(_currentSessionUser, pengajuan);
            if (success)
            {
                Console.WriteLine("Pengajuan KPR baru berhasil dibuat!");
            }
            else
            {
                Console.WriteLine("Gagal mengajukan KPR (mungkin ID sudah ada).");
            }
        }

        private static async Task HapusPengajuanKpr()
        {
            Console.WriteLine("--- Hapus Pengajuan KPR ---");
            Console.Write("Masukkan ID Pengajuan KPR yang akan dihapus: ");
            if (!int.TryParse(Console.ReadLine(), out int idPengajuan))
            {
                Console.WriteLine("ID Pengajuan tidak valid.");
                return;
            }

            bool success = await _apiClient.HapusPengajuanKprAsync(idPengajuan);
            if (success)
            {
                Console.WriteLine($"Pengajuan KPR dengan id {idPengajuan} berhasil dihapus.");
            }
            else
            {
                Console.WriteLine($"Gagal menghapus pengajuan KPR dengan id {idPengajuan}. Pastikan id benar dan backend aktif.");
            }
        }

        private static async Task ProcessKprStatus()
        {
            if (_currentSessionRole != "admin")
            {
                Console.WriteLine("Akses Ditolak! Hanya Admin yang bisa memproses status KPR.");
                return;
            }

            Console.WriteLine("--- Proses Status Pengajuan KPR (Automata System) ---");
            Console.Write("Masukkan ID Pengajuan KPR yang akan diproses: ");
            if (!int.TryParse(Console.ReadLine(), out int idPengajuan)) return;

            Console.WriteLine("Pilih Aksi (Trigger Automata):");
            Console.WriteLine("1. Setujui");
            Console.WriteLine("2. Tolak");
            Console.WriteLine("3. Batalkan");
            Console.Write("Pilihan (1-3): ");
            string? opsi = Console.ReadLine();

            string aksi = opsi switch
            {
                "1" => "Setujui",
                "2" => "Tolak",
                "3" => "Batalkan",
                _ => ""
            };

            if (string.IsNullOrEmpty(aksi))
            {
                Console.WriteLine("Pilihan aksi tidak valid.");
                return;
            }

            bool success = await _apiClient.UpdateStatusKprAsync(_currentSessionUser, idPengajuan, aksi);
            if (success)
            {
                Console.WriteLine($"Sukses! Status KPR berhasil diubah via Automata (Aksi: {aksi}).");
            }
            else
            {
                Console.WriteLine("Gagal memperbarui status KPR. Periksa status saat ini dan aturan transisi Automata.");
            }
        }

        private static void TampilkanInfoBankKPR(KprManager manager)
        {
            Console.WriteLine("--- Cari Informasi KPR Bank ---");
            Console.Write("Ketik nama bank (contoh: BCA, Mandiri, BNI): ");
            string? bankInput = Console.ReadLine();

            try
            {
                var info = manager.CariTahapanBank(bankInput ?? "");
                Console.WriteLine($"\n[Data Ditemukan]");
                Console.WriteLine($"Bank       : {info.NamaBank}");
                Console.WriteLine($"Suku Bunga : {info.SukuBunga}%");
                Console.WriteLine("Tahapan    :");
                foreach (var tahap in info.Tahapan)
                {
                    Console.WriteLine($"  {tahap}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
    }
}
