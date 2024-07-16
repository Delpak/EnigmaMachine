namespace Enigma;

public interface IPlugboard
{
    char Process(char chr);
    void ConnectSockets(char from, char to);
    event EventHandler<PatchCableConnectedEventArgs>? OnCableConnected;
}
