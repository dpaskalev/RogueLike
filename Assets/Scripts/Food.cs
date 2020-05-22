using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IReturnValue
{
    [SerializeField]
    private int valuePoints;

    public int ValuePoints
    {
        get { return valuePoints; }
        private set { valuePoints = value; }
    }

    [SerializeField]
    private AudioClip sound1;

    public AudioClip Sound1
    {
        get { return sound1; }
        private set { sound1 = value; }
    }

    [SerializeField]
    private AudioClip sound2;

    public AudioClip Sound2
    {
        get { return sound2; }
        private set { sound2 = value; }
    }
}
