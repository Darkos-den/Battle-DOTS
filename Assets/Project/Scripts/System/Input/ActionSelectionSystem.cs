using Unity.Entities;
using UnityEngine;

namespace Darkos {

    [DisableAutoCreation]
    public partial class ActionSelectionSystem : SystemBase {

        private bool _active = false;

        protected override void OnCreate() {
            Debug.Log(">>> OnCreate");
            UnitActionInput.Instance.OnActionSelected += OnUnitAction;
        }

        protected override void OnDestroy() {
            UnitActionInput.Instance.OnActionSelected -= OnUnitAction;
        }

        protected override void OnUpdate() {
            if( !_active) {
                Debug.Log(">>> activate action selection");
                _active = true;
            }
        }

        private void OnUnitAction(UnitAction action) {
            Debug.Log(">>> OnUnitAction | " + _active);
            if (!_active) {
                return;
            }

            if (!SystemAPI.TryGetSingletonEntity<TargetComponent>(out Entity entity)) {
                entity = EntityManager.CreateSingleton<TargetComponent>();
                EntityManager.AddComponent<ActiveTag>(entity);
            }
            SystemAPI.SetComponentEnabled<ActiveTag>(entity, true);

            switch (action) {
                case UnitAction.Attack: {
                        EntityManager.AddComponent<AttackActionFlag>(entity);
                        EntityManager.SetComponentData(entity, new TargetComponent { Type = TargetType.Enemy });
                        break;
                    }
                case UnitAction.Heal: {
                        EntityManager.AddComponent<HealActionFlag>(entity);
                        EntityManager.SetComponentData(entity, new TargetComponent { Type = TargetType.Friend });
                        break;
                    }
            }

            _active = false;
        }
    }
}
