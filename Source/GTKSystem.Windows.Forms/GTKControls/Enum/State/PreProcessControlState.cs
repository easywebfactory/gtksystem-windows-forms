namespace System.Windows.Forms
{
    //
    // summary:
    //     Provides options that specify the relationship between the control and preprocessing
    //     messages.
    public enum PreProcessControlState
    {
        //
        // summary:
        //     Specifies that the message has been processed and no further processing is required.
        MessageProcessed = 0,
        //
        // summary:
        //     Specifies that the control requires the message and that processing should continue.
        MessageNeeded = 1,
        //
        // summary:
        //     Specifies that the control does not require the message.
        MessageNotNeeded = 2
    }
}