using System;

namespace MySoft.IoC
{
    /// <summary>
    /// TypeMismatchedException
    /// </summary>
    public class TypeMismatchedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMismatchedException"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        public TypeMismatchedException(string info) : base(info) { }
    }
}