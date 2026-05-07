using System;
using System.Collections.Generic;

class repository<T>
{
    private List<T> entity = new List<T>();

    public void add(T item)
    {
        entity.Add(item);
    }

    public List<T> GetAll() => entity;

    public List<T> filterTersedia()
    {
        List<T> hasilfilter = new List<T>();
        foreach (var item in entity)
        {
            if (item is rumah rumah)
            {
                if (rumah.statusKetersediaan == true)
                {
                    hasilfilter.Add(item);
                }
            }
        }

        return hasilfilter;
    }
}

public class rumah
{
    public int idRumah { get; set; }
    public string blokNomor { get; set; }
    public double harga { get; set; }
    public bool statusKetersediaan { get; set; }
}

class program
{
    static void Main(string[] args)
    {
        var repoTapalosa = new repository<rumah>();
        repoTapalosa.add(new rumah
        {
            idRumah = 1,
            blokNomor = "A1",
            harga = 172000050,
            statusKetersediaan = true
        });
        repoTapalosa.add(new rumah
        {
            idRumah = 2,
            blokNomor = "A2",
            harga = 172000050,
            statusKetersediaan = false
        });

        Console.WriteLine("informasi rumah: ");
        foreach (var item in repoTapalosa.GetAll())
        {
            Console.WriteLine($"Rumah {item.idRumah}, Harga: {item.harga}, Tersedia: {item.statusKetersediaan}");
        }

        Console.WriteLine("rumah tersedia: ");
        foreach (var item in repoTapalosa.filterTersedia())
        {
            Console.WriteLine($"Rumah {item.idRumah}, Harga: {item.harga}");
        }
    }
}