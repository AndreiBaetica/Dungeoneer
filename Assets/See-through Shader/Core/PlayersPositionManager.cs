using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//[ExecuteInEditMode]
public class PlayersPositionManager : MonoBehaviour
{
    int _PlayersPos_ID;
    int _PlayersData_ID;
    // Start is called before the first frame update
    public List<GameObject> playableCharacters;


    void OnEnable()
    {
        init();
    }
    void Start()
    {
        Debug.Log("START");
        init();
    }
    
    //private void Reset()
    //{
    //    if (SeeThroughShaderStatus.playerPositionMangerHolder != null)
    //    {
    //        this.enabled = false;
    //        Debug.Log("PlayerPositionManager is already used on GameObject " + SeeThroughShaderStatus.playerPositionMangerHolder.name);
    //    }
    //    else
    //    {
    //        SeeThroughShaderStatus.playerPositionMangerHolder = this.gameObject;
    //    }
    //}

    private void init()
    {
        _PlayersPos_ID = Shader.PropertyToID("_PlayersPosArray");
        _PlayersData_ID = Shader.PropertyToID("_PlayersDataArray");

        Vector4[] playerData = new Vector4[100];
        Shader.SetGlobalVectorArray(_PlayersData_ID, playerData);
    }

    // Update is called once per frame
    void Update()
    {
        if(playableCharacters!=null)
        {
            // clean up duplicates
            List<GameObject> playableCharactersDestinct = playableCharacters.Distinct().ToList();
            // clean up null elements
            playableCharactersDestinct.RemoveAll(item => item == null);
            Vector4[] playersPositions = new Vector4[100];
            Vector4[] playerData = Shader.GetGlobalVectorArray(_PlayersData_ID);

            for (int i = 0; i < playableCharactersDestinct.Count; i++)
            {
                if(playableCharactersDestinct[i]!=null)
                {
                    playersPositions[i] = new Vector4(playableCharactersDestinct[i].transform.position.x,
                                                      playableCharactersDestinct[i].transform.position.y,
                                                      playableCharactersDestinct[i].transform.position.z,
                                                      playableCharactersDestinct[i].transform.GetInstanceID());
                    playerData[i] = new Vector4(playableCharactersDestinct[i].transform.GetInstanceID(), playerData[i][1], playerData[i][2], playerData[i][3]); //necessary?
                }

            }
            if (playableCharacters.Count > 0)
            {
                Shader.SetGlobalVectorArray(_PlayersPos_ID, playersPositions);
                Shader.SetGlobalVectorArray(_PlayersData_ID, playerData);
                Shader.SetGlobalFloat("_ArrayLength", playableCharactersDestinct.Count);
            }
            else
            {
                Shader.SetGlobalFloat("_ArrayLength", 0);
            }
        } 
        else
        {
            Shader.SetGlobalFloat("_ArrayLength", 0);
        }


    }
}
