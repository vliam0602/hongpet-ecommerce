using Newtonsoft.Json;

namespace HongPet.Application.Heplers;

public class JsonHelper
{
    public static List<T> LoadDataFromJson<T>(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<T>>(jsonData)!;
    }
}
