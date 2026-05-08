const express = require('express');
const app = express();
const port = 3000;

app.get('/', (req, res) => {
    res.send('Express is running!');
});

app.listen(port, () => {
    console.log(`Server listening at http://localhost:${port}`);
});
using System;
using System.Collections.Generic;

class repository<T>
{
    private List<T> entity = new List<T>();

    public void add(T item) => entity.Add(item);

    public List<T> GetAll() => entity;

    public List<T> filterTersedia()
    {
        List<T> hasilfilter = new List<T>();
        foreach (var item in entity)
        {
            if (item is rumah r && r.status == statusKetersediaan.Tersedia)
            {
                hasilfilter.Add(item);
            }
        }
        return hasilfilter;
    }
    public List<T> filterDibooking()
    {
        List<T> hasilFilter = new List<T>();
        foreach (var item in entity)
        {
            if (item is rumah r && r.status == statusKetersediaan.Dibooking)
            {
                hasilFilter.Add(item);
            }
        }
        return hasilFilter;
    }
    public List<T> filterLunas()
    {
        List<T> hasilFilter = new List<T>();
        foreach (var item in entity)
        {
            if (item is rumah r && r.status == statusKetersediaan.Terjual)
            {
                hasilFilter.Add(item);
            }
        }
        return hasilFilter;
    }

}
public enum statusKetersediaan
{
    Tersedia,
    Dibooking,
    Terjual
}
public enum statusPembayaran
{
    PembatalanPembayaran,
    ProsesCicilan,
    Lunas
}
public class rumah
{
    public int idRumah { get; set; }
    public string blokNomor { get; set; }
    public double harga { get; set; }
    public statusKetersediaan status { get; set; } = statusKetersediaan.Tersedia;
    public void updateStatus(statusPembayaran trigger)
    {
        switch (status)
        {
            case statusKetersediaan.Tersedia:
                if (trigger == statusPembayaran.ProsesCicilan)
                    status = statusKetersediaan.Dibooking;
                break;
            case statusKetersediaan.Dibooking:
                if (trigger == statusPembayaran.PembatalanPembayaran)
                    status = statusKetersediaan.Tersedia;
                else if (trigger == statusPembayaran.Lunas)
                    status = statusKetersediaan.Terjual;
                break;
            case statusKetersediaan.Terjual:
                Console.WriteLine($"Rumah {idRumah} sudah terjual, status tidak bisa diubah lagi.");
                break;
        }
    }

    class program
    {
        static void Main(string[] args)
        {
            var repoTapalosa = new repository<rumah>();
            var rumah1 = new rumah { idRumah = 1, blokNomor = "A1", harga = 172000050 };
            var rumah2 = new rumah { idRumah = 2, blokNomor = "A2", harga = 172000050 };
            repoTapalosa.add(rumah1);
            repoTapalosa.add(rumah2);
            // test logic automata 
            rumah1.updateStatus(statusPembayaran.ProsesCicilan);
            Console.WriteLine($"Status rumah 1 sekarang: {rumah1.status}");
            rumah1.updateStatus(statusPembayaran.Lunas);
            Console.WriteLine($"Status rumah 1 sekarang: {rumah1.status}");
            Console.WriteLine("informasi rumah: ");
            foreach (var item in repoTapalosa.GetAll())
            {
                Console.WriteLine($"Rumah {item.idRumah}, Harga: {item.harga}, Status rumah: {item.status}");
            }
            // list status rumah
            Console.WriteLine("rumah tersedia: ");
            foreach (var item in repoTapalosa.filterTersedia())
            {
                Console.WriteLine($"Rumah {item.idRumah}, Harga: {item.harga}");
            }
            Console.WriteLine("rumah dibooking: ");
            foreach (var item in repoTapalosa.filterDibooking())
            {
                Console.WriteLine($"Rumah {item.idRumah}, Blok Nomor {item.blokNomor}");
            }
            Console.WriteLine("rumah lunas: ");
            foreach (var item in repoTapalosa.filterLunas())
            {
                Console.WriteLine($"Rumah {item.idRumah}, Blok Nomor {item.blokNomor}");
            }
        }
    }
}