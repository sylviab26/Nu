﻿namespace MyGame
open MultiAssetAnimation
open System
open Nu
open Nu.FacetModule
open PlayerDispatcher
open StaticEntity
open TileMap

// this is a plugin for the Nu game engine by which user-defined dispatchers, facets, and other
// sorts of types can be obtained by both your application and Gaia.
type MyGamePlugin () =
    inherit NuPlugin ()

    // make our game-specific game dispatcher...
    override this.MakeGameDispatchers () =
        [MyGameDispatcher () :> GameDispatcher]
        
    override this.MakeScreenDispatchers () =
        [MainScreenDispatcher () :> ScreenDispatcher]
        
    override this.MakeLayerDispatchers () =
        [MainLayerDispatcher () :> LayerDispatcher]
        
    override this.MakeEntityDispatchers () =
        [PlayerEntityDispatcher () :> EntityDispatcher
         StaticEntityDispatcher () :> EntityDispatcher]
        
    override this.MakeFacets () =
        [MultiAssetAnimationFacet () :> Facet
         TileMapFacet () :> Facet]

    // specify the above game dispatcher to use
    override this.GetStandAloneGameDispatcherName () =
        typeof<MyGameDispatcher>.Name
    
// this is the main module for our program.
module Program =

    // this the entry point for your Nu application
    let [<EntryPoint; STAThread>] main _ =

        // initialize Nu
        Nu.init false

        // this specifies the window configuration used to display the game.
        let sdlWindowConfig = { SdlWindowConfig.defaultConfig with WindowTitle = "MyGame" }
        
        // this specifies the configuration of the game engine's use of SDL.
        let sdlConfig = { SdlConfig.defaultConfig with ViewConfig = NewWindow sdlWindowConfig }

        // this is a callback that attempts to make 'the world' in a functional programming
        // sense. In a Nu game, the world is represented as an abstract data type named World.
        let tryMakeWorld sdlDeps =

            // an instance of the above plugin
            let plugin = MyGamePlugin ()

            // here is an attempt to make the world with the various initial states, the engine
            // plugin, and SDL dependencies.
            World.tryMake true 1L () plugin sdlDeps

        // after some configuration it is time to run the game. We're off and running!
        World.run tryMakeWorld id id sdlConfig