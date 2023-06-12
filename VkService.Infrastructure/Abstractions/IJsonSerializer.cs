namespace VkService.Client.Abstractions;

public interface IJsonSerializer
{
    string Serialize<T>(T? value);
    T? Deserialize<T>(string inputStr);
}
