
using System.Net.Http.Headers;
using WebAPI.Services.Interfaces;
using WebClient.DTOs;

namespace WebClient.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string ApiUrl = "https://localhost:7147/api/Products";

        public ProductService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

        }


         private void AddJwtHeader()
         {
             var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
             if (!string.IsNullOrEmpty(token))
             {
                 _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
             }
             //throw new UnauthorizedAccessException("You must log in before accessing.");
         }



        public async Task<IEnumerable<ReadProductDTO>> GetAllAsync()
        {
            //AddJwtHeader();
            var response = await _httpClient.GetAsync(ApiUrl);
            response.EnsureSuccessStatusCode();

            var Products = await response.Content.ReadFromJsonAsync<List<ReadProductDTO>>();
            return Products ?? new List<ReadProductDTO>();

        }

        public async Task<ReadProductDTO?> GetByIdAsync(Guid id)
        {
            AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<ReadProductDTO>($"{ApiUrl}/{id}");
        }



        public async Task CreateAsync(CreateProductDTO dto)
        {
            AddJwtHeader();
            await _httpClient.PostAsJsonAsync(ApiUrl, dto);
        }

        public async Task UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            AddJwtHeader();
            await _httpClient.PutAsJsonAsync($"{ApiUrl}/{id}", dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            AddJwtHeader();
            await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
        }

        public async Task<string> GetRawAsync(string format = "json")
        {
            var accept = format.ToLower() switch
            {
                "xml" => "application/xml",
                "csv" => "text/csv",
                _ => "application/json"
            };
            // Request the Products endpoint (use the ApiUrl constant) so the correct route is called.
            var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(accept));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }


}
