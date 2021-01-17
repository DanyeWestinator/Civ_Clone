using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    private static Dictionary<string, HashSet<string>> keys = new Dictionary<string, HashSet<string>>();
    // Start is called before the first frame update
    void Start()
    {
        LoadKeys();
        foreach(string key in keys.Keys)
        {
            string n = "";
            foreach (string s in keys[key])
            {
                n += s + ", ";
            }
            print(key + " : " + n);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadKeys()
    {
        string rawKeysTxt = Resources.Load<TextAsset>("CurrentInput").text;
        foreach (string l in rawKeysTxt.Split('\n'))
        {
            string line = l.Replace(' '.ToString(), "").Trim();
            if (line.StartsWith("#") == false && line != string.Empty)
            {
                string key = line.Split(':')[0];
                keys[key] = new HashSet<string>();
                foreach (string input in line.Split(':')[1].Split(','))
                {
                    keys[key].Add(input);
                }
            }
                
        }
    }


    public static bool GetKey(string key)
    {
        foreach (string s in keys[key])
        {
            if (Input.GetKey(s))
                return true;
        }
        return false;
    }
    public static bool GetKeyDown(string key)
    {
        foreach (string s in keys[key])
        {
            if (Input.GetKeyDown(s))
                return true;
        }
        return false;
    }
}
