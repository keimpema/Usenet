using Usenet.Extensions;

namespace Usenet.Util
{
    /// <summary>
    /// Represents a validation failure.
    /// </summary>
    public class ValidationFailure
    {
        /// <summary>
        /// A code associated with the validation failure.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// A message describing the validation failure.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// A data object containing information about the validation failure.
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidationFailure"/> class.
        /// </summary>
        /// <param name="code">A code associated with the validation failure.</param>
        /// <param name="message">A message describing the validation failure.</param>
        /// <param name="data">A data object containing information about the validation failure.</param>
        public ValidationFailure(string code, string message, object data = null)
        {
            Code = code.ThrowIfNullOrWhiteSpace(nameof(code));
            Message = message;
            Data = data;
        }
    }
}
