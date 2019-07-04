namespace MyGame

open Nu
open Nu.Declarative
open OpenTK
open Prime
open SDL2

#nowarn "1182"

type Player2EntityDispatcher () =
  inherit EntityDispatcher ()
  
  static member FacetNames =
    [typeof<StaticSpriteFacet>.Name]
    
  static member Properties =
    [define Entity.StaticImage Assets.Human1DownWalk1]
    
  override this.Register (entity, world) =
    world

type PlayerEntityDispatcher () =
  inherit EntityDispatcher ()
  
  static member FacetNames =
    [typeof<StaticSpriteFacet>.Name]
    
  static member Properties =
    [define Entity.Depth 1.0f
     define Entity.StaticImage Assets.Human1DownIdle]
  
  override this.Register (entity, world) =
    let onKeyDown (e: Event<KeyboardKeyData, Entity>) (world: World) =
      let entity = e.Subscriber
      let p = entity.GetPosition world
      let units = 100.0f / 60.0f
      
      let delta =
        match enum<SDL.SDL_Scancode>(e.Data.ScanCode) with
        | SDL.SDL_Scancode.SDL_SCANCODE_D ->
          Vector2(units, 0.0f) |> Some
        | SDL.SDL_Scancode.SDL_SCANCODE_A ->
          Vector2(-units, 0.0f) |> Some
        | SDL.SDL_Scancode.SDL_SCANCODE_W ->
          Vector2(0.0f, units) |> Some
        | SDL.SDL_Scancode.SDL_SCANCODE_S ->
          Vector2(0.0f, -units) |> Some
        | _ -> None
      
      let world =
        match delta with
        | Some x -> entity.SetPosition (p + x) world
        | None -> world

      world
    
    let world = World.monitor onKeyDown Events.KeyboardKeyDown entity world
    
    world
    
type MainLayerDispatcher () =
  inherit LayerDispatcher ()
  
  override this.Register (layer, world) =
    let player1, world = World.createEntity<PlayerEntityDispatcher> (Some "player1") DefaultOverlay layer world
    let player2, world = World.createEntity<Player2EntityDispatcher> (Some "player2") DefaultOverlay layer world
    
    let world = player1.SetPosition (Vector2(1.0f, 1.0f)) world
    let world = player2.SetPosition (Vector2(64.0f, 64.0f)) world
    
    world
    
type MainScreenDispatcher () =
  inherit ScreenDispatcher ()
  
  override this.Register (screen, world) =
    let layer, world = World.createLayer<MainLayerDispatcher> (Some "main layer") screen world
    
    world
