using System;
using System.Collections.Generic;
using System.Text;

namespace System.Drawing.Drawing2D
{
    //
    // 摘要:
    //     The System.Drawing.Drawing2D.InterpolationMode enumeration specifies the algorithm
    //     that is used when images are scaled or rotated.
    public enum InterpolationMode
    {
        //
        // 摘要:
        //     Equivalent to the System.Drawing.Drawing2D.QualityMode.Invalid element of the
        //     System.Drawing.Drawing2D.QualityMode enumeration.
        Invalid = -1,
        //
        // 摘要:
        //     Specifies default mode.
        Default = 0,
        //
        // 摘要:
        //     Specifies low quality interpolation.
        Low = 1,
        //
        // 摘要:
        //     Specifies high quality interpolation.
        High = 2,
        //
        // 摘要:
        //     Specifies bilinear interpolation. No prefiltering is done. This mode is not suitable
        //     for shrinking an image below 50 percent of its original size.
        Bilinear = 3,
        //
        // 摘要:
        //     Specifies bicubic interpolation. No prefiltering is done. This mode is not suitable
        //     for shrinking an image below 25 percent of its original size.
        Bicubic = 4,
        //
        // 摘要:
        //     Specifies nearest-neighbor interpolation.
        NearestNeighbor = 5,
        //
        // 摘要:
        //     Specifies high-quality, bilinear interpolation. Prefiltering is performed to
        //     ensure high-quality shrinking.
        HighQualityBilinear = 6,
        //
        // 摘要:
        //     Specifies high-quality, bicubic interpolation. Prefiltering is performed to ensure
        //     high-quality shrinking. This mode produces the highest quality transformed images.
        HighQualityBicubic = 7
    }
}
