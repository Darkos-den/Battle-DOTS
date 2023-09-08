using Unity.Entities;

namespace Darkos {

    [DisableAutoCreation]
    public partial class ActionSelectionSystem : SystemBase {

        private UnitAction? _lastAction;

        protected override void OnStartRunning() {
            _lastAction = null;
            UnitActionInput.Instance.OnActionSelected += OnUnitAction;
        }

        protected override void OnStopRunning() {
            UnitActionInput.Instance.OnActionSelected -= OnUnitAction;
        }

        protected override void OnUpdate() {
            if (_lastAction != null) {
                if (!SystemAPI.TryGetSingletonEntity<TargetComponent>(out Entity entity)) {
                    entity = EntityManager.CreateSingleton<TargetComponent>();
                    EntityManager.AddComponent<ActiveTag>(entity);
                }
                SystemAPI.SetComponentEnabled<ActiveTag>(entity, true);

                switch (_lastAction) {
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

                _lastAction = null;
            }
        }

        private void OnUnitAction(UnitAction action) {
            _lastAction = action;
        }
    }
}
