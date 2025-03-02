namespace System.Windows.Forms;

/// <summary>Specifies values representing possible roles for an accessible object.</summary>
/// <filterpriority>2</filterpriority>
public enum AccessibleRole
{
    /// <summary>A system-provided role.</summary>
    Default = -1,
    /// <summary>No role.</summary>
    None = 0,
    /// <summary>A title or caption bar for a window.</summary>
    TitleBar = 1,
    /// <summary>A menu bar, usually beneath the title bar of a window, from which users can select menus.</summary>
    MenuBar = 2,
    /// <summary>A vertical or horizontal scroll bar, which can be either part of the client area or used in a control.</summary>
    ScrollBar = 3,
    /// <summary>A special mouse pointer, which allows a user to manipulate user interface elements such as a window. For example, a user can click and drag a sizing grip in the lower-right corner of a window to resize it.</summary>
    Grip = 4,
    /// <summary>A system sound, which is associated with various system events.</summary>
    Sound = 5,
    /// <summary>A mouse pointer.</summary>
    Cursor = 6,
    /// <summary>A caret, which is a flashing line, block, or bitmap that marks the location of the insertion point in a window's client area.</summary>
    Caret = 7,
    /// <summary>An alert or condition that you can notify a user about. Use this role only for objects that embody an alert but are not associated with another user interface element, such as a message box, graphic, text, or sound.</summary>
    Alert = 8,
    /// <summary>A window frame, which usually contains child objects such as a title bar, client, and other objects typically contained in a window.</summary>
    Window = 9,
    /// <summary>A window's user area.</summary>
    Client = 10,
    /// <summary>A menu, which presents a list of options from which the user can make a selection to perform an action. All menu types must have this role, including drop-down menus that are displayed by selection from a menu bar, and shortcut menus that are displayed when the right mouse button is clicked.</summary>
    MenuPopup = 11,
    /// <summary>A menu item, which is an entry in a menu that a user can choose to carry out a command, select an option, or display another menu. Functionally, a menu item can be equivalent to a push button, radio button, check box, or menu.</summary>
    MenuItem = 12,
    /// <summary>A tool tip, which is a small rectangular pop-up window that displays a brief description of the purpose of a button.</summary>
    ToolTip = 13,
    /// <summary>The main window for an application.</summary>
    Application = 14,
    /// <summary>A document window, which is always contained within an application window. This role applies only to multiple-document interface (MDI) windows and refers to an object that contains the MDI title bar.</summary>
    Document = 15,
    /// <summary>A separate area in a frame, a split document window, or a rectangular area of the status bar that can be used to display information. Users can navigate between panes and within the contents of the current pane, but cannot navigate between items in different panes. Thus, panes represent a level of grouping lower than frame windows or documents, but above individual controls. Typically, the user navigates between panes by pressing TAB, F6, or CTRL+TAB, depending on the context.</summary>
    Pane = 16,
    /// <summary>A graphical image used to represent data.</summary>
    Chart = 17,
    /// <summary>A dialog box or message box.</summary>
    Dialog = 18,
    /// <summary>A window border. The entire border is represented by a single object, rather than by separate objects for each side.</summary>
    Border = 19,
    /// <summary>The objects grouped in a logical manner. There can be a parent-child relationship between the grouping object and the objects it contains.</summary>
    Grouping = 20,
    /// <summary>A space divided visually into two regions, such as a separator menu item or a separator dividing split panes within a window.</summary>
    Separator = 21,
    /// <summary>A toolbar, which is a grouping of controls that provide easy access to frequently used features.</summary>
    ToolBar = 22,
    /// <summary>A status bar, which is an area typically at the bottom of an application window that displays information about the current operation, state of the application, or selected object. The status bar can have multiple fields that display different kinds of information, such as an explanation of the currently selected menu command in the status bar.</summary>
    StatusBar = 23,
    /// <summary>A table containing rows and columns of cells and, optionally, row headers and column headers.</summary>
    Table = 24,
    /// <summary>A column header, which provides a visual label for a column in a table.</summary>
    ColumnHeader = 25,
    /// <summary>A row header, which provides a visual label for a table row.</summary>
    RowHeader = 26,
    /// <summary>A column of cells within a table.</summary>
    Column = 27,
    /// <summary>A row of cells within a table.</summary>
    Row = 28,
    /// <summary>A cell within a table.</summary>
    Cell = 29,
    /// <summary>A link, which is a connection between a source document and a destination document. This object might look like text or a graphic, but it acts like a button.</summary>
    Link = 30,
    /// <summary>A Help display in the form of a ToolTip or Help balloon, which contains buttons and labels that users can click to open custom Help topics.</summary>
    HelpBalloon = 31,
    /// <summary>A cartoon-like graphic object, such as Microsoft Office Assistant, which is typically displayed to provide help to users of an application.</summary>
    Character = 32,
    /// <summary>A list box, which allows the user to select one or more items.</summary>
    List = 33,
    /// <summary>An item in a list box or the list portion of a combo box, drop-down list box, or drop-down combo box.</summary>
    ListItem = 34,
    /// <summary>An outline or tree structure, such as a tree view control, which displays a hierarchical list and usually allows the user to expand and collapse branches.</summary>
    Outline = 35,
    /// <summary>An item in an outline or tree structure.</summary>
    OutlineItem = 36,
    /// <summary>A property page that allows a user to view the attributes for a page, such as the page's title, whether it is a home page, or whether the page has been modified. Normally, the only child of this control is a grouped object that contains the contents of the associated page.</summary>
    PageTab = 37,
    /// <summary>A property page, which is a dialog box that controls the appearance and the behavior of an object, such as a file or resource. A property page's appearance differs according to its purpose.</summary>
    PropertyPage = 38,
    /// <summary>An indicator, such as a pointer graphic, that points to the current item.</summary>
    Indicator = 39,
    /// <summary>A picture.</summary>
    Graphic = 40,
    /// <summary>The read-only text, such as in a label, for other controls or instructions in a dialog box. Static text cannot be modified or selected.</summary>
    StaticText = 41,
    /// <summary>The selectable text that can be editable or read-only.</summary>
    Text = 42,
    /// <summary>A push button control, which is a small rectangular control that a user can turn on or off. A push button, also known as a command button, has a raised appearance in its default off state and a sunken appearance when it is turned on.</summary>
    PushButton = 43,
    /// <summary>A check box control, which is an option that can be turned on or off independent of other options.</summary>
    CheckButton = 44,
    /// <summary>An option button, also known as a radio button. All objects sharing a single parent that have this attribute are assumed to be part of a single mutually exclusive group. You can use grouped objects to divide option buttons into separate groups when necessary.</summary>
    RadioButton = 45,
    /// <summary>A combo box, which is an edit control with an associated list box that provides a set of predefined choices.</summary>
    ComboBox = 46,
    /// <summary>A drop-down list box. This control shows one item and allows the user to display and select another from a list of alternative choices.</summary>
    DropList = 47,
    /// <summary>A progress bar, which indicates the progress of a lengthy operation by displaying colored lines inside a horizontal rectangle. The length of the lines in relation to the length of the rectangle corresponds to the percentage of the operation that is complete. This control does not take user input.</summary>
    ProgressBar = 48,
    /// <summary>A dial or knob. This can also be a read-only object, like a speedometer.</summary>
    Dial = 49,
    /// <summary>A hot-key field that allows the user to enter a combination or sequence of keystrokes to be used as a hot key, which enables users to perform an action quickly. A hot-key control displays the keystrokes entered by the user and ensures that the user selects a valid key combination.</summary>
    HotkeyField = 50,
    /// <summary>A control, sometimes called a trackbar, that enables a user to adjust a setting in given increments between minimum and maximum values by moving a slider. The volume controls in the Windows operating system are slider controls.</summary>
    Slider = 51,
    /// <summary>A spin box, also known as an up-down control, which contains a pair of arrow buttons. A user clicks the arrow buttons with a mouse to increment or decrement a value. A spin button control is most often used with a companion control, called a buddy window, where the current value is displayed.</summary>
    SpinButton = 52,
    /// <summary>A graphical image used to diagram data.</summary>
    Diagram = 53,
    /// <summary>An animation control, which contains content that is changing over time, such as a control that displays a series of bitmap frames, like a filmstrip. Animation controls are usually displayed when files are being copied, or when some other time-consuming task is being performed.</summary>
    Animation = 54,
    /// <summary>A mathematical equation.</summary>
    Equation = 55,
    /// <summary>A button that drops down a list of items.</summary>
    ButtonDropDown = 56,
    /// <summary>A button that drops down a menu.</summary>
    ButtonMenu = 57,
    /// <summary>A button that drops down a grid.</summary>
    ButtonDropDownGrid = 58,
    /// <summary>A blank space between other objects.</summary>
    WhiteSpace = 59,
    /// <summary>A container of page tab controls.</summary>
    PageTabList = 60,
    /// <summary>A control that displays the time.</summary>
    Clock = 61,
    /// <summary>A toolbar button that has a drop-down list icon directly adjacent to the button.</summary>
    SplitButton = 62,
    /// <summary>A control designed for entering Internet Protocol (IP) addresses.</summary>
    IpAddress = 63,
    /// <summary>A control that navigates like an outline item.</summary>
    OutlineButton = 64
}