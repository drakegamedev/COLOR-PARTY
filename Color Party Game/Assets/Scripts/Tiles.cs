using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    private GameObject playerStep;

    public GameObject GetPlayerStep()
    {
        return playerStep;
    }

    public void SetPlayerStep(GameObject go)
    {
        playerStep = go;
    }
}
