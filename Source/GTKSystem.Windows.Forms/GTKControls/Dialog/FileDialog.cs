/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;
using Gtk;

namespace System.Windows.Forms;

public abstract class FileDialog : CommonDialog
{

    public bool ValidateNames { get; set; } = true;

    public string? Title { get; set; } = string.Empty;

    public bool SupportMultiDottedExtensions { get; set; }

    public bool ShowHelp { get; set; }

    public bool RestoreDirectory { get; set; }

    public string? InitialDirectory { get; set; }
    public string? Description { get; set; }
    internal bool Multiselect { get; set; }
    public int FilterIndex { get; set; }

    private string? _filter;
    private FileChooserDialog? _dialog;
    private string defaultExt = string.Empty;

    public string Filter
    {
        get
        {
            return _filter ?? string.Empty;
        }
        set
        {
            if (value == _filter)
            {
                return;
            }
            if (!string.IsNullOrEmpty(value))
            {
                string[] filters = value.Split('|');
                var pipeCount = filters.Length;
                if (pipeCount == 1 || pipeCount % 2 == 1)
                {
                    throw new ArgumentException("FileDialog Invalid Filter", value);
                }
                for (var i = 1; i < pipeCount; i += 2)
                {
                    if (filters[i].Split('.').Length == 1)
                    {
                        throw new ArgumentException("FileDialog Invalid Filter", value);
                    }
                }
            }
            else
            {
                value = null!;
            }

            _filter = value;
        }
    }

    public bool AutoUpgradeEnabled { get; set; }
    internal string? SelectedDirectory { get; set; }
    public string? FileName { get; set; }
    public string[]? FileNames { get; internal set; }

    public bool DereferenceLinks { get; set; } = true;

    public string DefaultExt
    {
        get => defaultExt;
        set => defaultExt = value?.TrimStart('.') ?? string.Empty;
    }

    public bool CheckPathExists { get; set; } = true;

    public virtual bool CheckFileExists { get; set; } = true;

