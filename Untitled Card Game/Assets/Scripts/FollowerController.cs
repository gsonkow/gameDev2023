using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowerController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject playerHand;
    PlayerBoard playerFollowersList;
    HandController playerHandController;
    FollowerDisplay cardDisplay;

    private bool playable;

    void Start()
    {
        playable = true;
        playerHand = GameObject.Find("Hand");
        playerFollowersList = GameObject.Find("PlayerFollowers").GetComponent<PlayerBoard>();
        playerHandController = playerHand.GetComponent<HandController>();
        cardDisplay = this.GetComponent<FollowerDisplay>();
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
            //play card
            playable = false;
            playerHandController.SetMana(playerHandController.GetMana() - int.Parse(cardDisplay.cost.text));
            transform.SetParent(GameObject.Find("PlayerFollowers").transform);
            playerFollowersList.followers.Add(this.gameObject);
            playerFollowersList.RearrangeBoard();
            cardDisplay.attack.transform.localScale = new Vector3(2, 2, 0);
            cardDisplay.health.transform.localScale = new Vector3(2, 2, 0);
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
