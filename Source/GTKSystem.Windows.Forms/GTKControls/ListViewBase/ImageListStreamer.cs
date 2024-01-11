// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using Cairo;
using Pango;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using static GTKSystem.Resources.ResourceManager;

namespace System.Windows.Forms
{
    [Serializable] // This type is participating in resx serialization scenarios.
    public sealed class ImageListStreamer : ISerializable, IDisposable
    {
        // compressed magic header.  If we see this, the image stream is compressed.
        // (unicode for MSFT).
        private static readonly byte[] HEADER_MAGIC = new byte[] { 0x4D, 0x53, 0x46, 0X74 };
        private static readonly object s_syncObject = new object();
        public ResourceInfo ResourceInfo { get; set; }

        private readonly ImageList _imageList;
        internal ImageListStreamer(ImageList il)
        {
            _imageList = il;
        }

        private ImageListStreamer(SerializationInfo info, StreamingContext context)
        {
            SerializationInfoEnumerator sie = info.GetEnumerator();
            if (sie is null)
            {
                return;
            }

            while (sie.MoveNext())
            {
                if (string.Equals(sie.Name, "Data", StringComparison.OrdinalIgnoreCase))
                {
#if DEBUG
                    try
                    {
#endif
                        byte[] data = (byte[])sie.Value;
                        if (data is not null)
                        {
                            Deserialize(data);
                        }
#if DEBUG
                    }
                    catch (Exception e)
                    {
                        Debug.Fail("ImageList serialization failure: " + e.ToString());
                        throw;
                    }
#endif
                }
            }
        }
        internal ImageListStreamer(Stream stream)
        {
            if (stream is MemoryStream ms
                && ms.TryGetBuffer(out ArraySegment<byte> buffer)
                && buffer.Offset == 0)
            {
                Deserialize(buffer.Array);
            }
            else
            {
                stream.Position = 0;
                using MemoryStream copyStream = new(checked((int)stream.Length));
                stream.CopyTo(copyStream);
                Deserialize(copyStream.GetBuffer());
            }
        }

        /// <summary>
        ///  Compresses the given input, returning a new array that represents
        ///  the compressed data.
        /// </summary>
        private static byte[] Compress(byte[] input)
        {
            int finalLength = 0;
            int idx = 0;
            int compressedIdx = 0;

            while (idx < input.Length)
            {
                byte current = input[idx++];
                byte runLength = 1;

                while (idx < input.Length && input[idx] == current && runLength < 0xFF)
                {
                    runLength++;
                    idx++;
                }

                finalLength += 2;
            }

            byte[] output = new byte[finalLength + HEADER_MAGIC.Length];

            Buffer.BlockCopy(HEADER_MAGIC, 0, output, 0, HEADER_MAGIC.Length);
            int idxOffset = HEADER_MAGIC.Length;
            idx = 0;

            while (idx < input.Length)
            {
                byte current = input[idx++];
                byte runLength = 1;

                while (idx < input.Length && input[idx] == current && runLength < 0xFF)
                {
                    runLength++;
                    idx++;
                }

                output[idxOffset + compressedIdx++] = runLength;
                output[idxOffset + compressedIdx++] = current;
            }

            Debug.Assert(idxOffset + compressedIdx == output.Length, "RLE Compression failure in ImageListStreamer -- didn't fill array");

            // Validate that our compression routine works
#if DEBUG
            byte[] debugCompare = Decompress(output);
            Debug.Assert(debugCompare.Length == input.Length, "RLE Compression in ImageListStreamer is broken.");
            int debugMaxCompare = input.Length;
            for (int debugIdx = 0; debugIdx < debugMaxCompare; debugIdx++)
            {
                if (debugCompare[debugIdx] != input[debugIdx])
                {
                    Debug.Fail("RLE Compression failure in ImageListStreamer at byte offset " + debugIdx);
                    break;
                }
            }
#endif // DEBUG

            return output;
        }

        /// <summary>
        ///  Decompresses the given input, returning a new array that represents
        ///  the uncompressed data.
        /// </summary>
        private static byte[] Decompress(byte[] input)
        {
            int finalLength = 0;
            int idx = 0;
            int outputIdx = 0;

            // Check for our header. If we don't have one,
            // we're not actually decompressed, so just return
            // the original.
            //
            if (input.Length < HEADER_MAGIC.Length)
            {
                return input;
            }

            for (idx = 0; idx < HEADER_MAGIC.Length; idx++)
            {
                if (input[idx] != HEADER_MAGIC[idx])
                {
                    return input;
                }
            }

            // Ok, we passed the magic header test.

            for (idx = HEADER_MAGIC.Length; idx < input.Length; idx += 2)
            {
                finalLength += input[idx];
            }

            byte[] output = new byte[finalLength];

            idx = HEADER_MAGIC.Length;

            while (idx < input.Length)
            {
                byte runLength = input[idx++];
                byte current = input[idx++];

                int startIdx = outputIdx;
                int endIdx = outputIdx + runLength;

                while (startIdx < endIdx)
                {
                    output[startIdx++] = current;
                }

                outputIdx += runLength;
            }

            return output;
        }

        private void Deserialize(byte[] data)
        {
            try
            {
                //using MemoryStream ms = new MemoryStream(Decompress(data));
                //lock (s_syncObject)
                //{ 
                //    //ComCtl32.InitCommonControls();
                //  _nativeImageList = new ImageList.NativeImageList(new Ole32.GPStream(ms));
                //}


            }
            finally
            {

            }

        }

        public void GetObjectData(SerializationInfo si, StreamingContext context)
        {
            using MemoryStream stream = new MemoryStream();
            //if (!WriteImageList(stream))
            //{
            //    throw new InvalidOperationException("ImageListStreamerSaveFailed");
            //}

            si.AddValue("Data", Compress(stream.ToArray()));
        }

        internal void GetObjectData(Stream stream)
        {

        }

        //public static implicit  operator ResourceInfo(ResourceInfo resource)
        //{
        //    try
        //    {
        //        return new ImageListStreamer(new MemoryStream(resource.ImageBytes)) { ResourceInfo = resource };
        //    }
        //    catch
        //    {
        //        SerializationInfo info = new SerializationInfo(typeof(ImageListStreamer), new FormatterConverter());
        //        return new ImageListStreamer(info, new StreamingContext()) { ResourceInfo = resource };
        //    }
        //}
        public static explicit operator ImageListStreamer(ResourceInfo resource)
        {
            try
            {
                return new ImageListStreamer(new MemoryStream(resource.ImageBytes)) { ResourceInfo = resource };
            }
            catch
            {
                SerializationInfo info = new SerializationInfo(typeof(ImageListStreamer), new FormatterConverter());
                return new ImageListStreamer(info, new StreamingContext()) { ResourceInfo = resource };
            }
        }

        /// <summary>
        ///  Disposes the native image list handle.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
}
