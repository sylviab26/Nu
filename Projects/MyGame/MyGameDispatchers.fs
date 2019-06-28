namespace MyGame

open Nu

type MainScreenDispatcher() =
  inherit ScreenDispatcher()
  
  override this.Register (screen, world) =
    world
  
type MainLayerDispatcher() =
  inherit LayerDispatcher()
  
  override this.Register (layer, world) =
    world
  
type PlayerEntityDispatcher() =
  inherit EntityDispatcher()
  
  override this.Register (entity, world) =
    world