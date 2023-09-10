using Unity.Entities;
using UnityEngine;

namespace Darkos {

    [UpdateAfter(typeof(GameLoopSystem))]
    public partial class DebugStateMachineSystem : SystemBase {

        protected override void OnCreate() {
            RequireForUpdate<GameStateDebugComponent>();
        }

        protected override void OnUpdate() {
            foreach (var item in SystemAPI.Query<GameStateComponent>())
            {
                var gameTree = SystemAPI.ManagedAPI.GetSingleton<GameStateDebugComponent>();
                switch (item.Value) {
                    case GameState.Idle: {
                            gameTree.StateTree.OnActiveStateChanged?.Invoke(3);
                            break;
                        }
                    case GameState.PlayerActivation: {
                            gameTree.StateTree.OnActiveStateChanged?.Invoke(1);
                            break;
                        }
                    case GameState.UnitActivation: {
                            gameTree.StateTree.OnActiveStateChanged?.Invoke(2);
                            break;
                        }
                    case GameState.UnitAction: {
                            gameTree.StateTree.OnActiveStateChanged?.Invoke(4);
                            break;
                        }
                }
            }
        }
    }
}
