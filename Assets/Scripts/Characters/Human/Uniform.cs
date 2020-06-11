using System;
using System.Collections.Generic;
using UnityEngine;

public class Uniform : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private List<Material> uniformTextures;
    private MeshRenderer meshRenderer;
    private int textureID = 0;

    public int ID { get { return id; } }

    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public int GetTextureID()
    {
        return textureID;
    }

    public void SetTexture(int newTextureID)
    {
        if (newTextureID >= uniformTextures.Count)
            throw new IndexOutOfRangeException(message: "Invalid uniform texture id: " + newTextureID);

        textureID = newTextureID;
        meshRenderer.material = uniformTextures[newTextureID];
    }

    public List<string> GetTextureNames()
    {
        var textureNames = new List<string>();

        foreach (var material in uniformTextures)
            textureNames.Add(material.name);

        return textureNames;
    }
}
