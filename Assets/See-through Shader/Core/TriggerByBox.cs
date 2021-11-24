using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerByBox : MonoBehaviour
{
    public TriggerBox triggerBox;    
    public bool thisIsEnterTrigger = false;
    public bool thisIsExitTrigger = false;
    public Material refMaterial;
    public bool optionalResetColliders= false;
    public List<Material> listMaterialNoApply;
    Shader seeThroughShader;    
    private SeeThroughShaderController seeThroughShaderController;
    private List<GameObject> listPlayerInside = new List<GameObject>();
    private string seeThroughShaderName;

    void Start()
    {
        if (this.isActiveAndEnabled)
        {
            seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;
            seeThroughShader = Shader.Find(seeThroughShaderName);
            if (triggerBox != null)
            {

                List<Material> listM = buildListMaterial();
                if (listM != null && listM.Count > 0)
                {
                    if (seeThroughShaderController == null)
                    {
                        seeThroughShaderController = new SeeThroughShaderController(listM);
                    }
                }



            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (seeThroughShaderController != null)
        {
            seeThroughShaderController.destroy();
        }

    }

    private void OnDestroy()
    {
        if (seeThroughShaderController != null)
        {
            seeThroughShaderController.destroy();
        }
    }

    public void SetSeeThroughActive(Collider other)
    {

        if (triggerBox != null)
        {
            
            List<Material> listM = buildListMaterial();
            if (listM != null && listM.Count > 0)
            {
                if (seeThroughShaderController == null)
                {
                    seeThroughShaderController = new SeeThroughShaderController(listM);
                }
                seeThroughShaderController.notifyOnTriggerEnter(listM,other.transform);
            }
            


        }
    }

    public void SetSeeThroughInActive(Collider other)
    {

        if (triggerBox != null)
        {
            List<Material> listM = buildListMaterial();
            if (listM != null && listM.Count > 0)
            {
                seeThroughShaderController = new SeeThroughShaderController(listM);
                seeThroughShaderController.notifyOnTriggerExit(listM,other.transform);
            }


        }
    }

    public void setFloorColliders()
    {
        if (triggerBox != null)
        {
            triggerBox.enableAllInsideColliders();
            Collider[] hitColliders = triggerBox.GetColliderInsideBox();
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.isActiveAndEnabled && other.gameObject.GetComponent<SeeThroughShaderPlayer>() != null)
        {
            if (thisIsEnterTrigger)
            {

                bool isInside = false;
                List<GameObject> groupMembersFound = new List<GameObject>();
                foreach (Transform t in this.gameObject.transform.root)
                {
                    if (t.gameObject != this.gameObject)
                    {
                        if (t.gameObject.GetComponent<TriggerByBox>() && t.gameObject.GetComponent<TriggerByBox>().triggerBox == this.triggerBox)
                        {
                            if (t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Count > 0 && t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Contains(other.gameObject))
                            {
                                isInside = true;
                                groupMembersFound.Add(t.gameObject);
                            }
                        }
                    }
                }

                TriggerByBox[] groupMembersFoundScene = GameObject.FindObjectsOfType<TriggerByBox>();
                if (groupMembersFoundScene.Length > 0)
                {
                    foreach (TriggerByBox item in groupMembersFoundScene)
                    {
                        if (item.listPlayerInside.Count > 0)
                        {
                            if (item.listPlayerInside.Contains(other.gameObject))
                            {
                                isInside = true;
                                groupMembersFound.Add(item.transform.gameObject);
                            }
                        }
                    }
                }

                if (!listPlayerInside.Contains(other.gameObject) && !isInside)
                {
                    listPlayerInside.Add(other.gameObject);
                    SetSeeThroughActive(other);
                }

            }
            else if (thisIsExitTrigger)
            {
                bool isInside = false;
                List<GameObject> groupMembersFound = new List<GameObject>();
                foreach (Transform t in this.gameObject.transform.root)
                {
                    if (t.gameObject != this.gameObject)
                    {
                        if (t.gameObject.GetComponent<TriggerByBox>() && t.gameObject.GetComponent<TriggerByBox>().triggerBox == this.triggerBox)
                        {
                            if (t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Count > 0 && t.gameObject.GetComponent<TriggerByBox>().listPlayerInside.Contains(other.gameObject))
                            {
                                isInside = true;
                                groupMembersFound.Add(t.gameObject);
                            }
                        }
                    }
                }

                TriggerByBox[] groupMembersFoundScene = GameObject.FindObjectsOfType<TriggerByBox>();
                if (groupMembersFoundScene.Length > 0)
                {
                    foreach (TriggerByBox item in groupMembersFoundScene)
                    {
                        if (item.listPlayerInside.Count > 0)
                        {
                            if (item.listPlayerInside.Contains(other.gameObject))
                            {
                                isInside = true;
                                groupMembersFound.Add(item.transform.gameObject);
                            }
                        }
                    }
                }

                if (isInside)
                {
                    if (groupMembersFound.Count > 0)
                    {
                        foreach (GameObject item in groupMembersFound)
                        {
                            if (item.GetComponent<TriggerByBox>() != null && item.GetComponent<TriggerByBox>().listPlayerInside.Count > 0)
                            {
                                if (item.GetComponent<TriggerByBox>().listPlayerInside.Contains(other.gameObject))
                                {
                                    item.GetComponent<TriggerByBox>().listPlayerInside.Remove(other.gameObject);
                                }
                            }
                        }
                    }
                    SetSeeThroughInActive(other);

                }

            }


            if (optionalResetColliders)
            {
                setFloorColliders();
            }

        }
    }

    public List<Material> buildListMaterial()
    {
        Collider[] hitColliders = triggerBox.GetColliderInsideBox();
        List<Material> listMaterial = new List<Material>();
        if (hitColliders != null && hitColliders.Length > 0)
        {
            foreach (Collider item in hitColliders)
            {

                if (listMaterialNoApply.Count > 0)
                {
                    foreach (Material m in listMaterialNoApply)
                    {
                        if (item.GetComponent<Renderer>())
                        {
                            if (item.GetComponent<Renderer>().materials.Length > 1)
                            {
                                foreach (Material mat in item.GetComponent<Renderer>().materials)
                                {
                                    if (mat != null && !mat.name.Contains(m.name))
                                    {
                                        if (mat.shader != seeThroughShader)
                                        {
                                            mat.shader = seeThroughShader;
                                            Texture disTex = refMaterial.GetTexture("_DissolveTex");
                                            mat.SetTexture("_DissolveTex", disTex);
                                            mat.SetColorArray("_DissolveColor", refMaterial.GetColorArray("_DissolveColor"));
                                            if (refMaterial != null)
                                            {
                                                foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                                {
                                                    float temp = refMaterial.GetFloat(propertyName);
                                                    mat.SetFloat(propertyName, temp);
                                                }
                                            }
                                        }
                                        listMaterial.Add(mat);
                                    }
                                }
                            }
                            else
                            {
                                if (item.GetComponent<Renderer>().material)
                                {
                                    if (!item.GetComponent<Renderer>().material.name.Contains(m.name))
                                    {
                                        if (item.GetComponent<Renderer>().material.shader.name != seeThroughShader.name)
                                        {
                                            item.GetComponent<Renderer>().material.shader = seeThroughShader;
                                            Texture disTex = refMaterial.GetTexture("_DissolveTex");
                                            item.GetComponent<Renderer>().material.SetTexture("_DissolveTex", disTex);
                                            item.gameObject.GetComponent<Renderer>().material.SetColorArray("_DissolveColor", refMaterial.GetColorArray("_DissolveColor"));
                                            if (refMaterial != null)
                                            {
                                                foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                                {
                                                    float temp = refMaterial.GetFloat(propertyName);
                                                    item.GetComponent<Renderer>().material.SetFloat(propertyName, temp);
                                                }
                                            }
                                        }
                                        listMaterial.Add(item.GetComponent<Renderer>().material);
                                    }
                                    else
                                    {
                                        Debug.Log("Error");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (item.GetComponent<Renderer>() != null && item.GetComponent<Renderer>().materials.Length > 1)
                    {
                        foreach (Material mat in item.GetComponent<Renderer>().materials)
                        {
                            if (mat != null)
                            {
                                if (mat.shader.name != seeThroughShader.name)
                                {
                                    mat.shader = seeThroughShader;
                                    Texture disTex = refMaterial.GetTexture("_DissolveTex");
                                    mat.SetTexture("_DissolveTex", disTex);
                                    mat.SetColorArray("_DissolveColor", refMaterial.GetColorArray("_DissolveColor"));
                                    if (refMaterial != null)
                                    {
                                        foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                        {
                                            float temp = refMaterial.GetFloat(propertyName);
                                            mat.SetFloat(propertyName, temp);
                                        }
                                    }
                                }
                                listMaterial.Add(mat);

                            }

                        }

                    }
                    else
                    {
                        if (item.GetComponent<Renderer>() != null && item.GetComponent<Renderer>().material)
                        {

                            if (item.GetComponent<Renderer>().material.shader.name != seeThroughShader.name)
                            {
                                item.GetComponent<Renderer>().material.shader = seeThroughShader;

                            }
                            Texture disTex = refMaterial.GetTexture("_DissolveTex");
                            item.GetComponent<Renderer>().material.SetTexture("_DissolveTex", disTex);
                            item.gameObject.GetComponent<Renderer>().material.SetColorArray("_DissolveColor", refMaterial.GetColorArray("_DissolveColor"));
                            if (refMaterial != null)
                            {
                                foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                {
                                    float temp = refMaterial.GetFloat(propertyName);
                                    item.GetComponent<Renderer>().material.SetFloat(propertyName, temp);
                                }
                            }
                            listMaterial.Add(item.GetComponent<Renderer>().material);

                        }
                    }
                    
                }
                
            }
            return listMaterial;
        }
        return null;
    }




}
