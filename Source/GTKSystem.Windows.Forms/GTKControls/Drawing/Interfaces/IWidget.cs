using System.Collections;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using Pango;

namespace System.Drawing;

public interface IWidget: IDisposable
{
    event EventHandler? Destroyed;
    string? Name { get; set; }
    Widget Parent { get; set; }
    int WidthRequest { get; set; }
    int HeightRequest { get; set; }
    bool Visible { get; set; }
    bool Sensitive { get; set; }
    bool AppPaintable { get; set; }
    bool CanFocus { get; set; }
    bool HasFocus { get; set; }
    bool IsFocus { get; set; }
    bool FocusOnClick { get; set; }
    bool CanDefault { get; set; }
    bool HasDefault { get; set; }
    bool ReceivesDefault { get; set; }
    bool CompositeChild { get; }
    EventMask Events { get; set; }
    bool NoShowAll { get; set; }
    bool HasTooltip { get; set; }
    string TooltipText { get; set; }
    string TooltipMarkup { get; set; }
    Gdk.Window Window { get; set; }
    bool DoubleBuffered { get; set; }
    Align Halign { get; set; }
    Align Valign { get; set; }
    int MarginLeft { get; set; }
    int MarginRight { get; set; }
    int MarginStart { get; set; }
    int MarginEnd { get; set; }
    int MarginTop { get; set; }
    int MarginBottom { get; set; }
    int Margin { get; set; }
    bool Hexpand { get; set; }
    bool HexpandSet { get; set; }
    bool Vexpand { get; set; }
    bool VexpandSet { get; set; }
    bool Expand { get; set; }
    double Opacity { get; set; }
    int ScaleFactor { get; }
    bool InteriorFocus { get; }
    int FocusLineWidth { get; }
    string FocusLinePattern { get; }
    int FocusPadding { get; }
    Gdk.Color? CursorColor { get; }
    Gdk.Color? SecondaryCursorColor { get; }
    float CursorAspectRatio { get; }
    bool WindowDragging { get; }
    Gdk.Color? LinkColor { get; }
    Gdk.Color? VisitedLinkColor { get; }
    bool WideSeparators { get; }
    int SeparatorWidth { get; }
    int SeparatorHeight { get; }
    int ScrollArrowHlength { get; }
    int ScrollArrowVlength { get; }
    int TextHandleWidth { get; }
    int TextHandleHeight { get; }
    Atk.Object Accessible { get; }
    int AllocatedBaseline { get; }
    int AllocatedHeight { get; }
    int AllocatedWidth { get; }
    Gdk.Rectangle Allocation { get; }
    Requisition ChildRequisition { get; }
    bool ChildVisible { get; set; }
    Gdk.Rectangle Clip { get; }
    string CompositeName { get; set; }
    TextDirection Direction { get; set; }
    Display Display { get; }
    FontMap FontMap { get; set; }
    FontOptions FontOptions { get; set; }
    FrameClock FrameClock { get; }
    bool HasWindow { get; set; }
    bool IsMapped { get; set; }
    RcStyle ModifierStyle { get; }
    Pango.Context PangoContext { get; }
    Gdk.Window ParentWindow { get; set; }
    WidgetPath WidgetPath { get; }
    bool IsRealized { get; set; }
    SizeRequestMode RequestMode { get; }
    Gdk.Window RootWindow { get; }
    Screen Screen { get; }
    StateFlags StateFlags { get; }
    StyleContext StyleContext { get; }
    bool SupportMultidevice { get; set; }
    Widget Toplevel { get; }
    Align ValignWithBaseline { get; }
    Visual Visual { get; set; }
    bool HasGrab { get; }
    bool HasRcStyle { get; }
    bool HasScreen { get; }
    bool HasVisibleFocus { get; }
    bool IsComposited { get; }
    bool IsDrawable { get; }
    bool IsSensitive { get; }
    bool IsToplevel { get; }
    bool IsVisible { get; }
    bool RedrawOnAllocate { set; }
    Gdk.Window GdkWindow { get; set; }
    IntPtr Handle { get; }
    Hashtable Data { get; }
    GType NativeType { get; }
    IntPtr OwnedHandle { get; }
    bool IsFloating { get; set; }
    event UnmapEventHandler? UnmapEvent;
    event ConfigureEventHandler? ConfigureEvent;
    event DragBeginHandler DragBegin;
    event DragDataGetHandler DragDataGet;
    event EventHandler? CompositedChanged;
    event FocusOutEventHandler? FocusOutEvent;
    event DragMotionHandler DragMotion;
    event DamageEventHandler? DamageEvent;
    event DrawnHandler Drawn;
    event StyleSetHandler StyleSet;
    event ScreenChangedHandler ScreenChanged;
    event QueryTooltipHandler QueryTooltip;
    event EventHandler? Shown;
    event SelectionNotifyEventHandler? SelectionNotifyEvent;
    event StateFlagsChangedHandler StateFlagsChanged;
    event HelpShownHandler HelpShown;
    event SelectionClearEventHandler? SelectionClearEvent;
    event EventHandler? FocusGrabbed;
    event EventHandler? Realized;
    event DragDataReceivedHandler DragDataReceived;
    event DragDropHandler DragDrop;
    event GrabNotifyHandler GrabNotify;
    event TouchEventHandler? TouchEvent;
    event ChildNotifiedHandler ChildNotified;
    event DragEndHandler DragEnd;
    event PropertyNotifyEventHandler? PropertyNotifyEvent;
    event EventHandler? Unrealized;
    event FocusInEventHandler? FocusInEvent;
    event ButtonPressEventHandler? ButtonPressEvent;
    event DirectionChangedHandler DirectionChanged;
    event EventHandler? Unmapped;
    event AccelCanActivateHandler AccelCanActivate;
    event GrabBrokenEventHandler? GrabBrokenEvent;
    event WidgetEventAfterHandler WidgetEventAfter;
    event ProximityOutEventHandler? ProximityOutEvent;
    event DragFailedHandler DragFailed;
    event MotionNotifyEventHandler? MotionNotifyEvent;
    event EventHandler? Hidden;
    event EventHandler? AccelClosuresChanged;
    event EventHandler? StyleUpdated;
    event ParentSetHandler ParentSet;
    event DragLeaveHandler DragLeave;
    event SizeAllocatedHandler SizeAllocated;
    event WidgetEventHandler? WidgetEvent;
    event LeaveNotifyEventHandler? LeaveNotifyEvent;
    event FocusedHandler Focused;
    event MoveFocusHandler MoveFocus;
    event SelectionRequestEventHandler? SelectionRequestEvent;
    event DragDataDeleteHandler DragDataDelete;
    event EnterNotifyEventHandler? EnterNotifyEvent;
    event MnemonicActivatedHandler MnemonicActivated;
    event SelectionReceivedHandler SelectionReceived;
    event KeyReleaseEventHandler? KeyReleaseEvent;
    event VisibilityNotifyEventHandler? VisibilityNotifyEvent;
    event DestroyEventHandler? DestroyEvent;
    event PopupMenuHandler PopupMenu;
    event DeleteEventHandler? DeleteEvent;
    event EventHandler? Mapped;
    event WindowStateEventHandler? WindowStateEvent;
    event MapEventHandler? MapEvent;
    event HierarchyChangedHandler HierarchyChanged;
    event ButtonReleaseEventHandler? ButtonReleaseEvent;
    event ScrollEventHandler? ScrollEvent;
    event ProximityInEventHandler? ProximityInEvent;
    event KeyPressEventHandler? KeyPressEvent;
    event SelectionGetHandler SelectionGet;
    bool Activate();

