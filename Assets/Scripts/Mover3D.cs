using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover3D : MonoBehaviour {

    Vector2 startDragPos;
    Axis draggedAxis = Axis.X;
    GameObject shape = null;
    bool dragging = false;
    public enum Axis
    {
        X,
        Y,
        Z
    }

	void Start () {
		
	}

    void Update()
    {
        Transform axes = transform.Find("Axes");
        bool active = Player.selectedShape != null && Player.handle1 != null && Player.handle2 != null;
        for (int i = 0; i < axes.childCount; ++i)
        {
            axes.GetChild(i).gameObject.SetActive(active);
        }


        if (!dragging)
            return;

        float delta = Vector3.Distance(Input.mousePosition, startDragPos);
        int bins = (int)(delta) / 25;

        int dx = Input.mousePosition.x - startDragPos.x > 0 ? 1 : -1;
        int dy = Input.mousePosition.y - startDragPos.y > 0 ? 1 : -1;
        int dz = Input.mousePosition.x - startDragPos.x > 0 ? 1 : -1;

        Vector3 dir = Vector3.up;
        if (draggedAxis == Axis.X)
            dir = Vector3.right * dx;
        if (draggedAxis == Axis.Y)
            dir = Vector3.up * dy;
        if (draggedAxis == Axis.Z)
            dir = Vector3.forward * dz;

        if (Mathf.Abs(bins) > 0)
        {
            shape.transform.localScale += bins * .0625f * dir;
            shape.transform.localPosition = shape.transform.localScale / 2;
            startDragPos = Input.mousePosition;
        }
    }

    public void grabAxis(GameObject go, Axis axis)
    {
        startDragPos = Input.mousePosition;
        this.draggedAxis = axis;
        this.shape = go;
        dragging = true;
    }

    public void releaseAxis()
    {
        dragging = false;
    }
}
