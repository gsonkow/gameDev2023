using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoard : MonoBehaviour
{
    public List<GameObject> followers = new();

    public void RearrangeBoard()
    {
        int cardPoint = 170;
        foreach (GameObject card in followers)
        {
            card.transform.localPosition = new Vector3(cardPoint, 110, 0);
            cardPoint -= 90;
        }
    }

    public void UntapAll()
    {
        foreach(GameObject card in followers)
        {
            card.GetComponent<FollowerController>().enemyAttack = true;
        }
    }

    public void KillDead()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (int.Parse(followers[i].GetComponent<FollowerDisplay>().health.text) <= 0)
            {
                GameObject toDestroy = followers[i];
                followers.RemoveAt(i);
                Destroy(toDestroy);
                RearrangeBoard();
                KillDead();
            }
        }
    }

    public int TotalAttack()
    {
        int total = 0;

        foreach(GameObject card in followers)
        {
            if (card.GetComponent<FollowerController>().enemyAttack)
            {
                total += int.Parse(card.GetComponent<FollowerDisplay>().attack.text);
            }
        }

        return total;
    }

    public void GiveAll(int attackChange, int healthChange)
    {
        foreach (GameObject card in followers)
        {
            FollowerDisplay cardDisplay = card.GetComponent<FollowerDisplay>();
            cardDisplay.attack.text = (int.Parse(cardDisplay.attack.text) + attackChange).ToString();
            cardDisplay.health.text = (int.Parse(cardDisplay.health.text) + healthChange).ToString();
        }
        KillDead();
    }
}
