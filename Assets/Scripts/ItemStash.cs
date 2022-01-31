using System.Collections.Generic;
using UnityEngine;

public class ItemStash : MonoBehaviour
{
    [SerializeField] private int _sizeOfStash;
    [SerializeField] private Slot _slotPrefab;
    [SerializeField] private int _height;

    private int _amountOfGroups;

    private const float _deltaY = 0.1f;
    private const float _deltaZ = 0.25f;

    private List<Slot> Slots = new List<Slot>();

    public int SizeOfStash => _sizeOfStash;

    public int AmountOfGroups
    {
        get => _amountOfGroups;
        set => _amountOfGroups = value;
    }


    public bool CheckEnoughSpace(Dictionary<ResourceType, int> produceResources)
    {
        int amountResourceNeeded = 0;
        foreach (var resource in produceResources)
        {
            amountResourceNeeded += resource.Value;
        }

        if (GetFreeSlotsCount() >= amountResourceNeeded)
        {
            return true;
        }

        return false;
    }

    public bool CheckResourceTypeInStash(Dictionary<ResourceType, int> resourcesNeed)
    {
        if (resourcesNeed == null)
        {
            return false;
        }

        foreach (var resource in resourcesNeed)
        {
            int foundCount = 0;
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].Resource == null)
                {
                    continue;
                }

                if (Slots[i].Resource.ResourceType == resource.Key)
                {
                    foundCount++;
                    if (foundCount == resource.Value)
                    {
                        break;
                    }
                }
            }

            if (foundCount < resource.Value)
            {
                return false;
            }
        }

        return true;
    }

    public int GetFreeSlotsCount()
    {
        int freeSlots = 0;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].Resource == null)
            {
                freeSlots++;
            }
        }

        return freeSlots;
    }

    public int GetFreeSlotsCountByType(ResourceType resourceType)
    {
        int takenSlots = 0;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].Resource != null)
            {
                if (Slots[i].Resource.ResourceType == resourceType)
                {
                    takenSlots++;
                }
            }
        }

        return Slots.Count / _amountOfGroups - takenSlots;
    }

    public Slot AddResource(Resource resource)
    {
        Slot slot = TryGetFreeSlot();
        if (slot == null)
        {
            return null;
        }

        slot.Resource = resource;
        return slot;
    }

    public void Transfer(ItemStash to, List<ResourceType> resourceTypes)
    {
        foreach (var resourceType in resourceTypes)
        {
            int freeSlotsCount = to.GetFreeSlotsCountByType(resourceType);
            for (int i = 0; i < Slots.Count; i++)
            {
                if (freeSlotsCount == 0)
                {
                    break;
                }

                Resource resource = Slots[i].Resource;
                if (resource != null)
                {
                    if (resource.ResourceType == resourceType)
                    {
                        Slots[i].Resource = null;
                        resource.StartPutItem(to.AddResource(resource).transform);
                        RebuildStash();
                        return;
                    }
                }
            }
        }
    }

    public void Transfer(ItemStash to)
    {
        int freeSlotsCount = to.GetFreeSlotsCount();
        int counterAddedItems = 0;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (counterAddedItems >= freeSlotsCount)
            {
                break;
            }

            Resource resource = Slots[i].Resource;
            if (resource != null)
            {
                Slots[i].Resource = null;
                resource.StartPutItem(to.AddResource(resource).transform);
                counterAddedItems++;
            }
        }

        RebuildStash();
    }

    public List<Resource> RemoveResources(Dictionary<ResourceType, int> needToProduce)
    {
        List<Resource> deletedResource = new List<Resource>();
        foreach (var resource in needToProduce)
        {
            int foundCount = 0;
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].Resource == null)
                {
                    continue;
                }

                if (Slots[i].Resource.ResourceType == resource.Key)
                {
                    deletedResource.Add(Slots[i].Resource);
                    Slots[i].Resource = null;
                    foundCount++;
                }

                if (foundCount == resource.Value)
                {
                    break;
                }
            }
        }

        RebuildStash();
        return deletedResource;
    }

    private void RebuildStash()
    {
        List<Resource> resources = new List<Resource>();
        foreach (Slot slot in Slots)
        {
            if (slot.Resource != null)
            {
                resources.Add(slot.Resource);
                slot.Resource = null;
            }
        }

        foreach (Resource resource in resources)
        {
            Slot slot = AddResource(resource);
            resource.StartPutItem(slot.transform);
        }
    }

    private void Start()
    {
        MaxSizeStashMaker();
    }

    private void MaxSizeStashMaker() //refactor
    {
        for (int i = 0; i < _sizeOfStash; i++)
        {
            Slot item = Instantiate(_slotPrefab, GetSlotPosition(i), transform.rotation, transform);
            Slots.Add(item);
            item.name = "slot";
        }
    }

    private Vector3 GetSlotPosition(int index)
    {
        if (index == 0)
        {
            return transform.position;
        }

        if (index % _height == 0)
        {
            return new Vector3(transform.position.x, transform.position.y,
                Slots[Slots.Count - 1].transform.position.z - _deltaZ);
        }

        return Slots[Slots.Count - 1].transform.position + Vector3.up * _deltaY;
    }

    private Slot TryGetFreeSlot()
    {
        foreach (var slot in Slots)
        {
            if (slot.Resource == null)
            {
                return slot;
            }
        }

        return null;
    }
}