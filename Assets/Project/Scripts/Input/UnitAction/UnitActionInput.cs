using UnityEngine.Events;

public class UnitActionInput {

    private UnitActionInput() { }

    private static UnitActionInput _instance;
    public static UnitActionInput Instance { 
        get {
            _instance ??= new UnitActionInput();
            return _instance;
        }
    }

    public UnityAction<UnitAction> OnActionSelected;

    private bool _enabled;

    public void SelectAction(UnitAction action) {
        if (_enabled) {
            OnActionSelected?.Invoke(action);
        }
    }

    public void Enable() {
        _enabled = true;
    }

    public void Disable() {
        _enabled = false;
    }
}
