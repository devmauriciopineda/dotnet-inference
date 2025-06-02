using System;
using System.Formats.Asn1;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using DotNetEnv;

Env.Load();

string? token = Environment.GetEnvironmentVariable("TOKEN");


async Task<string> GetLastChats()
{
    Console.WriteLine($"Consultando últimos chats...");
    string apiUrl = "https://nexusai.integral.com.co/chats";

    using HttpClient client = new HttpClient();

    try
    {
        string responseBody = await client.GetStringAsync(apiUrl);

        Console.WriteLine("Respuesta recibida:");
        Console.WriteLine(responseBody);
        return responseBody;
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine($"Error al enviar la solicitud: {e.Message}");
        return string.Empty;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Ha ocurrido un error inesperado: {e.Message}");
        return string.Empty;
    }
}


async Task<string> AskQuestion(string question)
{
    Console.WriteLine($"Llamando a Nexus con la pregunta: {question}");
    string apiUrl = "https://nexusai.integral.com.co/inference";

    var data = new
    {
        role_id = 2,
        message = new {
            content = question,
            role = "user"
        }
    };

    Console.WriteLine($"Enviando solicitud a: {apiUrl}");

    // Crea una instancia de HttpClient para hacer la solicitud
    using HttpClient client = new HttpClient();
    string json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);       

    var response = await client.PostAsync(apiUrl, content);

    Console.WriteLine("Código de estado: " + response.StatusCode); 
    string answer = await response.Content.ReadAsStringAsync();
    Console.WriteLine("Respuesta recibida:");
    Console.WriteLine(answer);    

    return answer;
}

await GetLastChats();

string question = "¿Cuál es la capital de Francia?";
Console.WriteLine($"Pregunta: {question}");

var answer = await AskQuestion(question);
Console.WriteLine($"Respuesta: {answer}");



