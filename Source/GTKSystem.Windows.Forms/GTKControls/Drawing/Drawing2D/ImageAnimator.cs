using System.Diagnostics;
using System.Drawing.Imaging;

namespace System.Drawing;

/// <summary>Animates an image that has time-based frames.</summary>
public sealed class ImageAnimator
{
    private sealed class ImageInfo
    {
        private const int propertyTagFrameDelay = 20736;

        private const int propertyTagLoopCount = 20737;

        private readonly Image _image;

        private int _frame;

        private short _loop;

        private readonly int _frameCount;

        private readonly short _loopCount;

        private bool _frameDirty;

        private readonly bool _animated;

        private EventHandler? _onFrameChangedHandler;

        private readonly long[]? _frameEndTimes;

        private readonly long _totalAnimationTime;

        private long _frameTimer;

        public bool Animated => _animated;

        public bool FrameDirty => _frameDirty;

        public EventHandler? FrameChangedHandler
        {
            get => _onFrameChangedHandler;
            set => _onFrameChangedHandler = value;
        }

        private long TotalAnimationTime
        {
            get
            {
                if (!Animated)
                {
                    return 0L;
                }
                return _totalAnimationTime;
            }
        }

        private bool ShouldAnimate
        {
            get
            {
                if (TotalAnimationTime <= 0)
                {
                    return false;
                }
                if (_loopCount != 0)
                {
                    return _loop <= _loopCount;
                }
                return true;
            }
        }

        internal Image Image => _image;

			public ImageInfo(Image image)
			{
				_image = image;
				_animated = CanAnimate(image);
				_frameEndTimes = null;
				if (_animated)
				{
					_frameCount = image.GetFrameCount(FrameDimension.Time);
					var propertyItem = image.GetPropertyItem(20736);
					if (propertyItem != null)
					{
						var value = propertyItem.Value;
						_frameEndTimes = new long[_frameCount];
						var num = 0L;
						var num2 = 0;
						var num3 = 0;
						while (num2 < _frameCount)
						{
							if (num3 >= value?.Length)
							{
								num3 = 0;
							}

                            if (value != null)
                            {
                                var num4 = BitConverter.ToInt32(value, num3) * 10;
                                num += ((num4 > 0) ? num4 : 40);
                            }

                            if (num < _totalAnimationTime)
							{
								num = _totalAnimationTime;
							}
							else
							{
								_totalAnimationTime = num;
							}
							_frameEndTimes[num2] = num;
							num2++;
							num3 += 4;
						}
					}
					var propertyItem2 = image.GetPropertyItem(20737);
					if (propertyItem2 != null)
                    {
                        var value2 = propertyItem2.Value;
                        if (value2 != null)
                        {
                            _loopCount = BitConverter.ToInt16(value2, 0);
                        }
                    }
					else
					{
						_loopCount = 0;
					}
				}
				else
				{
					_frameCount = 1;
				}
			}

        public void AdvanceAnimationBy(long milliseconds)
        {
            if (!ShouldAnimate)
            {
                return;
            }
            var frame = _frame;
            _frameTimer += milliseconds;
            if (_frameTimer > TotalAnimationTime)
            {
                _loop += (short)Math.DivRem(_frameTimer, TotalAnimationTime, out var result);
                _frameTimer = result;
                if (!ShouldAnimate)
                {
                    _frame = _frameCount - 1;
                    _frameTimer = TotalAnimationTime;
                }
                else if (_frame > 0 && _frameEndTimes != null && _frameTimer < _frameEndTimes[_frame - 1])
                {
                    _frame = 0;
                }
            }
            while (_frameEndTimes != null && _frameTimer > _frameEndTimes[_frame])
            {
                _frame++;
            }
            if (_frame != frame)
            {
                _frameDirty = true;
                OnFrameChanged(EventArgs.Empty);
            }
        }

        internal void UpdateFrame()
        {
            if (_frameDirty)
            {
                _image.SelectActiveFrame(FrameDimension.Time, _frame);
                _frameDirty = false;
            }
        }

        private void OnFrameChanged(EventArgs e)
        {
            _onFrameChangedHandler?.Invoke(_image, e);
        }
    }

    internal const int animationDelayMs = 40;

    private static List<ImageInfo>? imageInfoList;

    private static bool anyFrameDirty;

    private static Thread? animationThread;

    private static readonly ReaderWriterLock rwImgListLock = new();

    [ThreadStatic]
    private static int tThreadWriterLockWaitCount;

    private ImageAnimator()
    {
    }

    /// <summary>Advances the frame in the specified image. The new frame is drawn the next time the image is rendered. This method applies only to images with time-based frames.</summary>
    /// <param name="image">The <see cref="T:System.Drawing.Image" /> object for which to update frames.</param>
    public static void UpdateFrames(Image image)
    {
        if (image == null || imageInfoList == null || tThreadWriterLockWaitCount > 0)
        {
            return;
        }
        rwImgListLock.AcquireReaderLock(-1);
        try
        {
            var flag = false;
            var flag2 = false;
            foreach (var imageInfo in imageInfoList)
            {
                if (imageInfo.Image == image)
                {
                    if (imageInfo.FrameDirty)
                    {
                        lock (imageInfo.Image)
                        {
                            imageInfo.UpdateFrame();
                        }
                    }
                    flag2 = true;
                }
                else if (imageInfo.FrameDirty)
                {
                    flag = true;
                }
                if (flag && flag2)
                {
                    break;
                }
            }
            anyFrameDirty = flag;
        }
        finally
        {
            rwImgListLock.ReleaseReaderLock();
        }
    }

