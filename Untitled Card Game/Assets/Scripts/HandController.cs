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

    private List<string> deck = new();
    public List<GameObject> hand = new();
    //bool turnStart = false;
    int mana = 7;

    private readonly int STARTING_HAND = 5;
    private readonly int MAX_HAND = 8;

    void Start()
    {
        deck = GetDeck();
        deckCounter.text = "Deck: " + deck.Count.ToString();
        manaCounter.text = "$" + mana.ToString() + "0,000";
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
        mana = newMana;
        manaCounter.text = "$" + mana.ToString() + (mana <= 0 ? "" : "0,000");
    }
}


