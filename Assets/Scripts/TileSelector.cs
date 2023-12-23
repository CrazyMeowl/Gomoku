using UnityEngine;
using UnityEngine.SceneManagement;

public class TileSelector : MonoBehaviour
{
    public Material selectedMaterial;

    public Material leftMaterial;
    public Material rightMaterial;
    public Material topMaterial;
    public Material bottomMaterial;
    public Material topleftMaterial;
    public Material toprightMaterial;
    public Material bottomleftMaterial;
    public Material bottomrightMaterial;
    public Material centerMaterial;
    private Material originalMaterial;

    public GameObject crosshair_prefab;

    public bool crosshairCreated = false;
    private void Start()
    {
        string[] str = gameObject.name.Split('.');
        string name_part = str[0];
        string coord_part = str[1];
        if (name_part == "C")
        {
            GetComponent<Renderer>().material = centerMaterial;
        }
        if (name_part == "L")
        {
            GetComponent<Renderer>().material = leftMaterial;
        }
        if (name_part == "R")
        {
            GetComponent<Renderer>().material = rightMaterial;
        }
        if (name_part == "B")
        {
            GetComponent<Renderer>().material = bottomMaterial;
        }
        if (name_part == "T")
        {
            GetComponent<Renderer>().material = topMaterial;
        }

        if (name_part == "TL")
        {
            GetComponent<Renderer>().material = topleftMaterial;
        }
        if (name_part == "TR")
        {
            GetComponent<Renderer>().material = toprightMaterial;
        }

        if (name_part == "BL")
        {
            GetComponent<Renderer>().material = bottomleftMaterial;
        }
        if (name_part == "BR")
        {
            GetComponent<Renderer>().material = bottomrightMaterial;
        }

        originalMaterial = GetComponent<Renderer>().material;

    }

    private void OnMouseOver()
    {
        if (crosshairCreated == false)
        {
            GameObject crosshair = Instantiate(crosshair_prefab);

            crosshair.transform.position = gameObject.transform.position + new Vector3(0, -0.001f, 0);

            crosshair.transform.parent = gameObject.transform;
            crosshair.name = "crosshair";
            crosshairCreated = true;
        }


    }

    private void OnMouseExit()
    {
        crosshairCreated = false;
        GameObject[] crosshairs;

        crosshairs = GameObject.FindGameObjectsWithTag("crosshair");

        foreach (GameObject crosshair in crosshairs)
        {
            Destroy(crosshair);
        }
        // GetComponent<Renderer>().material = originalMaterial;
    }
    private void OnMouseUpAsButton()
    {
        string[] str = gameObject.name.Split('.');
        string name_part = str[0];
        string coord_part = str[1];
        string[] coord_string = coord_part.Split('_');
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
