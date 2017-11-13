[System.Serializable]
public class Wave
{
    public enum ObjectiveType
    {
        KillAll = 0,
        Protect = 1,
        Survive = 3,
    }

    public enum EnemyType
    {
        Asteroid = 0,
        Droid = 1,
    }

    public ObjectiveType objective;
    public EnemyType enemyType;
    public int enemyCount = 1;
    public float difficulty = 0.0f;
    public float duration = -1;
    public int objectiveRequiredValue = 0;

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

    private float overrideDifficulty = 0;


    public string GetObjectiveString()
    {
        switch (objective)
        {
            case ObjectiveType.Protect:
                return "Protect " + objectiveRequiredValue + " bases";
            case ObjectiveType.KillAll:
                return "Destroy " + enemyCount + " " + enemyType + (enemyCount > 1 ? "s" : "");
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