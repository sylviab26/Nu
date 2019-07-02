namespace MyGame

open Nu
open Nu.Declarative
open Prime

#nowarn "1182"
  
type PlayerEntityDispatcher () =
  inherit EntityDispatcher ()
  
  static member FacetNames =
    [typeof<StaticSpriteFacet>.Name]
    
  static member Properties =
    [ define Entity.Depth 1.0f
      define Entity.StaticImage Assets.Human1DownIdle ]
  
  override this.Register (entity, world) =
    Log.info (sprintf "%s[%s].Register registered" <| this.GetType().Name <|  entity.GetName world)
    
    world
    
type MainLayerDispatcher () =
  inherit LayerDispatcher ()
  
  override this.Register (layer, world) =
    let player, world = World.createEntity<PlayerEntityDispatcher> (Some "player") DefaultOverlay layer world
    
    Log.info (sprintf "%s registered" <| player.GetName world)
    
    world
    
type MainScreenDispatcher () =
  inherit ScreenDispatcher ()
  
  override this.Register (screen, world) =
    let layer, world = World.createLayer<MainLayerDispatcher> (Some "main layer") screen world
    
    world
