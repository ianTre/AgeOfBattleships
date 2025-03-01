using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AtackRadarCanvasController : MonoBehaviour
{
    private int spotSize= 60;
    [SerializeField] Image gridImage;
    public static AtackRadarCanvasController instance;
    

    void Awake() {
        instance = this;    
    }
    public void CreateGrid(int rows, int columns)
    {
        for (int i = 0; i < rows; i++)
        {
            var obj = Instantiate(gridImage,this.transform);
            obj.gameObject.SetActive(true);
            var oldPos =obj.rectTransform.position;
            obj.rectTransform.position = new Vector3(oldPos.x, (float)(oldPos.y - (Math.Ceiling(spotSize * 1.1)) * i ), oldPos.z);
        }
    }
}