    /// <summary>Advances the frame in all images currently being animated. The new frame is drawn the next time the image is rendered.</summary>
    public static void UpdateFrames()
    {
        if (!anyFrameDirty || imageInfoList == null || tThreadWriterLockWaitCount > 0)
        {
            return;
        }
        rwImgListLock.AcquireReaderLock(-1);
        try
        {
            foreach (var imageInfo in imageInfoList)
            {
                lock (imageInfo.Image)
                {
                    imageInfo.UpdateFrame();
                }
            }
            anyFrameDirty = false;
        }
        finally
        {
            rwImgListLock.ReleaseReaderLock();
        }
    }

    /// <summary>Displays a multiple-frame image as an animation.</summary>
    /// <param name="image">The <see cref="T:System.Drawing.Image" /> object to animate.</param>
    /// <param name="onFrameChangedHandler">An <see langword="EventHandler" /> object that specifies the method that is called when the animation frame changes.</param>
    public static void Animate(Image image, EventHandler? onFrameChangedHandler)
    {
        if (image == null)
        {
            return;
        }
        ImageInfo imageInfo;
        lock (image)
        {
            imageInfo = new ImageInfo(image);
        }
        StopAnimate(image, onFrameChangedHandler);
        var isReaderLockHeld = rwImgListLock.IsReaderLockHeld;
        var lockCookie = default(LockCookie);
        tThreadWriterLockWaitCount++;
        try
        {
            if (isReaderLockHeld)
            {
                lockCookie = rwImgListLock.UpgradeToWriterLock(-1);
            }
            else
            {
                rwImgListLock.AcquireWriterLock(-1);
            }
        }
        finally
        {
            tThreadWriterLockWaitCount--;
        }
        try
        {
            if (imageInfo.Animated)
            {
                if (imageInfoList == null)
                {
                    imageInfoList = [];
                }
                imageInfo.FrameChangedHandler = onFrameChangedHandler;
                imageInfoList.Add(imageInfo);
                if (animationThread == null)
                {
                    animationThread = new Thread(AnimateImages)
                    {
                        Name = "ImageAnimator",
                        IsBackground = true
                    };
                    animationThread.Start();
                }
            }
        }
        finally
        {
            if (isReaderLockHeld)
            {
                rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
            }
            else
            {
                rwImgListLock.ReleaseWriterLock();
            }
        }
    }

    /// <summary>Returns a Boolean value indicating whether the specified image contains time-based frames.</summary>
    /// <param name="image">The <see cref="T:System.Drawing.Image" /> object to test.</param>
    /// <returns>This method returns <see langword="true" /> if the specified image contains time-based frames; otherwise, <see langword="false" />.</returns>
    public static bool CanAnimate(Image image)
    {
        if (image == null)
        {
            return false;
        }
        lock (image)
        {
            var frameDimensionsList = image.FrameDimensionsList;
            var array = frameDimensionsList;
            if (array != null)
            {
                foreach (var guid in array)
                {
                    var frameDimension = new FrameDimension(guid);
                    if (frameDimension.Equals(FrameDimension.Time))
                    {
                        return image.GetFrameCount(FrameDimension.Time) > 1;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>Terminates a running animation.</summary>
    /// <param name="image">The <see cref="T:System.Drawing.Image" /> object to stop animating.</param>
    /// <param name="onFrameChangedHandler">An <see langword="EventHandler" /> object that specifies the method that is called when the animation frame changes.</param>
    public static void StopAnimate(Image image, EventHandler? onFrameChangedHandler)
    {
        if (image == null || imageInfoList == null)
        {
            return;
        }
        var isReaderLockHeld = rwImgListLock.IsReaderLockHeld;
        var lockCookie = default(LockCookie);
        tThreadWriterLockWaitCount++;
        try
        {
            if (isReaderLockHeld)
            {
                lockCookie = rwImgListLock.UpgradeToWriterLock(-1);
            }
            else
            {
                rwImgListLock.AcquireWriterLock(-1);
            }
        }
        finally
        {
            tThreadWriterLockWaitCount--;
        }
        try
        {
            for (var i = 0; i < imageInfoList.Count; i++)
            {
                var imageInfo = imageInfoList[i];
                if (image == imageInfo.Image)
                {
                    if ((Delegate?)onFrameChangedHandler == (Delegate?)imageInfo.FrameChangedHandler || (onFrameChangedHandler != null && onFrameChangedHandler.Equals(imageInfo.FrameChangedHandler)))
                    {
                        imageInfoList.Remove(imageInfo);
                    }
                    break;
                }
            }
        }
        finally
        {
            if (isReaderLockHeld)
            {
                rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
            }
            else
            {
                rwImgListLock.ReleaseWriterLock();
            }
        }
    }

    private static void AnimateImages()
    {
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            Thread.Sleep(40);
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
            rwImgListLock.AcquireReaderLock(-1);
            try
            {
                for (var i = 0; i < imageInfoList?.Count; i++)
                {
                    var imageInfo = imageInfoList[i];
                    if (imageInfo.Animated)
                    {
                        imageInfo.AdvanceAnimationBy(elapsedMilliseconds);
                        if (imageInfo.FrameDirty)
                        {
                            anyFrameDirty = true;
                        }
                    }
                }
            }
            finally
            {
                rwImgListLock.ReleaseReaderLock();
            }
        }
    }
}