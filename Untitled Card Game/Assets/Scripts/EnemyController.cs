using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public GameObject followerPrototype;
    public GameObject spellPrototype;
    public GameObject blankCard;
    public EnemyBoard enemyFollowersList;
    public PlayerBoard playerFollowersList;
    public HandController player;
    public TextMeshProUGUI deckCounter;
    public TextMeshProUGUI manaCounter;
    public TextMeshProUGUI healthCounter;

    private List<string> deck = new();
    public List<string> hand = new();
    private List<GameObject> displayHand = new();

    int maxMana = 0;
    int mana = 0;
    int health = 20;

    private readonly int STARTING_HAND = 3;
    private readonly int MAX_HAND = 8;

    void Start()
    {
        enemyFollowersList = GameObject.Find("EnemyFollowers").GetComponent<EnemyBoard>();
        playerFollowersList = GameObject.Find("PlayerFollowers").GetComponent<PlayerBoard>();
        player = GameObject.Find("Hand").GetComponent<HandController>();
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
                            "WearyAssistant",
                            "Goat",
                            "&BackroomDeal",
                            "&BackroomDeal",
                            "ContractDealer",
                            "ContractDealer",
                            "Goat",
                            "Goat",
                            "LatteLieutenant",
                            "LatteLieutenant",
                            "LatteLieutenant",
                            "&CrispyAwakening",
                            "&CrispyAwakening",
                            "BigCow",
                            "BigCow",
                            "BigCow",
                            "DairyFarmer",
                            "DairyFarmer"};
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


        hand.Add(drawnCard);
        RearrangeHand();

    }

    public void RearrangeHand()
    {
        foreach (GameObject card in displayHand)
        {
            Destroy(card);
        }
        displayHand.RemoveRange(0, displayHand.Count);

        int cardPoint = -300;
        foreach (string card in hand)
        {
            GameObject cardToDraw = Instantiate(blankCard, this.transform);
            cardToDraw.transform.localPosition = new Vector3(cardPoint, 190, 0);
            displayHand.Add(cardToDraw);
            cardPoint += 70;
        }
    }

    public void TakeTurn()
    {
        DrawCard();
        maxMana += 1;
        SetMana(maxMana);

        bool hasPlays = true;
        bool follower = true;
        do
        {
            //selects highest score playable card
            string cardToPlay = "";
            int toPlayScore = 0;
            foreach (string card in hand)
            {
                if (card[0] == '&')
                {
                    SpellCard cardToCheck = Resources.Load($"spells/{card[1..]}") as SpellCard;
                    if (cardToCheck.cost > mana) { continue; }
                    if (cardToCheck.botScore > toPlayScore)
                    {
                        cardToPlay = card;
                        toPlayScore = cardToCheck.botScore;
                        follower = false;
                    }
                } else
                {
                    FollowerCard cardToCheck = Resources.Load($"followers/{card}") as FollowerCard;
                    if (cardToCheck.cost > mana) { continue; }
                    if (cardToCheck.botScore > toPlayScore)
                    {
                        cardToPlay = card;
                        toPlayScore = cardToCheck.botScore;
                        follower = true;
                    }
                }
            }



            if (cardToPlay.Length == 0) { hasPlays = false; }
            else
            {
                hand.Remove(cardToPlay);
                RearrangeHand();
                GameObject playCard = Instantiate(follower ? followerPrototype : spellPrototype, this.transform);
                if (follower)
                {
                    playCard.GetComponent<FollowerDisplay>().card = Resources.Load($"followers/{cardToPlay}") as FollowerCard;
                    mana -= playCard.GetComponent<FollowerDisplay>().card.cost;
                    SetMana(mana);
                    StartCoroutine(PlayFollower(playCard));
                }
                else
                {
                    playCard.GetComponent<SpellDisplay>().card = Resources.Load($"spells/{cardToPlay[1..]}") as SpellCard;
                    mana -= playCard.GetComponent<SpellDisplay>().card.cost;
                    SetMana(mana);
                    playCard.GetComponent<SpellController>().playable = false;
                    playCard.transform.localScale = new Vector3(0.6f, 0.6f, 1);
                    switch (cardToPlay)
                    {
                        case "&BackroomDeal":
                            DrawCard();
                            DrawCard();
                            break;

                        case "&CrispyAwakening":
                            enemyFollowersList.GiveAll(1, 2);
                            break;

                        case "&MassLayoff":
                            playerFollowersList.GiveAll(0, -99);
                            enemyFollowersList.GiveAll(0, -99);
                            break;

                        case "&Stampede":
                            playerFollowersList.GiveAll(0, -5);
                            break;
                    }
                    StartCoroutine(PlaySpell(playCard));
                    
                }
            }
        } while (hasPlays);

        StartCoroutine(Attacks());
        
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

    IEnumerator PlayFollower(GameObject card)
    {
        yield return new WaitForSeconds(0.0001f);
        card.GetComponent<FollowerController>().playable = false;
        card.transform.SetParent(GameObject.Find("EnemyFollowers").transform);
        card.GetComponent<FollowerDisplay>().attack.transform.localScale = new Vector3(2, 2, 0);
        card.GetComponent<FollowerDisplay>().health.transform.localScale = new Vector3(2, 2, 0);
        enemyFollowersList.followers.Add(card);
        enemyFollowersList.RearrangeBoard();
    }

    IEnumerator Attacks()
    {
        yield return new WaitForSeconds(0.0001f); //space alien reason why this is needed
        foreach (GameObject attacker in enemyFollowersList.followers)
        {
            FollowerController attackerController = attacker.GetComponent<FollowerController>();
            if (attackerController.enemyAttack)
            {
                FollowerDisplay attackerDisplay = attacker.GetComponent<FollowerDisplay>();
                int bestTargetIndex = playerFollowersList.highestAttackable(int.Parse(attackerDisplay.attack.text));
                if (enemyFollowersList.TotalAttack() >= player.getHealth())
                {
                    player.setHealth(player.getHealth() - int.Parse(attackerDisplay.attack.text));
                }
                else if (bestTargetIndex >= 0)
                {
                    FollowerDisplay targetDisplay = playerFollowersList.followers[bestTargetIndex].GetComponent<FollowerDisplay>();
                    targetDisplay.health.text = (int.Parse(targetDisplay.health.text) - int.Parse(attackerDisplay.attack.text)).ToString();
                    attackerDisplay.health.text = (int.Parse(attackerDisplay.health.text) - int.Parse(targetDisplay.attack.text)).ToString();
                    yield return new WaitForSeconds(0.2f);
                    playerFollowersList.KillDead();
                }
                else
                {
                    player.setHealth(player.getHealth() - int.Parse(attackerDisplay.attack.text));
                }
            }
        }
        enemyFollowersList.KillDead();
    }


    IEnumerator PlaySpell(GameObject card)
    {
        yield return new WaitForSeconds(2);
        Destroy(card);
    }

}


