using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactoryType
{
    Sawmill,
    IronSmeltery,
    GoldSmeltery
}

public class Factory : MonoBehaviour
{
    [SerializeField] private FactoryType _factoryType;
    [SerializeField] private FactoryEnterance _factoryEnterance;
    [SerializeField] private FactoryExit _factoryExit;
    [SerializeField] private float _timeToProduce;

    private Dictionary<ResourceType, int> _resourcesNeedToProduce;
    private Dictionary<ResourceType, int> _resourceToProduce;

    public FactoryType FactoryType => _factoryType;

    public event Action StashFull;
    public event Action NotEnoughResource;
    public event Action<float> StartProduce;

    private void Start()
    {
        switch (_factoryType)
        {
            case FactoryType.Sawmill:
                _resourcesNeedToProduce = new Dictionary<ResourceType, int>();
                _resourceToProduce = new Dictionary<ResourceType, int> {{ResourceType.Wood, 1}};
                break;
            case FactoryType.GoldSmeltery:
                _resourcesNeedToProduce = new Dictionary<ResourceType, int>
                    {{ResourceType.Wood, 1}, {ResourceType.Iron, 1}};
                _resourceToProduce = new Dictionary<ResourceType, int> {{ResourceType.Gold, 1}};
                break;
            case FactoryType.IronSmeltery:
                _resourcesNeedToProduce = new Dictionary<ResourceType, int> {{ResourceType.Wood, 1}};
                _resourceToProduce = new Dictionary<ResourceType, int> {{ResourceType.Iron, 1}};
                break;
        }

        List<ResourceType> needToProduceTypes = new List<ResourceType>();
        foreach (var resource in _resourcesNeedToProduce)
        {
            if (!needToProduceTypes.Contains(resource.Key))
            {
                needToProduceTypes.Add(resource.Key);
            }
        }

        _factoryEnterance.InitializeAvailableResourceTypes(needToProduceTypes);

        List<ResourceType> resourceProducedTypes = new List<ResourceType>();
        foreach (var resource in _resourceToProduce)
        {
            if (!resourceProducedTypes.Contains(resource.Key))
            {
                resourceProducedTypes.Add(resource.Key);
            }
        }

        _factoryEnterance.SetAmountOfGroups(_resourcesNeedToProduce);
        _factoryExit.SetAmountOfGroups(_resourceToProduce);
        StartCoroutine(TryProduceResource());
    }

    private IEnumerator TryProduceResource()
    {
        WaitForSeconds produceDelay = new WaitForSeconds(_timeToProduce);
        while (true)
        {
            bool canProduce = true;
            if (!_factoryEnterance.CheckEnoughResourceInStash(_resourcesNeedToProduce))
            {
                canProduce = false;
                NotEnoughResource?.Invoke();
            }

            if (!_factoryExit.HaveEnoughSpace(_resourceToProduce))
            {
                canProduce = false;
                StashFull?.Invoke();
            }

            if (canProduce)
            {
                StartProduce?.Invoke(_timeToProduce);
                _factoryEnterance.DeleteResources(_resourcesNeedToProduce);
                yield return produceDelay;
                _factoryExit.SpawnProducedItem(_resourceToProduce);
            }

            yield return null;
        }
    }
}