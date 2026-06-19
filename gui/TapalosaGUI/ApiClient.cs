using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TapalosaCommon.Models;

namespace TapalosaGUI
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
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Rumah>?> GetKatalogRumahAsync(string role, string nama)
        {
            try
            {
                return await _client.GetFromJsonAsync<List<Rumah>>("/rumah/rumah");
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Rumah?> GetRumahByIdAsync(int idRumah)
        {
            try
            {
                return await _client.GetFromJsonAsync<Rumah>($"/rumah/rumah/{idRumah}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateRumahStatusAsync(int idRumah, bool statusKetersediaan)
        {
            try
            {
                var rumah = new Rumah { idRumah = idRumah, statusKetersediaan = statusKetersediaan };
                var response = await _client.PutAsJsonAsync($"/rumah/rumah/{idRumah}", rumah);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRumahAsync(int idRumah)
        {
            try
            {
                var response = await _client.DeleteAsync($"/rumah/rumah/{idRumah}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PengajuanKpr?> GetPengajuanKprByIdAsync(int idPengajuan)
        {
            try
            {
                return await _client.GetFromJsonAsync<PengajuanKpr>($"/pengajuanKpr/{idPengajuan}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeletePengajuanKprAsync(int idPengajuan)
        {
            try
            {
                var response = await _client.DeleteAsync($"/pengajuanKpr/{idPengajuan}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Rumah?> GetRumahByPengajuanIdAsync(int idPengajuan)
        {
            try
            {
                return await _client.GetFromJsonAsync<Rumah>($"/rumah/pengajuan/{idPengajuan}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<PengajuanKpr>?> GetPengajuanKprAsync(string role, string nama)
        {
            try
            {
                if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    return await _client.GetFromJsonAsync<List<PengajuanKpr>>("/pengajuanKpr");
                }

                return await _client.GetFromJsonAsync<List<PengajuanKpr>>($"/pengajuanKpr/{role}/{nama}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> TambahPengajuanKprAsync(string adminName, PengajuanKpr pengajuan)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"/pengajuanKpr/{adminName}", pengajuan);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
