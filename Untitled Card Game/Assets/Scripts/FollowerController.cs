using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowerController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject playerHand;
    PlayerBoard playerFollowersList;
    EnemyBoard enemyFollowersList;
    HandController playerHandController;
    EnemyController enemyControl;
    FollowerDisplay cardDisplay;

    public bool playable;
    public bool ableToAttack;
    public bool enemyAttack;

    void Start()
    {
        playable = true;
        ableToAttack = false;
        enemyAttack = false;
        playerHand = GameObject.Find("Hand");
        playerFollowersList = GameObject.Find("PlayerFollowers").GetComponent<PlayerBoard>();
        enemyFollowersList = GameObject.Find("EnemyFollowers").GetComponent<EnemyBoard>();
        playerHandController = playerHand.GetComponent<HandController>();
        enemyControl = GameObject.Find("Enemy").GetComponent<EnemyController>();
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
        if (playable)
        {
            //detach card
            transform.SetParent(transform.root);
            playerHandController.hand.Remove(this.gameObject);
            playerHandController.RearrangeHand();
            transform.localScale = new Vector3(0.6f, 0.6f, 1);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (playable || ableToAttack)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (playable)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1);
            if (transform.localPosition.y >= -10)
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
            else if (transform.localPosition.y < -10)
            {
                //return card
                transform.SetParent(playerHand.transform);
                playerHandController.hand.Add(this.gameObject);
                playerHandController.RearrangeHand();
            }
        }

        if (ableToAttack)
        {
            if (transform.localPosition.y >= 60)
            {
                if (transform.localPosition.x > 220)
                {
                    ableToAttack = false;
                    enemyControl.setHealth(enemyControl.getHealth() - int.Parse(cardDisplay.attack.text));
                }
                else if (transform.localPosition.x <= 220)
                {
                    int attackIndex = (int)Mathf.Abs((transform.localPosition.x - 220f) / 100f);

                    if (attackIndex < enemyFollowersList.followers.Count)
                    {
                        ableToAttack = false;
                        FollowerDisplay followerToAttack = enemyFollowersList.followers[attackIndex].GetComponent<FollowerDisplay>();
                        followerToAttack.health.text = (int.Parse(followerToAttack.health.text) - int.Parse(cardDisplay.attack.text)).ToString();
                        cardDisplay.health.text = (int.Parse(cardDisplay.health.text) - int.Parse(followerToAttack.attack.text)).ToString();
                        enemyFollowersList.KillDead();
                        playerFollowersList.KillDead();
                    }
                }
            }
            playerFollowersList.RearrangeBoard();
        }
    }
}
