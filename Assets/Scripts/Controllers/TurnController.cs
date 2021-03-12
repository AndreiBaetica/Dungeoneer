/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private enum State { PlayerMove, EnemyMove}
    private State state;.
    public GameObject enemy;
    public GameObject player;
    //int c;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("player");
        //enemy = GameObject.Find("enemy");

        state = State.PlayerMove;
        //player =
    }

    // Update is called once per frame
    void Update()
    {
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
*/