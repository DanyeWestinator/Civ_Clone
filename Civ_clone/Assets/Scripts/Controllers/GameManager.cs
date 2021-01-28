using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static Dictionary<Vector2Int, GameObject> blockByPos = new Dictionary<Vector2Int, GameObject>();
    [SerializeField]
    public GameObject MapHolder;
    public static Tilemap grid;
    public GameObject gridParent;
    public static GameManager instance = null;
    private static bool started = false;

    private void Awake()
    {
        if (instance == null || instance == this)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    public void Start()
    {
        grid = gridParent.transform.GetChild(0).GetComponent<Tilemap>();
        PopulateBlockByPos();
        SetAllTileFlags();
        started = true;
    }
    public static void CheckStart()
    {
        if (instance == null || started == false)
        {
            instance = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        instance.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PopulateBlockByPos()
    {
        blockByPos = new Dictionary<Vector2Int, GameObject>();
        foreach (GameObject go in GetChildren(MapHolder))
        {
            Vector2Int pos = new Vector2Int((int)go.transform.position.x, (int)go.transform.position.z * -1);
            if (blockByPos.ContainsKey(pos) == false)
                blockByPos.Add(pos, go);
        }
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
    public static GameObject GetBlockByPos(Vector2Int pos)
    {
        if (blockByPos.ContainsKey(pos))
            return blockByPos[pos];
        return null;
    }
    public static string GetTagByPos(Vector2Int pos)
    {
        if (blockByPos.ContainsKey(pos))
            return blockByPos[pos].tag;
        return "";
    }
    private void SetAllTileFlags(TileFlags flag = TileFlags.None)
    {
        foreach (Vector2Int tile in GetAllTiles())
        {
            grid.SetTileFlags((Vector3Int)tile, flag);
        }
    }
    //gets all the tiles in a tilemap
    public static HashSet<Vector2Int> GetAllTiles(Tilemap tiles = null)
    {
        if (tiles == null)
            tiles = grid;
        //gets all the tiles in a given tilemap
        HashSet<Vector2Int> filledTiles = new HashSet<Vector2Int>();
        for (int x = tiles.cellBounds.xMin; x < tiles.cellBounds.xMax; x++)
        {
            for (int y = tiles.cellBounds.yMin; y < tiles.cellBounds.yMax; y++)
            {
                if (tiles.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    filledTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        return filledTiles;
    }
    public static Vector2Int WorldspaceToTilemap(Vector3 pos)
    {
        Vector2Int tilePos = new Vector2Int((int)pos.x, -1 * (int)pos.z);
        

        return tilePos;
    }
    public static Vector3 TileToWorldSpace(Vector2Int pos)
    {
        Vector3 worldPos = new Vector3(pos.x, 0f, pos.y * -1f);
        return worldPos;
    }
    public static void SetGridColor(Vector2Int pos, Color color)
    {
        Vector3Int newPos = new Vector3Int(pos.x, pos.y, 0);
        grid.SetColor(newPos, color);
    }
    public static Color GetGridColor(Vector2Int pos) { return grid.GetColor(new Vector3Int(pos.x, pos.y, 0)); }
    public static bool TagContains(Vector2Int pos, string contains)
    {
        string s = "NULL";
        if (blockByPos.ContainsKey(pos))
            s = blockByPos[pos].tag;
        s = s.ToLower();
        contains = contains.ToLower();
        return s.Contains(contains);
    }
    public static bool IsWater(Vector2Int pos) { return TagContains(pos, "water"); }
}
