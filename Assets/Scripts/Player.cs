using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isMouseReleased = false;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public static GameObject selectedShape = null;
    public static GameObject hoveringShape = null;

    static GameObject lastHoveredShape = null;
    public static ShapeHandle handle1, handle2, lastHoveredHandle; 

    // Use this for initialization
    void Start()
    {
        StartCoroutine(updateCameraTarget());
    }

    void Update()
    {
        cameraUpdate();
        movement();

        updateActions();
        handleActions();
        Player.lastHoveredShape = Player.hoveringShape;
    }

    void cameraUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isMouseReleased = !isMouseReleased;
        }

        if (!isMouseReleased)
        {
            Cursor.visible = false;
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.Find("Head").transform.localEulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
        else
        {
            Cursor.visible = true;
        }
    }

    void updateActions()
    {
        bool LMouseClicked = false, LMouseHeld = false, RMouseClicked = false, RMouseHeld = false;
        if (Input.GetMouseButtonDown(0))
            LMouseClicked = true;
        if (Input.GetMouseButton(0))
            LMouseHeld = true;
        if (Input.GetMouseButtonDown(1))
            RMouseClicked = true;
        if (Input.GetMouseButton(1))
            RMouseHeld = true;

        if (LMouseClicked)
        {
            if(Player.selectedShape == null)
            {
                if(Player.hoveringShape != null)
                {
                    ModelShape ms = Player.hoveringShape.GetComponent<ModelShape>();
                    if (ms != null)
                    {
                        Player.selectedShape = Player.hoveringShape;
                        ms.setSelected(true);
                    }
                }
            }
            else
            {
                if(hoveringShape && hoveringShape.GetComponent<ShapeHandle>())
                {
                    ShapeHandle handle = hoveringShape.GetComponent<ShapeHandle>();
                    if (handle.isSelected)
                    {
                        handle.isSelected = false;
                        handle.GetComponent<MeshRenderer>().material = handle.unselected;
                        if (handle == handle1)
                        {
                            handle1 = handle2;
                            handle2 = null;
                        }else if(handle == handle2)
                        {
                            handle2 = null;
                        }
                    }
                    else
                    {
                        if (handle1 == null)
                        {
                            handle1 = handle;
                        }
                        else 
                        {
                            if(handle2)
                                handle2.GetComponent<MeshRenderer>().material = handle.unselected;

                            handle2 = handle;
                        }
                        handle.GetComponent<MeshRenderer>().material = handle.selected;
                        handle.isSelected = true;
                    }
                   
                }
                if (Player.hoveringShape == Player.selectedShape || Player.hoveringShape == null || Player.hoveringShape.GetComponent<ShapeHandle>() == null)
                {
                    if(Player.hoveringShape == null || hoveringShape.transform.parent == null || !hoveringShape.transform.parent.name.Equals("Axes"))
                    {
                        Player.selectedShape.GetComponent<ModelShape>().setSelected(false);
                        Player.selectedShape = null;
                    }
                }

                
            }
        }
        else
        {
            if (hoveringShape == null)
            {
                if (Player.lastHoveredShape != null)
                {
                    if (lastHoveredShape.GetComponent<ModelShape>() != null && 
                        lastHoveredShape.transform.Find("Cube").Find("OutlineHover") != null)
                    {
                        lastHoveredShape.transform.Find("Cube").Find("OutlineHover").gameObject.SetActive(false);
                    }

                    lastHoveredShape = null;
                }

                if (lastHoveredHandle)
                {
                    if (lastHoveredHandle.isSelected)
                        lastHoveredHandle.gameObject.GetComponent<MeshRenderer>().material = lastHoveredHandle.selected;
                    else
                        lastHoveredHandle.gameObject.GetComponent<MeshRenderer>().material = lastHoveredHandle.unselected;

                    lastHoveredHandle = null;
                }
            }
            else
            {
                if(Player.selectedShape == null)
                {
                    if (hoveringShape != null && hoveringShape.GetComponent<ModelShape>() != null)
                    {
                        hoveringShape.transform.Find("Cube").Find("OutlineHover").gameObject.SetActive(true);
                    }

                }
                else
                {
                    ShapeHandle handle = Player.hoveringShape.GetComponent<ShapeHandle>();
                    if (handle)
                    {
                        if(handle.isSelected)
                            hoveringShape.GetComponent<MeshRenderer>().material = handle.selectedHighlighted;
                        else
                            hoveringShape.GetComponent<MeshRenderer>().material = handle.highlighted;

                        lastHoveredHandle = handle;
                    }
                    else
                    {
                        if (lastHoveredHandle != null)
                        {
                            if(lastHoveredHandle.isSelected)
                                lastHoveredHandle.gameObject.GetComponent<MeshRenderer>().material = lastHoveredHandle.selected;
                            else
                                lastHoveredHandle.gameObject.GetComponent<MeshRenderer>().material = lastHoveredHandle.unselected;

                            lastHoveredHandle = null;
                        }
                    }
                }
            }
        }
    }

    void handleActions()
    {

        if (handle1 == null || handle2 == null)
            return;

        GameObject mover = GameObject.Find("Mover3D");
        

        mover.transform.position = handle2.transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            if(hoveringShape && hoveringShape.gameObject.transform.parent && hoveringShape.gameObject.transform.parent.name.Equals("Axes"))
            {
                hoveringShape.transform.parent.parent.GetComponent<Mover3D>().grabAxis(selectedShape.transform.Find("Cube").gameObject, (Mover3D.Axis)System.Enum.Parse(typeof(Mover3D.Axis), hoveringShape.name));
            }
        }

        if (!Input.GetMouseButton(0))
        {
            if (GameObject.Find("Mover3D").GetComponent<Mover3D>())
                GameObject.Find("Mover3D").GetComponent<Mover3D>().releaseAxis();
        }

        if(selectedShape == null || handle1 == null || handle2 == null)
        {
            //GameObject.Find("Mover3D").SetActive(false);
        }
    }

    public float speed = 1;
    void movement()
    {
        Vector3 movement = Vector3.zero;
        Vector3 forward = transform.Find("Head").forward;
        Vector3 right = transform.Find("Head").right;

        if (Input.GetKey(KeyCode.W))
            movement += new Vector3(forward.x, 0, forward.z);
        if (Input.GetKey(KeyCode.A))
            movement -= new Vector3(right.x, 0, right.z);
        if (Input.GetKey(KeyCode.S))
            movement -= new Vector3(forward.x, 0, forward.z);
        if (Input.GetKey(KeyCode.D))
            movement += new Vector3(right.x, 0, right.z);

        transform.position += movement * speed * Time.deltaTime;
    }


    IEnumerator updateCameraTarget()
    {
        while (true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!isMouseReleased)
            {
                ray = new Ray(transform.Find("Head").position, transform.Find("Head").forward);
            }

            Physics.Raycast(ray, out hit, 100);
            if (hit.transform != null)
                hoveringShape = hit.transform.gameObject;
            else
                hoveringShape = null;

            yield return new WaitForSeconds(.1f);
        }
    }

    void setHoveredObject(GameObject go)
    {
        if (Player.selectedShape != null)
            return;

        if (Player.hoveringShape != null && hoveringShape != go)
        {
            Player.hoveringShape.transform.Find("Cube").Find("OutlineHover").gameObject.SetActive(false);
        }

        Player.hoveringShape = go;
        if (go != null)
            Player.hoveringShape.transform.Find("Cube").Find("OutlineHover").gameObject.SetActive(true);
    }

    void setSelectedObject(GameObject go)
    {
        if (Player.selectedShape != null && selectedShape != go)
        {
            Player.selectedShape.transform.Find("OutlineSelected").gameObject.SetActive(false);
            Player.selectedShape.transform.Find("OutlineHover").gameObject.SetActive(false);
            Player.selectedShape = null;
        }
        else
        {
            Player.selectedShape = go;
            if (go != null)
            {
                Player.selectedShape.transform.Find("OutlineSelected").gameObject.SetActive(true);
                Player.selectedShape.transform.Find("OutlineHover").gameObject.SetActive(true);
            }

        }


    }


    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 5, Screen.height / 2 - 5, 10, 10), "");
    }

    public void buttonNewObjectClicked()
    {

    }

}
