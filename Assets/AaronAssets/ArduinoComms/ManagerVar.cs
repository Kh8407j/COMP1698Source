using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerVar : MonoBehaviour
{
    public static ManagerVar Instance;
    public bool shouldSave = false;
    public string lastTagID, barValue;
    public int jumpState;
    public int fireState;
    public int power;
    public int enemyDamage;
    public int nutritionGrade;
    public int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveBool()
    {
        shouldSave = true;
    }
}