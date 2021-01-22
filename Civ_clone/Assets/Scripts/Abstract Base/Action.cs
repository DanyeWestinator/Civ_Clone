using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    private HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> MarkedValid = new HashSet<Vector2Int>();
    public Color markedColor;
    public int MaxDistance = 5;
    public int recursiveCalls = 0;
    public Vector2Int position;

    // Start is called before the first frame update
    void Start()
    {
        position = GameManager.WorldspaceToTilemap(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //MarkedValid = GetValid(GameManager.WorldspaceToTilemap(transform.position));
            MarkValid();
        }
    }
    private HashSet<Vector2Int> RecursiveFinder(Vector2Int center, int depth)
    {
        recursiveCalls++;
        HashSet<Vector2Int> toReturn = new HashSet<Vector2Int>();
        if (depth <= 0)
            return toReturn;
        toReturn = GetValid(center);
        
        
        foreach (Vector2Int next in GetValid(center))
        {
            toReturn.UnionWith(RecursiveFinder(next, depth - Pathfinding.GetDistance(next, center)));
        }

        visited.Add(center);
        return toReturn;
    }

    private HashSet<Vector2Int> GetValid(Vector2Int center)
    {
        HashSet<Vector2Int> valid = new HashSet<Vector2Int>();
        if (IsValidTile(center) == false)
            return valid;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int next = new Vector2Int(center.x + i, center.y + j);
                if (next != center && IsValidTile(next))
                {
                    //print(GameManager.GetTagByPos(next) + " " + next);
                    valid.Add(next);
                }
                    
            }
        }
        return valid;
    }

    public void MarkValid()
    {
        //MarkedValid = GetValid(GameManager.WorldspaceToTilemap(transform.position));
        MarkedValid = RecursiveFinder(position, MaxDistance);
        visited = new HashSet<Vector2Int>();
        foreach (Vector3Int tile in MarkedValid)
        {
            GameManager.grid.SetColor(tile, markedColor);
        }
    }

    private bool IsValidTile(Vector2Int tile)
    {
        if (GameManager.TagContains(tile, "Water"))
            return false;
        if (visited.Contains(tile))
            return false;
        if (GameManager.TagContains(tile, "null"))
            return false;
        return true;
    }
}
