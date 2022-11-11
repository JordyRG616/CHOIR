using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    #region Main
    private static RewardManager _instance;
    public static RewardManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<RewardManager>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private List<WeaponCard> cards;
    public List<WeaponCard> defaultCards
    {
        get => cards;
    }
    [SerializeField] private List<WeaponCard> postInitializeCards;
    private List<WeaponCard> cardsOnCooldown = new List<WeaponCard>();
    [SerializeField] private List<WeaponCardSelection> selections;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private List<UpgradesByLevel> upgradesByLevel;
    [SerializeField] private float extraEnemieChanceIncrement;

    [Header("Initial Reward")]
    [SerializeField] private List<InitialWeaponCards> initialCards;

    [Header("Special Cards")]
    [SerializeField] private List<SpecialCardData> specialCards;

    public List<string> initialLanes = new List<string>();

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI costUI;
    [SerializeField] private GameObject rerrollButton;
    public int rerrollCost
    {
        get
        {
            var cost = Mathf.CeilToInt(CrystalManager.Main.level / 10);
            if (cost == 0) cost += 1;
            return cost;
        }
    }


    public IEnumerator Start()
    {
        yield return new WaitUntil(() => FindObjectOfType<MainMenuController>().started == true && initialLanes.Count == 2);

        foreach (var lane in initialLanes)
        {
            var cardList = initialCards.Find(x => x.lane == lane);
            OpenReward(cardList.cardList);

            selections[0].PreviewUpgrade();

            TutorialManager.Main.DoTutorialStep("Card Choice");

            yield return new WaitUntil(() => TutorialManager.Main.tutorialOn == false);

            TutorialManager.Main.DoTutorialStep("Weapon Preview");

            yield return new WaitUntil(() => TutorialManager.Main.tutorialOn == false);

            TutorialManager.Main.DoTutorialStep("Weapon Class");

            yield return new WaitUntil(() => TutorialManager.Main.tutorialOn == false);

            TutorialManager.Main.DoTutorialStep("Class Bonus");

            yield return new WaitUntil(() => TutorialManager.Main.tutorialOn == false);

            selections[0].DisablePreviews();

            yield return new WaitUntil(() => rewardPanel.activeSelf == false);
        }

        PostInitialize();

        OpenReward(cards);
        yield return new WaitUntil(() => rewardPanel.activeSelf == false);

        rerrollButton.SetActive(true);

        TutorialManager.Main.DoTutorialStep("Dragging Tutorial");

    }

    private void Update()
    {
        costUI.text = "-" + rerrollCost;
    }

    private void PostInitialize()
    {
        cards.AddRange(postInitializeCards);
    }

    public void Rerroll()
    {
        CrystalManager.Main.TakeDamage(rerrollCost);
        OpenReward(cards);
    }

    public void OpenReward(List<WeaponCard> _cards)
    {
        CheckSpecialCards();

        rewardPanel.SetActive(true);
        AudioManager.Main.PauseAudio();

        if(cards.Count < cardsOnCooldown.Count / 2)
        {
            cards.AddRange(cardsOnCooldown);
            cardsOnCooldown.Clear();
        }

        foreach (WeaponCardSelection selection in selections)
        {
            var rdm = Random.Range(0, _cards.Count);
            var card = _cards[rdm];

            if(_cards != cards) cards.Remove(card);
            _cards.Remove(card);
            cardsOnCooldown.Add(card);

            selection.ConfigureCard(card, GetSpawnerUpgrade((int)card.rarity));
        }


        foreach (var spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.chanceForExtraEnemy += extraEnemieChanceIncrement;
        }
    }

    private void CheckSpecialCards()
    {
        var unlockedSpecials = new List<SpecialCardData>();

        foreach (var card in specialCards)
        {
            if (card.ConditionsMet())
            {
                for (int i = 0; i < card.weight; i++)
                {
                    cards.Add(card.card);
                }
                unlockedSpecials.Add(card);
            }
        }

        foreach (var _c in unlockedSpecials)
        {
            specialCards.Remove(_c);
        }
    }

    private BaseSpawnerUpgrade GetSpawnerUpgrade(int upgradeLevel)
    {
        if (upgradeLevel == 3) upgradeLevel = 0;
        var rdm = Random.Range(0, upgradesByLevel[upgradeLevel].upgrades.Count);

        return upgradesByLevel[upgradeLevel].upgrades[rdm];
    }

    public void CloseReward()
    {
        selections.ForEach(x => x.ClearCard());
        rewardPanel.SetActive(false);
        AudioManager.Main.UnpauseAudio();
    }

    public void ReceiveCard(List<WeaponCard> unlockedCards)
    {
        cards.AddRange(unlockedCards);
    }

    public void ReceiveCard(WeaponCard unlockedCard)
    {
        cards.Add(unlockedCard);
    }

    public void RemoveCard(WeaponCard weaponCard)
    {
        var card = cards.Find(x => x.name == weaponCard.name);
        if (card != null)
        {
            cards.Remove(card);
        } else
        {
            card = cardsOnCooldown.Find(x => x.name == weaponCard.name);
            if (card != null) cardsOnCooldown.Remove(card);
        }
    }

    public void RemoveCard(List<WeaponCard> weaponCards)
    {
        foreach (var card in weaponCards)
        {
            RemoveCard(card);
        }
    }
}

[System.Serializable]
public class UpgradesByLevel
{
    public List<BaseSpawnerUpgrade> upgrades;
}

[System.Serializable]
public class SpecialCardData
{
    public WeaponCard card;
    public int weight;

    [Header("Multi Weapon Card")]
    public bool multiWeapon;
    public GameObject[] weapons;

    public bool ConditionsMet()
    {
        if (multiWeapon) return MultiWeaponCondition();
        return false;
    }

    private bool MultiWeaponCondition()
    {
        foreach (var weapon in weapons)
        {
            if (!weapon.activeSelf) return false;
        }

        return true;
    }
}

[System.Serializable]
public class InitialWeaponCards
{
    public string lane;
    public List<WeaponCard> cardList;
}