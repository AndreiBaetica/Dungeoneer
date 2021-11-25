using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerById : MonoBehaviour
{
    public string triggerID;    
    public bool thisIsEnterTrigger = false;
    public bool thisIsExitTrigger = false;
    public bool addSeeThroughShaderAtStart = false;
    Shader seeThroughShader;    
    
    // Start is called before the first frame update    
    
    private TriggerBox triggerBox;
    public Material refMaterial;
    public List<Material> listMaterialNoApply;
    private SeeThroughShaderController seeThroughShaderController;
    private List<Material> listM;
    private List<GameObject> listPlayerInside = new List<GameObject>();

    private string seeThroughShaderName;

    void Start()
    {
        if (this.isActiveAndEnabled)
        {
            seeThroughShaderName = SeeThroughShaderGeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader;


            if (addSeeThroughShaderAtStart)
            {
                AddSeeThroughShaderAtStart();
            }

            listM = buildListMaterial();
            if (listM.Count > 0)
            {

                seeThroughShaderController = new SeeThroughShaderController(listM);
            }
            seeThroughShader = Shader.Find(seeThroughShaderName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        if(seeThroughShaderController != null)
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
        listM = buildListMaterial();
        if (listM != null && listM.Count > 0)
        {
            if (seeThroughShaderController == null)
            {

                seeThroughShaderController = new SeeThroughShaderController(listM);
            }
            seeThroughShaderController.notifyOnTriggerEnter(listM,other.transform);
        }
        
    }

    public void SetSeeThroughInActive(Collider other)
    {
        listM = buildListMaterial();
        if (listM != null && listM.Count > 0)
        {
            if (seeThroughShaderController == null)
            {
                seeThroughShaderController = new SeeThroughShaderController();  // why not new SeeThroughShaderController(listM);?
            }
        }
        seeThroughShaderController.notifyOnTriggerExit(listM,other.transform);

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
                        if (t.gameObject.GetComponent<TriggerById>() && t.gameObject.GetComponent<TriggerById>().triggerID == this.triggerID)
                        {
                            if (t.gameObject.GetComponent<TriggerById>().listPlayerInside.Count > 0 && t.gameObject.GetComponent<TriggerById>().listPlayerInside.Contains(other.gameObject))
                            {
                                isInside = true;
                                groupMembersFound.Add(t.gameObject);
                            }
                        }
                    }
                }

                TriggerById[] groupMembersFoundScene = GameObject.FindObjectsOfType<TriggerById>();
                if (groupMembersFoundScene.Length > 0)
                {
                    foreach (TriggerById item in groupMembersFoundScene)
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
                        if (t.gameObject.GetComponent<TriggerById>() && t.gameObject.GetComponent<TriggerById>().triggerID == this.triggerID)
                        {
                            if (t.gameObject.GetComponent<TriggerById>().listPlayerInside.Count > 0 && t.gameObject.GetComponent<TriggerById>().listPlayerInside.Contains(other.gameObject))
                            {
                                isInside = true;
                                groupMembersFound.Add(t.gameObject);
                            }
                        }
                    }
                }

                TriggerById[] groupMembersFoundScene = GameObject.FindObjectsOfType<TriggerById>();
                if (groupMembersFoundScene.Length > 0)
                {
                    foreach (TriggerById item in groupMembersFoundScene)
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
                            if (item.GetComponent<TriggerById>() != null && item.GetComponent<TriggerById>().listPlayerInside.Count > 0)
                            {
                                if (item.GetComponent<TriggerById>().listPlayerInside.Contains(other.gameObject))
                                {
                                    item.GetComponent<TriggerById>().listPlayerInside.Remove(other.gameObject);
                                }
                            }
                        }
                    }
                    SetSeeThroughInActive(other);

                }

            }
        }

    }

    public void AddSeeThroughShaderAtStart()
    {
        if (!string.IsNullOrEmpty(triggerID))
        {
            if (seeThroughShader == null)
            {
                seeThroughShader = Shader.Find(seeThroughShaderName);
            }
            TriggerObjectId[] idObjects = GameObject.FindObjectsOfType<TriggerObjectId>();
            List<GameObject> listAllNodeLeafs = new List<GameObject>();
            List<Material> listMaterial = new List<Material>();

            foreach (TriggerObjectId item in idObjects)
            {

                if (item.gameObject.GetComponent<TriggerObjectId>() != null && item.gameObject.GetComponent<TriggerObjectId>().triggerID == triggerID)
                {
                    listAllNodeLeafs.Add(item.transform.gameObject);
                    if (item.gameObject.GetComponent<Renderer>() != null && item.gameObject.GetComponent<Renderer>().material != null || item.gameObject.GetComponent<Renderer>().materials.Length > 0)
                    {
                        if (item.gameObject.GetComponent<Renderer>().materials.Length > 0)
                        {
                            foreach (Material m in item.gameObject.GetComponent<Renderer>().materials)
                            {
                                bool isInBanList = false;
                                if (listMaterialNoApply.Count > 0)
                                {
                                    foreach (Material mat in listMaterialNoApply)
                                    {
                                        if (m.name.Contains(mat.name))
                                        {
                                            isInBanList = true;
                                        }
                                    }
                                }
                                else
                                {
                                    isInBanList = false;
                                }

                                if (!isInBanList)
                                {
                                    m.shader = seeThroughShader;
                                    if (m.shader == null)
                                    {
                                        m.shader = Shader.Find(seeThroughShaderName);
                                    }

                                    if (m.shader != null)
                                    {
                                        if (refMaterial != null)
                                        {
                                            Texture disTex = refMaterial.GetTexture("_DissolveTex");
                                            m.SetTexture("_DissolveTex", disTex);
                                            m.SetColorArray("_DissolveColor", refMaterial.GetColorArray("_DissolveColor"));
                                            if (refMaterial != null)
                                            {
                                                foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                                {
                                                    float temp = refMaterial.GetFloat(propertyName);
                                                    m.SetFloat(propertyName, temp);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            bool isInBanList = false;
                            if (listMaterialNoApply.Count > 0)
                            {
                                foreach (Material material in listMaterialNoApply)
                                {
                                    if (item.gameObject.GetComponent<Renderer>().material.name.Contains(material.name))
                                    {
                                        isInBanList = true;
                                    }
                                }
                            }
                            else
                            {
                                isInBanList = false;
                            }

                            if (!isInBanList)
                            {
                                item.gameObject.GetComponent<Renderer>().material.shader = seeThroughShader;
                                if (item.gameObject.GetComponent<Renderer>().material.shader == null)
                                {
                                    item.gameObject.GetComponent<Renderer>().material.shader = Shader.Find(seeThroughShaderName);
                                    
                                }
                                if (item.gameObject.GetComponent<Renderer>().material.shader != null)
                                {
                                    if (refMaterial != null)
                                    {
                                        Texture disTex = refMaterial.GetTexture("_DissolveTex");
                                        item.gameObject.GetComponent<Renderer>().material.SetTexture("_DissolveTex", disTex);
                                        item.gameObject.GetComponent<Renderer>().material.SetColorArray("_DissolveColor", refMaterial.GetColorArray("_DissolveColor"));
                                        if (refMaterial != null)
                                        {
                                            foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
                                            {
                                                float temp = refMaterial.GetFloat(propertyName);
                                                item.gameObject.GetComponent<Renderer>().material.SetFloat(propertyName, temp);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
    }


    public List<Material> buildListMaterial()
    {

        if (!string.IsNullOrEmpty(triggerID))
        {
                if (seeThroughShader == null)
                {
                    seeThroughShader = Shader.Find(seeThroughShaderName);
                }
                TriggerObjectId[] idObjects = GameObject.FindObjectsOfType<TriggerObjectId>();
                List<GameObject> listAllNodeLeafs = new List<GameObject>();
                List<Material> listMaterial = new List<Material>();

                foreach (TriggerObjectId item in idObjects)
                {

                    if (item.gameObject.GetComponent<TriggerObjectId>() != null && item.gameObject.GetComponent<TriggerObjectId>().triggerID == triggerID)
                    {
                        listAllNodeLeafs.Add(item.transform.gameObject);

                    }
                }

            foreach (GameObject go in listAllNodeLeafs)
            {
                Transform child = go.transform;
                bool goMatIsInNoApplyList = false;
                if (child.gameObject.GetComponent<Renderer>() != null && child.gameObject.GetComponent<Renderer>().materials.Length > 1)
                {
                    foreach (Material material in child.gameObject.GetComponent<Renderer>().materials)
                    {
                        bool matIsInNoApplyList = false;


                        if (material.shader == seeThroughShader && material != null)
                        {
                            if (listMaterialNoApply == null || listMaterialNoApply.Count == 0)
                            {
                                
                                listMaterial.Add(material);
                            }
                            else if (listMaterialNoApply.Count >= 1)
                            {

                                foreach (Material item in listMaterialNoApply)
                                {
                                    if (material.name.Contains(item.name))
                                    {
                                        matIsInNoApplyList = true;
                                    }
                                }

                                if (!matIsInNoApplyList)
                                {
                                    ///material.shader = seeThroughShader;
                                    listMaterial.Add(material);

                                }
                            }

                        }

                    }

                }
                else
                {
                    if (child.gameObject.GetComponent<Renderer>() != null && child.gameObject.GetComponent<Renderer>().material != null)
                    {


                        if (child.gameObject.GetComponent<Renderer>().material.shader == seeThroughShader  && child.gameObject.GetComponent<Renderer>().material != null)
                        {
                            if (listMaterialNoApply == null || listMaterialNoApply.Count == 0)
                            {
                                
                                listMaterial.Add(child.gameObject.GetComponent<Renderer>().material);

                            }
                            else if (listMaterialNoApply.Count >= 1)
                            {
                                foreach (Material item in listMaterialNoApply)
                                {
                                    if (child.gameObject.GetComponent<Renderer>().material.name.Contains(item.name))
                                    {
                                        goMatIsInNoApplyList = true;
                                    }
                                }

                                if (!goMatIsInNoApplyList)
                                {
                                    
                                    listMaterial.Add(child.gameObject.GetComponent<Renderer>().material);
                                }
                            }

                        }

                    }
                }
                
            }
            return listMaterial;
        }
        return null;
    }

}
