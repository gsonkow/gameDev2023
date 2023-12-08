using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SpellController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject playerHand;
    HandController playerHandController;
    PlayerBoard playerFollowersList;
    EnemyBoard enemyFollowersList;
    SpellDisplay cardDisplay;

    public bool playable;

    void Start()
    {
        playerFollowersList = GameObject.Find("PlayerFollowers").GetComponent<PlayerBoard>();
        enemyFollowersList = GameObject.Find("EnemyFollowers").GetComponent<EnemyBoard>();
        playable = true;
        playerHand = GameObject.Find("Hand");
        playerHandController = playerHand.GetComponent<HandController>();
        cardDisplay = this.GetComponent<SpellDisplay>();
    }

    void Update()
    {
        if (playerHandController.GetMana() < int.Parse(cardDisplay.cost.text))
        {
            playable = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!playable)
        {
            return;
        }
        //detach card
        transform.SetParent(transform.root);
        playerHandController.hand.Remove(this.gameObject);
        playerHandController.RearrangeHand();
        transform.localScale = new Vector3(0.6f, 0.6f, 1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (playable)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1);
        if (transform.localPosition.y >= -10 && playable)
        {
            //TODO: placeholder
            playable = false;
            playerHandController.SetMana(playerHandController.GetMana() - int.Parse(cardDisplay.cost.text));

            switch (cardDisplay.nameText.text)
            {
                case "Backroom Deal":
                    playerHandController.DrawCard();
                    playerHandController.DrawCard();
                    break;

                case "Crispy Awakening":
                    playerFollowersList.GiveAll(1, 2);
                    break;

                case "Mass Layoff":
                    playerFollowersList.GiveAll(0, -99);
                    enemyFollowersList.GiveAll(0, -99);
                    break;

                case "Cow Stampede":
                    enemyFollowersList.GiveAll(0, -5);
                    break;
            }

            Destroy(this.gameObject);
        }
        else if (transform.localPosition.y < -10 && playable)
        {
            //return card
            transform.SetParent(playerHand.transform);
            playerHandController.hand.Add(this.gameObject);
            playerHandController.RearrangeHand();
        }
    }
}
