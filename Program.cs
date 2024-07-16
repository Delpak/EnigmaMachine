using Enigma;

IPlugboardBuilder plugboardBuilder = new PlugboardBuilder();

Director director = new Director(plugboardBuilder);

director.ConstructEnigmaMachine("ABCDEFGHIJKLMNOPQRST");

var plugboard = plugboardBuilder.Build();

Console.WriteLine(plugboard.Process('A'));
Console.WriteLine(plugboard.Process('B'));
Console.WriteLine(plugboard.Process('X'));
Console.WriteLine(plugboard.Process('.'));


Console.WriteLine("Hello, World!");