namespace MyGame

open Nu

#nowarn "1182"

type MyGameDispatcher () = 
  inherit GameDispatcher()
  
  override dispatcher.Register (game, world) =
    let screen, world = World.createScreen<MainScreenDispatcher> (Some "main screen") world
    let world = World.selectScreen screen world
    
    world