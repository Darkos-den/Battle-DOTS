using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;

    private Vector3 _cameraPosition;
    private float _scaleFactor;

    void Start() {
        _cameraPosition = Camera.main.transform.position;
        _scaleFactor = (_maxScale = _minScale) / 100f;
    }

    // Update is called once per frame
    void Update() {
        var tmp = Vector3.Distance(transform.position, _cameraPosition);

        var distance = Mathf.Clamp(tmp, _minDistance, _maxDistance);

        var percent = (distance - _minDistance) * 100f / (_maxDistance - _minDistance);

        var scale = _minScale + percent * _scaleFactor;

        transform.localScale = new Vector3(scale, scale, scale);
    }
}
