namespace MyGame

open Prime
open OpenTK
open Nu
open Nu.Declarative

module EntityForVectorRotation =
  ()

module MultiAssetAnimation =
  type Animation = Image AssetTag list
  type AnimationKey = string
  type AnimationList = Map<AnimationKey, Animation>

  type Entity with
    member this.GetAnimationList = this.Get Property? AnimationList
    member this.SetAnimationList = this.Set Property? AnimationList
    member this.GetAnimationKey = this.Get Property? AnimationKey
    member this.SetAnimationKey = this.Set Property? AnimationKey
    member this.GetAnimationBeginTick = this.Get Property? AnimationBeginTick
    member this.SetAnimationBeginTick = this.Set Property? AnimationBeginTick
    member this.AnimationList = Lens.make<AnimationList, World> Property? AnimationList this.GetAnimationList this.SetAnimationList this
    member this.AnimationKey = Lens.make<AnimationKey, World> Property? AnimationKey this.GetAnimationKey this.SetAnimationKey this
    member this.AnimationBeginTick = Lens.make<int64, World> Property? AnimationBeginTick this.GetAnimationBeginTick this.SetAnimationBeginTick this
    
    member this.SetAnimationKeyWithRefresh key world =
      let world = this.SetAnimationKey key world
      let world = this.SetAnimationBeginTick (World.getTickTime world) world
      
      world

  type MultiAssetAnimationFacet() =
    inherit Facet()
    
    static member Properties =
      [define Entity.AnimationBeginTick 0L]
    
    override this.Actualize (entity, world) =
      let frameTime = 10L // хардкод время фрейма, 1 секунда на фрейм
      let tickTime = World.getTickTime world
      
      if entity.GetInView world then
        let animationList = entity.GetAnimationList world
        let animationKey = entity.GetAnimationKey world
        let animationBeginTick = entity.GetAnimationBeginTick world
        
        let world =
          match animationList |> Map.tryFind animationKey with
          | None -> world
          | Some animations ->
            let distance = tickTime - animationBeginTick
            let animationFrame = distance / frameTime % (animations |> List.length |> int64) |> int
            
            match animations |> List.tryItem animationFrame with
            | Some frame ->
              let sd = SpriteDescriptor {
                Position = entity.GetPosition world
                Size = entity.GetSize world
                Rotation = entity.GetRotation world
                Offset = Vector2.Zero
                ViewType = entity.GetViewType world
                InsetOpt = None
                Image = frame
                Color = Vector4.One
              }
              let ld = LayerableDescriptor {
                Depth = entity.GetDepth world
                PositionY = (entity.GetPosition world).Y
                LayeredDescriptor = sd
              }
              let renderMsg = RenderDescriptorsMessage [| ld |]
              
              World.enqueueRenderMessage renderMsg world
            | None ->
              failwithf "invalid frame %d" animationFrame
        world
      else
        world