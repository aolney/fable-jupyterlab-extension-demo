module App

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Browser.Dom
open Import.Jupyter

//sugar for creating js objects
let inline (!!) x = createObj x
let inline (=>) x y = x ==> y

//TODO: hack b/c I haven't dug through the imports to understand the `requires` semantics
[<ImportMember("@jupyterlab/apputils")>]
let ICommandPalette : obj = jsNative

let extension =
    !! [
        "id" => "jupyterlab_fable_extension"
        "autoStart" => true
        "requires" => [| ICommandPalette |] 
        //------------------------------------------------------------------------------------------------------------
        //NOTE: this **must** be wrapped in a Func, otherwise the arguments are tupled and Jupyter doesn't expect that
        //------------------------------------------------------------------------------------------------------------
        "activate" =>  System.Func<JupyterFrontEnd,ICommandPalette,unit>( fun app palette ->
            console.log("JupyterLab extension jupyterlab_fable_extension is activated!");
            // Create a blank content widget inside of a MainAreaWidget
            let content = PhosphorWidgets.Widget.Create();
            let widget = JupyterlabApputils.MainAreaWidget.Create( !![ "content" => content ]);
            widget.id <- "apod-jupyterlab";
            widget.title.label <- "Fable Demo";
            widget.title.closable <- Some(true);

            // Add html text to the widget
            let p = Browser.Dom.document.createElement("p")
            p.setAttribute("style","font-size:72px")
            p.innerText <- "Hello from Fable"
            content.node.appendChild(p) |> ignore

            // Add application command
            let command = "apod:open"
            //TODO: using dynamic (?) b/c the imports aren't fully implemented
            app.commands?addCommand( command, 
                !![
                    "label" => "Fable Jupyterlab Extension"
                    "execute" => fun () -> 
                        if not <| widget.isAttached then
                            app.shell?add(widget, "main")
                        app.shell?activateById(widget.id)
                ])
            //Add command to palette
            palette?addItem(
                !![
                    "command" => command
                    "category" => "Fable"
                ]
            )
        )
 
    ]

exportDefault extension