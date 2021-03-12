using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles interaction with the Enemy */

public class Enemy : MonoBehaviour
{
	#region Singleton

	public static Enemy instance;

	void Awake()
	{
		instance = this;
	}

	#endregion

	public GameObject enemy;
	//public bool isTurn = false;

	void Start()
	{
		//playerManager = PlayerManager.instance;
	}

	/*public override void Interact()
	{
		base.Interact();
		//do stuff
	}*/

}