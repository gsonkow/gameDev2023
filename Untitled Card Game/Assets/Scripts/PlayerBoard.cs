using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerBoard : MonoBehaviour
{
    public List<GameObject> followers = new();

    public void RearrangeBoard()
    {
        int cardPoint = 180;
        foreach (GameObject card in followers)
        {
            card.transform.localPosition = new Vector3(cardPoint, 0, 0);
            cardPoint -= 90;
        }
    }
}
