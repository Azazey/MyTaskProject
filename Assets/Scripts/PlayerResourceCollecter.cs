using UnityEngine;

public class PlayerResourceCollecter : MonoBehaviour
{
    [SerializeField] private ItemStash _itemStash;

    private void Start()
    {
        _itemStash.AmountOfGroups = 1;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Resource resource = collider.GetComponent<Resource>();
        if (resource == null)
        {
            return;
        }

        if (resource.IsPickable)
        {
            Slot slot = _itemStash.AddResource(resource);
            if (slot == null)
            {
                return;
            }

            resource.IsPickable = false;
            resource.StartPutItem(slot.transform);
        }
    }
}