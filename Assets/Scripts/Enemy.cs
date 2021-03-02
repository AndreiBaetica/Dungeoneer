using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles interaction with the Enemy */

public class Enemy : MonoBehaviour
{

	PlayerManager playerManager;

	void Start()
	{
		playerManager = PlayerManager.instance;
	}

	/*public override void Interact()
	{
		base.Interact();
		//do stuff
	}*/

}