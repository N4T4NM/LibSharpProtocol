using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Auth;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets.Handshaking;
using LibSharpProtocol.Core.Packets.Login;
using LibSharpProtocol.Protocol772.Packets.C2S.Configuration;
using LibSharpProtocol.Protocol772.Packets.C2S.Login;
using LibSharpProtocol.Protocol772.Packets.S2C.Configuration;
using LibSharpProtocol.Protocol772.Packets.S2C.Login;

namespace LibSharpProtocol.Protocol772.Auth;

public class OfflineUser(string username, UUID uuid) : IAuthProvider
{
    public async Task LogInAsync(ProtocolClient client, EndPoint ep)
    {
        string addr;
        int port;
        switch (ep)
        {
            case IPEndPoint ip: addr = ip.Address.ToString(); port = ip.Port; break;
            case DnsEndPoint dns: addr =  dns.Host; port = dns.Port; break;
            default: throw new NotSupportedException($"Unknown endpoint {ep}");
        }
        
        await client.WritePacketAsync(new C2SHandshake()
        {
            ProtocolVersion = client.Protocol.Version,
            ServerAddress = addr,
            ServerPort = (ushort)port,
            Intent = ProtocolState.Login
        });
        
        client.State =  ProtocolState.Login;
        await RunLoginProcess(client);

        client.State = ProtocolState.Configuration;
        await RunConfigurationProcess(client);
        
        client.State = ProtocolState.Play;
    }

    async Task RunLoginProcess(ProtocolClient client)
    {
        await client.WritePacketAsync(new C2SLoginStart()
        {
            Name = Username,
            PlayerUUID = PlayerUUID
        });

        while (true)
        {
            var rsp = (await client.ReadPacketAsync()).TranslatePacket(client.Protocol);
            switch (rsp)
            {
                case S2CSetCompression compression: client.CompressionThreshold = compression.Threshold; break;
                case S2CLoginSuccess loginSuccess: 
                    PlayerUUID = loginSuccess.UUID;
                    await client.WritePacketAsync(new C2SLoginAck());
                    return;
                default: throw new NotImplementedException(rsp.ToString());
            }
        }
    }
    async Task RunConfigurationProcess(ProtocolClient client)
    {
        await client.WritePacketAsync(new C2SClientInfo());
        while (true)
        {
            var src = await client.ReadPacketAsync();
            var pck = src.TranslatePacket(client.Protocol);

            switch (pck)
            {
                case S2CKeepAliveConfiguration ka:
                    await client.WritePacketAsync(new C2SKeepAliveConfiguration()
                    {
                        KeepAliveId = ka.KeepAliveId
                    });
                    break;
                case S2CKnownPacks knownPacks:
                    await client.WritePacketAsync(new C2SKnownPacks()
                    {
                        KnownPacks = knownPacks.KnownPacks
                    });
                    break;
                case S2CFinishConfiguration: await client.WritePacketAsync(new C2SFinishConfigurationAck()); return;
            }
        }
    }

    public string Username { get; } = username;
    public UUID PlayerUUID { get; set; } = uuid;
}