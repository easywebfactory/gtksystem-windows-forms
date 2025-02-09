//
// CompatTest.cs: Compatibility unit tests for ResXResourceReader.
//
// Authors:
//     Robert Jordan <robertj@gmx.net>
//

using GtkTests.Helpers;

namespace GtkTests.System.Resources;

[TestFixture]
public class CompatTest : Windows.Forms.TestHelper
{
    [Test]
    public void TestReader ()
    {
        CompatTestHelper.TestReader (TestResourceHelper.GetFullPathOfResource ("GtkTests.System.Resources.compat_1_1.resx"));
    }

    [Test]
    public void TestReader_2_0 ()
    {
        CompatTestHelper.TestReader (TestResourceHelper.GetFullPathOfResource ("GtkTests.System.Resources.compat_2_0.resx"));
    }
}