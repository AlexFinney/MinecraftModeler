using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelShape : MonoBehaviour {

    GameObject handles = null;
    GameObject cube = null;

    public Vector3 offset;
    public Vector3 scale;
    public Vector3 rotation;
    void Start () {
        handles = transform.Find("Handles").gameObject;
        cube = transform.Find("Cube").gameObject;

        scale = cube.transform.localScale;
        offset = cube.transform.localPosition - Vector3.one * .5f;
    }
	
	// Update is called once per frame
	void Update () {
        setHandlePositions();

        GetComponent<BoxCollider>().size = transform.localScale;
        GetComponent<BoxCollider>().center = cube.transform.localPosition;

        float scale = Mathf.Min(cube.transform.lossyScale.x, cube.transform.lossyScale.y);
        scale = Mathf.Min(scale, cube.transform.lossyScale.z);
	}

    public void setSelected(bool selected)
    {
        if(selected)
            this.transform.Find("Cube").Find("OutlineHover").gameObject.SetActive(false);

        transform.Find("Cube").Find("OutlineSelected").gameObject.SetActive(selected);
        handles.SetActive(selected);
    }

    void setHandlePositions()
    {
        Transform handles = transform.Find("Handles");
        Vector3 size = cube.transform.localScale / 2;
        Vector3 cubePos = cube.transform.localPosition;
        for(int i = 0; i < handles.childCount; ++i)
        {
            GameObject handle = handles.GetChild(i).gameObject;
            switch (handle.name)
            {
                case "blb":
                    handle.transform.localPosition = new Vector3(cubePos.x - size.x, cubePos.y - size.y, cubePos.z - size.z);
                    break;
                case "brb":
                    handle.transform.localPosition = new Vector3(cubePos.x + size.x, cubePos.y - size.y, cubePos.z - size.z);
                    break;
                case "tlb":
                    handle.transform.localPosition = new Vector3(cubePos.x - size.x, cubePos.y + size.y, cubePos.z - size.z);
                    break;
                case "trb":
                    handle.transform.localPosition = new Vector3(cubePos.x + size.x, cubePos.y + size.y, cubePos.z - size.z);
                    break;
                case "blf":
                    handle.transform.localPosition = new Vector3(cubePos.x - size.x, cubePos.y - size.y, cubePos.z + size.z);
                    break;
                case "brf":
                    handle.transform.localPosition = new Vector3(cubePos.x + size.x, cubePos.y - size.y, cubePos.z + size.z);
                    break;
                case "tlf":
                    handle.transform.localPosition = new Vector3(cubePos.x - size.x, cubePos.y + size.y, cubePos.z + size.z);
                    break;
                case "trf":
                    handle.transform.localPosition = new Vector3(cubePos.x + size.x, cubePos.y + size.y, cubePos.z + size.z);
                    break;

            }
        }
    }
}
