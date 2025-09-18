namespace LibSharpProtocol.Core.Data;

public interface IProtocolSerializer
{
    void Write(ProtocolStream stream);
    void Read(ProtocolStream stream);

    public static void WriteElement(ProtocolStream s, IProtocolSerializer e) => e.Write(s);
}