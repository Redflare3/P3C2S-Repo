using System;

namespace TapalosaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEM MANAJEMEN TAPALOSA GROUP ===");
            Console.WriteLine("1. Cari Informasi Katalog Rumah");
            Console.WriteLine("2. Cari Tahapan KPR Bank");
            Console.Write("Pilih menu (1/2): ");

            string menu = Console.ReadLine();

            if (menu == "1")
            {
                KatalogRumah katalog = new KatalogRumah();
                Console.WriteLine("\n======== TEST FITUR KATALOG RUMAH ========");
                Console.WriteLine("Pilihan Kategori Tersedia: Gold, Platinum, Diamond");
                Console.WriteLine("--------------------------------------------------");

                Console.Write("Ketik kategori rumah yang ingin dicari: ");
                string inputKategori = Console.ReadLine();
                Console.WriteLine("\n--- Hasil Pencarian ---");
                string hasil = katalog.CariInformasiRumah(inputKategori);
                Console.WriteLine(hasil);
            }
            else if (menu == "2")
            {
                KprManager kprManager = new KprManager();
                Console.WriteLine("\n======== FITUR TAHAPAN KPR BANK ========");
                Console.WriteLine("Bank Tersedia: BCA, Mandiri, BNI");
                Console.WriteLine("----------------------------------------");
                Console.Write("Ketik nama bank: ");

                string inputBank = Console.ReadLine();

                try
                {
                    InfoBankKPR info = kprManager.CariTahapanBank(inputBank);

                    Console.WriteLine("\n--- Hasil Pencarian ---");
                    Console.WriteLine($"Nama Bank  : {info.NamaBank}");
                    Console.WriteLine($"Suku Bunga : {info.SukuBunga}%");
                    Console.WriteLine("Tahapan KPR:");

                    foreach (string tahap in info.Tahapan)
                    {
                        Console.WriteLine($"  {tahap}");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n[ERROR INPUT] {ex.Message}");
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine($"\n[DATA TIDAK DITEMUKAN] {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nMenu tidak valid.");
            }

            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("Tekan tombol apa saja untuk keluar...");
            Console.ReadKey();
        }
    }
}