using LibSharpProtocol.Core;

namespace LibSharpProtocol.Protocol772;

public class Protocol1_21_8 : Protocol
{
    public Protocol1_21_8()
    {
        this.RegisterAssemblyPackets(typeof(Protocol1_21_8).Assembly);
    }
    
    public override string Name => "1.21.8";
    public override int Version => 772;
}