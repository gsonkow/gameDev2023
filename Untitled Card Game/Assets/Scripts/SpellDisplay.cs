using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellDisplay : MonoBehaviour
{

    public SpellCard card;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public Image art;
    public TextMeshProUGUI cost;


    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        effectText.text = card.text;
        art.sprite = card.artwork;
        cost.text = card.cost.ToString();
    }

}