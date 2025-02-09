//
// MonthCalendarTest.cs: Test case for MonthCalendar
// 
// Authors:
//   Ritvik Mayank (mritvik@novell.com)
//
// (C) 2005 Novell, Inc. (http://www.novell.com)
//

using System.Drawing;
using System.Windows.Forms;

namespace GtkTests.System.Windows.Forms;

[TestFixture]
public class MonthCalendarTest : TestHelper
{
    [Test]
    public void MonthCalendarPropertyTest()
    {
        Form myfrm = new Form();
        myfrm.ShowInTaskbar = false;
        MonthCalendar myMonthCal1 = new MonthCalendar();
        //MonthCalendar myMonthCal2 = new MonthCalendar ();
        myMonthCal1.Name = "MonthCendar";
        myMonthCal1.TabIndex = 1;
        //DateTime myDateTime = new DateTime ();

        // F
        Assert.AreEqual(Day.Default, myMonthCal1.FirstDayOfWeek, "#F1");
        myMonthCal1.FirstDayOfWeek = Day.Sunday;
        Assert.AreEqual(Day.Sunday, myMonthCal1.FirstDayOfWeek, "#F2");
        Assert.AreEqual("WindowText", myMonthCal1.ForeColor.Name, "#F3");

        // M 
        Assert.AreEqual(new DateTime(9998, 12, 31), myMonthCal1.MaxDate, "#M1");
        Assert.AreEqual(new DateTime(1753, 1, 1), myMonthCal1.MinDate, "#M3");
        Assert.AreEqual(true, myMonthCal1.ShowToday, "#S5");
        Assert.AreEqual(true, myMonthCal1.ShowTodayCircle, "#S6");
        Assert.AreEqual(false, myMonthCal1.ShowWeekNumbers, "#S7");
        // Font dependent. // Assert.AreEqual (153, myMonthCal1.SingleMonthSize.Height, "#S8a");
        // Font dependent. // Assert.AreEqual (176, myMonthCal1.SingleMonthSize.Width, "#S8b");
        Assert.AreEqual(null, myMonthCal1.Site, "#S9");
        Assert.AreEqual(DateTime.Today, myMonthCal1.TodayDate, "#T3");

        myfrm.Dispose();
    }

    [Test]
    public void InitialSizeTest()
    {
        MonthCalendar cal = new MonthCalendar();
        Assert.IsTrue(cal.Size != Size.Empty, "#01");
    }

    [Test]
    public void MonthCalMaxDateException()
    {
        MonthCalendar myMonthCal1 = new MonthCalendar();

        try
        {
            myMonthCal1.MaxDate = new DateTime(1752, 1, 1, 0, 0, 0, 0); // value is less than min date (01/01/1753)
            Assert.Fail("#A1");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType(), "#A2");
            Assert.IsNotNull(ex.Message, "#A3");
            Assert.IsNotNull(ex.ParamName, "#A4");
            Assert.AreEqual("MaxDate", ex.ParamName, "#A5");
            Assert.IsNull(ex.InnerException, "#A6");
        }
    }

    [Test]
    public void MonthCalMinDateException()
    {
        MonthCalendar myMonthCal1 = new MonthCalendar();

        try
        {
            myMonthCal1.MinDate = new DateTime(1752, 1, 1, 0, 0, 0, 0); // Date earlier than 01/01/1753
            Assert.Fail("#A1");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType(), "#A2");
            Assert.IsNotNull(ex.Message, "#A3");
            Assert.IsNotNull(ex.ParamName, "#A4");
            Assert.AreEqual("MinDate", ex.ParamName, "#A5");
            Assert.IsNull(ex.InnerException, "#A6");
        }

        try
        {
            myMonthCal1.MinDate = new DateTime(9999, 12, 31, 0, 0, 0, 0); // Date greater than max date
            Assert.Fail("#B1");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.AreEqual(typeof(ArgumentOutOfRangeException), ex.GetType(), "#B2");
            Assert.IsNotNull(ex.Message, "#B3");
            Assert.IsNotNull(ex.ParamName, "#B4");
            Assert.AreEqual("MinDate", ex.ParamName, "#B5");
            Assert.IsNull(ex.InnerException, "#B6");
        }
    }

    [Test]
    public void MonthCalSelectRangeException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            MonthCalendar myMonthCal1 = new MonthCalendar();
            myMonthCal1.SelectionRange = new SelectionRange(new DateTime(1752, 01, 01), new DateTime(1752, 01, 02));
        });
    }

    [Test]
    public void MonthCalSelectRangeException2()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            MonthCalendar myMonthCal1 = new MonthCalendar();
            myMonthCal1.SelectionRange = new SelectionRange(new DateTime(9999, 12, 30), new DateTime(9999, 12, 31));
        });
    }

}

[TestFixture]
public class MonthCalendarPropertiesTest : MonthCalendar
{
    private bool clickRaised;
    private bool doubleClickRaised;

    [SetUp]
    protected void SetUp () {
        clickRaised = false;
        doubleClickRaised = false;
    }

    [Test]
    public void DefaultMarginTest ()
    {
        Assert.AreEqual (DefaultMargin.All, 9, "#01");
    }
}

