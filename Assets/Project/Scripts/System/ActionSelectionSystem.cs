using Unity.Entities;

namespace Darkos {

    [UpdateInGroup(typeof(InitializationSystemGroup))]
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
                _lastAction = null;
                var entity = EntityManager.CreateSingleton<TargetComponent>();

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
            }
        }

        private void OnUnitAction(UnitAction action) {
            _lastAction = action;
        }
    }
}
