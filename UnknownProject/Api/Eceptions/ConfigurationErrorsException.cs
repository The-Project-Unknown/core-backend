using System.Runtime.Serialization;

namespace Api.Eceptions;

[Serializable]
public class ConfigurationErrorsException : Exception
{
    public ConfigurationErrorsException()
    {
    }

    protected ConfigurationErrorsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ConfigurationErrorsException(string? message) : base(message)
    {
    }

    public ConfigurationErrorsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
public class ConfigurationMissingException : ConfigurationErrorsException
{
    public ConfigurationMissingException()
    {
    }

    protected ConfigurationMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ConfigurationMissingException(string? message) : base(message)
    {
    }

    public ConfigurationMissingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}