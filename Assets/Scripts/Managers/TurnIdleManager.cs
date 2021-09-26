using System.Collections;
using UnityEngine;

public class TurnIdleManager: MonoBehaviour
{
    public void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        //StartCoroutine(Wait());
    }

    /*IEnumerator Wait()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }*/
}
