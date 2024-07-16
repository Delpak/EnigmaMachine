namespace Enigma;
public class Plugboard : IPlugboard
{
    private readonly Socket[] _sockets;
    private const string A_to_Z = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public Plugboard()
    {
        char[] alphabets = A_to_Z.ToCharArray();
        _sockets = alphabets.Select(x => new Socket(x)).ToArray();
    }

    public event EventHandler<PatchCableConnectedEventArgs>? OnPatchCableConnected;

    protected virtual void RaiseOnCableConnectedEvent(PatchCableConnectedEventArgs e)
    {
        OnPatchCableConnected?.Invoke(this, e);
    }

    public Socket? this[char c] => _sockets.ToList().SingleOrDefault(x => x.Character == c);

    public class PatchCable
    {
        public PatchCable ConnectTo(Socket socket)
        {
            if (End1 != default && End2 != default)
                throw new InvalidOperationException($"This cable can't connected to this soket: {socket.ToString}");

            if (this.End1 == default)
                this.End1 = socket.Character;
            else
                this.End2 = socket.Character;

            socket.Cable = this;
            return this;
        }
        public char OtherEndOf(char chr) => (End1 == chr) ? End2 : End1;
        public char End1 { get; private set; }
        public char End2 { get; private set; }
        public override string ToString()
        {
            return $"{this.End1} <---> {this.End2}";
        }
    }

    char IPlugboard.Process(char chr)
    {
        if (BoardCanProcessThis(chr))
            return this[chr]!.Cable is null ? chr : this[chr]!.Cable!.OtherEndOf(chr);

        return chr;

        bool BoardCanProcessThis(char chr) => this[chr] != null;
    }

    public void ConnectSockets(char end1, char end2)
    {
        var end1Socket = this._sockets.Single(x => x.Character == end1);
        var end2Socket = this._sockets.Single(x => x.Character == end2);

        if (end1Socket.Cable is null && end2Socket.Cable is null)
        {
            var patchCable = new PatchCable();

            patchCable.ConnectTo(end1Socket)
                .ConnectTo(end2Socket);

            end1Socket.Cable = patchCable; end2Socket.Cable = patchCable;
            RaiseOnCableConnectedEvent(new PatchCableConnectedEventArgs(patchCable));
        }
    }
    public class Socket
    {
        private readonly char _character;
        public Socket(char character)
        {
            this._character = character;
        }
        public char Character { get { return _character; } }
        public PatchCable? Cable { get; set; }
    }
}