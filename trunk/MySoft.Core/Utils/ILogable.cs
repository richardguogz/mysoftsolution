
namespace MySoft.Core
{
    /// <summary>
    /// A delegate used for log.
    /// </summary>
    /// <param name="log">The msg to write to log.</param>
    public delegate void LogEventHandler(string log);

    /// <summary>
    /// Mark a implementing class as loggable.
    /// </summary>
    public interface ILogable
    {
        /// <summary>
        /// OnLog event.
        /// </summary>
        event LogEventHandler OnLog;
    }
}
