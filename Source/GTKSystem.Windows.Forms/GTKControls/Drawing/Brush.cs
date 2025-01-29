namespace System.Drawing
{
    /// <summary>Defines objects used to fill the interiors of graphical shapes such as rectangles, ellipses, pies, polygons, and paths.</summary>
    public abstract class Brush : MarshalByRefObject, ICloneable, IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Brush" /> class.</summary>
        protected Brush()
        {
        }

        /// <summary>When overridden in a derived class, creates an exact copy of this <see cref="T:System.Drawing.Brush" />.</summary>
        /// <returns>The new <see cref="T:System.Drawing.Brush" /> that this method creates.</returns>
        public abstract object Clone();

        /// <summary>Releases all resources used by this <see cref="T:System.Drawing.Brush" /> object.</summary>
        public void Dispose()
        {
        }

        /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Brush" /> and optionally releases the managed resources.</summary>
        /// <param name="disposing">
        ///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~Brush()
        {
        }

        /// <summary>In a derived class, sets a reference to a GDI+ brush object.</summary>
        /// <param name="brush">A pointer to the GDI+ brush object.</param>
        protected internal void SetNativeBrush(IntPtr brush)
        {
        }
    }
}