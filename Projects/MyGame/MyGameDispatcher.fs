namespace MyGame

open Nu.WorldTypes

#nowarn "1182"

type MyGameDispatcher () = 
  inherit GameDispatcher()
  
  override dispatcher.Register (game, world) =
    world