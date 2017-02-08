using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

    private MeshRenderer renderer = null;
    private Color startcolor;

    // get the renderer
    void Start () {
        renderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    void OnMouseEnter()
    {
        startcolor = renderer.material.color;
        renderer.material.color = Color.yellow;
    }
    void OnMouseExit()
    {
        renderer.material.color = startcolor;
    }

    void OnMouseDown()
    {

    }
}
