using UnityEngine;

public class ColorChangingCube : Interactable
{
    
    private MeshRenderer meshRenderer;
    public Color[] colors;
    private int colorIndex;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void Interact()
    {
        colorIndex++;
        if (colorIndex > colors.Length - 1)
        {
            colorIndex = 0;
        }
        meshRenderer.material.color = colors[colorIndex];
    }

}
