namespace System.Windows.Forms
{
    //
    // 摘要:
    //     Specifies identifiers to indicate the return value of a dialog box.
    public enum DialogResult
    {
        //
        // 摘要:
        //     Nothing is returned from the dialog box. This means that the modal dialog continues
        //     running.
        None = 0,
        //
        // 摘要:
        //     The dialog box return value is OK (usually sent from a button labeled OK).
        OK = 1,
        //
        // 摘要:
        //     The dialog box return value is Cancel (usually sent from a button labeled Cancel).
        Cancel = 2,
        //
        // 摘要:
        //     The dialog box return value is Abort (usually sent from a button labeled Abort).
        Abort = 3,
        //
        // 摘要:
        //     The dialog box return value is Retry (usually sent from a button labeled Retry).
        Retry = 4,
        //
        // 摘要:
        //     The dialog box return value is Ignore (usually sent from a button labeled Ignore).
        Ignore = 5,
        //
        // 摘要:
        //     The dialog box return value is Yes (usually sent from a button labeled Yes).
        Yes = 6,
        //
        // 摘要:
        //     The dialog box return value is No (usually sent from a button labeled No).
        No = 7
    }
}