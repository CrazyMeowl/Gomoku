using UnityEngine;

public class TileSelector : MonoBehaviour
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
        print("Y:" + coord_string[0] + ", X:" + coord_string[1]);
        // Debug.Log(gameObject.name);
        GameObject gameControllerObject = GameObject.Find("GameController");
        gameControllerObject.GetComponent<GameController>().MakeMove(int.Parse(coord_string[0]), int.Parse(coord_string[1]));
    }
}
