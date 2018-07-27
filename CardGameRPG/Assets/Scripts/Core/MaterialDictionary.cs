using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDictionary : MonoBehaviour {

    public List<MaterialLookup> materials;
    public Dictionary<string, Material> materialDictionary;

    private void Awake() {
        materialDictionary = new Dictionary<string, Material>();
        foreach (MaterialLookup ml in materials) {
            materialDictionary[ml.key] = ml.material;
        }
    }
    
    private static MaterialDictionary materialDictionaryInstance;
    public static MaterialDictionary Instance() {
        if (!materialDictionaryInstance) {
            materialDictionaryInstance = FindObjectOfType<MaterialDictionary>();
        }
        return materialDictionaryInstance;
    }
}

[Serializable]
public struct MaterialLookup {
    public string key;
    public Material material;
}

