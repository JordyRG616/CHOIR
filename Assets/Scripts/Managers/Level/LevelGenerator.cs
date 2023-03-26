using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.TotalJSON;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private CameraManager cam;
    [Space]
    [Header("Prefabs")]
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject weaponSlot;
    [Space]
    [SerializeField] private TextAsset levelData;
    public LevelData loadedData;
    [Space]
    [SerializeField] private List<GameObject> entities;
    [Space]
    public bool loaded;

    private Vector2 levelOffset;
    private DeserializeSettings deserializeSettings;

    [ContextMenu("Load")]
    private void LoadData()
    {
        deserializeSettings = new DeserializeSettings();
        deserializeSettings.RequireAllFieldsArePopulated = false;

        //loadedData = JsonUtility.FromJson<LevelData>(levelData.text);
        var json = JSON.ParseString(levelData.text);
        Debug.Log(json.CreateString());
        loadedData = json.Deserialize<LevelData>(deserializeSettings);
        levelOffset = loadedData.LevelSize / 2;
        levelOffset.y *= -1;
        loaded = true;
    }

    [ContextMenu("Create Entities")]
    private void CreateEntities()
    {
        if(entities.Count > 0)
        {
            Debug.LogError("Clear entities first!");
            return;
        } else if (!loaded)
        {
            Debug.LogError("Load data first!");
            return;
        }

        var c_data = loadedData.entities.Crystal_Position[0];
        var _c = Instantiate(crystal, GetWorldPosition(c_data.Position), Quaternion.identity, transform);
        entities.Add(_c);

        var ws_data = loadedData.entities.Weapon_Slot;
        foreach (var slot in ws_data)
        {
            var _s = Instantiate(weaponSlot, GetWorldPosition(slot.Position), Quaternion.identity, transform.Find("Weapon Slots"));
            // _s.GetComponent<WeaponSlot>().UnlockDirectionalButtons(slot.customFields.Directions);
            entities.Add(_s);
        }

        var spw_data = loadedData.entities.Enemy_Spawner;
        foreach (var spw in spw_data)
        {
            var _spw = Instantiate(spawner, GetWorldPosition(spw.Position), Quaternion.identity, transform.Find("Spawners"));
            entities.Add(_spw);
        }
    }

    [ContextMenu("Clear")]
    private void ClearData()
    {
        loadedData = new LevelData();
        loaded = false;

        entities.ForEach(x => DestroyImmediate(x));
        entities.Clear();
    }

    private Vector2 GetWorldPosition(Vector2 levelPosition)
    {
        var pos = levelPosition - levelOffset;
        pos /= 16;
        return pos;
    }
}

[System.Serializable]
public class EntityInterpreter
{
    public string entityId;
    public GameObject prefab;
}