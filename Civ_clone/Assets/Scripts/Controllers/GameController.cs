using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2Int lastPos = Vector2Int.zero;
    private Color originalColor = Color.white;
    public Color HoverColor = Color.grey;
    Vector2Int lastTilePos = Vector2Int.zero;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Clicked();
        MoveCursor();
        lastPos = GetMousePosition();
        lastTilePos = GetMousePosition();
    }
    void Clicked()
    {
        //print("clicked");
        
        GameManager.GetBlockByPos(GetMousePosition()).SetActive(false);
    }
    private Vector2Int GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.collider.gameObject.transform.position;
            return new Vector2Int((int)hitPos.x, -1 * (int)hitPos.z);

            //return Vector3Int.RoundToInt(hit.collider.gameObject.transform.position);
        }
        return Vector2Int.up;
    }
    
    void MoveCursor()
    {
        if (lastTilePos != GetMousePosition())
        {
            GameManager.SetGridColor(lastTilePos, originalColor);
            lastTilePos = GetMousePosition();
            originalColor = GameManager.GetGridColor(lastTilePos); 
            GameManager.SetGridColor(lastTilePos, HoverColor);
        }
    }
}
