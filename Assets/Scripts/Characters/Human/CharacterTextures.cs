using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTextures : MonoBehaviour
{
    [SerializeField] private List<Material> textures;

    public void SetTexture(int newTextureID)
    {
        if (newTextureID >= textures.Count)
            throw new IndexOutOfRangeException(message: "Invalid texture id: " + newTextureID);

        foreach (Renderer childRenderer in gameObject.transform)
            childRenderer.material = textures[newTextureID];
    }

    public List<string> GetTextureNames()
    {
        var textureNames = new List<string>();

        foreach (var material in textures)
            textureNames.Add(material.name);

        return textureNames;
    }
}