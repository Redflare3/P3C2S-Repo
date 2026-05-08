using System;

namespace TapalosaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            KatalogRumah katalog = new KatalogRumah();

            Console.WriteLine("======== TEST FITUR KATALOG RUMAH ========");
            Console.WriteLine("Pilihan Kategori Tersedia: Gold, Platinum, Diamond");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Ketik kategori rumah yang ingin dicari: ");
            string inputKategori = Console.ReadLine();
            Console.WriteLine("\n--- Hasil Pencarian ---");
            string hasil = katalog.CariInformasiRumah(inputKategori);
            Console.WriteLine(hasil);

            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("Tekan tombol apa saja untuk keluar...");
            Console.ReadKey();


        }
    }
}