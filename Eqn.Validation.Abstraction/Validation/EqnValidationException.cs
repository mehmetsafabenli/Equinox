using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Eqn.Core;
using Eqn.Core.Logging;
using Eqn.Core.Microsoft.Extensions.Logging;
using Eqn.Core.System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Eqn.Validation.Abstraction.Validation;

/// <summary>
/// This exception type is used to throws validation exceptions.
/// </summary>
[Serializable]
public class EqnValidationException : EqnException,
    IHasLogLevel,
    IHasValidationErrors,
    IExceptionWithSelfLogging
{
    /// <summary>
    /// Detailed list of validation errors for this exception.
    /// </summary>
    public IList<ValidationResult> ValidationErrors { get; }

    /// <summary>
    /// Exception severity.
    /// Default: Warn.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public EqnValidationException()
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor for serializing.
    /// </summary>
    public EqnValidationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    public EqnValidationException(string message)
        : base(message)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    public EqnValidationException(IList<ValidationResult> validationErrors)
    {
        ValidationErrors = validationErrors;
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="validationErrors">Validation errors</param>
    public EqnValidationException(string message, IList<ValidationResult> validationErrors)
        : base(message)
    {
        ValidationErrors = validationErrors;
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public EqnValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    public void Log(ILogger logger)
    {
        if (ValidationErrors.IsNullOrEmpty())
        {
            return;
        }

        var validationErrors = new StringBuilder();
        validationErrors.AppendLine("There are " + ValidationErrors.Count + " validation errors:");
        foreach (var validationResult in ValidationErrors)
        {
            var memberNames = "";
            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
            }

            validationErrors.AppendLine(validationResult.ErrorMessage + memberNames);
        }

        logger.LogWithLevel(LogLevel, validationErrors.ToString());
    }
}
