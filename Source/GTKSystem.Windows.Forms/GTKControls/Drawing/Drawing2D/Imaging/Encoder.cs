namespace System.Drawing.Imaging;

/// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object encapsulates a globally unique identifier (GUID) that identifies the category of an image encoder parameter.</summary>
public sealed class Encoder
{
    /// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the compression parameter category.</summary>
    public static readonly Encoder compression = new(new Guid(-526552163, -13100, 17646, [142, 186, 63, 191, 139, 228, 252, 88]));

    /// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the color depth parameter category.</summary>
    public static readonly Encoder colorDepth = new(new Guid(1711829077, -21146, 19580, [154, 24, 56, 162, 49, 11, 131, 55]));

    /// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the scan method parameter category.</summary>
    public static readonly Encoder scanMethod = new(new Guid(978200161, 12553, 20054, [133, 54, 66, 193, 86, 231, 220, 250]));

    /// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the version parameter category.</summary>
    public static readonly Encoder version = new(new Guid(617712758, -32438, 16804, [191, 83, 28, 33, 156, 204, 247, 151]));

    /// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the render method parameter category.</summary>
    public static readonly Encoder renderMethod = new(new Guid(1833092410, 8858, 18469, [139, 183, 92, 153, 226, 185, 168, 184]));

    /// <summary>Gets an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the quality parameter category.</summary>
    public static readonly Encoder quality = new(new Guid(492561589, -1462, 17709, [156, 221, 93, 179, 81, 5, 231, 235]));

    /// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the transformation parameter category.</summary>
    public static readonly Encoder transformation = new(new Guid(-1928416559, -23154, 20136, [170, 20, 16, 128, 116, 183, 182, 249]));

    /// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the luminance table parameter category.</summary>
    public static readonly Encoder luminanceTable = new(new Guid(-307020850, 614, 19063, [185, 4, 39, 33, 96, 153, 231, 23]));

    /// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the chrominance table parameter category.</summary>
    public static readonly Encoder chrominanceTable = new(new Guid(-219916836, 2483, 17174, [130, 96, 103, 106, 218, 50, 72, 28]));

    /// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the save flag parameter category.</summary>
    public static readonly Encoder saveFlag = new(new Guid(690120444, -21440, 18367, [140, 252, 168, 91, 137, 166, 85, 222]));

    public static readonly Encoder colorSpace = new(new Guid(-1367711072, -4564, 18904, [157, 7, 27, 168, 169, 39, 89, 110]));

    public static readonly Encoder imageItems = new(new Guid(1669815827, 7965, 17835, [145, 149, 162, 155, 96, 102, 166, 80]));

    public static readonly Encoder saveAsCmyk = new(new Guid(-1575371831, 2717, 16389, [163, 238, 58, 66, 27, 139, 176, 108]));

    private readonly Guid _guid;

    /// <summary>Gets a globally unique identifier (GUID) that identifies an image encoder parameter category.</summary>
    /// <returns>The GUID that identifies an image encoder parameter category.</returns>
    public Guid Guid => _guid;

    /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Encoder" /> class from the specified globally unique identifier (GUID). The GUID specifies an image encoder parameter category.</summary>
    /// <param name="guid">A globally unique identifier that identifies an image encoder parameter category.</param>
    public Encoder(Guid guid)
    {
        _guid = guid;
    }
}