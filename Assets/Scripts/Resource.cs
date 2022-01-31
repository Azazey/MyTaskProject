using System.Collections;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Iron,
    Gold
}

public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private float _itemTravelTime;

    private Coroutine _putItem;
    private bool _isPickable = true;

    public ResourceType ResourceType => _resourceType;

    public bool IsPickable
    {
        get => _isPickable;
        set => _isPickable = value;
    }

    public void StartPutItem(Transform slot)
    {
        if (_putItem != null)
        {
            StopCoroutine(_putItem);
        }

        _putItem = StartCoroutine(PutItemToStash(slot));
    }

    private IEnumerator PutItemToStash(Transform slot)
    {
        for (float t = 0; t < _itemTravelTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, slot.position, t / _itemTravelTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, slot.rotation, t / _itemTravelTime);
            yield return null;
        }

        transform.parent = slot;
    }
}