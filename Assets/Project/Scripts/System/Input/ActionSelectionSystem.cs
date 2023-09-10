using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Darkos {

    public partial class ActionSelectionSystem : SystemBase {

        private UnitAction? _lastAction = null;

        protected override void OnCreate() {
            RequireForUpdate<AwaitActionTag>();
            UnitActionInput.Instance.OnActionSelected += OnUnitAction;
        }

        protected override void OnDestroy() {
            UnitActionInput.Instance.OnActionSelected -= OnUnitAction;
        }

        protected override void OnUpdate() {
            if(_lastAction == null) {
                return;
            }

            Entity unit = SystemAPI.GetSingleton<ActiveUnit>().Value;

            if (unit == Entity.Null) {
                return;
            }

            var entity = EntityManager.CreateSingleton<TargetComponent>();

            switch (_lastAction) {
                case UnitAction.Attack: {
                        EntityManager.SetComponentData(entity, new TargetComponent { Type = TargetType.Enemy, HealtEffect = -30 });
                        break;
                    }
                case UnitAction.Heal: {
                        EntityManager.SetComponentData(entity, new TargetComponent { Type = TargetType.Friend, HealtEffect = 20 });
                        break;
                    }
            }

            var tag = SystemAPI.GetSingletonEntity<AwaitActionTag>();
            EntityManager.DestroyEntity(tag);

            _lastAction = null;
        }

        private void OnUnitAction(UnitAction action) {
            _lastAction = action;
        }
    }
}
