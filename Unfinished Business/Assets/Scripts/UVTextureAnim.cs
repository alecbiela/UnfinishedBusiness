using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVTextureAnim : MonoBehaviour {

    public float uvTileY = 1.0f;
    public float uvTileX = 1.0f;
    public float uvTileMY = 1.0f;
    public float uvTileMX = 1.0f;

    public bool talking;



    public float fps = 2f;

    private int index;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        updateEyes();
        updateMouth();
    }

    void updateEyes()
    {
        var mats = GetComponent<Renderer>().materials;

        index = (int)(Time.time * fps);

        index = index % (int)(uvTileY * uvTileX);

        Vector2 size = new Vector2(1.0f, 1.0f);

        var uIndex = index % uvTileX;
        var vIndex = index / uvTileX;

        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        mats[1].SetTextureOffset("_MainTex", offset);
        mats[1].SetTextureScale("_MainTex", size);
    }

    void updateMouth()
    {
        if (talking)
        {
            var mats = GetComponent<Renderer>().materials;

            index = (int)(Time.time * (fps * 2));

            index = index % (int)(uvTileMY * uvTileMX);

            Vector2 size = new Vector2(1.0f, 1.0f);

            var uIndex = index % uvTileMX;
            var vIndex = index / uvTileMX;

            Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

            mats[2].SetTextureOffset("_MainTex", offset);
            mats[2].SetTextureScale("_MainTex", size);
        }
    }
}
