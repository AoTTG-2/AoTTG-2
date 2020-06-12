using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
    [SerializeField] private List<Material> textures;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void SetTexture(int newTextureID)
    {
        if (newTextureID >= textures.Count)
            throw new IndexOutOfRangeException(message: "Invalid texture id: " + newTextureID);

        meshRenderer.material = textures[newTextureID];
    }

    public List<string> GetTextureNames()
    {
        var textureNames = new List<string>();

        foreach (var material in textures)
            textureNames.Add(material.name);

        return textureNames;
    }
}