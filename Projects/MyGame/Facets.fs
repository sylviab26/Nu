namespace MyGame

open Nu
open Nu.Declarative
open OpenTK
open Prime

module TileMap =
  type TilePosition = Image AssetTag * Vector2
  type TileMatrix = TilePosition list

  type Entity with
    member this.GetTileMatrix = this.Get Property? TileMatrix
    member this.SetTileMatrix = this.Set Property? TileMatrix
    member this.TileMatrix = Lens.make<TileMatrix, World> Property? TileMatrix this.GetTileMatrix this.SetTileMatrix this

  type TileMapFacet () =
    inherit Facet ()

    override this.Register (e, w) =
      (e, w) |> ignore

      w

    override this.Actualize (entity, world) =
      let world = 
        if entity.GetInView world then
          let w =
            entity.GetTileMatrix world
            |> List.fold (fun w t ->
              let (img, pos) = t

              match World.tryGetTextureSizeF img world with
              | Some sizeF ->
                let sd = SpriteDescriptor {
                  Position = entity.GetPosition w + pos
                  Size = sizeF
                  Rotation = entity.GetRotation w
                  ViewType = entity.GetViewType w
                  Offset = Vector2.Zero
                  InsetOpt = None
                  Color = Vector4.One
                  Image = t |> fst
                }
                let ld = LayerableDescriptor { Depth = entity.GetDepth w; PositionY = (entity.GetPosition w).Y; LayeredDescriptor = sd }
                let renderMsg = RenderDescriptorsMessage [| ld |]

                World.enqueueRenderMessage renderMsg w
              | None ->
                w
            ) world
          w
        else
          world

      world

    static member Properties =
      []