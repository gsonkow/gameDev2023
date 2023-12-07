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
            }
        }
    }
}
