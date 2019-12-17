module Import.Jupyter

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Browser.Types

/// An object which holds data related to an object's title.
/// 
/// #### Notes
/// A title object is intended to hold the data necessary to display a
/// header for a particular object. A common example is the `TabPanel`,
/// which uses the widget title to populate the tab for a child widget.
/// The namespace for the `Title` class statics.
type [<AllowNullLiteral>] Title<'T> =
    /// The object which owns the title.
    abstract owner: 'T
    /// Get the label for the title.
    /// 
    /// #### Notes
    /// The default value is an empty string.
    /// Set the label for the title.
    abstract label: string with get, set
    /// The closable state for the title.
    abstract closable: bool option with get, set

type [<AllowNullLiteral>] Widget =
    /// Get the DOM node owned by the widget.
    abstract node: HTMLElement
    /// Test whether the widget's node is attached to the DOM.
    abstract isAttached: bool
    /// The title object for the widget.
    /// 
    /// #### Notes
    /// The title object is used by some container widgets when displaying
    /// the widget alongside some title, such as a tab panel or side bar.
    /// 
    /// Since not all widgets will use the title, it is created on demand.
    /// 
    /// The `owner` property of the title is set to this widget.
    abstract title: Title<Widget>
    /// Get the id of the widget's DOM node.
    /// Set the id of the widget's DOM node.
    abstract id: string with get, set

/// The base class of the Phosphor widget hierarchy.
/// 
/// #### Notes
/// This class will typically be subclassed in order to create a useful
/// widget. However, it can be used directly to host externally created
/// content.
/// The namespace for the `Widget` class statics.
type [<AllowNullLiteral>] WidgetStatic =
    /// <summary>Construct a new widget.</summary>
    /// <param name="options">- The options for initializing the widget.</param>
    [<Emit "new $0()">] abstract Create : unit -> Widget

type [<AllowNullLiteral>] IPhosphorWidgets =
    abstract Widget: WidgetStatic with get,set

[<Import("*","@phosphor/widgets")>]
let PhosphorWidgets : IPhosphorWidgets = jsNative

/// A widget meant to be contained in the JupyterLab main area.
/// 
/// #### Notes
/// Mirrors all of the `title` attributes of the content.
/// This widget is `closable` by default.
/// This widget is automatically disposed when closed.
/// This widget ensures its own focus when activated.
/// The namespace for the `MainAreaWidget` class statics.
type [<AllowNullLiteral>] MainAreaWidget =
    inherit Widget
    /// The content hosted by the widget.
    abstract content: obj

/// A widget meant to be contained in the JupyterLab main area.
/// 
/// #### Notes
/// Mirrors all of the `title` attributes of the content.
/// This widget is `closable` by default.
/// This widget is automatically disposed when closed.
/// This widget ensures its own focus when activated.
/// The namespace for the `MainAreaWidget` class statics.
type [<AllowNullLiteral>] MainAreaWidgetStatic =
    /// <summary>Construct a new main area widget.</summary>
    /// <param name="options">- The options for initializing the widget.</param>
    [<Emit "new $0($1...)">] abstract Create: options:obj -> MainAreaWidget

/// The command palette token.
/// The interface for a Jupyter Lab command palette.
type [<AllowNullLiteral>] ICommandPalette =
    /// The placeholder text of the command palette's search input.
    abstract placeholder: string with get, set
    /// Activate the command palette for user input.
    abstract activate: unit -> unit
    /// <summary>Add a command item to the command palette.</summary>
    /// <param name="options">- The options for creating the command item.</param>
    abstract addItem: options: obj -> obj

type [<AllowNullLiteral>] IJupyterlabApputils =
    abstract MainAreaWidget: MainAreaWidgetStatic 

[<Import("*","@jupyterlab/apputils")>]
let JupyterlabApputils : IJupyterlabApputils = jsNative

/// The base Jupyter front-end application class.
/// The namespace for `JupyterFrontEnd` class statics.
type [<AllowNullLiteral>] JupyterFrontEnd =
    /// The name of this Jupyter front-end application.
    abstract name: string
    abstract commands: obj with get,set
    abstract shell: obj with get,set

/// The base Jupyter front-end application class.
/// The namespace for `JupyterFrontEnd` class statics.
type [<AllowNullLiteral>] JupyterFrontEndStatic =
    /// Construct a new JupyterFrontEnd object.
    [<Emit "new $0($1...)">] abstract Create: options: obj -> JupyterFrontEnd

type [<AllowNullLiteral>] IJupyterlabApplication =
    abstract JupyterFrontEnd: JupyterFrontEndStatic

[<Import("*","@jupyterlab/application")>]
let JupyterlabApplication : IJupyterlabApplication = jsNative
