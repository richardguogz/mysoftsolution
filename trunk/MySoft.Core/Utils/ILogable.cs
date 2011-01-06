
namespace MySoft
{
    /// <summary>
    /// A delegate used for log.
    /// </summary>
    /// <param name="log">The msg to write to log.</param>
    public delegate void LogHandler(string log);

    /// <summary>
    /// Mark a implementing class as loggable.
    /// </summary>
    public interface ILogable
    {
        /// <summary>
        /// OnLog event.
        /// </summary>
        event LogHandler OnLog;
    }
}
