using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
    public int width;
    public int height;
    public GenerationEntities entities;

    [System.Serializable]
    public class GenerationEntities
    { 
        public List<LevelCrystalData> Crystal_Position;
        public List<LevelSpawnerData> Enemy_Spawner;
        public List<LevelSlotData> Weapon_Slot;
        public List<LevelDoorData> Door_In;
        public List<LevelDoorData> Door_Out;
    }

    public Vector2 LevelSize => new Vector2(width, height);
}

[System.Serializable]
public abstract class LevelEntity
{
    public string id;
    public string iid;
    public int x;
    public int y;

    public virtual Vector2 Position => new Vector2(x, -y);
}

[System.Serializable]
public class LevelSpawnerData : LevelEntity
{
    public override Vector2 Position => new Vector2(x, -y + 40);
}

[System.Serializable]
public class LevelSlotData : LevelEntity
{
    [System.Serializable]
    public class SlotCustomFields
    {
        public string[] Directions;
    }

    public SlotCustomFields customFields;
}

[System.Serializable]
public class LevelCrystalData : LevelEntity
{
    
}

[System.Serializable]
public class LevelDoorData
{
    public class OutData
    {
        public string entityIid;
    }

    public OutData outDoor;
}
