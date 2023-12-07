using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HandController : MonoBehaviour
{

    public GameObject followerPrototype;
    public GameObject spellPrototype;
    public TextMeshProUGUI deckCounter;
    public TextMeshProUGUI manaCounter;
    public TextMeshProUGUI healthCounter;

    private List<string> deck = new();
    public List<GameObject> hand = new();

    int maxMana = 0;
    int mana = 0;
    int health = 20;

    private readonly int STARTING_HAND = 3;
    private readonly int MAX_HAND = 8;

    void Start()
    {
        deck = GetDeck();
        deckCounter.text = "Deck: " + deck.Count.ToString();
        SetMana(mana);
        healthCounter.text = health.ToString();
        for (int draws = 0; draws < STARTING_HAND; draws++)
        {
            DrawCard();
        }

    }

    List<string> GetDeck()
    {
        //placeholder deck
        string[] dummy = {  "WearyAssistant",
                            "WearyAssistant",
                            "&BackroomDeal",
                            "&BackroomDeal",
                            "WearyAssistant",
                            "WearyAssistant",
                            "&BackroomDeal",
                            "&BackroomDeal",
                            "WearyAssistant",
                            "WearyAssistant"};
        List<string> dummyDeck = new List<string>(dummy);
        return dummyDeck;
    }

    public void StartTurn()
    {
        DrawCard();
        maxMana += 1;
        SetMana(maxMana);

        foreach (GameObject card in hand)
        {
            if (card.TryGetComponent<FollowerController>(out FollowerController followCon) && card.TryGetComponent<FollowerDisplay>(out FollowerDisplay followDis))
            {
                if (int.Parse(followDis.cost.text) <= GetMana())
                {
                    followCon.playable = true;
                }
            }
            else if (card.TryGetComponent<SpellController>(out SpellController spellCon) && card.TryGetComponent<SpellDisplay>(out SpellDisplay spellDis))
            {
                if (int.Parse(spellDis.cost.text) <= GetMana())
                {
                    spellCon.playable = true;
                }
            }
        }
    }

    public void DrawCard()
    {
        if (hand.Count >= MAX_HAND) { return; }
        if (deck.Count <= 0) { return; }


        int drawIndex = Random.Range(0, deck.Count);
        string drawnCard = deck[drawIndex];
        deck.RemoveAt(drawIndex);
        deckCounter.text = "Deck: " + deck.Count.ToString();


        bool follower = true;

        if (drawnCard[0] == '&')
        {
            follower = false;
        }

        GameObject cardToAdd = Instantiate(follower ? followerPrototype : spellPrototype, this.transform);

        if (follower)
        {
            cardToAdd.GetComponent<FollowerDisplay>().card = Resources.Load($"followers/{drawnCard}") as FollowerCard;
        }
        else
        {
            cardToAdd.GetComponent<SpellDisplay>().card = Resources.Load($"spells/{drawnCard[1..]}") as SpellCard;
        }

        hand.Add(cardToAdd);
        RearrangeHand();
    }

    public void RearrangeHand()
    {
        int cardPoint = -300;
        foreach (GameObject card in hand)
        {
            card.transform.localPosition = new Vector3(cardPoint, -110, 0);
            cardPoint += 70;
        }
    }

    public int GetMana()
    {
        return mana;
    }

    public void SetMana(int newMana)
    {
        if (newMana > 10)
        {
            newMana = 10;
        }
        mana = newMana;
        manaCounter.text = "$" + mana.ToString() + (mana <= 0 ? "" : "0,000");
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int newHealth)
    {
        health = newHealth;
        if (health < 0) { health = 0; }
        healthCounter.text = health.ToString();
    }
}


