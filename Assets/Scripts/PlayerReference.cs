using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    #region Singelton

    public static PlayerReference instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;

}
