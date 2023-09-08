using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Darkos {

    public class SelectionIndicatorPooling : MonoBehaviour {

        public static SelectionIndicatorPooling Instance;

        [SerializeField]
        private Transform _indicator;

        private void Awake() {
            Instance = this;
        }

        public Transform GetSelectionIndicator() {
            return _indicator;
        }
    }

}