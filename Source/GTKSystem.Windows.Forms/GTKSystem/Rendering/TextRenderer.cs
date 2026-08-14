// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using Cairo;
using Pango;

namespace System.Windows.Forms
{
    /// <summary>
    /// Provides methods for measuring and rendering text using the GTK/Pango backend,
    /// closely mirroring the native Windows <see cref="TextRenderer"/> behavior.
    /// </summary>
    public sealed class TextRenderer
    {
        private const string CJK_FALLBACK = ",Noto Sans Mono CJK SC,WenQuanYi Micro Hei,WenQuanYi Zen Hei,Noto Sans CJK SC,Noto Sans CJK,Droid Sans Fallback,Microsoft YaHei,SimHei,AR PL UKai CN";

        private TextRenderer() { }

        #region DrawText
        /// <summary>Draws the specified text at the specified point using the specified font and color.</summary>
        public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor)
        {
            DrawText(g, text, font, new System.Drawing.Rectangle(pt.X, pt.Y, int.MaxValue / 2, int.MaxValue / 2), foreColor, TextFormatFlags.Default);
        }

        /// <summary>Draws the specified text at the specified point using the specified font, color and format flags.</summary>
        public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor, TextFormatFlags flags)
        {
            DrawText(g, text, font, new System.Drawing.Rectangle(pt.X, pt.Y, int.MaxValue / 2, int.MaxValue / 2), foreColor, flags);
        }

        /// <summary>Draws the specified text within the specified bounds using the specified font and color.</summary>
        public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor)
        {
            DrawText(g, text, font, bounds, foreColor, TextFormatFlags.Default);
        }

        /// <summary>Draws the specified text within the specified bounds using the specified font, color and format flags.</summary>
        public static void DrawText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor, TextFormatFlags flags)
        {
            if (g == null || string.IsNullOrEmpty(text) || font == null)
                return;

            StringFormat format = new StringFormat();
            if (flags.HasFlag(TextFormatFlags.HorizontalCenter))
                format.Alignment = StringAlignment.Center;
            else if (flags.HasFlag(TextFormatFlags.Right))
                format.Alignment = StringAlignment.Far;

            if (flags.HasFlag(TextFormatFlags.VerticalCenter))
                format.LineAlignment = StringAlignment.Center;
            else if (flags.HasFlag(TextFormatFlags.Bottom))
                format.LineAlignment = StringAlignment.Far;

            g.DrawString(text, font, new SolidBrush(foreColor), new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height), format);
        }

        /// <summary>Draws text using an <see cref="IDeviceContext"/> (e.g. a <see cref="Graphics"/>).</summary>
        public static void DrawText(IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Point pt, System.Drawing.Color foreColor)
        {
            if (dc is System.Drawing.Graphics g)
                DrawText(g, text, font, pt, foreColor);
        }

        /// <summary>Draws text using an <see cref="IDeviceContext"/> (e.g. a <see cref="Graphics"/>).</summary>
        public static void DrawText(IDeviceContext dc, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, System.Drawing.Color foreColor, TextFormatFlags flags)
        {
            if (dc is System.Drawing.Graphics g)
                DrawText(g, text, font, bounds, foreColor, flags);
        }
        #endregion

        #region MeasureText
        /// <summary>Measures the specified text when drawn with the specified font.</summary>
        public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font)
        {
            return MeasureText(text, font, new System.Drawing.Size(int.MaxValue / 2, int.MaxValue / 2), TextFormatFlags.Default);
        }

        /// <summary>Measures the specified text when drawn with the specified font and format flags.</summary>
        public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font, TextFormatFlags flags)
        {
            return MeasureText(text, font, new System.Drawing.Size(int.MaxValue / 2, int.MaxValue / 2), flags);
        }

        /// <summary>Measures the specified text when drawn with the specified font within the specified available area.</summary>
        public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font, System.Drawing.Size proposedSize)
        {
            return MeasureText(text, font, proposedSize, TextFormatFlags.Default);
        }

        /// <summary>Measures the specified text when drawn with the specified font within the specified available area and format flags.</summary>
        public static System.Drawing.Size MeasureText(string text, System.Drawing.Font font, System.Drawing.Size proposedSize, TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text) || font == null)
                return System.Drawing.Size.Empty;

            using Cairo.ImageSurface imagesurface = new Cairo.ImageSurface(Cairo.Format.Argb32, proposedSize.Width, proposedSize.Height);
            using Cairo.Context imagecontext = new Cairo.Context(imagesurface);
            System.Drawing.Graphics grap = new System.Drawing.Graphics(imagecontext, new Gdk.Rectangle(0, 0, proposedSize.Width, proposedSize.Height));
            StringFormat format = new StringFormat();
            if (flags.HasFlag(TextFormatFlags.HorizontalCenter))
                format.Alignment = StringAlignment.Center;
            else if (flags.HasFlag(TextFormatFlags.Right))
                format.Alignment = StringAlignment.Far;

            if (flags.HasFlag(TextFormatFlags.VerticalCenter))
                format.LineAlignment = StringAlignment.Center;
            else if (flags.HasFlag(TextFormatFlags.Bottom))
                format.LineAlignment = StringAlignment.Far;

            System.Drawing.SizeF size = grap.MeasureString(text, font, new SizeF(proposedSize.Width, proposedSize.Height), format);
            return new System.Drawing.Size((int)size.Width, (int)size.Height);
        }

        /// <summary>Measures the specified text when drawn on the given <see cref="Graphics"/> with the specified font.</summary>
        public static System.Drawing.Size MeasureText(System.Drawing.Graphics g, string text, System.Drawing.Font font)
        {
            return MeasureText(text, font);
        }

        /// <summary>Measures the specified text when drawn on the given <see cref="Graphics"/> with the specified font and format flags.</summary>
        public static System.Drawing.Size MeasureText(System.Drawing.Graphics g, string text, System.Drawing.Font font, TextFormatFlags flags)
        {
            return MeasureText(text, font, flags);
        }

        /// <summary>Measures the specified text when drawn on the given <see cref="Graphics"/> with the specified font within the proposed area.</summary>
        public static System.Drawing.Size MeasureText(System.Drawing.Graphics g, string text, System.Drawing.Font font, System.Drawing.Size proposedSize, TextFormatFlags flags)
        {
            return MeasureText(text, font, proposedSize, flags);
        }
        #endregion

        #region Internal helpers
        //private static void ConfigureLayout(Pango.Layout layout, string text, System.Drawing.Font font, System.Drawing.Rectangle bounds, TextFormatFlags flags)
        //{
        //    string family = font?.Name ?? "Sans";
        //    double sizeInPoints = font?.Size ?? 14f;
        //    if (font != null)
        //    {
        //        if (font.Unit == GraphicsUnit.Pixel)
        //            sizeInPoints = font.Size * 72f / 96f;
        //        else if (font.Unit == GraphicsUnit.Inch)
        //            sizeInPoints = font.Size * 72f;
        //    }
        //    family += CJK_FALLBACK;
        //    var desc = FontDescription.FromString($"{family} {Math.Max(1, (int)Math.Round(sizeInPoints))}");
        //    if (font != null)
        //    {
        //        desc.Weight = font.Style.HasFlag(FontStyle.Bold) ? Weight.Bold : Weight.Normal;
        //        desc.Style = font.Style.HasFlag(FontStyle.Italic) ? Pango.Style.Italic : Pango.Style.Normal;
        //    }
        //    layout.FontDescription = desc;

        //    // 下划线/删除线通过属性列表设置（FontDescription 不支持这两个属性）
        //    var attrs = new AttrList();
        //    if (font != null && font.Style.HasFlag(FontStyle.Underline))
        //        attrs.Insert(new AttrUnderline(Underline.Single));
        //    if (font != null && font.Style.HasFlag(FontStyle.Strikeout))
        //        attrs.Insert(new AttrStrikethrough(true));
        //    layout.Attributes = attrs;

        //    // 移除 mnemonic 前缀(&)，除非要求保留
        //    string drawText = text;
        //    if ((flags & TextFormatFlags.NoPrefix) == 0 && (flags & TextFormatFlags.PrefixOnly) == 0)
        //        drawText = text.Replace("&", "");
        //    layout.SetText(drawText);

        //    // 对齐方式
        //    if ((flags & TextFormatFlags.HorizontalCenter) != 0 || (flags & TextFormatFlags.Center) != 0)
        //        layout.Alignment = Alignment.Center;
        //    else if ((flags & TextFormatFlags.Right) != 0)
        //        layout.Alignment = Alignment.Right;
        //    else
        //        layout.Alignment = Alignment.Left;

        //    // 换行与省略
        //    bool noWrap = (flags & TextFormatFlags.NoWrap) != 0;
        //    if (bounds.Width > 0 && bounds.Width != int.MaxValue / 2)
        //    {
        //        layout.Width = (int)(bounds.Width * Scale.PangoScale);
        //        if (noWrap)
        //            layout.Wrap = WrapMode.Char; // 宽度受限但不换行：配合 ellipsis 截断
        //        else
        //            layout.Wrap = WrapMode.WordChar;
        //    }
        //    else
        //    {
        //        layout.Width = -1;
        //        layout.Wrap = WrapMode.Word;
        //    }

        //    if ((flags & TextFormatFlags.EndEllipsis) != 0)
        //        layout.Ellipsize = EllipsizeMode.End;
        //    else if ((flags & TextFormatFlags.WordEllipsis) != 0)
        //        layout.Ellipsize = EllipsizeMode.End;
        //    else if ((flags & TextFormatFlags.PathEllipsis) != 0)
        //        layout.Ellipsize = EllipsizeMode.Middle;
        //    else
        //        layout.Ellipsize = EllipsizeMode.None;

        //    if ((flags & TextFormatFlags.RightToLeft) != 0)
        //        layout.AutoDir = false;
        //}
       
        #endregion
    }
}
