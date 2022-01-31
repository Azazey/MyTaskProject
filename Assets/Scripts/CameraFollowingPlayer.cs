using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _lerpChange;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _lerpChange);
    }
}