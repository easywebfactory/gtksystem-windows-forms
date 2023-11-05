namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Provides options that specify the relationship between the control and preprocessing
    //     messages.
    public enum PreProcessControlState
    {
        //
        // 摘要:
        //     Specifies that the message has been processed and no further processing is required.
        MessageProcessed = 0,
        //
        // 摘要:
        //     Specifies that the control requires the message and that processing should continue.
        MessageNeeded = 1,
        //
        // 摘要:
        //     Specifies that the control does not require the message.
        MessageNotNeeded = 2
    }
}