namespace MyGame

open Prime
open OpenTK
open Nu
open Nu.Declarative

module MultiAssetAnimation =
  type Animation = Image AssetTag list
  type AnimationKey = string
  type AnimationList = Map<AnimationKey, Animation>

  type Entity with
    member this.GetAnimationList = this.Get Property? AnimationList
    member this.SetAnimationList = this.Set Property? AnimationList
    member this.GetAnimationKey = this.Get Property? AnimationKey
    member this.SetAnimationKey = this.Set Property? AnimationKey
    member this.AnimationList = Lens.make<AnimationList, World> Property? AnimationList this.GetAnimationList this.SetAnimationList this
    member this.AnimationKey = Lens.make<AnimationKey, World> Property? AnimationKey this.GetAnimationKey this.SetAnimationKey this

  type MultiAssetAnimationFacet() =
    inherit Facet()

    override this.Actualize (entity, world) =
      if entity.GetInView world then
        let animationList = entity.GetAnimationList world
        let animationKey = entity.GetAnimationKey world
        
        let world =
          match animationList |> Map.tryFind animationKey with
          | None -> world
          | Some x ->
            let sd = SpriteDescriptor {
              Position = entity.GetPosition world
              Size = entity.GetSize world
              Rotation = entity.GetRotation world
              Offset = Vector2.Zero
              ViewType = entity.GetViewType world
              InsetOpt = None
              Image = x.[0]
              Color = Vector4.One
            }
            let ld = LayerableDescriptor {
              Depth = entity.GetDepth world
              PositionY = (entity.GetPosition world).Y
              LayeredDescriptor = sd
            }
            let renderMsg = RenderDescriptorsMessage [| ld |]
            
            World.enqueueRenderMessage renderMsg world
          
        world
      else
        world