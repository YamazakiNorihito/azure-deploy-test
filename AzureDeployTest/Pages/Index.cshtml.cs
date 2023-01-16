using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AzureDeployTest.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory = null!;


#pragma warning disable CRRSP08 // A misspelled word has been found
        public Todo[]? Todos = Array.Empty<Todo>();
#pragma warning restore CRRSP08 // A misspelled word has been found

        public IndexModel(
        IHttpClientFactory httpClientFactory,
        ILogger<IndexModel> logger) =>
        (_httpClientFactory, _logger) = (httpClientFactory, logger);

        public async Task<IActionResult> OnGetAsync(int userId =1)
        {
            // Create the client
            using HttpClient client = _httpClientFactory.CreateClient();
            try
            {
                // Make HTTP GET request
                // Parse JSON response deserialize into Todo types
                Todos = await client.GetFromJsonAsync<Todo[]>(
                    $"https://jsonplaceholder.typicode.com/todos?userId={userId}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting something fun to say: {Error}", ex);
            }
            return Page();

        }
    }

    public class Todo
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}