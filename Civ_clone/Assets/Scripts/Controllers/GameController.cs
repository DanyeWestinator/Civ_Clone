using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector3Int lastPos = Vector3Int.zero;
    private Color originalColor = Color.white;
    public Color HoverColor = Color.grey;
    Vector3Int lastTilePos = Vector3Int.zero;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Clicked();
        MoveCursor();
        lastPos = GetMousePosition();
        lastTilePos = GetTilePosition();
    }
    void Clicked()
    {
        //print("clicked");
        
        GameManager.GetBlockByPos(GetMousePosition()).SetActive(false);
    }
    private Vector3Int GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            return Vector3Int.RoundToInt(hit.collider.gameObject.transform.position);
        }
        return Vector3Int.up;
    }
    private Vector3Int GetTilePosition()
    {
        Vector3Int tilePos = GetMousePosition();
        tilePos.y = tilePos.z * -1;
        tilePos.z = 0;
        return tilePos;
    }
    void MoveCursor()
    {
        if (lastTilePos != GetTilePosition())
        {
            GameManager.grid.SetColor(lastTilePos, originalColor);
            lastTilePos = GetTilePosition();
            originalColor = GameManager.grid.GetColor(lastTilePos);
            GameManager.grid.SetColor(lastTilePos, HoverColor);
        }
    }
}
