using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TapalosaCommon.Models;

namespace TapalosaCLI
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            string backendUrl = configuration["ApiSettings:BackendUrl"] ?? "http://localhost:5064";

            _client = new HttpClient { BaseAddress = new Uri(backendUrl) };
        }

        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var request = new { username, password };
                var response = await _client.PostAsJsonAsync("/tapalosaapi/user/login", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login Gagal: {response.StatusCode} - {errorMsg}");
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error login: {ex.Message}");
                return null;
            }
        }

        public async Task<Rumah?> GetRumahByIdAsync(int idRumah)
        {
            try
            {
                var response = await _client.GetAsync($"/rumah/rumah/{idRumah}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Rumah>();
                }
                Console.WriteLine($"Gagal mendapatkan rumah: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> TambahRumahAsync(Rumah rumah)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("/rumah/rumah", rumah);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error tambah rumah: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> TambahPengajuanKprAsync(string adminName, PengajuanKpr pengajuan)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"/pengajuanKpr/{adminName}", pengajuan);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error pengajuan KPR: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateStatusKprAsync(string adminName, int idPengajuan, string aksi)
        {
            try
            {
                var response = await _client.PutAsync($"/pengajuanKpr/{adminName}/{idPengajuan}?aksi={aksi}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error update status KPR: {ex.Message}");
                return false;
            }
        }

        public async Task<List<PengajuanKpr>?> GetAllPengajuanKprAsync()
        {
            try
            {
                return await _client.GetFromJsonAsync<List<PengajuanKpr>>("/pengajuanKpr");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching pengajuan KPR: {ex.Message}");
                return null;
            }
        }

        public async Task<Rumah?> GetRumahByPengajuanIdAsync(int idPengajuan)
        {
            try
            {
                var response = await _client.GetAsync($"/rumah/pengajuan/{idPengajuan}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Rumah>();
                }
                Console.WriteLine($"Gagal mendapatkan rumah untuk pengajuan: {response.ReasonPhrase}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> HapusPengajuanKprAsync(int idPengajuan)
        {
            try
            {
                var response = await _client.DeleteAsync($"/pengajuanKpr/{idPengajuan}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error hapus pengajuan KPR: {ex.Message}");
                return false;
            }
        }
    }

    public class LoginResponse
    {
        public string message { get; set; } = string.Empty;
        public int idUser { get; set; }
        public string nama { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
    }
}
