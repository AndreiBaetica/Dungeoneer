using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughShaderGroupReplacement : MonoBehaviour
{
    public Transform parentTransform;
    public Shader seeThroughShader;
    private string seeThroughShaderName;

    public Material referenceMaterial;
    public List<Material> materialExemptions;
    public LayerMask layerMaskToAdd;

    public Transform[] transformsWithSTS;
    public bool keepMaterialsInSyncWithReference = true;


    void Awake()
    {
        if (this.isActiveAndEnabled)
        {
            seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;

            // always null because option for shader selection got removed
            if (seeThroughShader == null)
            {
                seeThroughShader = Shader.Find(seeThroughShaderName);
            }
            else
            {
                seeThroughShaderName = seeThroughShader.name;
            }

            if (parentTransform == null)
            {
                parentTransform = this.transform;
            }
            doSetupOfAllMaterials(parentTransform);
        }
    }


    //void Start()
    //{
    //    if (this.isActiveAndEnabled)
    //    { 

    //    }
    //}


    void OnEnable()
    {

    }

    private void OnValidate()
    {
        
    }

    private void LateUpdate()
    {
        if (this.isActiveAndEnabled)
        {
            if (keepMaterialsInSyncWithReference && referenceMaterial != null)
            {
                if(transformsWithSTS != null && seeThroughShaderName != null && referenceMaterial != null)
                {
                    SeeThroughShaderGeneralUtils.updateSeeThroughShaderMaterialProperties(transformsWithSTS, seeThroughShaderName, referenceMaterial);
                }
            }
        }
    }

    private void doSetupOfAllMaterials(Transform parentTransform)
    {
        if (seeThroughShaderName != null && !seeThroughShaderName.Equals(""))
        {
            if (seeThroughShader != null)
            {
                seeThroughShaderName = seeThroughShader.name;
            }
            else
            {
                seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;
            }
        }

        List<string> materialNoApplyNames = materialsNoApplyListToNameList();

        List<GameObject> listAllNodeLeafs = new List<GameObject>();
        AddDescendantsWithTag(parentTransform, listAllNodeLeafs);
        List<Transform> tmpTransforms = new List<Transform>();
        foreach (GameObject go in listAllNodeLeafs)
        {
            Renderer child = go.GetComponent<Renderer>();
            if (child != null && child.materials.Length > 0)
            {
                foreach (Material material in child.gameObject.GetComponent<Renderer>().materials)
                {
                    if (material != null)
                    {
                        // if material has already SeeThroughShader and is in no apply list -> add SeeThroughShaderExemption component
                        if (material.shader == seeThroughShader && materialNoApplyNames != null && materialNoApplyNames.Contains(material.name.Replace(" (Instance)", "")))
                        {
                            if (child.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
                            {
                                child.gameObject.AddComponent<SeeThroughShaderExemption>();
                            }
                        }
                        // adds SeeThroughShader to materials, depending on layer and if not in materialNoApplyList
                        else if (((1 << child.gameObject.layer) & layerMaskToAdd) != 0 && (materialNoApplyNames == null || !materialNoApplyNames.Contains(material.name.Replace(" (Instance)", ""))))
                        {
                            material.shader = seeThroughShader;
                            //material.SetFloat("_RaycastMode", 0);
                            //material.SetFloat("_TriggerMode", 1);
                            if(referenceMaterial != null)
                            {
                                Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
                                material.SetTexture("_DissolveTex", disTex);
                                material.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));


                                foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                {
                                    if (referenceMaterial.HasProperty(propertyName))
                                    {
                                        float temp = referenceMaterial.GetFloat(propertyName);
                                        material.SetFloat(propertyName, temp);

                                        if(!tmpTransforms.Contains(child.gameObject.transform))
                                        {
                                            tmpTransforms.Add(child.gameObject.transform);
                                        }
                                    }
                                }

                                
                            }
                        }
                    }
                }
            }
        }
        transformsWithSTS = tmpTransforms.ToArray();
    }


    // equivalent to transform.GetComponentsInChildren<Transform>(); ?
    private void AddDescendantsWithTag(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, list);
        }

    }

    private List<string> materialsNoApplyListToNameList()
    {
        List<string> materialNoApplyNames;
        if (materialExemptions != null && materialExemptions.Count > 0)
        {
            materialNoApplyNames = new List<string>();
            foreach (Material mat in materialExemptions)
            {
                if (!materialNoApplyNames.Contains(mat.name))
                {
                    materialNoApplyNames.Add(mat.name);
                }
            }
            return materialNoApplyNames;
        }
        else
        {
            return null;
        }
    }

}
