using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Windows.Forms
{
    /// <summary>
    /// 向当前拥有键盘焦点的应用程序发送按键。
    /// 语法兼容 WinForms SendKeys 的常用功能：
    /// +（Shift）、^（Control）、%（Alt）、~（Enter）、组合键以及 {按键 [重复次数]}。
    /// </summary>
    public static class SendKeys
    {
        private static readonly object SyncRoot = new object();

        private static readonly Dictionary<string, Key> NamedKeys =
            new Dictionary<string, Key>(StringComparer.OrdinalIgnoreCase)
            {
                { "BACKSPACE", Key.Backspace }, { "BS", Key.Backspace }, { "BKSP", Key.Backspace },
                { "TAB", Key.Tab }, { "ENTER", Key.Enter }, { "RETURN", Key.Enter },
                { "ESC", Key.Escape }, { "ESCAPE", Key.Escape }, { "SPACE", Key.Space },
                { "PGUP", Key.PageUp }, { "PRIOR", Key.PageUp },
                { "PGDN", Key.PageDown }, { "NEXT", Key.PageDown },
                { "END", Key.End }, { "HOME", Key.Home },
                { "LEFT", Key.Left }, { "UP", Key.Up }, { "RIGHT", Key.Right }, { "DOWN", Key.Down },
                { "INS", Key.Insert }, { "INSERT", Key.Insert },
                { "DEL", Key.Delete }, { "DELETE", Key.Delete },
                { "HELP", Key.Help }, { "BREAK", Key.Pause }, { "PAUSE", Key.Pause },
                { "CAPSLOCK", Key.CapsLock }, { "NUMLOCK", Key.NumLock },
                { "SCROLLLOCK", Key.ScrollLock }, { "PRTSC", Key.PrintScreen },
                { "PRINTSCREEN", Key.PrintScreen },
                { "ADD", Key.Add }, { "SUBTRACT", Key.Subtract },
                { "MULTIPLY", Key.Multiply }, { "DIVIDE", Key.Divide }, { "DECIMAL", Key.Decimal },
                { "F1", Key.F1 }, { "F2", Key.F2 }, { "F3", Key.F3 }, { "F4", Key.F4 },
                { "F5", Key.F5 }, { "F6", Key.F6 }, { "F7", Key.F7 }, { "F8", Key.F8 },
                { "F9", Key.F9 }, { "F10", Key.F10 }, { "F11", Key.F11 }, { "F12", Key.F12 },
                { "F13", Key.F13 }, { "F14", Key.F14 }, { "F15", Key.F15 }, { "F16", Key.F16 },
                { "F17", Key.F17 }, { "F18", Key.F18 }, { "F19", Key.F19 }, { "F20", Key.F20 },
                { "F21", Key.F21 }, { "F22", Key.F22 }, { "F23", Key.F23 }, { "F24", Key.F24 }
            };

        /// <summary>向当前活动的应用程序发送指定按键。</summary>
        public static void Send(string keys) => SendWait(keys);

        /// <summary>发送指定按键，并等待所有按键完成注入。</summary>
        public static void SendWait(string keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            lock (SyncRoot)
            {
                using (IKeyboardBackend backend = CreateBackend())
                {
                    int position = 0;
                    ParseSequence(keys, ref position, Modifier.None, backend, false);
                    if (position != keys.Length)
                        throw InvalidKeys(keys, position);
                    backend.Flush();
                }
            }
        }

        private static void ParseSequence(string text, ref int position, Modifier inherited,
            IKeyboardBackend backend, bool stopAtParenthesis)
        {
            while (position < text.Length)
            {
                if (text[position] == ')')
                {
                    if (!stopAtParenthesis)
                        throw InvalidKeys(text, position);
                    position++;
                    return;
                }

                Modifier modifiers = inherited;
                while (position < text.Length)
                {
                    char prefix = text[position];
                    if (prefix == '+') modifiers |= Modifier.Shift;
                    else if (prefix == '^') modifiers |= Modifier.Control;
                    else if (prefix == '%') modifiers |= Modifier.Alt;
                    else break;
                    position++;
                }

                if (position >= text.Length)
                    throw InvalidKeys(text, position);

                if (text[position] == '(')
                {
                    position++;
                    ParseSequence(text, ref position, modifiers, backend, true);
                }
                else if (text[position] == '{')
                {
                    ParseBraceExpression(text, ref position, modifiers, backend);
                }
                else
                {
                    char character = text[position++];
                    if (character == '~')
                        backend.SendKey(Key.Enter, modifiers);
                    else
                        backend.SendCharacter(character, modifiers);
                }
            }

            if (stopAtParenthesis)
                throw InvalidKeys(text, position);
        }

        private static void ParseBraceExpression(string text, ref int position, Modifier modifiers,
            IKeyboardBackend backend)
        {
            int openingBrace = position++;
            // {}} 是 SendKeys 中表示右花括号字符的转义序列。
            int closingBrace = position < text.Length - 1 && text[position] == '}' && text[position + 1] == '}'
                ? position + 1
                : text.IndexOf('}', position);
            if (closingBrace < 0)
                throw InvalidKeys(text, openingBrace);

            string expression = text.Substring(position, closingBrace - position);
            position = closingBrace + 1;
            if (expression.Length == 0)
                throw InvalidKeys(text, openingBrace);

            // WinForms 使用 {+}、{^}、{%}、{{} 和 {}} 转义特殊字符。
            if (expression.Length == 1)
            {
                backend.SendCharacter(expression[0], modifiers);
                return;
            }

            string name = expression;
            int repeat = 1;
            int separator = expression.LastIndexOf(' ');
            if (separator > 0)
            {
                int parsedRepeat;
                if (int.TryParse(expression.Substring(separator + 1), out parsedRepeat))
                {
                    if (parsedRepeat < 1)
                        throw InvalidKeys(text, openingBrace);
                    repeat = parsedRepeat;
                    name = expression.Substring(0, separator);
                }
            }

            Key key;
            if (!NamedKeys.TryGetValue(name, out key))
                throw new ArgumentException("Unknown SendKeys key name: {" + name + "}.", nameof(text));

            for (int i = 0; i < repeat; i++)
                backend.SendKey(key, modifiers);
        }

        private static ArgumentException InvalidKeys(string text, int position) =>
            new ArgumentException("Invalid SendKeys expression at position " + position + ": " + text, nameof(text));

        private static IKeyboardBackend CreateBackend()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return new WindowsBackend();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return LinuxBackend.Create();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return new MacBackend();
            throw new PlatformNotSupportedException("SendKeys is supported on Windows, X11 and macOS.");
        }

        [Flags]
        private enum Modifier { None = 0, Shift = 1, Control = 2, Alt = 4 }

        private enum Key
        {
            Backspace, Tab, Enter, Escape, Space, PageUp, PageDown, End, Home,
            Left, Up, Right, Down, Insert, Delete, Help, Pause, CapsLock, NumLock,
            ScrollLock, PrintScreen, Add, Subtract, Multiply, Divide, Decimal,
            F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
            F13, F14, F15, F16, F17, F18, F19, F20, F21, F22, F23, F24
        }

        private interface IKeyboardBackend : IDisposable
        {
            void SendCharacter(char character, Modifier modifiers);
            void SendKey(Key key, Modifier modifiers);
            void Flush();
        }

        private sealed class WindowsBackend : IKeyboardBackend
        {
            private const uint InputKeyboard = 1;
            private const uint KeyUp = 0x0002;
            private const uint Unicode = 0x0004;
            private const uint Extended = 0x0001;

            public void SendCharacter(char character, Modifier modifiers)
            {
                if (character == '\r' || character == '\n') { SendKey(Key.Enter, modifiers); return; }
                if (character == '\t') { SendKey(Key.Tab, modifiers); return; }
                if (character == '\b') { SendKey(Key.Backspace, modifiers); return; }
                if (character == ' ') { SendKey(Key.Space, modifiers); return; }

                short mapped = VkKeyScanW(character);
                if (mapped == -1)
                {
                    WithModifiers(modifiers, () => SendInputPair(0, character, Unicode));
                    return;
                }

                Modifier required = Modifier.None;
                int shiftState = (mapped >> 8) & 0xff;
                if ((shiftState & 1) != 0) required |= Modifier.Shift;
                if ((shiftState & 2) != 0) required |= Modifier.Control;
                if ((shiftState & 4) != 0) required |= Modifier.Alt;
                WithModifiers(modifiers | required, () => SendVirtualKey((ushort)(mapped & 0xff)));
            }

            public void SendKey(Key key, Modifier modifiers) =>
                WithModifiers(modifiers, () => SendVirtualKey(ToVirtualKey(key)));

            public void Flush() { }
            public void Dispose() { }

            private static void WithModifiers(Modifier modifiers, Action action)
            {
                if ((modifiers & Modifier.Shift) != 0) SendKeyEvent(0x10, false, false);
                if ((modifiers & Modifier.Control) != 0) SendKeyEvent(0x11, false, false);
                if ((modifiers & Modifier.Alt) != 0) SendKeyEvent(0x12, false, false);
                try { action(); }
                finally
                {
                    if ((modifiers & Modifier.Alt) != 0) SendKeyEvent(0x12, true, false);
                    if ((modifiers & Modifier.Control) != 0) SendKeyEvent(0x11, true, false);
                    if ((modifiers & Modifier.Shift) != 0) SendKeyEvent(0x10, true, false);
                }
            }

            private static void SendVirtualKey(ushort virtualKey)
            {
                bool extended = virtualKey == 0x21 || virtualKey == 0x22 || virtualKey == 0x23 ||
                    virtualKey == 0x24 || virtualKey == 0x25 || virtualKey == 0x26 || virtualKey == 0x27 ||
                    virtualKey == 0x28 || virtualKey == 0x2D || virtualKey == 0x2E || virtualKey == 0x6F;
                SendKeyEvent(virtualKey, false, extended);
                SendKeyEvent(virtualKey, true, extended);
            }

            private static void SendKeyEvent(ushort virtualKey, bool keyUp, bool extended)
            {
                uint flags = (keyUp ? KeyUp : 0) | (extended ? Extended : 0);
                SendInputPairPart(virtualKey, '\0', flags);
            }

            private static void SendInputPair(ushort virtualKey, char scan, uint flags)
            {
                SendInputPairPart(virtualKey, scan, flags);
                SendInputPairPart(virtualKey, scan, flags | KeyUp);
            }

            private static void SendInputPairPart(ushort virtualKey, char scan, uint flags)
            {
                INPUT input = new INPUT
                {
                    type = InputKeyboard,
                    union = new InputUnion
                    {
                        keyboard = new KEYBDINPUT
                        {
                            virtualKey = virtualKey,
                            scanCode = scan,
                            flags = flags,
                            time = 0,
                            extraInfo = UIntPtr.Zero
                        }
                    }
                };
                if (SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT))) != 1)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to inject keyboard input.");
            }

            private static ushort ToVirtualKey(Key key)
            {
                if (key >= Key.F1 && key <= Key.F24) return (ushort)(0x70 + key - Key.F1);
                switch (key)
                {
                    case Key.Backspace: return 0x08;
                    case Key.Tab: return 0x09;
                    case Key.Enter: return 0x0D;
                    case Key.Escape: return 0x1B;
                    case Key.Space: return 0x20;
                    case Key.PageUp: return 0x21;
                    case Key.PageDown: return 0x22;
                    case Key.End: return 0x23;
                    case Key.Home: return 0x24;
                    case Key.Left: return 0x25;
                    case Key.Up: return 0x26;
                    case Key.Right: return 0x27;
                    case Key.Down: return 0x28;
                    case Key.PrintScreen: return 0x2C;
                    case Key.Insert: return 0x2D;
                    case Key.Delete: return 0x2E;
                    case Key.Help: return 0x2F;
                    case Key.Pause: return 0x13;
                    case Key.CapsLock: return 0x14;
                    case Key.NumLock: return 0x90;
                    case Key.ScrollLock: return 0x91;
                    case Key.Multiply: return 0x6A;
                    case Key.Add: return 0x6B;
                    case Key.Subtract: return 0x6D;
                    case Key.Decimal: return 0x6E;
                    case Key.Divide: return 0x6F;
                    default: throw new ArgumentOutOfRangeException(nameof(key));
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct INPUT { public uint type; public InputUnion union; }
            [StructLayout(LayoutKind.Explicit)]
            private struct InputUnion
            {
                [FieldOffset(0)] public MOUSEINPUT mouse;
                [FieldOffset(0)] public KEYBDINPUT keyboard;
            }
            [StructLayout(LayoutKind.Sequential)]
            private struct MOUSEINPUT
            {
                public int dx, dy; public uint mouseData, flags, time; public UIntPtr extraInfo;
            }
            [StructLayout(LayoutKind.Sequential)]
            private struct KEYBDINPUT
            {
                public ushort virtualKey; public char scanCode; public uint flags, time; public UIntPtr extraInfo;
            }

            [DllImport("user32.dll", SetLastError = true)]
            private static extern uint SendInput(uint count, INPUT[] inputs, int size);
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            private static extern short VkKeyScanW(char character);
        }

        private sealed class LinuxBackend
        {
            private static UInputBackend? sharedUInput;

            public static IKeyboardBackend Create()
            {
                bool wayland = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")) ||
                    string.Equals(Environment.GetEnvironmentVariable("XDG_SESSION_TYPE"), "wayland", StringComparison.OrdinalIgnoreCase);
                if (wayland)
                {
                    try { return GetUInput(); }
                    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                    {
                        // XWayland 应用程序仍然可以接收 XTest 事件。
                        try { return new X11Backend(); }
                        catch (PlatformNotSupportedException)
                        {
                            throw new PlatformNotSupportedException(
                                "Wayland（包括麒麟/UOS）需要授予当前用户 /dev/uinput 写权限，才能向系统输入法发送虚拟按键。", ex);
                        }
                    }
                }

                try { return new X11Backend(); }
                catch (PlatformNotSupportedException)
                {
                    try { return GetUInput(); }
                    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                    {
                        throw new PlatformNotSupportedException(
                            "Linux 需要 X11/libXtst，或授予当前用户 /dev/uinput 写权限。", ex);
                    }
                }
            }

            private static UInputBackend GetUInput()
            {
                if (sharedUInput == null) sharedUInput = new UInputBackend();
                return sharedUInput;
            }
        }

        /// <summary>
        /// 用于麒麟、UOS 及其他 Wayland 桌面环境的内核级虚拟键盘。
        /// 使用物理 EV_KEY 事件，确保 IBus、Fcitx 等输入法能够接收按键。
        /// </summary>
        private sealed class UInputBackend : IKeyboardBackend
        {
            private const int O_WRONLY = 1;
            private const int O_NONBLOCK = 0x800;
            private const uint UI_SET_EVBIT = 0x40045564;
            private const uint UI_SET_KEYBIT = 0x40045565;
            private const uint UI_DEV_CREATE = 0x5501;
            private const uint UI_DEV_DESTROY = 0x5502;
            private const ushort EV_SYN = 0;
            private const ushort EV_KEY = 1;
            private const ushort SYN_REPORT = 0;
            private int descriptor = -1;

            public UInputBackend()
            {
                descriptor = open("/dev/uinput", O_WRONLY | O_NONBLOCK);
                if (descriptor < 0) descriptor = open("/dev/input/uinput", O_WRONLY | O_NONBLOCK);
                if (descriptor < 0)
                    throw CreateLinuxIOException("无法打开 Linux uinput 虚拟键盘设备");

                try
                {
                    Check(ioctl(descriptor, UI_SET_EVBIT, EV_KEY));
                    for (int keyCode = 1; keyCode <= 255; keyCode++)
                        Check(ioctl(descriptor, UI_SET_KEYBIT, keyCode));

                    UInputUserDevice device = new UInputUserDevice
                    {
                        name = "GTKSystem SendKeys",
                        id = new InputId { bustype = 0x03, vendor = 0x4754, product = 0x4B53, version = 1 },
                        absmax = new int[64],
                        absmin = new int[64],
                        absfuzz = new int[64],
                        absflat = new int[64]
                    };
                    int size = Marshal.SizeOf(typeof(UInputUserDevice));
                    IntPtr memory = Marshal.AllocHGlobal(size);
                    try
                    {
                        Marshal.StructureToPtr(device, memory, false);
                        long written = write(descriptor, memory, new UIntPtr((uint)size)).ToInt64();
                        if (written < 0)
                            throw CreateLinuxIOException("无法配置 Linux uinput 虚拟键盘");
                        if (written != size)
                            throw new IOException("无法完整写入 Linux uinput 设备配置，预期 " + size + " 字节，实际写入 " + written + " 字节。");
                    }
                    finally { Marshal.FreeHGlobal(memory); }

                    Check(ioctl(descriptor, UI_DEV_CREATE));
                    // 为桌面输入系统预留少量时间，以便发现新注册的虚拟键盘。
                    Thread.Sleep(30);
                }
                catch
                {
                    Destroy();
                    throw;
                }
            }

            public void SendCharacter(char character, Modifier modifiers)
            {
                if (character == '\r' || character == '\n') { SendKey(Key.Enter, modifiers); return; }
                if (character == '\t') { SendKey(Key.Tab, modifiers); return; }
                if (character == '\b') { SendKey(Key.Backspace, modifiers); return; }

                int keyCode;
                Modifier required;
                if (!TryMapCharacter(character, out keyCode, out required))
                    throw new NotSupportedException(
                        "Wayland/uinput 只能发送物理键。请向输入法发送拼音、五笔等键序列，而不是直接发送无法映射的 Unicode 字符。"
                    );
                WithModifiers(modifiers | required, () => Tap(keyCode));
            }

            public void SendKey(Key key, Modifier modifiers) => WithModifiers(modifiers, () => Tap(ToLinuxKeyCode(key)));
            public void Flush() => Emit(EV_SYN, SYN_REPORT, 0);

            // 所有软键盘点击共用该设备，并保持有效直至进程退出。
            public void Dispose() { }

            private void Destroy()
            {
                if (descriptor < 0) return;
                ioctl(descriptor, UI_DEV_DESTROY);
                close(descriptor);
                descriptor = -1;
            }

            ~UInputBackend() { Destroy(); }

            private void WithModifiers(Modifier modifiers, Action action)
            {
                if ((modifiers & Modifier.Shift) != 0) EmitKey(42, true);
                if ((modifiers & Modifier.Control) != 0) EmitKey(29, true);
                if ((modifiers & Modifier.Alt) != 0) EmitKey(56, true);
                try { action(); }
                finally
                {
                    if ((modifiers & Modifier.Alt) != 0) EmitKey(56, false);
                    if ((modifiers & Modifier.Control) != 0) EmitKey(29, false);
                    if ((modifiers & Modifier.Shift) != 0) EmitKey(42, false);
                }
            }

            private void Tap(int keyCode) { EmitKey(keyCode, true); EmitKey(keyCode, false); }
            private void EmitKey(int keyCode, bool down)
            {
                Emit(EV_KEY, (ushort)keyCode, down ? 1 : 0);
                Emit(EV_SYN, SYN_REPORT, 0);
            }

            private void Emit(ushort type, ushort code, int value)
            {
                InputEvent input = new InputEvent { type = type, code = code, value = value };
                int size = Marshal.SizeOf(typeof(InputEvent));
                long written = write_event(descriptor, ref input, new UIntPtr((uint)size)).ToInt64();
                if (written < 0)
                    throw CreateLinuxIOException("Linux uinput 按键发送失败");
                if (written != size)
                    throw new IOException("Linux uinput 按键事件写入不完整，预期 " + size + " 字节，实际写入 " + written + " 字节。");
            }

            private static bool TryMapCharacter(char character, out int code, out Modifier required)
            {
                required = Modifier.None;
                char lower = char.ToLowerInvariant(character);
                const string letters = "abcdefghijklmnopqrstuvwxyz";
                int[] letterCodes = { 30, 48, 46, 32, 18, 33, 34, 35, 23, 36, 37, 38, 50, 49, 24, 25, 16, 19, 31, 20, 22, 47, 17, 45, 21, 44 };
                int index = letters.IndexOf(lower);
                if (index >= 0)
                {
                    code = letterCodes[index];
                    if (char.IsUpper(character)) required = Modifier.Shift;
                    return true;
                }

                const string normal = "1234567890-=[]\\;',.`/ ";
                int[] normalCodes = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 26, 27, 43, 39, 40, 51, 52, 41, 53, 57 };
                index = normal.IndexOf(character);
                if (index >= 0) { code = normalCodes[index]; return true; }

                const string shifted = "!@#$%^&*()_+{}|:\"<>?~";
                int[] shiftedCodes = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 26, 27, 43, 39, 40, 51, 52, 53, 41 };
                index = shifted.IndexOf(character);
                if (index >= 0) { code = shiftedCodes[index]; required = Modifier.Shift; return true; }
                code = 0; return false;
            }

            private static int ToLinuxKeyCode(Key key)
            {
                if (key >= Key.F1 && key <= Key.F10) return 59 + (key - Key.F1);
                if (key == Key.F11) return 87; if (key == Key.F12) return 88;
                switch (key)
                {
                    case Key.Escape: return 1;
                    case Key.Backspace: return 14;
                    case Key.Tab: return 15;
                    case Key.Enter: return 28;
                    case Key.Space: return 57;
                    case Key.CapsLock: return 58;
                    case Key.NumLock: return 69;
                    case Key.ScrollLock: return 70;
                    case Key.Home: return 102;
                    case Key.Up: return 103;
                    case Key.PageUp: return 104;
                    case Key.Left: return 105;
                    case Key.Right: return 106;
                    case Key.End: return 107;
                    case Key.Down: return 108;
                    case Key.PageDown: return 109;
                    case Key.Insert: return 110;
                    case Key.Delete: return 111;
                    case Key.PrintScreen: return 99;
                    case Key.Pause: return 119;
                    case Key.Help: return 138;
                    case Key.Multiply: return 55;
                    case Key.Add: return 78;
                    case Key.Subtract: return 74;
                    case Key.Decimal: return 83;
                    case Key.Divide: return 98;
                    default: throw new NotSupportedException("该按键没有可用的 Linux evdev 映射。");
                }
            }

            private static void Check(int result)
            {
                if (result < 0) throw CreateLinuxIOException("Linux uinput 配置失败");
            }

            private static IOException CreateLinuxIOException(string message)
            {
                int errorNumber = Marshal.ReadInt32(__errno_location());
                IntPtr errorTextPointer = strerror(errorNumber);
                string? errorText = errorTextPointer == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(errorTextPointer);
                return new IOException(message + "，errno=" + errorNumber +
                    (string.IsNullOrEmpty(errorText) ? "。" : "（" + errorText + "）。"));
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            private struct UInputUserDevice
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string name;
                public InputId id;
                public int ffEffectsMax;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] public int[] absmax;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] public int[] absmin;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] public int[] absfuzz;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] public int[] absflat;
            }
            [StructLayout(LayoutKind.Sequential)]
            private struct InputId { public ushort bustype, vendor, product, version; }
            [StructLayout(LayoutKind.Sequential)]
            private struct InputEvent
            {
                public IntPtr seconds, microseconds;
                public ushort type, code;
                public int value;
            }

            [DllImport("libc", SetLastError = true)] private static extern int open(string path, int flags);
            [DllImport("libc", SetLastError = true)] private static extern int close(int fd);
            [DllImport("libc", SetLastError = true)] private static extern int ioctl(int fd, uint request, int value);
            [DllImport("libc", SetLastError = true, EntryPoint = "ioctl")] private static extern int ioctl(int fd, uint request);
            [DllImport("libc", SetLastError = true, EntryPoint = "write")] private static extern IntPtr write(int fd, IntPtr buffer, UIntPtr count);
            [DllImport("libc", SetLastError = true, EntryPoint = "write")] private static extern IntPtr write_event(int fd, ref InputEvent input, UIntPtr count);
            [DllImport("libc")] private static extern IntPtr __errno_location();
            [DllImport("libc")] private static extern IntPtr strerror(int errorNumber);
        }

        private sealed class X11Backend : IKeyboardBackend
        {
            private IntPtr display;

            public X11Backend()
            {
                try { display = XOpenDisplay(IntPtr.Zero); }
                catch (DllNotFoundException ex) { throw NotAvailable(ex); }
                catch (EntryPointNotFoundException ex) { throw NotAvailable(ex); }
                if (display == IntPtr.Zero) throw NotAvailable(null);
            }

            ~X11Backend() { if (display != IntPtr.Zero) XCloseDisplay(display); }

            public void SendCharacter(char character, Modifier modifiers)
            {
                if (character == '\r' || character == '\n') { SendKey(Key.Enter, modifiers); return; }
                if (character == '\t') { SendKey(Key.Tab, modifiers); return; }
                if (character == '\b') { SendKey(Key.Backspace, modifiers); return; }
                if (character == ' ') { SendKey(Key.Space, modifiers); return; }

                string keyName = character <= 0x7f ? character.ToString() : "U" + ((int)character).ToString("X4");
                UIntPtr keysym = XStringToKeysym(keyName);
                if (keysym == UIntPtr.Zero)
                    throw new NotSupportedException("The current X11 keyboard cannot represent character U+" + ((int)character).ToString("X4") + ".");

                Modifier required = NeedsShift(character) ? Modifier.Shift : Modifier.None;
                SendKeysym(keysym, modifiers | required);
            }

            public void SendKey(Key key, Modifier modifiers) => SendKeysym(new UIntPtr(ToKeysym(key)), modifiers);
            public void Flush() => XFlush(display);
            public void Dispose()
            {
                if (display == IntPtr.Zero) return;
                XCloseDisplay(display);
                display = IntPtr.Zero;
                GC.SuppressFinalize(this);
            }

            private void SendKeysym(UIntPtr keysym, Modifier modifiers)
            {
                uint keycode = XKeysymToKeycode(display, keysym);
                if (keycode == 0)
                    throw new NotSupportedException("The requested key is not present in the current X11 keyboard map.");
                PressModifiers(modifiers, true);
                try
                {
                    FakeKey(keycode, true);
                    FakeKey(keycode, false);
                }
                finally { PressModifiers(modifiers, false); }
            }

            private void PressModifiers(Modifier modifiers, bool down)
            {
                if (down)
                {
                    if ((modifiers & Modifier.Shift) != 0) FakeKeysym(0xFFE1, true);
                    if ((modifiers & Modifier.Control) != 0) FakeKeysym(0xFFE3, true);
                    if ((modifiers & Modifier.Alt) != 0) FakeKeysym(0xFFE9, true);
                }
                else
                {
                    if ((modifiers & Modifier.Alt) != 0) FakeKeysym(0xFFE9, false);
                    if ((modifiers & Modifier.Control) != 0) FakeKeysym(0xFFE3, false);
                    if ((modifiers & Modifier.Shift) != 0) FakeKeysym(0xFFE1, false);
                }
            }

            private void FakeKeysym(ulong keysym, bool down)
            {
                uint keycode = XKeysymToKeycode(display, new UIntPtr(keysym));
                if (keycode == 0) throw new NotSupportedException("A required modifier is unavailable in X11.");
                FakeKey(keycode, down);
            }

            private void FakeKey(uint keycode, bool down)
            {
                if (XTestFakeKeyEvent(display, keycode, down, UIntPtr.Zero) == 0)
                    throw new InvalidOperationException("X11 rejected the synthetic keyboard event.");
            }

            private static bool NeedsShift(char c) =>
                char.IsUpper(c) || "~!@#$%^&*()_+{}|:\"<>?".IndexOf(c) >= 0;

            private static ulong ToKeysym(Key key)
            {
                if (key >= Key.F1 && key <= Key.F24) return 0xFFBEUL + (ulong)(key - Key.F1);
                switch (key)
                {
                    case Key.Backspace: return 0xFF08;
                    case Key.Tab: return 0xFF09;
                    case Key.Enter: return 0xFF0D;
                    case Key.Escape: return 0xFF1B;
                    case Key.Space: return 0x20;
                    case Key.Home: return 0xFF50;
                    case Key.Left: return 0xFF51;
                    case Key.Up: return 0xFF52;
                    case Key.Right: return 0xFF53;
                    case Key.Down: return 0xFF54;
                    case Key.PageUp: return 0xFF55;
                    case Key.PageDown: return 0xFF56;
                    case Key.End: return 0xFF57;
                    case Key.PrintScreen: return 0xFF61;
                    case Key.Insert: return 0xFF63;
                    case Key.Pause: return 0xFF13;
                    case Key.Delete: return 0xFFFF;
                    case Key.Help: return 0xFF6A;
                    case Key.CapsLock: return 0xFFE5;
                    case Key.NumLock: return 0xFF7F;
                    case Key.ScrollLock: return 0xFF14;
                    case Key.Multiply: return 0xFFAA;
                    case Key.Add: return 0xFFAB;
                    case Key.Subtract: return 0xFFAD;
                    case Key.Decimal: return 0xFFAE;
                    case Key.Divide: return 0xFFAF;
                    default: throw new ArgumentOutOfRangeException(nameof(key));
                }
            }

            private static PlatformNotSupportedException NotAvailable(Exception inner) =>
                new PlatformNotSupportedException("SendKeys on Linux requires an X11 session and libXtst. Wayland does not permit global synthetic keyboard input.", inner);

            [DllImport("libX11.so.6")] private static extern IntPtr XOpenDisplay(IntPtr displayName);
            [DllImport("libX11.so.6")] private static extern int XCloseDisplay(IntPtr display);
            [DllImport("libX11.so.6")] private static extern int XFlush(IntPtr display);
            [DllImport("libX11.so.6")] private static extern UIntPtr XStringToKeysym([MarshalAs(UnmanagedType.LPStr)] string value);
            [DllImport("libX11.so.6")] private static extern uint XKeysymToKeycode(IntPtr display, UIntPtr keysym);
            [DllImport("libXtst.so.6")] private static extern int XTestFakeKeyEvent(IntPtr display, uint keycode, [MarshalAs(UnmanagedType.Bool)] bool isPress, UIntPtr delay);
        }

        private sealed class MacBackend : IKeyboardBackend
        {
            private const string Framework = "/System/Library/Frameworks/ApplicationServices.framework/ApplicationServices";

            public void SendCharacter(char character, Modifier modifiers)
            {
                if (character == '\r' || character == '\n') { SendKey(Key.Enter, modifiers); return; }
                if (character == '\t') { SendKey(Key.Tab, modifiers); return; }
                if (character == '\b') { SendKey(Key.Backspace, modifiers); return; }

                ushort keyCode;
                Modifier required;
                if (TryMapCharacter(character, out keyCode, out required))
                    WithModifiers(modifiers | required, () => PostKey(keyCode));
                else
                    WithModifiers(modifiers, () => PostUnicode(character));
            }

            public void SendKey(Key key, Modifier modifiers) => WithModifiers(modifiers, () => PostKey(ToKeyCode(key)));
            public void Flush() { }
            public void Dispose() { }

            private static void WithModifiers(Modifier modifiers, Action action)
            {
                if ((modifiers & Modifier.Shift) != 0) PostKeyPart(56, true);
                if ((modifiers & Modifier.Control) != 0) PostKeyPart(59, true);
                if ((modifiers & Modifier.Alt) != 0) PostKeyPart(58, true);
                try { action(); }
                finally
                {
                    if ((modifiers & Modifier.Alt) != 0) PostKeyPart(58, false);
                    if ((modifiers & Modifier.Control) != 0) PostKeyPart(59, false);
                    if ((modifiers & Modifier.Shift) != 0) PostKeyPart(56, false);
                }
            }

            private static void PostUnicode(char character)
            {
                IntPtr down = CGEventCreateKeyboardEvent(IntPtr.Zero, 0, true);
                IntPtr up = CGEventCreateKeyboardEvent(IntPtr.Zero, 0, false);
                if (down == IntPtr.Zero || up == IntPtr.Zero) throw new InvalidOperationException("macOS could not create a keyboard event.");
                try
                {
                    char[] value = { character };
                    CGEventKeyboardSetUnicodeString(down, new UIntPtr(1), value);
                    CGEventKeyboardSetUnicodeString(up, new UIntPtr(1), value);
                    CGEventPost(0, down); CGEventPost(0, up);
                }
                finally { if (down != IntPtr.Zero) CFRelease(down); if (up != IntPtr.Zero) CFRelease(up); }
            }

            private static void PostKey(ushort keyCode) { PostKeyPart(keyCode, true); PostKeyPart(keyCode, false); }
            private static void PostKeyPart(ushort keyCode, bool down)
            {
                IntPtr e = CGEventCreateKeyboardEvent(IntPtr.Zero, keyCode, down);
                if (e == IntPtr.Zero) throw new InvalidOperationException("macOS could not create a keyboard event.");
                try { CGEventPost(0, e); } finally { CFRelease(e); }
            }

            private static bool TryMapCharacter(char c, out ushort code, out Modifier required)
            {
                required = char.IsUpper(c) ? Modifier.Shift : Modifier.None;
                switch (char.ToLowerInvariant(c))
                {
                    case 'a': code = 0; return true;
                    case 's': code = 1; return true;
                    case 'd': code = 2; return true;
                    case 'f': code = 3; return true;
                    case 'h': code = 4; return true;
                    case 'g': code = 5; return true;
                    case 'z': code = 6; return true;
                    case 'x': code = 7; return true;
                    case 'c': code = 8; return true;
                    case 'v': code = 9; return true;
                    case 'b': code = 11; return true;
                    case 'q': code = 12; return true;
                    case 'w': code = 13; return true;
                    case 'e': code = 14; return true;
                    case 'r': code = 15; return true;
                    case 'y': code = 16; return true;
                    case 't': code = 17; return true;
                    case '1': code = 18; return true;
                    case '2': code = 19; return true;
                    case '3': code = 20; return true;
                    case '4': code = 21; return true;
                    case '6': code = 22; return true;
                    case '5': code = 23; return true;
                    case '9': code = 25; return true;
                    case '7': code = 26; return true;
                    case '8': code = 28; return true;
                    case '0': code = 29; return true;
                    case 'o': code = 31; return true;
                    case 'u': code = 32; return true;
                    case 'i': code = 34; return true;
                    case 'p': code = 35; return true;
                    case 'l': code = 37; return true;
                    case 'j': code = 38; return true;
                    case 'k': code = 40; return true;
                    case 'n': code = 45; return true;
                    case 'm': code = 46; return true;
                    case ' ': code = 49; return true;
                    default: code = 0; return false;
                }
            }

            private static ushort ToKeyCode(Key key)
            {
                ushort[] functionCodes = { 122, 120, 99, 118, 96, 97, 98, 100, 101, 109, 103, 111, 105, 107, 113, 106, 64, 79, 80, 90, 0, 0, 0, 0 };
                if (key >= Key.F1 && key <= Key.F24)
                {
                    ushort code = functionCodes[key - Key.F1];
                    if (code == 0) throw new NotSupportedException("This function key is not available on macOS.");
                    return code;
                }
                switch (key)
                {
                    case Key.Backspace: return 51;
                    case Key.Tab: return 48;
                    case Key.Enter: return 36;
                    case Key.Escape: return 53;
                    case Key.Space: return 49;
                    case Key.Home: return 115;
                    case Key.End: return 119;
                    case Key.PageUp: return 116;
                    case Key.PageDown: return 121;
                    case Key.Left: return 123;
                    case Key.Right: return 124;
                    case Key.Down: return 125;
                    case Key.Up: return 126;
                    case Key.Insert: return 114;
                    case Key.Delete: return 117;
                    case Key.Help: return 114;
                    case Key.Pause: return 113;
                    case Key.CapsLock: return 57;
                    case Key.NumLock: return 71;
                    case Key.Multiply: return 67;
                    case Key.Add: return 69;
                    case Key.Subtract: return 78;
                    case Key.Decimal: return 65;
                    case Key.Divide: return 75;
                    default: throw new NotSupportedException("The requested key has no macOS equivalent.");
                }
            }

            [DllImport(Framework)] private static extern IntPtr CGEventCreateKeyboardEvent(IntPtr source, ushort virtualKey, [MarshalAs(UnmanagedType.I1)] bool keyDown);
            [DllImport(Framework)] private static extern void CGEventKeyboardSetUnicodeString(IntPtr keyEvent, UIntPtr length, [In] char[] value);
            [DllImport(Framework)] private static extern void CGEventPost(uint tap, IntPtr keyEvent);
            [DllImport(Framework)] private static extern void CFRelease(IntPtr value);
        }
    }
}
