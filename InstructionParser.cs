namespace Enigma;

static class InstructionParser
{
    public static bool TryParse(string text, out Instruction result)
    {
        text = text.Replace(" ", string.Empty);
        result = new Instruction(text);
        return true;
    }
}