using UnityEngine;

public class CubeSelector : MonoBehaviour
{
    public Material selectedMaterial;

    private Material originalMaterial;

    private void Start()
    {
        // Store the original material of the cube.
        originalMaterial = GetComponent<Renderer>().material;
    }

    private void OnMouseOver()
    {
        // Change the cube's material to the selected material.
        GetComponent<Renderer>().material = selectedMaterial;
    }

    private void OnMouseExit()
    {
        // Change the cube's material back to the original material.
        GetComponent<Renderer>().material = originalMaterial;
    }
    private void OnMouseUpAsButton()
    {
        string[] coord_string = gameObject.name.Split('_');
        print(coord_string[0] + ' ' + coord_string[1]);
        // Debug.Log(gameObject.name);
    }
}
