namespace MyGame

open Nu

#nowarn "1182"
  
type PlayerEntityDispatcher() =
  inherit EntityDispatcher()
  
  override this.Register (entity, world) =
    world
    
type MainLayerDispatcher() =
  inherit LayerDispatcher()

  override this.Register (layer, world) =
    world
    
type MainScreenDispatcher() =
  inherit ScreenDispatcher()
  
  override this.Register (screen, world) =
    let layer = World.createLayer<MainLayerDispatcher> (Some "main layer") screen world
    
    world
