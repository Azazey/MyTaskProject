using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buildingText;
    [SerializeField] private Factory _factory;

    private FactoryType _factoryType;
    private string _factoryName;

    private void Start()
    {
        _factoryType = _factory.FactoryType;
        switch (_factoryType)
        {
            case FactoryType.Sawmill:
                _factoryName = "Лесопилка:";
                break;
            case FactoryType.GoldSmeltery:
                _factoryName = "Золотая кузня:";
                break;
            case FactoryType.IronSmeltery:
                _factoryName = "Железная кузня:";
                break;
        }

        _factory.StashFull += UIOnMaxStash;
        _factory.NotEnoughResource += NotEnoughResource;
        _factory.StartProduce += StartProduce;
    }

    private void UIOnMaxStash()
    {
        _buildingText.gameObject.SetActive(true);
        _buildingText.text = _factoryName + "склад переполнен!";
    }

    private void NotEnoughResource()
    {
        _buildingText.gameObject.SetActive(true);
        _buildingText.text = _factoryName + "недостаточно ресурсов!";
    }

    private void StartProduce(float timeToProduce)
    {
        _buildingText.gameObject.SetActive(true);
        StartCoroutine(ItemProduceProcess(timeToProduce));
    }

    private IEnumerator ItemProduceProcess(float timeToProduce)
    {
        for (float i = 0; i < timeToProduce; i += Time.deltaTime)
        {
            _buildingText.text = _factoryName + "Сделано:" + Math.Round(i / timeToProduce * 100) + "%";
            yield return null;
        }

        _buildingText.gameObject.SetActive(false);
    }
}