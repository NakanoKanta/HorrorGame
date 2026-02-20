using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [SerializeField] int targetItems = 4;
    int itemsCount = 0;

    public int ItemCount => itemsCount;
    public int TargetItem => targetItems;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem()
    {
        itemsCount++;
        Debug.Log("ƒAƒCƒeƒ€Žæ“¾”: " + itemsCount);
    }

    public bool HasEnoughItems()
    {
        return itemsCount >= targetItems;
    }
}
