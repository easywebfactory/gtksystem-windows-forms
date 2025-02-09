using System.Windows.Forms;
using Sys_Threading=System.Threading;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class FormThreadTest : TestHelper
{
    private static void GuiThread()
    {
        Form form1;

        form1 = new Form();
        form1.Show();
        form1.Dispose();
    }

    [Test]
    public void TestThreadFormsInit ()
    {
        Thread thread;

        thread = new Thread(new ThreadStart(GuiThread));
        thread.Start();
        thread.Join();

        try
        {
            GuiThread();
        }
        catch (Exception e)
        {
            Assert.Fail ("#1");
        }
    }
}