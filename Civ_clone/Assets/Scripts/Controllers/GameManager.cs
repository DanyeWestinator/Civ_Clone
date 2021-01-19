using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private static Dictionary<Vector3Int, GameObject> blockByPos = new Dictionary<Vector3Int, GameObject>();
    [SerializeField]
    public GameObject MapHolder;
    public static Tilemap grid;
    public GameObject gridParent;
    // Start is called before the first frame update
    void Start()
    {
        grid = gridParent.transform.GetChild(0).GetComponent<Tilemap>();
        PopulateBlockByPos();
        SetAllTileFlags();
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
        //print("Counted: " + blockByPos.Count + " and added them to the count");
    }

    private HashSet<GameObject> GetChildren(GameObject go)
    {
        HashSet<GameObject> toReturn = new HashSet<GameObject>();
        if (go != MapHolder)
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
    public static string GetTagByPos(Vector3Int pos)
    {
        if (pos.y != 0 && pos.z == 0)
        {
            pos.z = pos.y * -1;
            pos.y = 0;
        }
        if (blockByPos.ContainsKey(pos))
            return blockByPos[pos].tag;
        return "";
    }
    private void SetAllTileFlags(TileFlags flag = TileFlags.None)
    {
        foreach (Vector3Int tile in GetAllTiles())
        {
            grid.SetTileFlags(tile, flag);
        }
    }
    //gets all the tiles in a tilemap
    public static HashSet<Vector3Int> GetAllTiles(Tilemap tiles = null)
    {
        if (tiles == null)
            tiles = grid;
        //gets all the tiles in a given tilemap
        HashSet<Vector3Int> filledTiles = new HashSet<Vector3Int>();
        for (int x = tiles.cellBounds.xMin; x < tiles.cellBounds.xMax; x++)
        {
            for (int y = tiles.cellBounds.yMin; y < tiles.cellBounds.yMax; y++)
            {
                if (tiles.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    filledTiles.Add(new Vector3Int(x, y, 0));
                }
            }
        }
        return filledTiles;
    }
    public static Vector3Int WorldspaceToTilemap(Vector3 pos)
    {
        Vector3Int tilePos = Vector3Int.RoundToInt(pos);
        tilePos.y = tilePos.z * -1;
        tilePos.z = 0;

        return tilePos;
    }
}
