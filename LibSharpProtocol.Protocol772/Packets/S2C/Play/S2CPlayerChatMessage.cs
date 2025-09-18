using System;
using LibSharpProtocol.Core;
using LibSharpProtocol.Core.Data;
using LibSharpProtocol.Core.Packets;

namespace LibSharpProtocol.Protocol772.Packets.S2C.Play;

[PacketInfo(0x3A, PacketDirection.S2C, ProtocolState.Play)]
public class S2CPlayerChatMessage : IPacket
{
    public void Read(ProtocolStream stream)
    {
        Header.Read(stream);
        Body.Read(stream);
    }

    public void Write(ProtocolStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Id => 0x3A;
    public MessageHeader Header { get; set; } = new();
    public MessageBody Body { get; set; } = new();
    //public MessageValidators[] Validators { get; set; } = [];
    //public UnsignedData Other { get; set; } = new();
    

    public class MessageHeader : IProtocolSerializer
    {
        public void Write(ProtocolStream stream)
        {
            throw new System.NotImplementedException();
        }

        public void Read(ProtocolStream stream)
        {
            GlobalIndex = stream.ReadVarInt();
            Sender = stream.ReadUUID();
            Index = stream.ReadVarInt();
            Signature = stream.ReadOptional(ReadSignature);
        }

        byte[] ReadSignature(ProtocolStream stream) => stream.Read(256);
        
        public int GlobalIndex { get; set; }
        public UUID Sender { get; set; }
        public int Index { get; set; }
        public byte[]? Signature { get; set; }
    }

    public class MessageBody : IProtocolSerializer
    {
        public void Read(ProtocolStream stream)
        {
            Message = stream.ReadString();
            Timestamp = stream.ReadI64();
            Salt = stream.ReadI64();
        }

        public void Write(ProtocolStream stream)
        {
            throw new NotImplementedException();
        }
        
        public string Message { get; set; } = "";
        public long Timestamp { get; set; }
        public long Salt { get; set; }
    }
}