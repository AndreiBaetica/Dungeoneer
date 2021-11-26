using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShaderPropertySync : MonoBehaviour
{

    public Material referenceMaterial;
    public List<Material> children;
    private Transform[] listNodesLeafs;
    public bool syncContinuus = true;

    private string seeThroughShaderName;

    void Start()
    {
        if (this.isActiveAndEnabled)
        {
            seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;


            if (referenceMaterial != null && children != null && children.Count > 0)
            {
                foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                {
                    float temp = referenceMaterial.GetFloat(propertyName);

                    foreach (Material child in children)
                    {
                        child.SetFloat(propertyName, temp);
                        Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
                        child.SetTexture("_DissolveTex", disTex);
                        child.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));
                    }

                }


            }
            listNodesLeafs = transform.GetComponentsInChildren<Transform>();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void LateUpdate()
    {
        if (this.isActiveAndEnabled)
        {
            if (syncContinuus && referenceMaterial != null)
            {
                SeeThroughShaderGeneralUtils.updateSeeThroughShaderMaterialProperties(listNodesLeafs, seeThroughShaderName, referenceMaterial);
            }
        }
    }

}
