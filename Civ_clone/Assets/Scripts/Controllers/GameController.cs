using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector3Int lastPos = Vector3Int.zero;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Clicked();
        lastPos = GetMousePosition();
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
}
