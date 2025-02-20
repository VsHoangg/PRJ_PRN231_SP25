using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebClient.Pages.admin
{
    public class SemesterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SemesterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> GetSemesterStatusAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5100/api/Semester/IsSemesterOnGoing");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(jsonString);
            }
            return false;
        }
    }
}
