using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Enigma;

public class RotorsConfig
{
    public RotorsConfig()
    {
        var alphabet = new List<char>
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        };

        var rotors = new List<List<char>>
        {
            new (alphabet.OrderBy(a => Guid.NewGuid())),
            new (alphabet.OrderBy(a => Guid.NewGuid())),
            new (alphabet.OrderBy(a => Guid.NewGuid()))
        };

        Model = new RotorConfigModel(alphabet, rotors);
    }

    public RotorConfigModel Model { get; private set; }

    public void SaveRotorConfig(string name)
    {
        var configText = JsonConvert.SerializeObject(Model);

        var basePath = Path.Join(Directory.GetCurrentDirectory(), "Configs");
        var fileName = name + ".enigma";
        var fullPath = Path.Join(basePath, fileName);

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        File.WriteAllText(fullPath, configText);
    }

    public void LoadRotorConfig(string name)
    {
        var basePath = Path.Join(Directory.GetCurrentDirectory(), "Configs");
        var fileName = name + ".enigma";
        var fullPath = Path.Join(basePath, fileName);

        if (!File.Exists(fullPath))
            return;

        var configText = File.ReadAllText(fullPath);

        Model = JsonConvert.DeserializeObject<RotorConfigModel>(configText);
    }

    public class RotorConfigModel
    {
        public RotorConfigModel(List<char> alphabet, List<List<char>> rotors)
        {
            Alphabet = alphabet;
            Rotors = rotors;
        }

        public List<char> Alphabet { get; }

        public List<List<char>> Rotors { get; }
    }
}