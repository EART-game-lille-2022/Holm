using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Vector3 _rotateAxis;
    [SerializeField] private float _rotateSpeed = 1;
    private float _dayTime;

    void Update()
    {

        _light.transform.Rotate(_rotateAxis * _rotateSpeed);
    }
}
