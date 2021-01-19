using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> MarkedValid = new HashSet<Vector3Int>();
    public Color markedColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MarkedValid = GetValid(GameManager.WorldspaceToTilemap(transform.position));
            MarkValid();
        }
    }

    private HashSet<Vector3Int> GetValid(Vector3Int center)
    {
        //print(center);
        HashSet<Vector3Int> valid = new HashSet<Vector3Int>();
        if (IsValidTile(center) == false)
            return valid;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3Int next = new Vector3Int(center.x + i, center.y + j, 0);
                //print(next);
                if (next != center && IsValidTile(next))
                    valid.Add(next);
            }
        }
        return valid;
    }

    public void MarkValid()
    {
        MarkedValid = GetValid(GameManager.WorldspaceToTilemap(transform.position));
        foreach (Vector3Int tile in MarkedValid)
        {
            GameManager.grid.SetColor(tile, markedColor);
        }
    }

    private bool IsValidTile(Vector3Int tile)
    {
        print(GameManager.GetTagByPos(tile) + " " + tile);
        return true;
    }
}
