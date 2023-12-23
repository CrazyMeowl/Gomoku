using UnityEngine;
using UnityEngine.SceneManagement;

public class TileSelector : MonoBehaviour
{
    public Material selectedMaterial;

    private Material originalMaterial;

    private void Start()
    {

        originalMaterial = GetComponent<Renderer>().material;

    }

    private void OnMouseOver()
    {

        GetComponent<Renderer>().material = selectedMaterial;
    }

    private void OnMouseExit()
    {

        GetComponent<Renderer>().material = originalMaterial;
    }
    private void OnMouseUpAsButton()
    {
        string[] coord_string = gameObject.name.Split('_');
        print("Y:" + coord_string[0] + ", X:" + coord_string[1]);

        GameObject gameControllerObject = GameObject.Find("GameController");
        gameControllerObject.GetComponent<GameController>().MakeMove(int.Parse(coord_string[0]), int.Parse(coord_string[1]));
    }
    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
