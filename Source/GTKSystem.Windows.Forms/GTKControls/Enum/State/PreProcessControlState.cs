namespace System.Windows.Forms
{
    /// <summary>
    ///     Provides options that specify the relationship between the control and preprocessing
    ///     messages.
    /// </summary>
    public enum PreProcessControlState
    {
        /// <summary>
        ///     Specifies that the message has been processed and no further processing is required.
        /// </summary>
        MessageProcessed = 0,
        /// <summary>
        ///     Specifies that the control requires the message and that processing should continue.
        /// </summary>
        MessageNeeded = 1,
        /// <summary>
        ///     Specifies that the control does not require the message.
        /// </summary>
        MessageNotNeeded = 2
    }
}