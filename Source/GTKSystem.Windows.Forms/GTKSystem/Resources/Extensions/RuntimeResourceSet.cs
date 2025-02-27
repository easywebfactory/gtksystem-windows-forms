using System.Collections;
using System.Resources;

namespace System.Windows.Forms.Resources;

internal sealed class RuntimeResourceSet : ResourceSet, IEnumerable
{
    internal const int version = 2;

    private Dictionary<string?, ResourceLocator>? _resCache;

    private DeserializingResourceReader? _defaultReader;

    private Dictionary<string?, ResourceLocator>? _caseInsensitiveTable;

    private bool _haveReadFromReader;

    private IResourceReader? Reader => _defaultReader;

    internal RuntimeResourceSet(IResourceReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException("reader");
        }
        _defaultReader = reader as DeserializingResourceReader ?? throw new ArgumentException(SR.Format(SR.NotSupportedWrongResourceReaderType, reader.GetType()), "reader");
        var fastResourceComparer = FastResourceComparer.@default;
        _resCache = new Dictionary<string?, ResourceLocator>(fastResourceComparer!);
        _defaultReader._resCache = _resCache;
    }

    protected override void Dispose(bool disposing)
    {
        if (Reader == null)
        {
            return;
        }
        if (disposing)
        {
            lock (Reader)
            {
                _resCache = null;
                if (_defaultReader != null)
                {
                    _defaultReader.Close();
                    _defaultReader = null;
                }
                _caseInsensitiveTable = null;
                base.Dispose(disposing);
            }
        }
        else
        {
            if (_resCache != null)
            {
                lock (_resCache)
                {
                    _resCache = null;
                }
            }

            _caseInsensitiveTable = null;
            _defaultReader = null;
            base.Dispose(disposing);
        }
    }

    public override IDictionaryEnumerator GetEnumerator()
    {
        return GetEnumeratorHelper();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumeratorHelper();
    }

    private IDictionaryEnumerator GetEnumeratorHelper()
    {
        var reader = Reader;
        if (reader == null || _resCache == null)
        {
            throw new ObjectDisposedException(null, SR.ObjectDisposedResourceSet);
        }
        return reader.GetEnumerator();
    }

    public override string? GetString(string? key)
    {
        var @object = GetObject(key, ignoreCase: false, isString: true);
        return @object as string;
    }

    public override string? GetString(string? key, bool ignoreCase)
    {
        var @object = GetObject(key, ignoreCase, isString: true);
        return @object as string;
    }

    public override object? GetObject(string? key)
    {
        return GetObject(key, ignoreCase: false, isString: false);
    }

    public override object? GetObject(string? key, bool ignoreCase)
    {
        return GetObject(key, ignoreCase, isString: false);
    }

    private object? GetObject(string? key, bool ignoreCase, bool isString)
    {
        if (key == null)
        {
            throw new ArgumentNullException("key");
        }
        if (Reader == null || _resCache == null)
        {
            throw new ObjectDisposedException(null, SR.ObjectDisposedResourceSet);
        }
        object? obj = null;
        lock (Reader)
        {
            if (Reader == null)
            {
                throw new ObjectDisposedException(null, SR.ObjectDisposedResourceSet);
            }
            ResourceLocator value;
            if (_defaultReader != null)
            {
                var num = -1;
                if (_resCache.TryGetValue(key, out value))
                {
                    obj = value.Value;
                    num = value.DataPosition;
                }
                if (num == -1 && obj == null)
                {
                    num = _defaultReader.FindPosForResource(key);
                }
                if (num != -1 && obj == null)
                {
                    ResourceTypeCode typeCode;
                    if (isString)
                    {
                        obj = _defaultReader.LoadString(num);
                        typeCode = ResourceTypeCode.String;
                    }
                    else
                    {
                        obj = _defaultReader.LoadObject(num, out typeCode);
                    }
                    value = new ResourceLocator(num, ResourceLocator.CanCache(typeCode) ? obj : null);
                    lock (_resCache)
                    {
                        _resCache[key] = value;
                    }
                }
                if (obj != null || !ignoreCase)
                {
                    return obj;
                }
            }
            if (!_haveReadFromReader)
            {
                if (ignoreCase && _caseInsensitiveTable == null)
                {
                    _caseInsensitiveTable = new Dictionary<string?, ResourceLocator>(StringComparer.OrdinalIgnoreCase);
                }
                if (_defaultReader == null)
                {
                    var enumerator = Reader.GetEnumerator();
                    using var disposable = enumerator as IDisposable;
                    while (enumerator.MoveNext())
                    {
                        var entry = enumerator.Entry;
                        var key2 = (string)entry.Key;
                        var value2 = new ResourceLocator(-1, entry.Value);
                        _resCache.Add(key2, value2);
                        if (ignoreCase)
                        {
                            _caseInsensitiveTable?.Add(key2, value2);
                        }
                    }
                    if (!ignoreCase)
                    {
                        Reader.Close();
                    }
                }
                else
                {
                    var enumeratorInternal = _defaultReader.GetEnumeratorInternal();
                    while (enumeratorInternal.MoveNext())
                    {
                        var key3 = (string?)enumeratorInternal.Key;
                        var dataPosition = enumeratorInternal.DataPosition;
                        var value3 = new ResourceLocator(dataPosition, null);
                        if (key3 != null)
                        {
                            _caseInsensitiveTable?.Add(key3, value3);
                        }
                    }
                }
                _haveReadFromReader = true;
            }
            object? result = null;
            var flag = false;
            var keyInWrongCase = false;
            if (_defaultReader != null && _resCache.TryGetValue(key, out value))
            {
                flag = true;
                result = ResolveResourceLocator(value, key, _resCache, keyInWrongCase);
            }
            if (!flag && ignoreCase && _caseInsensitiveTable != null && _caseInsensitiveTable.TryGetValue(key, out value))
            {
                keyInWrongCase = true;
                result = ResolveResourceLocator(value, key, _resCache, keyInWrongCase);
            }
            return result;
        }
    }

    private object? ResolveResourceLocator(ResourceLocator resLocation, string? key, Dictionary<string?, ResourceLocator>? copyOfCache, bool keyInWrongCase)
    {
        var obj = resLocation.Value;
        if (obj == null)
        {
            ResourceTypeCode typeCode=default;
            if (Reader != null)
            {
                lock (Reader)
                {
                    obj = _defaultReader?.LoadObject(resLocation.DataPosition, out typeCode);
                }
            }

            if (!keyInWrongCase && ResourceLocator.CanCache(typeCode))
            {
                resLocation.Value = obj;
                if (copyOfCache != null && key != null)
                {
                    copyOfCache[key] = resLocation;
                }
            }
        }
        return obj;
    }
}