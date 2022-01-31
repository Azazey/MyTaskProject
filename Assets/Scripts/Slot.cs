using UnityEngine;

public class Slot : MonoBehaviour
{
    private Resource _resource;

    public Resource Resource
    {
        get => _resource;
        set => _resource = value;
    }
}