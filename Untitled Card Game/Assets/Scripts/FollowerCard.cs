using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Follower")]
public class FollowerCard : ScriptableObject
{
    public new string name;
    public string text;

    public Sprite artwork;

    public int cost;
    public int attack;
    public int health;
}
