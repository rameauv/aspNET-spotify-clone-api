using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace Spotify.Shared.BLL.Jwt.Exceptions;

/// <summary>
/// Throw this exception when a received Security Token has been replayed.
/// </summary>
[Serializable]
public class MySecurityTokenReplayDetectedException: MySecurityTokenValidationException
{
    /// <summary>
    /// Initializes a new instance of  <see cref="MySecurityTokenReplayDetectedException"/>
    /// </summary>
    public MySecurityTokenReplayDetectedException(): base("SecurityToken replay detected")
    {
    }
    
    /// <summary>
    /// Initializes a new instance of  <see cref="MySecurityTokenReplayDetectedException"/>
    /// </summary>
    public MySecurityTokenReplayDetectedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of  <see cref="MySecurityTokenReplayDetectedException"/>
    /// </summary>
    public MySecurityTokenReplayDetectedException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySecurityTokenReplayDetectedException"/> class.
    /// </summary>
    /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected MySecurityTokenReplayDetectedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}