    void AddAccelerator(
        string accelSignal,
        AccelGroup accelGroup,
        uint accelKey,
        ModifierType accelMods,
        AccelFlags accelFlags);

    void AddAccelerator(string accelSignal, AccelGroup accelGroup, AccelKey accelKey);
    void AddDeviceEvents(Gdk.Device device, EventMask events);
    void AddEvents(int events);
    uint AddTickCallback(TickCallback cb);
    bool CanActivateAccel(uint signalId);
    bool ChildFocus(DirectionType direction);
    void ChildNotify(string childProperty);
    void ClassPath(out uint pathLength, out string path, out string pathReversed);
    bool ComputeExpand(Orientation orientation);
    Pango.Context CreatePangoContext();
    Pango.Layout CreatePangoLayout(string text);
    bool DeviceIsShadowed(Gdk.Device device);
    void Draw(Cairo.Context cr);
    void EnsureStyle();
    void ErrorBell();
    bool ProcessEvent(Event evnt);
    void FreezeChildNotify();
    IActionGroup GetActionGroup(string prefix);
    void GetAllocatedSize(out Gdk.Rectangle allocation, out int baseline);
    Clipboard GetClipboard(Atom selection);
    bool GetDeviceEnabled(Gdk.Device device);
    EventMask GetDeviceEvents(Gdk.Device device);
    ModifierType GetModifierMask(ModifierIntent intent);
    void GetPointer(out int x, out int y);
    void GetPreferredHeight(out int minimumHeight, out int naturalHeight);

