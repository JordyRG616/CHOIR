using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;
using TotemServices;
using TotemEntities.DNA;
using TotemServices.DNA;

public class TotemManager : MonoBehaviour
{
    #region Main
    private static TotemManager _instance;
    public static TotemManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<TotemManager>();

            return _instance;
        }

    }
    #endregion


    [SerializeField] private string gameId;
    private TotemCore totemDB;
    private TotemUser totemUser;

    public bool isLogged => totemUser != null;

    private List<TotemDNADefaultAvatar> userAvatars = new List<TotemDNADefaultAvatar>();
    private TotemDNADefaultAvatar currentAvatar;

    public AvatarData selectedAvatar;

    [ContextMenu("Login")]
    public void Login()
    {
        totemDB = new TotemCore(gameId);

        totemDB.AuthenticateCurrentUser((user) => GetTotemAssets(user));

        DontDestroyOnLoad(gameObject);
    }

    private void GetTotemAssets(TotemUser user)
    {
        totemDB.GetUserAvatars<TotemDNADefaultAvatar>(user, TotemDNAFilter.DefaultAvatarFilter, (avatars) => {
            foreach(var avatar in avatars)
            {
                AssetBrowserManager.Main.ReceiveNewAvatar(avatar);
            }

            userAvatars = avatars;
        });

        totemUser = user;
    }
}
