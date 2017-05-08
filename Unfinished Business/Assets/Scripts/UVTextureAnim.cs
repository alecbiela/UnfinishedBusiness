using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVTextureAnim : MonoBehaviour {

    public int uvTileY = 3;
    public int uvTileX = 2;

    public int fps = 1;

    private int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        index = (int)(Time.time * fps);

        index = index % (uvTileY * uvTileX);

        Vector2 size = new Vector2(1.0f / uvTileY, 1.0f / uvTileX);

        var uIndex = index % uvTileX;
        var vIndex = index / uvTileX;

        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        var mats = GetComponent<Renderer>().materials;

        mats[1].SetTextureOffset("_MainTex", offset);
        mats[1].SetTextureScale("_MainTex", size);

        mats[2].SetTextureOffset("_MainTex", offset);
        mats[2].SetTextureScale("_MainTex", size);
    }
}
