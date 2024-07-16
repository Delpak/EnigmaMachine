namespace Enigma;

public interface IPlugboardBuilder
{
    void ConnectCables(Instruction instruction);
    IPlugboard Build();
}
