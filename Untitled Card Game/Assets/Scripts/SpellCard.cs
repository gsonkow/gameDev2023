using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Spell")]
public class SpellCard : ScriptableObject
{
    public new string name;
    public string text;

    public Sprite artwork;

    public int cost;
}
