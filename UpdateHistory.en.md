### Update Log

#### 2025/3/4 V1.3.24.63
1. Fixed memory conflict issue in the file selector.
2. Updated dialog window icons.
3. Fixed issues with closing sub-windows and container scrollbars not displaying.

#### 2025/2/27 V1.3.24.62
1. Fixed the disabled sorting property in `DataGridView`.
2. Fixed the `filename` property of `FileDialog`.

#### 2025/2/26 V1.3.24.61
1. Updated `TextBox`, `CheckBox`, `ListView`, and `DataGridView`.
2. Fixed and optimized several features.
3. Added `SwitchBox` development control.
4. Added `PropertyGrid` control.

#### 2025/2/9 V1.3.24.60
1. Updated `ListView` and `TabControl`, optimized functionality, and fixed `ListView` scrollbar bug.
2. Fixed precision and layout mechanism of containers.
3. Added support for `netstandard2.0`.

#### 2025/2/5 V1.3.24.59
1. Updated the default display type of `ToolStripButton`.
2. Updated `ToolStrip` to support aligning the last menu item to the right.
3. Major `ListView` update: added implementation methods and optimized the interface.
4. Updated `Form` and fixed bugs.

#### 2025/1/2 V1.3.24.58
1. Updated container control layout functionality.
2. Updated `DataGridView` data assignment functionality.
3. Updated `Form` close functionality.

#### 2024/12/20 V1.3.24.57
1. Updated `ListView`, `ListBox`, `CheckedListBox`, and `DataGridView` functionality.

#### 2024/12/14 V1.3.24.56
1. Implemented `SizeChange` event for controls.
2. Updated message box content line wrapping and enabled automatic text wrapping in `DataGridView`.
3. Updated `ListView` to support adding and deleting items.
4. Fixed several discovered issues.

#### 2024/11/7 V1.3.24.53
1. Fixed container control functionality.
2. Improved `TreeView` functionality.
3. Implemented basic configuration and image retrieval for `ImageList` components.
4. Fixed several discovered issues.

#### 2024/10/23 V1.3.24.51
1. **[Major Update]** Enhanced container control functionality and fixed errors.
2. Improved `Graphics/Image` rendering functionality.
3. Fixed several discovered issues.

#### 2024/10/19 V1.3.24.50
1. **[Major Update]** Refactored container layout, improved `Dock`/`Anchor` functionality for better performance and stability in deep nesting scenarios.
2. Implemented automatic text wrapping functionality and properties for `DataGridView`.
3. Added operations for setting `Form` as topmost, activating, and retrieving open windows.
4. Fixed several discovered issues.

#### 2024/9/28 V1.3.24.49
1. Fixed `Paint` event handling and `CreateGraphics` method in `Form`.
2. Added `ScrollBar` and `FontDialog` controls.
3. Fixed issues with `SelectIndexChanged`, `SelectValueChanged`, and `SelectItemChanged` events.

#### 2024/8/28 V1.3.24.48
1. Implemented right-click context menus for all controls.
2. Added theme configuration and style configuration functionality.

#### 2024/8/28 V1.3.24.47
1. Adjusted and optimized control positioning and border properties for precise alignment.
2. Added support for additional control events.

#### 2024/8/27 V1.3.24.46
1. Optimized form closing methods to unify window title bars in Windows and Linux, improving compatibility with reopening native forms.
2. Implemented `CellStyle` styling functionality in `DataGridView`, modified table image loading, improved performance, and fixed bugs.
3. Optimized core dataset program library.
4. Improved printing component functionality, added print preview control and print preview window.

#### 2024/7/16 V1.3.24.45
1. Optimized event handling for controls.

#### 2024/7/16
1. Fixed issues with opening file dialog and message pop-ups.

#### 2024/7/12
1. Added printing component.
2. Fixed several issues.

#### 2024/7/7
1. Modified control styles.

#### 2024/6/28
1. Adjusted border styles and control mouse events.
2. Fixed some control property values and errors.
3. Refactored container scrolling form architecture.

#### 2024/6/22
1. Fixed several bugs.
2. Optimized window and control resize functionality.

#### 2024/6/20
1. Fixed several bugs.
2. Optimized window and control resize functionality.
3. Added mouse event handling for `UserControl`.

#### 2024/6/19
1. Adjusted styles.
2. Fixed several bugs.
3. Optimized window and control resize functionality.
4. Implemented adjustable position and size properties for controls.
5. Added mouse style properties for certain controls.

#### 2024/6/10
1. Modified background image rendering to support rounded corners and transparent backgrounds for most controls (**Important**).
2. Improved functionality and performance of multiple controls.
3. Enhanced control style rendering program to support theme switching.
4. Fixed several discovered functional or programmatic issues.
5. Added asynchronous URL image loading for `DataGridView`, optimizing data display performance.

#### 2024/5/30
1. **(Important)** Fixed synchronization issues in multi-threaded UI updates using `Invoker`, ensuring `Timer` execution syncs with UI.
2. Fixed background color issues for `ListBox`, `ListView`, and `RichTextBox`.
3. Enhanced project feature demonstrations, adding dynamic scrolling data display.

#### 2024/5/28
1. Fixed and added new methods for controls.
2. Added multi-threaded UI update handling for GTK.

#### 2024/5/21
1. Updated `ComboBox` and `ListBox` functionalities.
2. Fixed `ToolStripSeparator` issues.

#### 2024/5/17
1. Added time data and formatting modes for `DateTimePicker`.

#### 2024/5/16
1. Fixed transparency and border line issues in `Form`.
2. Added frequently used properties to several controls.
3. Enhanced `ComboBox` with selectable `DropDown` and `DropDownList` modes.
4. Fixed Visual Studio plugin issues, improving installation compatibility.

#### 2024/5/11
1. Fixed abnormal form sizes at startup.
2. Added `Image` property to `Button` controls.
3. Fixed control background positioning.

#### 2024/5/6
1. Enhanced `TreeView` and `ListView` functionalities.
2. Released `GTKSystem.Windows.FormsDesigner.dll` (NuGet installation), enabling automatic form designer configuration fixes during compilation.
3. Fixed data retrieval errors in `DataGridView`.

#### 2024/5/1
1. **[Major Update]** Refactored control architecture, improving many functionalities and performance while fixing various issues.
2. Optimized rendering and background drawing functionality to prevent background images from covering child controls.
3. Specifically optimized `Form` interface and performance.

#### 2024/4/20
1. Fixed position issues in `Graphics` rendering.
2. Implemented `GraphicsPath` rendering with gradient colors.
3. Added `BeginInvoke` and `EndInvoke` methods for controls.
4. Improved data loading for `DataGridView` and `ListBox`, fixing issues where data couldn't be loaded on window startup.

#### 2024/3/27
1. Fixed issues with `UserControl` in the form designer that caused exceptions (controls still not visible).
2. Implemented ellipse drawing in `Graphics`.

#### 2024/3/19
1. Enabled scrollbar display for overflowing `Panel` content and optimized window resizing.

#### 2024/3/14
1. Fixed data loading issues in `TreeView`.

#### 2024/3/6
1. Fixed various window configuration and binding issues.

#### 2024/3/2
1. Fixed font size issues in `Label` text and added alignment properties.
2. Implemented `ImageList` compatibility in the form designer.

#### 2024/2/29
1. Added curves and polygons in `Graphics` rendering and optimized text rendering.
2. Fixed several hidden exceptions.

#### 2024/2/23
1. Implemented and fixed `DataGridView` cell data editing and retrieval functionalities.