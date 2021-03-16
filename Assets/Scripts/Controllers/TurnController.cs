using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private enum State { PlayerMove, EnemyMove}
    private State state;
    public static Component enemy;
    public static Component player;
    //int c;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent("TurnController");
        enemy = GameObject.Find("enemy").GetComponent("TurnController");
        Debug.Log("enemy obj: " + enemy);
        Debug.Log("player obj: " + player);

        //player.Instantiate;

        //player = PlayerManager.instance.player;
        //enemy = Enemy.instance.enemy;

        state = State.PlayerMove;
        //player =
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isTurn)
        {
            state = State.EnemyMove;
        }
        else
        {
            state = State.PlayerMove;
        }
        if(state == State.PlayerMove)
        {
            player.isTurn = true;
            enemy.isTurn = false;
        }
        else
        {
            player.isTurn = false;
            enemy.isTurn = true;
        }
    }
}
