//la classe singleton ScoreManager Question2
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int initialEnergy = 1000; // Barre d’énergie initiale
    private int currentEnergy;
    private int collisions;
    private int wallsDestroyed;
    private int totalWalls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre les scènes si nécessaire
        }
    }

    public void Initialize(int totalWalls)
    {
        this.totalWalls = totalWalls;
        this.currentEnergy = initialEnergy;
        this.collisions = 0;
        this.wallsDestroyed = 0;
    }

    public void RegisterCollision()
    {
        collisions++;
        currentEnergy -= 100; // Diminue l'énergie par collision
        if (currentEnergy < 0) currentEnergy = 0;
    }

    public void RegisterWallDestroyed()
    {
        wallsDestroyed++;
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public int GetCollisions()
    {
        return collisions;
    }

    public int GetRemainingWalls()
    {
        return totalWalls - wallsDestroyed;
    }

    public int CalculateFinalScore()
    {
        // Exemple : score sur l'energie restant, les murs détruits et les collisions
        return (currentEnergy * 2) - (collisions * 10) + (GetRemainingWalls() * 5);
    }

    public void DisplayScore()
    {
        Debug.Log($"Energy: {currentEnergy}");
        Debug.Log($"Collisions: {collisions}");
        Debug.Log($"Walls Remaining: {GetRemainingWalls()}");
        Debug.Log($"Final Score: {CalculateFinalScore()}");
    }
}
