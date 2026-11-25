using Microsoft.AspNetCore.Mvc;

namespace Sep2026.MyanmarNameology.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MyanmarNameologyController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://burma-project-ideas.vercel.app/myanmar-nameology";

    public MyanmarNameologyController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("calculate")]
    public async Task<IActionResult> Calculate([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name is required.");

        var response = await _httpClient.GetAsync($"{BaseUrl}/calculate?name={Uri.EscapeDataString(name)}");
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "External API error.");

        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }

    [HttpGet("letter-groups")]
    public async Task<IActionResult> GetLetterGroups()
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/letter-groups");
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "External API error.");

        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }

    [HttpGet("meanings")]
    public async Task<IActionResult> GetMeanings()
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/meanings");
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "External API error.");

        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }

    [HttpGet("meanings/{number:int}")]
    public async Task<IActionResult> GetMeaningByNumber(int number)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/meanings/{number}");
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "External API error.");

        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }
}
