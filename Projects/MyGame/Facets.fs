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
    
    override this.Register (entity, world) =
      world
      
    override this.Actualize (entity, world) =
      world
      
    static member Properties =
      []