using System.Net;
using System.Threading.Tasks;
using LibSharpProtocol.Core.Data;

namespace LibSharpProtocol.Core.Auth;

public interface IAuthProvider
{
    public Task LogInAsync(ProtocolClient client, EndPoint ep);
    
    public string Username { get; }
    public UUID PlayerUUID { get; }
}