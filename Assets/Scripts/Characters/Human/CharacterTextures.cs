using System;
using System.Collections.Generic;
using UnityEngine;

//Manages the textures for all child renderers
public class CharacterTextures : MonoBehaviour
{
    [SerializeField] private List<Material> materials;
    private List<Renderer> childRenderers;

    //Add the renderers of all child objects
    private void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (childRenderer != null)
                childRenderers.Add(childRenderer);
        }
    }

    //Set the material texture for all of the child objects
    public void SetTexture(int newTextureID)
    {
        if (newTextureID >= materials.Count)
            throw new IndexOutOfRangeException(message: "Invalid texture ID: " + newTextureID);

        foreach (Renderer childRenderer in childRenderers)
            childRenderer.material = materials[newTextureID];
    }

    public List<string> GetTextureNames()
    {
        var textureNames = new List<string>();

        foreach (var material in materials)
            textureNames.Add(material.name);

        return textureNames;
    }
}