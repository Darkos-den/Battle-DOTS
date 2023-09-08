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

    public void SelectAction(UnitAction action) {
        OnActionSelected?.Invoke(action);
    }
}
