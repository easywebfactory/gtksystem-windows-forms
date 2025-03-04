namespace System.Drawing.Imaging;

/// <summary>Specifies types of metafiles. The <see cref="P:System.Drawing.Imaging.MetafileHeader.Type" /> property returns a member of this enumeration.</summary>
public enum MetafileType
{
    /// <summary>Specifies a metafile format that is not recognized in GDI+.</summary>
    Invalid,
    /// <summary>Specifies a WMF (Windows Metafile) file. Such a file contains only GDI records.</summary>
    Wmf,
    /// <summary>Specifies a WMF (Windows Metafile) file that has a placeable metafile header in front of it.</summary>
    WmfPlaceable,
    /// <summary>Specifies an Enhanced Metafile (EMF) file. Such a file contains only GDI records.</summary>
    Emf,
    /// <summary>Specifies an EMF+ file. Such a file contains only GDI+ records and must be displayed by using GDI+. Displaying the records using GDI may cause unpredictable results.</summary>
    EmfPlusOnly,
    /// <summary>Specifies an EMF+ Dual file. Such a file contains GDI+ records along with alternative GDI records and can be displayed by using either GDI or GDI+. Displaying the records using GDI may cause some quality degradation.</summary>
    EmfPlusDual
}