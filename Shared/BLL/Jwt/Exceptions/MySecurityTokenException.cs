using System.Runtime.Serialization;

namespace Spotify.Shared.BLL.Jwt.Exceptions;

    /// <summary>
    /// Represents a security token validation exception.
    /// </summary>
    [Serializable]
    public class MySecurityTokenValidationException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySecurityTokenValidationException"/> class.
        /// </summary>
        public MySecurityTokenValidationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySecurityTokenValidationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MySecurityTokenValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySecurityTokenValidationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public MySecurityTokenValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySecurityTokenValidationException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected MySecurityTokenValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }