using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DotNetEnv;

Env.Load();
string? token = Environment.GetEnvironmentVariable("TOKEN");

string question = "¿Cuál es la capital de Francia?";
Console.WriteLine($"Pregunta: {question}");

var answer = await AskQuestion(question);
Console.WriteLine($"Respuesta: {answer}");


async Task<string> AskQuestion(string question)
{
    string apiUrl = "https://nexusai.integral.com.co/inference";

    var data = new RequestData
    {
        role_id = 2,
        message = new Message
        {
            content = question,
            role = "user"
        }
    };

    using HttpClient client = new HttpClient();
    string json = JsonSerializer.Serialize(data);
    var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    try
    {
        Console.WriteLine("Enviando solicitud a la API...");
        var response = await client.PostAsync(apiUrl, requestContent);
        Console.WriteLine("Código de estado: " + response.StatusCode);
        string responseContent = await response.Content.ReadAsStringAsync();
        ResponseData answer = JsonSerializer.Deserialize<ResponseData>(responseContent);
        return answer.message.content;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error al preparar la solicitud: {e.Message}");
        return string.Empty;
    }

}

public class Message
{
    public string content { get; set; }
    public string role { get; set; }
}

public class RequestData
{
    public int role_id { get; set; }
    public Message message { get; set; }
}

public class ResponseData
{
    public int chat_id { get; set; }
    public int message_id { get; set; }
    public Message message { get; set; }
}





