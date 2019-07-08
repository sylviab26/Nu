namespace MyGame

open Assets
open System.Reflection
open Nu
open Nu.Declarative
open OpenTK
open Prime
open SDL2
open MultiAssetAnimation
open TileMap

#nowarn "1182"

module StaticEntity =
  type StaticEntityDispatcher () =
    inherit EntityDispatcher ()
    
    override this.Register (entity, world) =
      world
    
    static member FacetNames =
      [typeof<TileMapFacet>.Name]

    static member Properties =
      let tm = [
        FountainLarge1, Vector2(0.0f, 0.0f)
        FountainLarge2, Vector2(16.0f, 0.0f)
        FountainLarge3, Vector2(32.0f, 0.0f)
        FountainLarge4, Vector2(0.0f, -16.0f)
        FountainLarge5, Vector2(16.0f, -16.0f)
        FountainLarge6, Vector2(32.0f, -16.0f)
        FountainLarge7, Vector2(0.0f, -32.0f)
        FountainLarge8, Vector2(16.0f, -32.0f)
        FountainLarge9, Vector2(32.0f, -32.0f)
      ]

      [define Entity.TileMatrix tm
       define Entity.Size (Vector2(48.0f, 48.0f))]
    
module PlayerDispatcher =
  type Rotation = Up | Right | Down | Left | NoRotation
  type Moving = Up | Right | Down | Left | NoMove

  type Entity with
    member this.GetFourVectorRotation = this.Get Property? FourVectorRotation
    member this.SetFourVectorRotation = this.Set Property? FourVectorRotation
    member this.FourVectorRotation = Lens.make<Rotation, World> Property? FourVectorRotation this.GetFourVectorRotation this.SetFourVectorRotation this

  type PlayerEntityDispatcher () =
    inherit EntityDispatcher ()
    
    static let onWorldUpdate (evt: Event<unit, Entity>) (world: World) =
      let entity = evt.Subscriber
      let moving = 
        if World.isKeyboardKeyDown (SDL.SDL_Scancode.SDL_SCANCODE_W |> int) world then Up
        else if World.isKeyboardKeyDown (SDL.SDL_Scancode.SDL_SCANCODE_D |> int) world then Right
        else if World.isKeyboardKeyDown (SDL.SDL_Scancode.SDL_SCANCODE_S |> int) world then Down
        else if World.isKeyboardKeyDown (SDL.SDL_Scancode.SDL_SCANCODE_A |> int) world then Left
        else NoMove

      let animationKey = 
        match moving with
        | Up -> "walk-up"
        | Right -> "walk-right"
        | Down -> "walk-down"
        | Left -> "walk-left"
        | NoMove ->
          match entity.GetFourVectorRotation world with
          | Rotation.Up -> "idle-up"
          | Rotation.Right -> "idle-right"
          | Rotation.Down | Rotation.NoRotation -> "idle-down"
          | Rotation.Left -> "idle-left"
        
      let rotation =
        match moving with
        | Up -> Rotation.Up
        | Right -> Rotation.Right
        | Down -> Rotation.Down
        | Left -> Rotation.Left
        | NoMove -> entity.GetFourVectorRotation world
        
      let delta =
        match moving with
        | Up -> Vector2(0.0f, 1.0f) |> Some
        | Right -> Vector2(1.0f, 0.0f) |> Some
        | Down -> Vector2(0.0f, -1.0f) |> Some
        | Left -> Vector2(-1.0f, -0.0f) |> Some
        | NoMove -> None

      let world =   
        match delta with
        | Some x ->
          let units = 100.0f / 60.0f
          let pos = entity.GetPosition world
          
          entity.SetPosition (pos + x * units) world
        | None -> world
        
      let world =
        if evt.Subscriber.GetAnimationKey world = animationKey |> not then
          evt.Subscriber.SetAnimationKeyWithRefresh animationKey world
        else world
        
      let world =
        if entity.GetFourVectorRotation world = rotation |> not then
          entity.SetFourVectorRotation rotation world
        else world
      
      world

    override this.Register (entity, world) =
      let world = World.monitor onWorldUpdate Events.Update entity world

      world

    static member FacetNames =
      [typeof<MultiAssetAnimationFacet>.Name]
      
    static member Properties =
      let animationList =
        Map.empty
        |> Map.add "idle-up"     [Assets.Human1UpIdle]
        |> Map.add "idle-right"  [Assets.Human1RightIdle]
        |> Map.add "idle-down"   [Assets.Human1DownIdle]
        |> Map.add "idle-left"   [Assets.Human1LeftIdle]
        |> Map.add "walk-up"     [Assets.Human1UpWalk1; Assets.Human1UpWalk2]
        |> Map.add "walk-right"  [Assets.Human1RightWalk1; Assets.Human1RightWalk2]
        |> Map.add "walk-down"   [Assets.Human1DownWalk1; Assets.Human1DownWalk2]
        |> Map.add "walk-left"   [Assets.Human1LeftWalk1; Assets.Human1LeftWalk2]
        
      [define Entity.Size (Vector2(16.0f, 16.0f))
       define Entity.AnimationList animationList
       define Entity.AnimationKey "idle-down"
       define Entity.FourVectorRotation NoRotation]

open PlayerDispatcher
open StaticEntity

type MainLayerDispatcher () =
  inherit LayerDispatcher ()
  
  override this.Register (layer, world) =
    let player1, world = World.createEntity<PlayerEntityDispatcher> (Some Simulants.Player.EntityName) DefaultOverlay layer world
    let fountain, world = World.createEntity<StaticEntityDispatcher> (Some Simulants.Fountain.EntityName) DefaultOverlay layer world
    
    let world = player1.SetPosition (Vector2(1.0f, 1.0f)) world
    let world = fountain.SetPosition (Vector2(64.0f, 64.0f)) world
    
    world
    
type MainScreenDispatcher () =
  inherit ScreenDispatcher ()
  
  override this.Register (screen, world) =
    let layer, world = World.createLayer<MainLayerDispatcher> (Some "main layer") screen world
    
    world
