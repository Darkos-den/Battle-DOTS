using Darkos;
using System;
using UnityEngine;

public class UnitPrefabProvider: MonoBehaviour {

    public static UnitPrefabProvider Instance;

    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] public GameObject paladinPrefab;

    private void Awake() {
        Instance = this;
    }

    public GameObject GetPrefab(UnitType type) {
        switch (type) {
            case UnitType.Paladin:
                return paladinPrefab;
            case UnitType.Monster:
                return monsterPrefab;
            default:
                throw new Exception("invalid type");
        }
    }

}
