using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;


public class ReplacementShader : MonoBehaviour
{
    public Dictionary<Material, Shader> cachedOriginalShaders = new Dictionary<Material, Shader>();
    public Shader replacementShader;
    private string seeThroughShaderName;

    public Material referenceMaterial;

    public bool replacementByReplacementShader;
    public LayerMask layerMasksWithReplacement;

    public Camera replacementCamera;

    void Update()
    {
        if (this.isActiveAndEnabled)
        {
            updateGlobalShaderVariables();
        }
    }

    void Awake()
    {
        if (this.isActiveAndEnabled)
        {
            seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;

            // always null because option for shader selection got removed
            if (replacementShader == null)
            {
                replacementShader = Shader.Find(seeThroughShaderName);
            }
            else
            {
                seeThroughShaderName = replacementShader.name;
            }

            applyReplacementShader();
            updateGlobalShaderVariables();
        }

    }


    private void OnValidate()
    {
        if (this.isActiveAndEnabled)
        {
            updateGlobalShaderVariables();

            if (replacementCamera == null && GetComponent<Camera>() != null)
            {
                replacementCamera = GetComponent<Camera>();
            }

        }
    }
    void OnEnable()
    {
        if (this.isActiveAndEnabled)
        {
            applyReplacementShader();

            if(GetComponent<Camera>() != null)
            {
                replacementCamera = GetComponent<Camera>();
            }
        }

    }

    // only necessary when using [ExecuteInEditMode]
    //void OnDisable()
    //{

    //    //resetReplacementShadersAndSwappedShaders();
    //}

    //private void OnDestroy()
    //{
    //    //resetReplacementShadersAndSwappedShaders();
    //}

    //void OnApplicationQuit()
    //{
    //    //resetReplacementShadersAndSwappedShaders();
    //}



    private void applyReplacementShader()
    {
        if (replacementShader != null)
        {
            Shader.SetGlobalFloat("_IsReplacementShader", 1);

            if (replacementByReplacementShader)
            {
                if(replacementCamera!=null)
                {
                    replacementCamera.SetReplacementShader(replacementShader, "RenderType");
                }
            }
            else
            {
                applyReplacementShaderToGameObjectsMaterials(listAllGameObjectsFromLayerMask(layerMasksWithReplacement));
            }

        }
    }

    private List<GameObject> listAllGameObjectsFromLayerMask(LayerMask layerMask) 
    {   GameObject[] gameObjectArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> gameObjectList = new List<GameObject>(); 
        for (int i = 0; i < gameObjectArray.Length; i++) 
        {
            if(doesLayerMaskContainLayer(layerMask, gameObjectArray[i].layer))
            {
                gameObjectList.Add(gameObjectArray[i]);
            } 
        }

        //Debug.Log("gameObjectList.Count: " + gameObjectList.Count);
        if (gameObjectList.Count == 0) 
        { 
            return null; 
        }
        return gameObjectList; 
    }

    private bool doesLayerMaskContainLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void applyReplacementShaderToGameObjectsMaterials(List<GameObject> gameObjects)
    {
        if(seeThroughShaderName!=null && !seeThroughShaderName.Equals(""))
        {
            if(replacementShader!=null)
            {
                seeThroughShaderName = replacementShader.name;
            } else
            {
                seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;

            }
        }
        if (gameObjects != null && gameObjects.Count > 0)
        {        
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject != null && gameObject.GetComponent<Renderer>() != null)
                {
                    Material[] materials = gameObject.GetComponent<Renderer>().materials;
                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].shader.name != seeThroughShaderName)
                        {
                            if (!cachedOriginalShaders.ContainsKey(materials[i]))
                            {
                                cachedOriginalShaders.Add(materials[i], materials[i].shader);
                                materials[i].shader = replacementShader;
                                materials[i].SetFloat("_IsReplacementShader", 1);
                            }
                        }

                    }
                }

            }
        }
    }



    private void updateGlobalShaderVariables()
    {
        if (referenceMaterial != null)
        {
            Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
            Shader.SetGlobalTexture("_DissolveTexGlobal", disTex);
            Shader.SetGlobalColor("_DissolveColorGlobal", referenceMaterial.GetColor("_DissolveColor"));
            foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
            {
                if(referenceMaterial.HasProperty(propertyName))
                {
                    float temp = referenceMaterial.GetFloat(propertyName);
                    Shader.SetGlobalFloat(propertyName + "Global", temp);
                }
            }
        }
    }

    // currently not used
    //private void resetReplacementShadersAndSwappedShaders()
    //{
    //    Shader.SetGlobalFloat("_IsReplacementShader", 0);
    //    GetComponent<Camera>().ResetReplacementShader();
    //    if (cachedOriginalShaders.Count > 0)
    //    {
    //        foreach (KeyValuePair<Material, Shader> entry in cachedOriginalShaders)
    //        {
    //            entry.Key.shader = entry.Value;
    //        }
    //        cachedOriginalShaders.Clear();

    //    }
    //}
}
