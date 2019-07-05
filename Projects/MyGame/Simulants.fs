namespace MyGame

open Nu

module Simulants = 
  let Game = Default.Game
  
  let Gameplay = Screen "Gameplay"
  let Scene = Gameplay / "Scene"
  
  let Fountain = Scene / "Fountain"
  let Player = Scene / "Player"