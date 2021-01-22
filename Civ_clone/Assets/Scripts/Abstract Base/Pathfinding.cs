using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2Int pos;
    public bool known = false;
    public int d = int.MaxValue;

    

    private void Start()
    {
    }

    

    
}

public class Pathfinding : MonoBehaviour
{
    private Node FirstNode = new Node();
    private static Dictionary<Vector2Int, Dictionary<Vector2Int, int>> map = new Dictionary<Vector2Int, Dictionary<Vector2Int, int>>();

    //the Dict keys are K/V pairs, where the Key is a vector2int from, and the value is a vector2int to
    //the Dict values are linked lists of the shortest path nodes
    private static Dictionary<KeyValuePair<Vector2Int, Vector2Int>, LinkedList<Vector2Int>> knownPaths = new Dictionary<KeyValuePair<Vector2Int, Vector2Int>, LinkedList<Vector2Int>>();

    // Start is called before the first frame update
    void Start()
    {
        FillMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FillMap()
    {
        gameObject.GetComponent<GameManager>().Start();
        foreach (GameObject go in GameManager.blockByPos.Values)
        {
            Vector2Int blockPos = GameManager.WorldspaceToTilemap(go.transform.position);
            Dictionary<Vector2Int, int> paths = new Dictionary<Vector2Int, int>();
            foreach (Vector2Int next in GetNeighbors(blockPos))
            {
                paths.Add(next, GetDistance(blockPos, next));
            }
            map.Add(blockPos, paths);
            
        }
    }
    //gets all the valid neighbors next to a block
    private HashSet<Vector2Int> GetNeighbors(Vector2Int center, bool excludeWater = true)
    {
        HashSet<Vector2Int> valid = new HashSet<Vector2Int>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int next = new Vector2Int(center.x + i, center.y + j);
                if (IsPath(center, next))
                {
                    if (excludeWater && GameManager.IsWater(next) == false)
                        valid.Add(next);
                    if (excludeWater == false)
                        valid.Add(next);
                }

            }
        }
        return valid;
    }
    public static int GetDistance(Vector2Int to, Vector2Int from)
    {
        //very placeholder, but if you're side to side, the distance is 1
        //otherwise 2
        if (Vector2Int.Distance(to, from) == 1)
            return 1;
        return 2;
    }
    public static bool IsPath(Vector2Int to, Vector2Int from)
    {
        if (GameManager.IsWater(to) || GameManager.IsWater(from))
            return false;
        return true;
    }

    public static LinkedList<Vector2Int> GetShortestPath(Vector2Int to, Vector2 from)
    {
        LinkedList<Vector2Int> path = new LinkedList<Vector2Int>();


        return path;

    }
}
