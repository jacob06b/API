using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace CountryAPI;
//💀
class Program
{
    static async Task Main(string[] args)
    {
        

        var config = new ConfigurationBuilder().AddJsonFile("/Users/jacobbramwell/Documents/GitHub/CountriesAPI/appSettings.json").Build();
        string baseApi = config["BaseApi"] ?? string.Empty;

        

        Console.WriteLine("What country:");
        string name = Console.ReadLine();
        var loader = new CountryLoader(baseApi);
        var temp = await loader.LoadNamesDataAsync(name);
        bool Continue = true;
        while (Continue)
        {
            Console.WriteLine("What language: ");
            string lang = Console.ReadLine();
            
            

        }


    }
}
public class CountryLoader
{
    private readonly string _baseUrl;
    
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CountryLoader(string baseUrl)
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl;
        
    }


    public async Task<CountryDto> LoadNamesDataAsync(string name)
    {
        string url = $"{_baseUrl}name/{name}";
        Console.WriteLine(url);
        CountryDto result = new();
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<CountryDto>(responseBody, _options) ?? new();
        }
        
       return result;
    }

    private async Task<Dictionary<string, string>> LoadCountryInfoAsync(string name)
    {
        string url = $"{_baseUrl}{name}";
        Dictionary<string, string> result = new Dictionary<string, string>();
        using (var client = new HttpClient())
        { 
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody, _options) ?? new();

        }
        return result;
    }

    
    

}
public class CountryDto
{
    public Dictionary<string,string> name { get; set; }
    public string tld { get; set; }
    public string cca2 { get; set; }

    public CountryDto()
    {
        name = new Dictionary<string, string>();
        tld = string.Empty;
        cca2 = string.Empty;
  
    }
}


