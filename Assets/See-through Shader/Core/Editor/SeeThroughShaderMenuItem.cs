using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SeeThroughShaderMenuItem : MonoBehaviour
{

    // In development
    //[MenuItem("Assets/See-through Shader/Create See-through Shader Manager")]
    //static void CreateSeeThroughShaderManger()
    //{
    //    string seeThroughManagerName = "See-through Shader Manager";
    //    if (GameObject.Find(seeThroughManagerName) == null)
    //    {
    //        GameObject seeThroughShaderManager = new GameObject(seeThroughManagerName);
    //        seeThroughShaderManager.AddComponent<SeeThroughShaderManager>();

    //        PlayersPositionManager[] playersPositionManagers = (PlayersPositionManager[])GameObject.FindObjectsOfType(typeof(PlayersPositionManager));
    //        if (playersPositionManagers.Length > 0)
    //        {
    //            if (playersPositionManagers.Length > 1)
    //            {
    //                string gameObjectNames = "(";
    //                foreach (PlayersPositionManager playersPositionManager in playersPositionManagers)
    //                {
    //                    gameObjectNames += " " + playersPositionManager.name + ", ";
    //                }
    //                gameObjectNames = gameObjectNames.Remove(gameObjectNames.Length - 2);
    //                gameObjectNames += ")";
    //                Debug.LogWarning("There are multiple GameObjects with the script PlayerPositionManager attached to itself." +
    //                    " Only one instance of this script is allowed, otherwise the 'See-through Shader' asset won't work. " +
    //                    "List of GameObjects using the script: " + gameObjectNames);
    //            }
    //            else
    //            {
    //                Debug.Log("PlayerPositionManager is already attached to GameObject " + playersPositionManagers[0].gameObject.name);
    //            }

    //        }
    //        else
    //        {
    //            seeThroughShaderManager.AddComponent<PlayersPositionManager>();
    //        }
    //    } 
    //    else
    //    {
    //        Debug.LogWarning("There is already a See-through Shader Manager present in the scene! You can't add another one!");
    //    }

    //}

    [MenuItem("Assets/See-through Shader/Create Reference Material")]
    static void CreateReferenceMaterial()
    {
        SelectNamePopup window = ScriptableObject.CreateInstance<SelectNamePopup>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        //window.ShowPopup();
        window.titleContent = new GUIContent("See-through Shader Tools");
        window.ShowUtility();

    }
}