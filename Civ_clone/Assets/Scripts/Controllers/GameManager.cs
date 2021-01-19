using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Dictionary<Vector3Int, GameObject> blockByPos = new Dictionary<Vector3Int, GameObject>();
    [SerializeField]
    public GameObject MapHolder;
    // Start is called before the first frame update
    void Start()
    {
        PopulateBlockByPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateBlockByPos()
    {
        blockByPos = new Dictionary<Vector3Int, GameObject>();
        foreach (GameObject go in GetChildren(MapHolder))
        {
            Vector3Int pos = Vector3Int.RoundToInt(go.transform.position);
            if (blockByPos.ContainsKey(pos) == false)
                blockByPos.Add(pos, go);
        }
        print("Counted: " + blockByPos.Count + " and added them to the count");
    }

    private HashSet<GameObject> GetChildren(GameObject go)
    {
        HashSet<GameObject> toReturn = new HashSet<GameObject>();
        toReturn.Add(go);
        if (go.transform.childCount == 0)
            return toReturn;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            toReturn.UnionWith(GetChildren(go.transform.GetChild(i).gameObject));
        }


        return toReturn;
    }
    public static GameObject GetBlockByPos(Vector3Int pos)
    {
        if (blockByPos.ContainsKey(pos))
            return blockByPos[pos];
        return null;
    }
}
