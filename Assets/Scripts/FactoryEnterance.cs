using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEnterance : MonoBehaviour
{
    [SerializeField] private ItemStash _itemStash;
    [SerializeField] private float _transferDelay;

    private List<ResourceType> _availableResourceTypes;
    private Coroutine _transfer;

    public void SetAmountOfGroups(Dictionary<ResourceType, int> needToProduce)
    {
        _itemStash.AmountOfGroups = needToProduce.Count;
    }

    public void DeleteResources(Dictionary<ResourceType, int> needToProduce)
    {
        List<Resource> resourceToDestroy = _itemStash.RemoveResources(needToProduce);
        foreach (var resource in resourceToDestroy)
        {
            Destroy(resource.gameObject);
        }
    }

    public bool CheckEnoughResourceInStash(Dictionary<ResourceType, int> needToProduce)
    {
        return _itemStash.CheckResourceTypeInStash(needToProduce);
    }

    public void InitializeAvailableResourceTypes(List<ResourceType> resourceTypes)
    {
        _availableResourceTypes = resourceTypes;
    }

    private void OnTriggerEnter(Collider collider)
    {
        ItemStash playerStash = collider.GetComponentInChildren<ItemStash>();
        if (playerStash == null)
        {
            return;
        }

        _transfer = StartCoroutine(StartTransfer(playerStash));
    }

    private void OnTriggerExit(Collider collider)
    {
        ItemStash playerStash = collider.GetComponentInChildren<ItemStash>();
        if (playerStash == null)
        {
            return;
        }

        if (_transfer != null)
        {
            StopCoroutine(_transfer);
        }
    }

    private IEnumerator StartTransfer(ItemStash playerStash)
    {
        WaitForSeconds transferDelay = new WaitForSeconds(_transferDelay);
        while (true)
        {
            if (playerStash.GetFreeSlotsCount() == playerStash.SizeOfStash)
            {
                break;
            }
            playerStash.Transfer(_itemStash, _availableResourceTypes);
            yield return transferDelay;
        }
    }
}