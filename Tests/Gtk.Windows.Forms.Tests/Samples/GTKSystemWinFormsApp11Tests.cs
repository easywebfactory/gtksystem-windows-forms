using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using GTKSystemWinFormsApp11;

namespace GtkTests.Samples;

[TestFixture]
public class GTKSystemWinFormsApp11Tests
{
    [Test]
    [TestCase("en-US")]
    [TestCase("zh-HANS")]
    public async Task TestGtkForm(string language)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        GTKSystemWinFormsApp11.Properties.Resources.Culture = Thread.CurrentThread.CurrentCulture;

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var mainForm = new GtkForm();
        var taskCompletionSource = new TaskCompletionSource<string>();
        mainForm.Tag = taskCompletionSource;
        mainForm.Load += MainForm_Load;
        Application.Run(mainForm);
        await taskCompletionSource.Task;
        Assert.AreEqual(language.ToLower(), taskCompletionSource.Task.Result.ToLower());
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        await MainFormOnLoadAsync((sender as GtkForm)!);
    }

    private async Task MainFormOnLoadAsync(GtkForm form)
    {
        form.Text = Thread.CurrentThread.CurrentUICulture.Name;
#if TESTCLICK
        form.LinkLabel1.PerformClick();
#endif
        await Task.Delay(TimeSpan.FromSeconds(5));
        var taskCompletionSource = (TaskCompletionSource<string>)form.Tag!;
        form.Dispose();
        taskCompletionSource.SetResult(Thread.CurrentThread.CurrentUICulture.Name);
    }
}