    public event CancelEventHandler? FileOk;
    internal FileChooserAction ActionType { get; set; }
    public new virtual void Dispose()
    {
        _dialog?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    public override void Reset()
    {
        AddExtension = true;
        Title = null;
        InitialDirectory = null;
        FileName = null;
        FileNames = null;
        _filter = null;
        FilterIndex = 1;
        SupportMultiDottedExtensions = false;
    }

    public bool AddExtension { get; set; }

    protected override bool RunDialog(IWin32Window? owner)
    {
        _dialog = null;
        if (owner is Form ownerform)
        {
            _dialog = new FileChooserDialog(System.Windows.Forms.Properties.Resources.FileDialog_RunDialog_Select_File, ownerform.self, ActionType);
            _dialog.WindowPosition = WindowPosition.CenterOnParent;
        }
        else
        {
            _dialog = new FileChooserDialog(Properties.Resources.FileDialog_RunDialog_Select_File, null, ActionType);
            _dialog.WindowPosition = WindowPosition.Center;
        }
        _dialog.IconName = "document-open";
        _dialog.AddButton(Properties.Resources.FileDialog_RunDialog_OK, ResponseType.Ok);
        _dialog.AddButton(Properties.Resources.FileDialog_RunDialog_Cancel, ResponseType.Cancel);
        _dialog.SelectMultiple = Multiselect;
        _dialog.Title = Title ?? string.Empty;
        _dialog.TooltipText = Description ?? string.Empty;

        _dialog.KeepAbove = true;
        if (!string.IsNullOrWhiteSpace(SelectedDirectory))
            _dialog.SetCurrentFolder(SelectedDirectory);
        else if (!string.IsNullOrWhiteSpace(InitialDirectory))
            _dialog.SetCurrentFolder(InitialDirectory);


        if (_filter != null)
        {
            string[] filters = _filter.Split('|');
            for (var i = 1; i < filters.Length; i += 2)
            {
                string[] patterns = filters[i].Split(';');
                foreach (var pattern in patterns)
                {
                    var ffilter = new FileFilter();
                    var extand = pattern.TrimStart(new char[] { '*', ' ' });
                    if (MimeMapping.ContainsKey(extand))
                    {
                        ffilter.AddMimeType(MimeMapping[extand]);
                    }
                    ffilter.AddPattern(pattern);
                    ffilter.Name = $"{filters[i - 1]}（{pattern}）";
                    _dialog.AddFilter(ffilter);
                    if (pattern == DefaultExt)
                    {
                        _dialog.Filter = ffilter;
                    }
                }
            }
        }
        var response = _dialog.Run();
        FileName = _dialog.Filename;
        FileNames = _dialog.Filenames.Clone() as string[];
        SelectedDirectory = _dialog.Filename;
        _dialog.Dispose();
        _dialog.Destroy();
        return response == -5;
    }
    static readonly Dictionary<string, string> MimeMapping = new(StringComparer.OrdinalIgnoreCase);
    static FileDialog()
    {
        MimeMapping.Clear();
        MimeMapping.Add(".323", "text/h323");
        MimeMapping.Add(".aaf", "application/octet-stream");
        MimeMapping.Add(".aca", "application/octet-stream");
        MimeMapping.Add(".accdb", "application/msaccess");
        MimeMapping.Add(".accde", "application/msaccess");
        MimeMapping.Add(".accdt", "application/msaccess");
        MimeMapping.Add(".acx", "application/internet-property-stream");
        MimeMapping.Add(".afm", "application/octet-stream");
        MimeMapping.Add(".ai", "application/postscript");
        MimeMapping.Add(".aif", "audio/x-aiff");
        MimeMapping.Add(".aifc", "audio/aiff");
        MimeMapping.Add(".aiff", "audio/aiff");
        MimeMapping.Add(".application", "application/x-ms-application");
        MimeMapping.Add(".art", "image/x-jg");
        MimeMapping.Add(".asd", "application/octet-stream");
        MimeMapping.Add(".asf", "video/x-ms-asf");
        MimeMapping.Add(".asi", "application/octet-stream");
        MimeMapping.Add(".asm", "text/plain");
        MimeMapping.Add(".asr", "video/x-ms-asf");
        MimeMapping.Add(".asx", "video/x-ms-asf");
        MimeMapping.Add(".atom", "application/atom+xml");
        MimeMapping.Add(".au", "audio/basic");
        MimeMapping.Add(".avi", "video/x-msvideo");
        MimeMapping.Add(".axs", "application/olescript");
        MimeMapping.Add(".bas", "text/plain");
        MimeMapping.Add(".bcpio", "application/x-bcpio");
        MimeMapping.Add(".bin", "application/octet-stream");
        MimeMapping.Add(".bmp", "image/bmp");
        MimeMapping.Add(".c", "text/plain");
        MimeMapping.Add(".cab", "application/octet-stream");
        MimeMapping.Add(".calx", "application/vnd.ms-office.calx");
        MimeMapping.Add(".cat", "application/vnd.ms-pki.seccat");
        MimeMapping.Add(".cdf", "application/x-cdf");
        MimeMapping.Add(".chm", "application/octet-stream");
        MimeMapping.Add(".class", "application/x-java-applet");
        MimeMapping.Add(".clp", "application/x-msclip");
        MimeMapping.Add(".cmx", "image/x-cmx");
        MimeMapping.Add(".cnf", "text/plain");
        MimeMapping.Add(".cod", "image/cis-cod");
        MimeMapping.Add(".cpio", "application/x-cpio");
        MimeMapping.Add(".cpp", "text/plain");
        MimeMapping.Add(".crd", "application/x-mscardfile");
        MimeMapping.Add(".crl", "application/pkix-crl");
        MimeMapping.Add(".crt", "application/x-x509-ca-cert");
        MimeMapping.Add(".csh", "application/x-csh");
        MimeMapping.Add(".css", "text/css");
        MimeMapping.Add(".csv", "application/octet-stream");
        MimeMapping.Add(".cur", "application/octet-stream");
        MimeMapping.Add(".dcr", "application/x-director");
        MimeMapping.Add(".deploy", "application/octet-stream");
        MimeMapping.Add(".der", "application/x-x509-ca-cert");
        MimeMapping.Add(".dib", "image/bmp");
        MimeMapping.Add(".dir", "application/x-director");
        MimeMapping.Add(".disco", "text/xml");
        MimeMapping.Add(".dll", "application/x-msdownload");
        MimeMapping.Add(".dll.config", "text/xml");
        MimeMapping.Add(".dlm", "text/dlm");
        MimeMapping.Add(".doc", "application/msword");
        MimeMapping.Add(".docm", "application/vnd.ms-word.document.macroEnabled.12");
        MimeMapping.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        MimeMapping.Add(".dot", "application/msword");
        MimeMapping.Add(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
        MimeMapping.Add(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
        MimeMapping.Add(".dsp", "application/octet-stream");
        MimeMapping.Add(".dtd", "text/xml");
        MimeMapping.Add(".dvi", "application/x-dvi");
        MimeMapping.Add(".dwf", "drawing/x-dwf");
        MimeMapping.Add(".dwp", "application/octet-stream");
        MimeMapping.Add(".dxr", "application/x-director");
        MimeMapping.Add(".eml", "message/rfc822");
        MimeMapping.Add(".emz", "application/octet-stream");
        MimeMapping.Add(".eot", "application/octet-stream");
        MimeMapping.Add(".eps", "application/postscript");
        MimeMapping.Add(".etx", "text/x-setext");
        MimeMapping.Add(".evy", "application/envoy");
        MimeMapping.Add(".exe", "application/octet-stream");
        MimeMapping.Add(".exe.config", "text/xml");
        MimeMapping.Add(".fdf", "application/vnd.fdf");
        MimeMapping.Add(".fif", "application/fractals");
        MimeMapping.Add(".fla", "application/octet-stream");
        MimeMapping.Add(".flr", "x-world/x-vrml");
        MimeMapping.Add(".flv", "video/x-flv");
        MimeMapping.Add(".gif", "image/gif");
        MimeMapping.Add(".gtar", "application/x-gtar");
        MimeMapping.Add(".gz", "application/x-gzip");
        MimeMapping.Add(".h", "text/plain");
        MimeMapping.Add(".hdf", "application/x-hdf");
        MimeMapping.Add(".hdml", "text/x-hdml");
        MimeMapping.Add(".hhc", "application/x-oleobject");
        MimeMapping.Add(".hhk", "application/octet-stream");
        MimeMapping.Add(".hhp", "application/octet-stream");
        MimeMapping.Add(".hlp", "application/winhlp");
        MimeMapping.Add(".hqx", "application/mac-binhex40");
        MimeMapping.Add(".hta", "application/hta");
        MimeMapping.Add(".htc", "text/x-component");
        MimeMapping.Add(".htm", "text/html");
        MimeMapping.Add(".html", "text/html");
        MimeMapping.Add(".htt", "text/webviewhtml");
        MimeMapping.Add(".hxt", "text/html");
        MimeMapping.Add(".ico", "image/x-icon");
        MimeMapping.Add(".ics", "application/octet-stream");
        MimeMapping.Add(".ief", "image/ief");
        MimeMapping.Add(".iii", "application/x-iphone");
        MimeMapping.Add(".inf", "application/octet-stream");
        MimeMapping.Add(".ins", "application/x-internet-signup");
        MimeMapping.Add(".isp", "application/x-internet-signup");
        MimeMapping.Add(".IVF", "video/x-ivf");
        MimeMapping.Add(".jar", "application/java-archive");
        MimeMapping.Add(".java", "application/octet-stream");
        MimeMapping.Add(".jck", "application/liquidmotion");
        MimeMapping.Add(".jcz", "application/liquidmotion");
        MimeMapping.Add(".jfif", "image/pjpeg");
        MimeMapping.Add(".jpb", "application/octet-stream");
        MimeMapping.Add(".jpe", "image/jpeg");
        MimeMapping.Add(".jpeg", "image/jpeg");
        MimeMapping.Add(".jpg", "image/jpeg");
        MimeMapping.Add(".js", "application/x-javascript");
        MimeMapping.Add(".jsx", "text/jscript");
        MimeMapping.Add(".latex", "application/x-latex");
        MimeMapping.Add(".lit", "application/x-ms-reader");
        MimeMapping.Add(".lpk", "application/octet-stream");
        MimeMapping.Add(".lsf", "video/x-la-asf");
        MimeMapping.Add(".lsx", "video/x-la-asf");
        MimeMapping.Add(".lzh", "application/octet-stream");
        MimeMapping.Add(".m13", "application/x-msmediaview");
        MimeMapping.Add(".m14", "application/x-msmediaview");
        MimeMapping.Add(".m1v", "video/mpeg");
        MimeMapping.Add(".m3u", "audio/x-mpegurl");
        MimeMapping.Add(".man", "application/x-troff-man");
        MimeMapping.Add(".manifest", "application/x-ms-manifest");
        MimeMapping.Add(".map", "text/plain");
        MimeMapping.Add(".mdb", "application/x-msaccess");
        MimeMapping.Add(".mdp", "application/octet-stream");
        MimeMapping.Add(".me", "application/x-troff-me");
        MimeMapping.Add(".mht", "message/rfc822");
        MimeMapping.Add(".mhtml", "message/rfc822");
        MimeMapping.Add(".mid", "audio/mid");
        MimeMapping.Add(".midi", "audio/mid");
        MimeMapping.Add(".mix", "application/octet-stream");
        MimeMapping.Add(".mmf", "application/x-smaf");
        MimeMapping.Add(".mno", "text/xml");
        MimeMapping.Add(".mny", "application/x-msmoney");
        MimeMapping.Add(".mov", "video/quicktime");
        MimeMapping.Add(".movie", "video/x-sgi-movie");
        MimeMapping.Add(".mp2", "video/mpeg");
        MimeMapping.Add(".mp3", "audio/mpeg");
        MimeMapping.Add(".mpa", "video/mpeg");
        MimeMapping.Add(".mpe", "video/mpeg");
        MimeMapping.Add(".mpeg", "video/mpeg");
        MimeMapping.Add(".mpg", "video/mpeg");
        MimeMapping.Add(".mpp", "application/vnd.ms-project");
        MimeMapping.Add(".mpv2", "video/mpeg");
        MimeMapping.Add(".ms", "application/x-troff-ms");
        MimeMapping.Add(".msi", "application/octet-stream");
        MimeMapping.Add(".mso", "application/octet-stream");
        MimeMapping.Add(".mvb", "application/x-msmediaview");
        MimeMapping.Add(".mvc", "application/x-miva-compiled");
        MimeMapping.Add(".nc", "application/x-netcdf");
        MimeMapping.Add(".nsc", "video/x-ms-asf");
        MimeMapping.Add(".nws", "message/rfc822");
        MimeMapping.Add(".ocx", "application/octet-stream");
        MimeMapping.Add(".oda", "application/oda");
        MimeMapping.Add(".odc", "text/x-ms-odc");
        MimeMapping.Add(".ods", "application/oleobject");
        MimeMapping.Add(".one", "application/onenote");
        MimeMapping.Add(".onea", "application/onenote");
        MimeMapping.Add(".onetoc", "application/onenote");
        MimeMapping.Add(".onetoc2", "application/onenote");
        MimeMapping.Add(".onetmp", "application/onenote");
        MimeMapping.Add(".onepkg", "application/onenote");
        MimeMapping.Add(".osdx", "application/opensearchdescription+xml");
        MimeMapping.Add(".p10", "application/pkcs10");
        MimeMapping.Add(".p12", "application/x-pkcs12");
        MimeMapping.Add(".p7b", "application/x-pkcs7-certificates");
        MimeMapping.Add(".p7c", "application/pkcs7-mime");
        MimeMapping.Add(".p7m", "application/pkcs7-mime");
        MimeMapping.Add(".p7r", "application/x-pkcs7-certreqresp");
        MimeMapping.Add(".p7s", "application/pkcs7-signature");
        MimeMapping.Add(".pbm", "image/x-portable-bitmap");
        MimeMapping.Add(".pcx", "application/octet-stream");
        MimeMapping.Add(".pcz", "application/octet-stream");
        MimeMapping.Add(".pdf", "application/pdf");
        MimeMapping.Add(".pfb", "application/octet-stream");
        MimeMapping.Add(".pfm", "application/octet-stream");
        MimeMapping.Add(".pfx", "application/x-pkcs12");
        MimeMapping.Add(".pgm", "image/x-portable-graymap");
        MimeMapping.Add(".pko", "application/vnd.ms-pki.pko");
        MimeMapping.Add(".pma", "application/x-perfmon");
        MimeMapping.Add(".pmc", "application/x-perfmon");
        MimeMapping.Add(".pml", "application/x-perfmon");
        MimeMapping.Add(".pmr", "application/x-perfmon");
        MimeMapping.Add(".pmw", "application/x-perfmon");
        MimeMapping.Add(".png", "image/png");
        MimeMapping.Add(".pnm", "image/x-portable-anymap");
        MimeMapping.Add(".pnz", "image/png");
        MimeMapping.Add(".pot", "application/vnd.ms-powerpoint");
        MimeMapping.Add(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12");
        MimeMapping.Add(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
        MimeMapping.Add(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12");
        MimeMapping.Add(".ppm", "image/x-portable-pixmap");
        MimeMapping.Add(".pps", "application/vnd.ms-powerpoint");
        MimeMapping.Add(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
        MimeMapping.Add(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
        MimeMapping.Add(".ppt", "application/vnd.ms-powerpoint");
        MimeMapping.Add(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
        MimeMapping.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        MimeMapping.Add(".prf", "application/pics-rules");
        MimeMapping.Add(".prm", "application/octet-stream");
        MimeMapping.Add(".prx", "application/octet-stream");
        MimeMapping.Add(".ps", "application/postscript");
        MimeMapping.Add(".psd", "application/octet-stream");
        MimeMapping.Add(".psm", "application/octet-stream");
        MimeMapping.Add(".psp", "application/octet-stream");
        MimeMapping.Add(".pub", "application/x-mspublisher");
        MimeMapping.Add(".qt", "video/quicktime");
        MimeMapping.Add(".qtl", "application/x-quicktimeplayer");
        MimeMapping.Add(".qxd", "application/octet-stream");
        MimeMapping.Add(".ra", "audio/x-pn-realaudio");
        MimeMapping.Add(".ram", "audio/x-pn-realaudio");
        MimeMapping.Add(".rar", "application/octet-stream");
        MimeMapping.Add(".ras", "image/x-cmu-raster");
        MimeMapping.Add(".rf", "image/vnd.rn-realflash");
        MimeMapping.Add(".rgb", "image/x-rgb");
        MimeMapping.Add(".rm", "application/vnd.rn-realmedia");
        MimeMapping.Add(".rmi", "audio/mid");
        MimeMapping.Add(".roff", "application/x-troff");
        MimeMapping.Add(".rpm", "audio/x-pn-realaudio-plugin");
        MimeMapping.Add(".rtf", "application/rtf");
        MimeMapping.Add(".rtx", "text/richtext");
        MimeMapping.Add(".scd", "application/x-msschedule");
        MimeMapping.Add(".sct", "text/scriptlet");
        MimeMapping.Add(".sea", "application/octet-stream");
        MimeMapping.Add(".setpay", "application/set-payment-initiation");
        MimeMapping.Add(".setreg", "application/set-registration-initiation");
        MimeMapping.Add(".sgml", "text/sgml");
        MimeMapping.Add(".sh", "application/x-sh");
        MimeMapping.Add(".shar", "application/x-shar");
        MimeMapping.Add(".sit", "application/x-stuffit");
        MimeMapping.Add(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12");
        MimeMapping.Add(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
        MimeMapping.Add(".smd", "audio/x-smd");
        MimeMapping.Add(".smi", "application/octet-stream");
        MimeMapping.Add(".smx", "audio/x-smd");
        MimeMapping.Add(".smz", "audio/x-smd");
        MimeMapping.Add(".snd", "audio/basic");
        MimeMapping.Add(".snp", "application/octet-stream");
        MimeMapping.Add(".spc", "application/x-pkcs7-certificates");
        MimeMapping.Add(".spl", "application/futuresplash");
        MimeMapping.Add(".src", "application/x-wais-source");
        MimeMapping.Add(".ssm", "application/streamingmedia");
        MimeMapping.Add(".sst", "application/vnd.ms-pki.certstore");
        MimeMapping.Add(".stl", "application/vnd.ms-pki.stl");
        MimeMapping.Add(".sv4cpio", "application/x-sv4cpio");
        MimeMapping.Add(".sv4crc", "application/x-sv4crc");
        MimeMapping.Add(".swf", "application/x-shockwave-flash");
        MimeMapping.Add(".t", "application/x-troff");
        MimeMapping.Add(".tar", "application/x-tar");
        MimeMapping.Add(".tcl", "application/x-tcl");
        MimeMapping.Add(".tex", "application/x-tex");
        MimeMapping.Add(".texi", "application/x-texinfo");
        MimeMapping.Add(".texinfo", "application/x-texinfo");
        MimeMapping.Add(".tgz", "application/x-compressed");
        MimeMapping.Add(".thmx", "application/vnd.ms-officetheme");
        MimeMapping.Add(".thn", "application/octet-stream");
        MimeMapping.Add(".tif", "image/tiff");
        MimeMapping.Add(".tiff", "image/tiff");
        MimeMapping.Add(".toc", "application/octet-stream");
        MimeMapping.Add(".tr", "application/x-troff");
        MimeMapping.Add(".trm", "application/x-msterminal");
        MimeMapping.Add(".tsv", "text/tab-separated-values");
        MimeMapping.Add(".ttf", "application/octet-stream");
        MimeMapping.Add(".txt", "text/plain");
        MimeMapping.Add(".u32", "application/octet-stream");
        MimeMapping.Add(".uls", "text/iuls");
        MimeMapping.Add(".ustar", "application/x-ustar");
        MimeMapping.Add(".vbs", "text/vbscript");
        MimeMapping.Add(".vcf", "text/x-vcard");
        MimeMapping.Add(".vcs", "text/plain");
        MimeMapping.Add(".vdx", "application/vnd.ms-visio.viewer");
        MimeMapping.Add(".vml", "text/xml");
        MimeMapping.Add(".vsd", "application/vnd.visio");
        MimeMapping.Add(".vss", "application/vnd.visio");
        MimeMapping.Add(".vst", "application/vnd.visio");
        MimeMapping.Add(".vsto", "application/x-ms-vsto");
        MimeMapping.Add(".vsw", "application/vnd.visio");
        MimeMapping.Add(".vsx", "application/vnd.visio");
        MimeMapping.Add(".vtx", "application/vnd.visio");
        MimeMapping.Add(".wav", "audio/wav");
        MimeMapping.Add(".wax", "audio/x-ms-wax");
        MimeMapping.Add(".wbmp", "image/vnd.wap.wbmp");
        MimeMapping.Add(".wcm", "application/vnd.ms-works");
        MimeMapping.Add(".wdb", "application/vnd.ms-works");
        MimeMapping.Add(".wks", "application/vnd.ms-works");
        MimeMapping.Add(".wm", "video/x-ms-wm");
        MimeMapping.Add(".wma", "audio/x-ms-wma");
        MimeMapping.Add(".wmd", "application/x-ms-wmd");
        MimeMapping.Add(".wmf", "application/x-msmetafile");
        MimeMapping.Add(".wml", "text/vnd.wap.wml");
        MimeMapping.Add(".wmlc", "application/vnd.wap.wmlc");
        MimeMapping.Add(".wmls", "text/vnd.wap.wmlscript");
        MimeMapping.Add(".wmlsc", "application/vnd.wap.wmlscriptc");
        MimeMapping.Add(".wmp", "video/x-ms-wmp");
        MimeMapping.Add(".wmv", "video/x-ms-wmv");
        MimeMapping.Add(".wmx", "video/x-ms-wmx");
        MimeMapping.Add(".wmz", "application/x-ms-wmz");
        MimeMapping.Add(".wps", "application/vnd.ms-works");
        MimeMapping.Add(".wri", "application/x-mswrite");
        MimeMapping.Add(".wrl", "x-world/x-vrml");
        MimeMapping.Add(".wrz", "x-world/x-vrml");
        MimeMapping.Add(".wsdl", "text/xml");
        MimeMapping.Add(".wvx", "video/x-ms-wvx");
        MimeMapping.Add(".x", "application/directx");
        MimeMapping.Add(".xaf", "x-world/x-vrml");
        MimeMapping.Add(".xaml", "application/xaml+xml");
        MimeMapping.Add(".xap", "application/x-silverlight-app");
        MimeMapping.Add(".xbap", "application/x-ms-xbap");
        MimeMapping.Add(".xbm", "image/x-xbitmap");
        MimeMapping.Add(".xdr", "text/plain");
        MimeMapping.Add(".xla", "application/vnd.ms-excel");
        MimeMapping.Add(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
        MimeMapping.Add(".xlc", "application/vnd.ms-excel");
        MimeMapping.Add(".xlm", "application/vnd.ms-excel");
        MimeMapping.Add(".xls", "application/vnd.ms-excel");
        MimeMapping.Add(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
        MimeMapping.Add(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
        MimeMapping.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        MimeMapping.Add(".xlt", "application/vnd.ms-excel");
        MimeMapping.Add(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
        MimeMapping.Add(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
        MimeMapping.Add(".xlw", "application/vnd.ms-excel");
        MimeMapping.Add(".xml", "text/xml");
        MimeMapping.Add(".xof", "x-world/x-vrml");
        MimeMapping.Add(".xpm", "image/x-xpixmap");
        MimeMapping.Add(".xps", "application/vnd.ms-xpsdocument");
        MimeMapping.Add(".xsd", "text/xml");
        MimeMapping.Add(".xsf", "text/xml");
        MimeMapping.Add(".xsl", "text/xml");
        MimeMapping.Add(".xslt", "text/xml");
        MimeMapping.Add(".xsn", "application/octet-stream");
        MimeMapping.Add(".xtp", "application/octet-stream");
        MimeMapping.Add(".xwd", "image/x-xwindowdump");
        MimeMapping.Add(".z", "application/x-compress");
        MimeMapping.Add(".zip", "application/x-zip-compressed");
    }
}
