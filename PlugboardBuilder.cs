namespace Enigma;

public class PlugboardBuilder : IPlugboardBuilder
{
    IPlugboard _plugBoard = new Plugboard();
    Instruction _instruction;
    public PlugboardBuilder()
    {
        this.Reset();
        _instruction = new Instruction(string.Empty);
    }
    void HandleCableConnectedEvent(object? sender, PatchCableConnectedEventArgs e)
    {
        var completedStep = _instruction.Steps.Single(x => x.From == e.PatchCable.Side1 && x.To == e.PatchCable.Side2
        || (x.From == e.PatchCable.Side2 && x.To == e.PatchCable.Side1));

        //var completedStep = _instruction.Steps.Single(x => x == e.PatchCable);


        completedStep.MarkDone();
    }
    public void Reset()
    {
        this._plugBoard = new Plugboard();
        this._plugBoard.OnCableConnected += HandleCableConnectedEvent;
    }

    public void ConnectCables(Instruction instruction)
    {
        if (!instruction.Applicable)
            throw new InvalidOperationException($"This parameter is not valid: {nameof(instruction)}");

        Reset();

        _instruction = instruction;

        foreach (var step in instruction.Steps)
            _plugBoard.ConnectSockets(step.From!.Value, step.To!.Value);

    }
    public IPlugboard Build()
    {
        if (!_instruction.Applied)
            throw new Exception("This device is not confgured correctly!");

        var result = this._plugBoard;
        this.Reset();
        return result;
    }
}