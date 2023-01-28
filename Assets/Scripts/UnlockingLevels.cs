using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingLevels : MonoBehaviour
{
    private bool startedUp = false;
    [SerializeField] private bool[] levelsUnlocked = new bool[10];

    private void Awake()
    {
        if (!startedUp)
        {
            for (int i = 0; i < levelsUnlocked.Length; i++)
            {
                levelsUnlocked[i] = false;
            }
            startedUp = true;
        }
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UnlockLevel(int levelIndex)
    {
        levelsUnlocked[levelIndex - 1] = true;
    }
}
