using System;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughShaderController
{
    private List<Material> materials;

    private string currentMode;

    public SeeThroughShaderController()
    {}

    public SeeThroughShaderController(Transform transform)
    {
        initTrigger(transform);
    }
    public SeeThroughShaderController(List<Material> listMaterial)
    {
        initTrigger(listMaterial);
        
    }

    private void initTrigger(Transform transform)
    {

        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].materials.Length > 0)
            {
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
                    setSeeThroughShaderMaterialToTriggerMode(renderers[i].materials[j]);
                }
            }
        }

        currentMode = "Trigger";
    }

    private void initTrigger(List<Material> listMaterial)
    {
        foreach (Material mat in listMaterial)
        {
            setSeeThroughShaderMaterialToTriggerMode(mat);
        }
        currentMode = "Trigger";
    }

    // called from BuildingAutoDetector
    public void setupAutoDetect(Transform transform)
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].materials.Length > 0)
            {
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
                    setSeeThroughShaderMaterialToRaycastMode(renderers[i].materials[j]);
                }
            }
        }

        currentMode = "Raycast";
    }

    private void resetMaterials()
    {
        if(materials != null && materials.Count > 0)
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat("_RaycastMode", 0);
                mat.SetFloat("_TriggerMode", 0);
            }
            materials.Clear();
        }
    }

    public void destroy()
    {
        resetMaterials();
    }
    
    private void setSeeThroughShaderMaterialToTriggerMode(Material mat)
    {

        if (isSeeThroughShaderMaterial(mat))
        {
            //Debug.Log("setSeeThroughShaderMaterialToTriggerMode mat name: " + mat);
            mat.SetFloat("_RaycastMode", 0);
            mat.SetFloat("_TriggerMode", 1);
            if(materials==null)
            {
                materials = new List<Material>();
            }
            materials.Add(mat);
        }
    }


    private void setSeeThroughShaderMaterialToRaycastMode(Material mat)
    {
        if (isSeeThroughShaderMaterial(mat))
        {
            mat.SetFloat("_RaycastMode", 1);
            mat.SetFloat("_TriggerMode", 0);
            if (materials == null)
            {
                materials = new List<Material>();
            }
            materials.Add(mat);
        }
    }

    private bool isSeeThroughShaderMaterial(Material mat)
    {
        if (mat != null && mat.shader != null && SeeThroughShaderGeneralUtils.STS_SHADER_LIST.Contains(mat.shader.name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void notifyOnTriggerEnter(List<Material> materialList, Transform player = null)
    {
        if(player == null)
        {
            Debug.Assert(false, "Sorry, from now on you have to supply the transform of the playable character that collided with the trigger!");
        } else
        {
            transitionEffect(materialList, 1, player);
        }

    }

    public void notifyOnTriggerExit(List<Material> materialList, Transform player = null)
    {
        if (player == null)
        {
            Debug.Assert(false, "Sorry, from now on you have to supply the transform of the playable character that collided with the trigger!");
        }
        else
        {
            transitionEffect(materialList, -1, player);
        }
    }

    public void notifyOnTriggerEnter(Transform transform, Transform player = null)
    {
        notifyOnTriggerEnter(getMaterialList(transform), player);
    }

    public void notifyOnTriggerExit(Transform transform, Transform player = null)
    {
        notifyOnTriggerExit(getMaterialList(transform), player);
    }


    private List<Material> getMaterialList(Transform transform)
    {
        List<Material> materialList = new List<Material>();
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        //Renderer[] renderers = transform.root.GetComponentsInChildren<Renderer>();
        if(renderers != null && renderers.Length > 0)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].materials.Length > 0)
                {
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        if(isSeeThroughShaderMaterial(renderers[i].materials[j]))
                        {
                             materialList.Add(renderers[i].materials[j]);                            
                        }
                    }
                }
            }
        }


        return materialList;
    }


    private void transitionEffect(List<Material> materialList, float direction, Transform player)
    {
        //Debug.Log("current mode: " + currentMode);
        Vector4[] playersData = Shader.GetGlobalVectorArray("_PlayersDataArray");
        float arrayLength = Shader.GetGlobalFloat("_ArrayLength");
        int playerIndexToUpdate = -1;
        for (int i = 0; i < arrayLength; i++)
        {
            int temp = (int)playersData[i][0];
            if (temp == (player.transform.GetInstanceID()))
            {
                playerIndexToUpdate = i;
            }
        }
        if (playerIndexToUpdate != -1)
        {
            float generatedId = 0;
            float tValue = 0;
            foreach (Material mat in materialList)
            {
                if (!mat.HasProperty("_id") || mat.GetFloat("_id") == 0)
                {
                    if(generatedId == 0)
                    {
                        generatedId = IdGenerator.Instance.Id;
                    }
                    mat.SetFloat("_id", generatedId);
                } else
                {
                    if (generatedId == 0)
                    {
                        generatedId = mat.GetFloat("_id");
                    } else
                    {
                        Debug.Assert(generatedId == mat.GetFloat("_id"), "Materials have different Ids! Bug?");
                    }

                }
                // checks how many playable characters are already inside the building
                float numOfPlayersInside = 0;
                if (mat.HasProperty("_numOfPlayersInside"))
                {
                    numOfPlayersInside = mat.GetFloat("_numOfPlayersInside");
                }
                if ((numOfPlayersInside + direction) >= 0)
                {
                    numOfPlayersInside += direction;
                    mat.SetFloat("_numOfPlayersInside", numOfPlayersInside);
                }
                else
                {
                    mat.SetFloat("_numOfPlayersInside", 0);
                }



                float duration;
                if (mat.HasProperty("_TransitionDuration") && mat.GetFloat("_TransitionDuration") != 0)
                {
                    duration = mat.GetFloat("_TransitionDuration");
                }
                else
                {
                    duration = Shader.GetGlobalFloat("_TransitionDurationGlobal");
                    mat.SetFloat("_TransitionDuration", duration);
                }

                if (playersData[playerIndexToUpdate][1]!=0 && Time.timeSinceLevelLoad - playersData[playerIndexToUpdate][1] < duration)
                {
                    float lastTValue = playersData[playerIndexToUpdate][1];
                    float newTValue = duration - (((Time.timeSinceLevelLoad - 1) - lastTValue) - 1);
                    tValue = (Time.timeSinceLevelLoad - newTValue + 2);
                    mat.SetFloat("_tValue", tValue);
                }
                else
                {
                    tValue = Time.timeSinceLevelLoad;
                    mat.SetFloat("_tValue", tValue);
                }

                Debug.Assert(Math.Abs(direction) == 1, "direction argument needs to be either 1 or -1");
                mat.SetFloat("_tDirection", direction);
            }

            Vector4 temp = playersData[playerIndexToUpdate];
            temp[1] = tValue;
            temp[2] = direction;
            temp[3] = generatedId;
            playersData[playerIndexToUpdate] = temp;
            Shader.SetGlobalVectorArray("_PlayersDataArray", playersData);

        }


    }


}
