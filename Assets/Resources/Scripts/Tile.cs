using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public float Xpos,Ypos,Zpos;
    public int Id;
    public string Name;
    public int XCoord,ZCoord;
    MeshRenderer meshRenderer;
    public Material tileMaterial;
    Material hoverMainMaterial;
    Material hoverSecondaryMaterial;
    MapController mapController;
    List<Tile> neighborhoods;
    bool hoverTriggered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        this.meshRenderer.materials = new Material[] {tileMaterial};
        Color regularColor = tileMaterial.color;
        
        hoverMainMaterial = Instantiate(tileMaterial);
        hoverMainMaterial.color = new Color(regularColor.r + 100f,regularColor.g,regularColor.b);
        hoverSecondaryMaterial = Instantiate(tileMaterial);
        hoverSecondaryMaterial.color = new Color(regularColor.r + 25f,regularColor.g + 10f,regularColor.b);
    }

    // Update is called once per frame
    void Update()
    {
        if(mapController==null)
            mapController=FindAnyObjectByType<MapController>();
    }

    public void HighlighMe(Material material)
    {
        this.meshRenderer.SetMaterials(new List<Material>{material});
    }

        public void DeHighlighMe()
    {
        this.meshRenderer.SetMaterials(new List<Material>{tileMaterial});
    }

        void OnMouseOver()
    {
        if(hoverTriggered)
            return;
        Debug.Log("Mouse in ("  + ZCoord.ToString() + XCoord.ToString() + ")" );
        hoverTriggered=true;
        // Change the color of the GameObject to red when the mouse is over GameObject
        HighlighMe(hoverMainMaterial);
        HighlightNeighborhood();
    }

    private void HighlightNeighborhood()
    {
        if(neighborhoods == null)
            neighborhoods = mapController.getNeighborhoods(this);
        foreach (Tile nTile in neighborhoods)
        {
            nTile.HighlighMe(hoverSecondaryMaterial);
        }
    }

    void OnMouseExit()
    {
        hoverTriggered=false;
        // Reset the color of the GameObject back to normal
        DeHighlighMe();
        DeHighlightNeighborhood();
    }

    private void DeHighlightNeighborhood()
    {
        foreach (Tile nTile in neighborhoods)
        {
            nTile.DeHighlighMe();
        }
    }
}
