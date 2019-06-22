﻿// Nu Game Engine.
// Copyright (C) Bryan Edds, 2013-2018.

namespace Nu
open System
open Prime
open Nu

/// Contains primitives for describing simulant layouts.    
module Layout =

    /// Describe a game to be loaded from a file.
    let gameFromFile<'d when 'd :> GameDispatcher> filePath =
        GameFromFile filePath

    /// Describe a game with the given definitions and contained screens.
    let game<'d when 'd :> GameDispatcher> definitions children =
        GameFromDefinitions (typeof<'d>.Name, definitions, children)

    /// Describe a screen to be loaded from a file.
    let screenFromFile<'d when 'd :> ScreenDispatcher> (screen : Screen) behavior filePath =
        ScreenFromFile (screen.ScreenName, behavior, filePath)

    /// Describe a screen to be loaded from a file.
    let screenFromLayerFile<'d when 'd :> ScreenDispatcher> (screen : Screen) behavior filePath =
        ScreenFromLayerFile (screen.ScreenName, behavior, typeof<'d>, filePath)

    /// Describe a screen with the given definitions and contained layers.
    let screen<'d when 'd :> ScreenDispatcher> (screen : Screen) behavior definitions children =
        ScreenFromDefinitions (typeof<'d>.Name, screen.ScreenName, behavior, definitions, children)

    /// Describe a layer to be loaded from a file.
    let layerFromFile<'d when 'd :> LayerDispatcher> (layer : Layer) filePath =
        LayerFromFile (layer.LayerName, filePath)

    /// Describe a layer with the given definitions and contained entities.
    let layer<'d when 'd :> LayerDispatcher> (layer : Layer) definitions children =
        LayerFromDefinitions (typeof<'d>.Name, layer.LayerName, definitions, children)

    /// Describe entities to be obtained from a lens.
    let entities (lens : Lens<'a list, World>) (mapper : 'a -> EntityLayout) =
        let mapper = fun (a : obj) -> mapper (a :?> 'a)
        EntitiesFromStream (lens, mapper)

    /// Describe an entity to be optionally obtained from a lens.
    let entityOpt (lens : Lens<'a option, World>) (mapper : 'a -> EntityLayout) =
        entities (lens --> function Some a -> [a] | None -> []) mapper

    /// Describe an entity to be loaded from a file.
    let entityFromFile<'d when 'd :> EntityDispatcher> (entity : Entity) filePath =
        EntityFromFile (entity.EntityName, filePath)

    /// Describe an entity with the given definitions.
    let entity<'d when 'd :> EntityDispatcher> (entity : Entity) definitions =
        EntityFromDefinitions (typeof<'d>.Name, entity.EntityName, definitions)

    /// Describe an effect with the given definitions.
    let effect entity_ definitions = entity<EffectDispatcher> entity_ definitions

    /// Describe a node with the given definitions.
    let node entity_ definitions = entity<NodeDispatcher> entity_ definitions

    /// Describe a button with the given definitions.
    let button entity_ definitions = entity<ButtonDispatcher> entity_ definitions

    /// Describe a label with the given definitions.
    let label entity_ definitions = entity<LabelDispatcher> entity_ definitions

    /// Describe a text with the given definitions.
    let text entity_ definitions = entity<TextDispatcher> entity_ definitions

    /// Describe a toggle with the given definitions.
    let toggle entity_ definitions = entity<ToggleDispatcher> entity_ definitions

    /// Describe a feeler with the given definitions.
    let feeler entity_ definitions = entity<FeelerDispatcher> entity_ definitions

    /// Describe a fill bar with the given definitions.
    let fillBar entity_ definitions = entity<FillBarDispatcher> entity_ definitions

    /// Describe a block with the given definitions.
    let block entity_ definitions = entity<BlockDispatcher> entity_ definitions

    /// Describe a box with the given definitions.
    let box entity_ definitions = entity<BoxDispatcher> entity_ definitions

    /// Describe a top-view character with the given definitions.
    let topViewCharacter entity_ definitions = entity<TopViewCharacterDispatcher> entity_ definitions

    /// Describe a side-view character with the given definitions.
    let sideViewCharacter entity_ definitions = entity<SideViewCharacterDispatcher> entity_ definitions

    /// Describe a tile map with the given definitions.
    let tileMap entity_ definitions = entity<TileMapDispatcher> entity_ definitions