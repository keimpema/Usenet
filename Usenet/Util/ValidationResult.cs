using System.Collections.Generic;

namespace Usenet.Util
{
    /// <summary>
    /// Represents a validation result.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// A collection of <see cref="ValidationFailure"/> objects.
        /// </summary>
        public IList<ValidationFailure> Failures { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="failures">A collection of <see cref="ValidationFailure"/> objects.</param>
        public ValidationResult(IList<ValidationFailure> failures)
        {
            Failures = failures ?? new List<ValidationFailure>(0);
        }

        /// <summary>
        /// A property indicating whether the validation result is valid or not.
        /// </summary>
        /// <returns>true if the validation result contains no validation failures; otherwise false</returns>
        public bool IsValid => Failures.Count == 0;
    }
}
