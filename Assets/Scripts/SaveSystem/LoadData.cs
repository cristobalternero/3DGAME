using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    [SerializeField] SaveData saveObject;
    
    void Start()
    {
        switch (saveObject.typeOfSpawn)
        {
            case SaveData.TypesOfSpawn.InSpawn:
                saveObject.SpawnCharacterInSpawn();
                break;
            case SaveData.TypesOfSpawn.Load:
                saveObject.SpawnCharacterLoading();
                break;
            default:
                break;
        }
    }  
}
