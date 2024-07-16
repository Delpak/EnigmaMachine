namespace Enigma;
public class Director
{
    private readonly IPlugboardBuilder _builder;

    public Director(IPlugboardBuilder builder)
    {
        _builder = builder;
    }

    public void ConstructEnigmaMachine(string instructionText = "")
    {
        Instruction instruction;
        if (!InstructionParser.TryParse(instructionText, out instruction))
            Console.WriteLine("Given instruction is not valid!");

        _builder.ConnectCables(instruction);
    }
}