    void GetPreferredHeightAndBaselineForWidth(
        int width,
        out int minimumHeight,
        out int naturalHeight,
        out int minimumBaseline,
        out int naturalBaseline);

    void GetPreferredHeightForWidth(
        int width,
        out int minimumHeight,
        out int naturalHeight);

    void GetPreferredSize(out Requisition minimumSize, out Requisition naturalSize);
    void GetPreferredWidth(out int minimumWidth, out int naturalWidth);

    void GetPreferredWidthForHeight(
        int height,
        out int minimumWidth,
        out int naturalWidth);

    void GetSizeRequest(out int width, out int height);
    GLib.Object GetTemplateChild(GType widgetType, string name);
    void GrabDefault();
    void GrabFocus();
    void Hide();
    bool HideOnDelete();
    bool InDestruction();
    void InputShapeCombineRegion(Cairo.Region region);
    void InsertActionGroup(string name, IActionGroup group);
    bool Intersect(Gdk.Rectangle area, out Gdk.Rectangle intersection);
    bool KeynavFailed(DirectionType direction);
    string[] ListActionPrefixes();
    void Map();
    bool MnemonicActivate(bool groupCycling);
    void ModifyFont(FontDescription fontDesc);
    void ModifyStyle(RcStyle style);
    void OverrideBackgroundColor(StateFlags state, RGBA color);
    void OverrideColor(StateFlags state, RGBA color);
    void OverrideCursor(RGBA cursor, RGBA secondaryCursor);
    void OverrideFont(FontDescription fontDesc);
    void OverrideSymbolicColor(string name, RGBA color);
    void Path(out uint pathLength, out string path, out string pathReversed);
    void Path(out string path, out string pathReversed);
    void QueueAllocate();
    void QueueComputeExpand();
    void QueueDraw();
    void QueueDrawArea(int x, int y, int width, int height);
    void QueueDrawRegion(Cairo.Region region);
    void QueueResize();
    void QueueResizeNoRedraw();
    void Realize();
    Cairo.Region RegionIntersect(Cairo.Region region);
    void RegisterWindow(Gdk.Window window);
    bool RemoveAccelerator(AccelGroup accelGroup, uint accelKey, ModifierType accelMods);
    void RemoveMnemonicLabel(Widget label);
    void RemoveTickCallback(uint id);
    Pixbuf RenderIcon(string stockId, IconSize size, string detail);
    Pixbuf RenderIconPixbuf(string stockId, IconSize size);
    void Reparent(Widget newParent);
    void ResetRcStyles();
    void ResetStyle();
    int SendExpose(Event evnt);
    bool SendFocusChange(Event evnt);
    void SetAccelPath(string accelPath, AccelGroup accelGroup);
    void SetAllocation(Gdk.Rectangle allocation);
    void SetClip(Gdk.Rectangle clip);
    void SetDeviceEnabled(Gdk.Device device, bool enabled);
    void SetDeviceEvents(Gdk.Device device, EventMask events);
    void SetSizeRequest(int width, int height);
    void SetStateFlags(StateFlags flags, bool clear);
    void ShapeCombineRegion(Cairo.Region region);
    void Show();
    void ShowAll();
    void ShowNow();
    void SizeAllocate(Gdk.Rectangle allocation);
    void SizeAllocateWithBaseline(Gdk.Rectangle allocation, int baseline);
    Requisition SizeRequest();
    void StyleAttach();
    void ThawChildNotify();

    bool TranslateCoordinates(
        Widget destWidget,
        int srcX,
        int srcY,
        out int destX,
        out int destY);

    void TriggerTooltipQuery();
    void Unmap();
    void Unparent();
    void Unrealize();
    void UnregisterWindow(Gdk.Window window);
    void UnsetStateFlags(StateFlags flags);
    Atk.Object RefAccessible();
    object StyleGetProperty(string propertyName);
    void Destroy();
    int GetHashCode();
    void AddNotification(string property, NotifyHandler handler);
    void AddNotification(NotifyHandler handler);
    void RemoveNotification(string property, NotifyHandler handler);
    void RemoveNotification(NotifyHandler handler);
    Value GetProperty(string name);
    void SetProperty(string name, Value val);
    void AddSignalHandler(string name, Delegate handler);
    void AddSignalHandler(string name, Delegate handler, Delegate marshaler);
    void AddSignalHandler(string name, Delegate handler, Type argsType);
    void RemoveSignalHandler(string name, Delegate handler);
    void QueueSignalFree();
}