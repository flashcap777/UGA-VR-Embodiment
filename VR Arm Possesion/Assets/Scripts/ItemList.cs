using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemList", menuName ="ScriptableObjects/Utility")]
public class ItemList : ScriptableObject
{
    [SerializeField] public GameObject[] Items;
}
