using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class SeeThroughShaderGeneralUtils
{

    public static readonly List<string> STS_SHADER_LIST = new List<string>
    {
        "Custom/SeeThroughShaderHDRP2019","Custom/SeeThroughShaderHDRP2020", "Custom/SeeThroughShaderURP2019", "Custom/SeeThroughShaderURP2020", "Custom/SeeThroughShader",
        //dev 
        "Custom/SeeThroughShaderOriginal"
    };

    //public static readonly List<string> STS_REFMAT_LIST = new List<string>
    //{
    //    "refMaterial","refMaterial2","refMaterial3","refMaterial4",
    //};

    public static readonly List<string> STS_PROPERTIES_LIST = new List<string>
    {
        "_Obstruction", "_AnimationSpeed", "_AnimationEnabled", "_TransitionDuration",
        "_DissolveFallOff", "_PreviewMode", "_CircleObstructionDestroyRadius", "_CircleStrength",
        "_DissolveEmissionBooster", "_AngleStrength", "_IntrinsicDissolveStrength", "_ConeStrength",
        "_ConeObstructionDestroyRadius", "_CylinderStrength", "_CylinderObstructionDestroyRadius",
        "_Floor", "_FloorY", "_PlayerPosYOffset", "_UVs", "_hasClippedShadows", "_DissolveColorSaturation",
        "_DissolveEmission", "_TextureVisibility", "_FloorYTextureGradientLength", "_DefaultEffectRadius",
        "_TexturedEmissionEdgeStrength", "_TexturedEmissionEdge"
    };
   
    


    public class UnityVersionRenderPipelineShaderInfo
    {
        public string unityVersion;
        public string renderPipeline;
        public string versionAndRPCorrectedShader;
        
        public UnityVersionRenderPipelineShaderInfo(string unityVersion, string renderPipeline, string shader)
        {
            this.unityVersion = unityVersion;
            this.renderPipeline = renderPipeline;
            this.versionAndRPCorrectedShader = shader;
        }
    }
    public static UnityVersionRenderPipelineShaderInfo getUnityVersionAndRenderPipelineCorrectedShaderString()
    {
        string unityVersion;
        string renderPipeline;
        string shaderString;
        unityVersion = Application.unityVersion;
        if (GraphicsSettings.currentRenderPipeline)
        {
            if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
            {
                renderPipeline = "HDRP";
                if (unityVersion.Substring(0, 4).Equals("2019"))
                {
                    shaderString = "Custom/SeeThroughShaderHDRP2019";
                }
                else
                {
                    shaderString = "Custom/SeeThroughShaderHDRP2020";
                }
            }
            else
            {
                renderPipeline = "URP";
                if (Application.unityVersion.Substring(0, 4).Equals("2019"))
                {
                    shaderString = "Custom/SeeThroughShaderURP2019";
                }
                else
                {
                    shaderString = "Custom/SeeThroughShaderURP2020";
                }
            }
        }
        else
        {
            renderPipeline = "Built-in RP";
            shaderString = "Custom/SeeThroughShader";
        }

        //dev
        //shaderString = "Custom/SeeThroughShaderOriginal";
        

        return new UnityVersionRenderPipelineShaderInfo(unityVersion,renderPipeline, shaderString);
    }


    public static void updateSeeThroughShaderMaterialProperties(Transform[] transforms, string seeThroughShaderName, Material referenceMaterial)
    {

        Material firstInstancedMaterial = getFirstInstancedMaterial(transforms, seeThroughShaderName);
        List<string> namesOfChangedProperties = getNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial);
        if(namesOfChangedProperties.Count > 0)
        {
            foreach (Transform transform in transforms)
            {
                Renderer rendererNonLOD = transform.GetComponent<Renderer>();

                if (transform.GetComponent<LODGroup>() != null)
                {
                    foreach (LOD lod in transform.GetComponent<LODGroup>().GetLODs())
                    {
                        foreach (Renderer renderer in lod.renderers)
                        {
                            if (renderer == rendererNonLOD)
                            {
                                rendererNonLOD = null;
                            }
                            if (renderer != null && renderer.materials.Length > 0)
                            {
                                foreach (Material mat in renderer.materials)
                                {
                                    if (mat != null && mat.shader.name == seeThroughShaderName)
                                    {
                                        updateMaterialProperties(mat, referenceMaterial, namesOfChangedProperties);
                                    }
                                }
                            }
                        }
                    }
                }

                if (rendererNonLOD != null)
                {
                    Material[] materials = rendererNonLOD.materials;
                    if (materials.Length > 0)
                    {
                        foreach (Material material in materials)
                        {
                            if (material != null && material.shader.name == seeThroughShaderName)
                            {
                                updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
                            }
                        }
                    }
                }
            }
        }
    }


    private static void updateMaterialProperties(Material instancedMaterial, Material referenceMaterial, List<string> namesOfChangedProperties)
    {
        foreach (string propertyName in namesOfChangedProperties)
        {
            if(propertyName.Equals("_DissolveTex"))
            {
                Texture disTex = referenceMaterial.GetTexture("_DissolveTex");
                instancedMaterial.SetTexture("_DissolveTex", disTex);
            }
            else if (propertyName.Equals("_DissolveColor"))
            {
                instancedMaterial.SetColorArray("_DissolveColor", referenceMaterial.GetColorArray("_DissolveColor"));
            } 
            else
            {
                float temp = referenceMaterial.GetFloat(propertyName);
                instancedMaterial.SetFloat(propertyName, temp);
            }
        }
    }

    private static List<string> getNamesOfAllChangedPropertyValues(Material instancedMaterial, Material referenceMaterial)
    {
        if(instancedMaterial != null && referenceMaterial != null)
        {
            List<string> namesOfChangedProperties = new List<string>();
            foreach (string propertyName in SeeThroughShaderGeneralUtils.STS_PROPERTIES_LIST)
            {
                float instancedValue = instancedMaterial.GetFloat(propertyName);
                float referenceValue = referenceMaterial.GetFloat(propertyName);
                if(instancedValue != referenceValue)
                {
                    namesOfChangedProperties.Add(propertyName);
                }
            }
            string dissolveTexPropertyName = "_DissolveTex";
            Texture instancedDissolveTexture = instancedMaterial.GetTexture(dissolveTexPropertyName);
            Texture referenceDissolveTexture = referenceMaterial.GetTexture(dissolveTexPropertyName);
            
            if (!String.Equals(instancedDissolveTexture.name, referenceDissolveTexture.name))
            {
                namesOfChangedProperties.Add(dissolveTexPropertyName);
            }

            string dissolveColorPropertyName = "_DissolveColor";
            Color[] instancedColor = instancedMaterial.GetColorArray(dissolveColorPropertyName);
            Color[] referenceColor = referenceMaterial.GetColorArray(dissolveColorPropertyName);
            if (!instancedColor[0].Equals(referenceColor[0]))
            {
                namesOfChangedProperties.Add(dissolveColorPropertyName);
            }

            return namesOfChangedProperties;
        } else
        {
            Debug.LogError("InstancedMaterial or referenceMaterial are null");
            return null;
        }
    }


    private static Material getFirstInstancedMaterial(Transform[] transforms, string seeThroughShaderName)
    {
        if (transforms != null && transforms.Length > 0)
        {
            foreach(Transform transform in transforms)
            {
                if(transform != null)
                {
                    Renderer renderer = transform.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material[] materials = renderer.materials;
                        if (materials != null && materials.Length > 0)
                        {
                            foreach (Material material in materials)
                            {
                                if (material != null && material.shader.name == seeThroughShaderName)
                                {
                                    return material;
                                }
                            }
                        }

                    }
                }
            }
        }
        return null;
    }


}
