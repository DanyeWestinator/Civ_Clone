using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.PriorityQueue;
//using Priority_Queue;

public class Node
{
    public Vector2Int pos;
    public bool known = false;
    public float d = float.MaxValue;
    public Vector2Int previous;

    public HashSet<Edge> edges = new HashSet<Edge>();

    public Node(Vector2Int start)
    {
        pos = start;
        previous = pos;
        foreach (Vector2Int point in Pathfinding.GetNeighbors(pos))
        {
            Edge edge = new Edge(pos, point);
            edges.Add(edge);
        }
    }
    public bool IsStart() { return (pos == previous); }

    public HashSet<Vector2Int> Points()
    {
        HashSet<Vector2Int> to = new HashSet<Vector2Int>();
        foreach(Edge edge in edges)
        {
            to.Add(edge.end);
        }
        return to;
    }
    public bool LeadsTo(Vector2Int to)
    {
        foreach (Edge edge in edges)
        {
            if (edge.end == to)
            {
                return true;
            }
        }
        return false;
    }
}


public class Edge
{
    public Vector2Int start;
    public Vector2Int end;
    public float distance;

    public Edge(Vector2Int from, Vector2Int to)
    {
        end = to;
        start = from;
        distance = (float)Pathfinding.GetDistance(to, from);
    }
}
public enum DistanceMethod
{
    UniversalOne,
    TwoDiagonals,
    ActualDistance
}

public class Pathfinding : MonoBehaviour
{
    private Dictionary<Vector2Int, Node> points = new Dictionary<Vector2Int, Node>();
    //private Dictionary<Vector2Int, Node> knownPaths = new Dictionary<Vector2Int, Node>();

    public DistanceMethod method;
    private static DistanceMethod distanceMethod;

    public int LoopLimit = 1000;

    public Vector2Int testStart = new Vector2Int(2, 3);

    // Start is called before the first frame update
    void Start()
    {
        distanceMethod = method;
        FillMap();
    }

    void Update()
    {
        if (Input.GetKeyDown("t"))
            GetPath(testStart, new Vector2Int(3, 1));
    }

    private void FillMap()
    {
        GameManager.CheckStart();
        points = new Dictionary<Vector2Int, Node>();
        foreach (GameObject go in GameManager.blockByPos.Values)
        {
            Vector2Int blockPos = GameManager.WorldspaceToTilemap(go.transform.position);
            Node node = new Node(blockPos);
            points.Add(blockPos, node);
            //print(blockPos);
        }
        //print("filled a map with " + points.Count + " points");
        
    }

    //gets all the valid neighbors next to a block
    public static HashSet<Vector2Int> GetNeighbors(Vector2Int center, bool excludeWater = true)
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
    public static float GetDistance(Vector2Int to, Vector2Int from)
    {
        //very placeholder, but if you're side to side, the distance is 1
        //otherwise 2
        if (distanceMethod == DistanceMethod.TwoDiagonals)
        {
            if (Vector2.Distance(to, from) == 1f)
                return 1f;
            return 2f;
        }
        if (distanceMethod == DistanceMethod.ActualDistance)
        {
            return Vector2.Distance(to, from);
        }
        if (distanceMethod == DistanceMethod.UniversalOne)
        {
            return 1f;
        }


        return 1f;

    }
    public static bool IsPath(Vector2Int to, Vector2Int from)
    {
        if (GameManager.IsWater(to) || GameManager.IsWater(from))
            return false;
        return true;
    }

    private Node GetNode(Vector2Int pos)
    {
        if (points.ContainsKey(pos))
            return points[pos];

        return null;
    }

    public LinkedList<Vector2Int> GetPath(Vector2Int start, Vector2Int end)
    {
        print("calculating path from: " + start + " to " + end);
        LinkedList<Vector2Int> path = new LinkedList<Vector2Int>();
        PriorityQueue<Node, float> queue = new PriorityQueue<Node, float>(0f);
        FillMap();
        Node first = new Node(start);
        first.d = 0f;
        queue.Insert(first, 0f);
        int loops = 0;
        while (queue.IsEmpty == false && loops < LoopLimit)
        {
            loops++;
            Node current = queue.Pop();
            if (current.known == false)
            {
                current.known = true;
                GetNode(current.pos).known = true;
                foreach (Edge edge in current.edges)
                {
                    Node next = GetNode(edge.end);
                    if (next != null && next.d > current.d + GetDistance(current.pos, next.pos))
                    {
                        next.d = current.d + GetDistance(current.pos, next.pos);
                        GetNode(next.pos).d = current.d + GetDistance(current.pos, next.pos);
                        GetNode(next.pos).previous = current.pos;
                        next.previous = current.pos;
                        queue.Insert(next, next.d);
                    }

                }
            }
        }
        print(loops);
        Vector2Int nextPos = end;
        loops = 0;
        while (nextPos != start && loops < 10)
        {
            loops++;
            print(nextPos);
            path.AddFirst(nextPos);
            nextPos = points[nextPos].previous;
        }

        return path;
    }
}