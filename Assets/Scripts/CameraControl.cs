using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private Vector3 mouseWorldPosStart;
    private float zoomScale = 10.0f;
    // minimum size of the camera for zoom scaling.
    private float zoomMin = 1f;
    // maximum size of the camera for zoom scaling.
    private float zoomMax = 20.0f;
    // maximum position of the camera for position.
    public float maxpos;

    // Start is called before the first frame update
    void Start()
    {
        // Set the max position of the camera.
        maxpos = (int)(PlayerPrefs.GetInt("BoardSize") / 2);
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.LeftShift))
        {
            mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift))
        {
            Pan();
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));

        // keep the camera position inside of the board.
        // prevent the camera from moving away from the board.
        if (transform.position.x > maxpos)
        {
            transform.position = new Vector3(maxpos, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -maxpos)
        {
            transform.position = new Vector3(-maxpos, transform.position.y, transform.position.z);
        }
        if (transform.position.z > maxpos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxpos);
        }
        if (transform.position.z < -maxpos)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -maxpos);
        }


    }



    private void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            // calculate the difference of the mouse position.
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // add the difference of the mouse position to the camera position.
            transform.position += mouseWorldPosDiff;

        }
    }

    private void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            // set the point of the zoom location
            mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // set the size of the camera according to the input zoomDiff
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
            // calculate the difference of the mouse position.
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // add the difference of the mouse position to the camera position.
            transform.position += mouseWorldPosDiff;
        }
    }
}