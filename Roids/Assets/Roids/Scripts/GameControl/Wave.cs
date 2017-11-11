using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public enum ObjectiveType
    {
        KillAll,
        Protect,
        Rebuild,
        Survive
    }

    public enum EnemyType
    {
        Asteroid = 0,
        Shuttle = 1,
    }

    public float Difficulty
    {
        get
        {
            return overrideDifficulty > 0 ? overrideDifficulty : difficulty;
        }
    }

    public bool IsTimeBased
    {
        get
        {
            return duration > 0;
        }
    }

    public ObjectiveType objective;
    public EnemyType enemyType;
    public int enemyCount = 1;
    public float difficulty = 0.0f;
    public float duration = -1;
    public int objectiveRequiredValue = 0;
    private float overrideDifficulty = 0;

    public string GetObjectiveString()
    {
        switch (objective)
        {
            case ObjectiveType.Protect:
                return "Protect " + objectiveRequiredValue + " bases";
            case ObjectiveType.KillAll:
                return "Destroy " + enemyCount + " " + enemyType + (enemyCount > 0 ? "s" : "");
            case ObjectiveType.Survive:
                return "Survive for " + duration + " seconds";
            default:
                return "";
        }
    }

    public void SetOverrideDifficulty(float amount)
    {
        overrideDifficulty = amount;
    }
}