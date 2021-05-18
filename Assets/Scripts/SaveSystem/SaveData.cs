using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SaveData", menuName = "Saving/SavingDataObject", order = 1)]
public class SaveData : ScriptableObject
{
    public Vector3 characterPos;
    public float characterLife = 100;
    public enum TypesOfSpawn { InSpawn, Load};
    public TypesOfSpawn typeOfSpawn;

    [SerializeField] GameObject character;


    public void SpawnCharacterLoading()
    {
        Instantiate(character, characterPos, Quaternion.identity); //Continue
    }

    public void SpawnCharacterInSpawn() //New Game
    {
        characterPos = FindObjectOfType<SpawnPosition>() ? FindObjectOfType<SpawnPosition>().transform.position : Vector3.zero;
        Instantiate(character, characterPos, Quaternion.identity);
    }

    public void SaveCharacterData()
    {
        if (FindObjectOfType<PlayerController>())
        {
            PlayerController pc = FindObjectOfType<PlayerController>();
            characterPos = pc.transform.position;
            characterLife = pc.Health;
        }
    }

    public void SaveCharacterData(float li)
    {
        if (FindObjectOfType<PlayerController>())
        {
            PlayerController ch = FindObjectOfType<PlayerController>();
            characterPos = ch.transform.position;
            characterLife = li;
        }
    }

    public void ChangeSpawnType(int index)
    {
        typeOfSpawn = (TypesOfSpawn)index;
    }

    public void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }


}
