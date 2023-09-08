using UnityEngine;

public class HudController : MonoBehaviour {

    private UnitActionInput _input;

    private void Start() {
        _input = UnitActionInput.Instance;
    }

    public void OnAttackClick() {
        _input.SelectAction(UnitAction.Attack);
    }
}
