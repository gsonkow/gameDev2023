using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLoop : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject playerFollowers;
    public GameObject enemyObj;
    public GameObject enemyFollowers;
    public TextMeshProUGUI info;

    bool inPlay;
    bool first;

    HandController player;
    PlayerBoard playersBoard;
    EnemyController enemy;
    EnemyBoard enemysBoard;

    void Start()
    {
        player = playerObj.GetComponent<HandController>();
        playersBoard = playerFollowers.GetComponent<PlayerBoard>();
        enemy = enemyObj.GetComponent<EnemyController>();
        enemysBoard = enemyFollowers.GetComponent<EnemyBoard>();
        inPlay = true;
        first = Random.Range(0, 2) != 0;
        if (!first)
        {
            enemy.TakeTurn();
        }
        player.StartTurn();
    }


    void Update()
    {
        if (player.getHealth() <= 0 && inPlay)
        {
            inPlay = false;
            info.text = "You Lose";
            //todo: gameover
        }
        else if (enemy.getHealth() <= 0 && inPlay)
        {
            inPlay = false;
            info.text = "You Win!";
            //todo: win
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            enemysBoard.UntapAll();
            enemy.TakeTurn();
            playersBoard.UntapAll();
            player.StartTurn();
        }
    }

}
