using System.Diagnostics.CodeAnalysis;

namespace Enigma;
public class Plugboard : IPlugboard
{
    private Socket[] _sockets;

    public Plugboard()
    {
        char[] alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        _sockets = alphabets.Select(x => new Socket(x)).ToArray();
    }

    public event EventHandler<PatchCableConnectedEventArgs>? OnCableConnected;

    protected virtual void RaiseOnCableConnectedEvent(PatchCableConnectedEventArgs e)
    {
        OnCableConnected?.Invoke(this, e);
    }

    public Socket? this[char i] => _sockets.ToList().SingleOrDefault(x => x.Character == i);

    public class PatchCable
    {
        public void ConnectTo(Socket socket)
        {
            if (Side1 != default && Side2 != default)
                throw new InvalidOperationException($"This cable can't connected to this soket: {socket.ToString}");

            if (this.Side1 == default)
                this.Side1 = socket.Character;
            else
                this.Side2 = socket.Character;

            socket.Cable = this;
        }
        public char OpositeSide(char chr) => (Side1 == chr) ? Side2 : Side1;
        public char Side1 { get; private set; }
        public char Side2 { get; private set; }

        public override string ToString()
        {
            return $"{this.Side1} <---> {this.Side2}";
        }
    }

    char IPlugboard.Process(char chr)
    {
        if (BoardCanProcessThis(chr))
            return this[chr]!.Cable is null ? chr : this[chr]!.Cable!.OpositeSide(chr);

        return chr;

        bool BoardCanProcessThis(char chr) => this[chr] != null;
    }

    public void ConnectSockets(char from, char to)
    {
        var fromSocket = this._sockets.Single(x => x.Character == from);
        var toSocket = this._sockets.Single(x => x.Character == to);

        if (fromSocket.Cable is null && toSocket.Cable is null)
        {
            var patchCable = new PatchCable();
            patchCable.ConnectTo(fromSocket);
            patchCable.ConnectTo(toSocket);
            fromSocket.Cable = patchCable; toSocket.Cable = patchCable;
            RaiseOnCableConnectedEvent(new PatchCableConnectedEventArgs(patchCable));
        }
    }
    public class Socket : IEqualityComparer<Socket>
    {
        private readonly char _character;
        public Socket(char character)
        {
            this._character = character;
        }
        public char Character { get { return _character; } }
        public PatchCable? Cable { get; set; }

        public bool Equals(Socket? x, Socket? y)
        {
            return Char.ToUpper(x!.Character) == Char.ToUpper(y!.Character);
        }

        public int GetHashCode([DisallowNull] Socket obj)
        {
            return Char.ToUpper(obj.Character).GetHashCode();
        }
    }
}