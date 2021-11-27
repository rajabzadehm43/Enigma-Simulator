using System.Security;
using System.Text;
using System.Threading.Channels;
using Enigma;

var rtConfig = new RotorsConfig();
rtConfig.LoadRotorConfig("test");

var state = 0;

Console.WriteLine($"Rotor 1 : {string.Join(", ", rtConfig.Model.Rotors[0])}");
Console.WriteLine($"Rotor 2 : {string.Join(", ", rtConfig.Model.Rotors[1])}");
Console.WriteLine($"Rotor 3 : {string.Join(", ", rtConfig.Model.Rotors[2])}");
Console.WriteLine($"________________________________________\n");


var cmdState = false;
while (!cmdState)
{
    Console.WriteLine("Please Enter Your Command");
    Console.WriteLine("Commands : ");
    Console.WriteLine("Command: 'SaveRotorConfig'");
    Console.WriteLine("Command: 'LoadRotorConfig'");

    var command = Console.ReadLine();

    if (command == null)
        continue;

    command = command.Trim();

    string? fileName;
    switch (command)
    {
        case "SaveRotorConfig":
            Console.WriteLine("Please Enter Config File Name");
            fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
                break;

            rtConfig.SaveRotorConfig(fileName);
            cmdState = true;
            break;
        case "LoadRotorConfig":

            Console.WriteLine("Please Enter Config File Name");
            fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
                break;

            rtConfig.LoadRotorConfig(fileName);
            cmdState = true;
            break;
    }
}

Console.WriteLine("You Can Enter Your Words Now , Please Enter Words Without Space Or Symbols");

while (true)
{
    Console.WriteLine("Word : ");
    var word = Console.ReadLine();

    if (word == null)
        continue;

    StringBuilder result = new StringBuilder();

    foreach (var @char in word)
    {
        var encOrDec = EncodeOrDecode(@char);
        result.Append(encOrDec);
    }

    Console.WriteLine($"Encode/Decoded Word: {result}");
}

/*
 * var word = Console.ReadLine();
    
    if (word == null)
        continue;

    StringBuilder result = new StringBuilder();

    foreach (var @char in word)
    {
        //var encOrDec = EncodeOrDecode(@char);
        var encOrDec = Reflect(@char);
        result.Append(encOrDec);
    }

    Console.WriteLine($"Encode/Decoded Word: {result}");
 */









char EncodeOrDecode(char c)
{
    c = ReadCharFromRotor(0, c);
    c = ReadCharFromRotor(1, c);
    c = ReadCharFromRotor(2, c);

    c = Reflect(c);

    c = ReadRotorFromChar(2, c);
    c = ReadRotorFromChar(1, c);
    c = ReadRotorFromChar(0, c);

    RotateRotors();

    return c;
}


char Reflect(char c)
{
    var index = rtConfig.Model.Alphabet!.IndexOf(c);
    index = (rtConfig.Model.Alphabet.Count - 1) - index;
    return rtConfig.Model.Alphabet![index];
}

char ReadRotorFromChar(int rotor, char c)
{
    var index = rtConfig.Model.Rotors![rotor].IndexOf(c);
    return rtConfig.Model.Alphabet![index];
}

char ReadCharFromRotor(int rotor, char c)
{
    var index = rtConfig.Model.Alphabet!.IndexOf(c);
    return rtConfig.Model.Rotors![rotor][index];
}

void RotateRotors()
{
    RotateRotor(rtConfig.Model.Rotors![0]);
    state += 1;

    if (state % 26 == 0)
        RotateRotor(rtConfig.Model.Rotors[1]);

    if (state % (26 * 26) == 0)
        RotateRotor(rtConfig.Model.Rotors[2]);

}

void RotateRotor(List<char> rotor)
{
    var firstRow = rotor[0];
    rotor.Add(firstRow);
    rotor!.RemoveAt(0);
}