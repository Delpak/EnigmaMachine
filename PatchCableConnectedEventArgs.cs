using static Enigma.Plugboard;

namespace Enigma;

public class PatchCableConnectedEventArgs : EventArgs
{
    public PatchCableConnectedEventArgs(PatchCable patchCable)
    {
        PatchCable = patchCable;
    }

    public PatchCable PatchCable { get; private set; }
}
