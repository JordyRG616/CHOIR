using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlbumManager : MonoBehaviour
{
    #region Main
    private static AlbumManager _instance;
    public static AlbumManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<AlbumManager>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private Album currentAlbum;
    private int numberOfLevels;
    private int levelIndex;


    void Awake()
    {
        if(_instance != null) Destroy(gameObject);
    }

    void Start()
    {
        numberOfLevels = currentAlbum.levelNames.Count;
        DontDestroyOnLoad(gameObject);
        GetComponent<Inventory>().SetMaxHealth(currentAlbum.initialHealth);
    }

    public string GetNextLevelName()
    {
        levelIndex++;
        return currentAlbum.levelNames[levelIndex];
    }
}
