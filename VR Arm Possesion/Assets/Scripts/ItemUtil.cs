using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUtil : MonoBehaviour
{
    [SerializeField] public ItemList List;
    public GameObject[] GetAllItems()
    {
        return List.Items;
    }
}
