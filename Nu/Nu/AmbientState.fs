﻿// Nu Game Engine.
// Copyright (C) Bryan Edds, 2013-2018.

namespace Nu
open System
open Prime
open Nu

/// A command that transforms the world in some manner.
type [<NoEquality; NoComparison>] 'w Command =
    { Execute : 'w -> 'w }

/// A tasklet to be completed at the scheduled tick time.
type [<NoEquality; NoComparison>] 'w Tasklet =
    { ScheduledTime : int64
      Command : 'w Command }

[<AutoOpen>]
module AmbientStateModule =

    /// The ambient state of the world.
    type [<ReferenceEquality>] 'w AmbientState =
        private
            { TickRate : int64
              TickTime : int64
              UpdateCount : int64
              Liveness : Liveness
              Tasklets : 'w Tasklet UList
              TaskletsProcessing : bool
              Metadata : Metadata
              Overlayer : Overlayer
              OverlayRouter : OverlayRouter
              SymbolStore : SymbolStore
              KeyValueStore : UMap<string, obj>
              UserState : UserState }

    [<RequireQualifiedAccess; CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
    module AmbientState =

        /// Get the tick rate.
        let getTickRate state =
            state.TickRate

        /// Get the tick rate as a floating-point value.
        let getTickRateF state =
            single (getTickRate state)

        /// Set the tick rate.
        let setTickRateImmediate tickRate state =
            { state with TickRate = tickRate }

        /// Reset the tick time to 0.
        let resetTickTimeImmediate state =
            { state with TickTime = 0L }

        /// Increment the tick time.
        let incTickTimeImmediate state =
            { state with TickTime = inc state.TickTime }

        /// Increment the tick time.
        let decTickTimeImmediate state =
            { state with TickTime = dec state.TickTime }

        /// Get the tick time.
        let getTickTime state =
            state.TickTime

        /// Check that ticking is enabled.
        let isTicking state =
            getTickRate state <> 0L

        /// Update the tick time by the tick rate.
        let updateTickTime state =
            { state with TickTime = getTickTime state + getTickRate state }

        /// Get the world's update count.
        let getUpdateCount state =
            state.UpdateCount

        /// Increment the update count.
        let incrementUpdateCount state =
            { state with UpdateCount = inc (getUpdateCount state) }

        /// Get the the liveness state of the engine.
        let getLiveness state =
            state.Liveness

        /// Place the engine into a state such that the app will exit at the end of the current update.
        let exit state =
            { state with Liveness = Exiting }

        /// Get the tasklets scheduled for future processing.
        let getTasklets state =
            state.Tasklets

        /// Clear the tasklets from future processing.
        let clearTasklets state =
            { state with
                Tasklets = UList.makeEmpty (UList.getConfig state.Tasklets)
                TaskletsProcessing = true }

        /// Restore the given tasklets from future processing.
        let restoreTasklets tasklets state =
            { state with
                Tasklets = UList.makeFromSeq (UList.getConfig state.Tasklets) (Seq.append (state.Tasklets :> _ seq) (tasklets :> _ seq))
                TaskletsProcessing = false }

        /// Get the tasklets processing state.
        let getTaskletsProcessing state =
            state.TaskletsProcessing

        /// Add a tasklet to be executed at the scheduled time.
        let addTasklet tasklet state =
            { state with Tasklets = UList.add tasklet state.Tasklets }

        /// Add multiple tasklets to be executed at the scheduled times.
        let addTasklets tasklets state =
            { state with Tasklets = UList.makeFromSeq (UList.getConfig state.Tasklets) (Seq.append (tasklets :> _ seq) (state.Tasklets :> _ seq)) }

        /// Get the metadata.
        let getMetadata state =
            state.Metadata
    
        /// Set the metadata.
        let setMetadata metadata state =
            { state with Metadata = metadata }
    
        /// Get the overlayer.
        let getOverlayer state =
            state.Overlayer
    
        /// Set the overlayer.
        let setOverlayer overlayer state =
            { state with Overlayer = overlayer }
    
        /// Get the overlay router.
        let getOverlayRouter state =
            state.OverlayRouter
    
        /// Get the overlay router.
        let getOverlayRouterBy by state =
            by state.OverlayRouter

        /// Get the symbol store with the by map.
        let getSymbolStoreBy by state =
            by state.SymbolStore

        /// Get the symbol store.
        let getSymbolStore state =
            getSymbolStoreBy id state
    
        /// Set the symbol store.
        let setSymbolStore symbolStore state =
            { state with SymbolStore = symbolStore }

        /// Update the symbol store.
        let updateSymbolStore updater state =
            let store = updater (getSymbolStore state)
            { state with SymbolStore = store }

        /// Get the key-value store with the by map.
        let getKeyValueStoreBy by state =
            by state.KeyValueStore

        /// Get the key-value store.
        let getKeyValueStore state =
            getKeyValueStoreBy id state
    
        /// Set the key-value store.
        let setKeyValueStore symbolStore state =
            { state with KeyValueStore = symbolStore }

        /// Update the key-value store.
        let updateKeyValueStore updater state =
            let store = updater (getKeyValueStore state)
            { state with KeyValueStore = store }
    
        /// Get the user-defined state value, cast to 'a.
        let getUserValue state : 'a =
            UserState.get state.UserState
    
        /// Update the user-defined state value.
        let updateUserValue (updater : 'a -> 'b) state =
            { state with UserState = UserState.update updater state.UserState }
    
        /// Make an ambient state value.
        let make tickRate assetMetadataMap overlayRouter overlayer symbolStore userState =
            { TickRate = tickRate
              TickTime = 0L
              UpdateCount = 0L
              Liveness = Running
              Tasklets = UList.makeEmpty Constants.Engine.TaskletListConfig
              TaskletsProcessing = false
              Metadata = assetMetadataMap
              OverlayRouter = overlayRouter
              Overlayer = overlayer
              SymbolStore = symbolStore
              KeyValueStore = UMap.makeEmpty Constants.Engine.KeyValueMapConfig
              UserState = UserState.make userState false }

/// The ambient state of the world.
type 'w AmbientState = 'w AmbientStateModule.AmbientState