using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class persistentDataPath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(Application.persistentDataPath + "/" + "PlayerData" + ".json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
