module Assets

open System
open Nu

let Tiles32Package = "Tiles"

let img x y =
  // 27, 18
  let num = (x - 1) * 27 + y
  let stringy = num |> string
  
  AssetTag.make<Image> Tiles32Package ("tile_" + stringy.PadLeft(4, '0'))

let Human1LeftIdle = AssetTag.make<Image> Tiles32Package "tile_0023"
let Human1DownIdle = AssetTag.make<Image> Tiles32Package "tile_0024"
let Human1UpIdle = AssetTag.make<Image> Tiles32Package "tile_0025"
let Human1RightIdle = AssetTag.make<Image> Tiles32Package "tile_0026"

let Human1LeftWalk1 = AssetTag.make<Image> Tiles32Package "tile_0050"
let Human1DownWalk1 = AssetTag.make<Image> Tiles32Package "tile_0051"
let Human1UpWalk1 = AssetTag.make<Image> Tiles32Package "tile_0052"
let Human1RightWalk1 = AssetTag.make<Image> Tiles32Package "tile_0053"

let Human1LeftWalk2 = AssetTag.make<Image> Tiles32Package "tile_0077"
let Human1DownWalk2 = AssetTag.make<Image> Tiles32Package "tile_0078"
let Human1UpWalk2 = AssetTag.make<Image> Tiles32Package "tile_0079"
let Human1RightWalk2 = AssetTag.make<Image> Tiles32Package "tile_0080"

let FountainLarge1 = img 1 1
let FountainLarge2 = img 2 1
let FountainLarge3 = img 3 1
let FountainLarge4 = img 1 2
let FountainLarge5 = img 2 2
let FountainLarge6 = img 3 2
let FountainLarge7 = img 1 3
let FountainLarge8 = img 2 3
let FountainLarge9 = img 3 3
