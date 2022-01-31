using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryExit : MonoBehaviour
{
    [SerializeField] private ItemStash _itemStash;
    [SerializeField] private List<Resource> _prefabs;

    public void SpawnProducedItem(Dictionary<ResourceType, int> produceResources)
    {
        foreach (var resource in produceResources)
        {
            for (int i = 0; i < resource.Value; i++)
            {
                Resource resourceSpawned = Instantiate(_prefabs.First(x => x.ResourceType == resource.Key), transform);
                resourceSpawned.IsPickable = false;
                resourceSpawned.StartPutItem(_itemStash.AddResource(resourceSpawned).transform);
            }
        }
    }

    public void SetAmountOfGroups(Dictionary<ResourceType, int> produceResources)
    {
        _itemStash.AmountOfGroups = produceResources.Count;
    }

    public bool HaveEnoughSpace(Dictionary<ResourceType, int> produceResources)
    {
        return _itemStash.CheckEnoughSpace(produceResources);
    }

    private void OnTriggerEnter(Collider collider)
    {
        ItemStash playerStash = collider.GetComponentInChildren<ItemStash>();
        if (playerStash == null)
        {
            return;
        }

        _itemStash.Transfer(playerStash);
    }
}