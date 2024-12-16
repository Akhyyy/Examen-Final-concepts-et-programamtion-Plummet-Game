using UnityEngine;
using UnityEngine.UI;  // Ajoutez cette ligne pour utiliser l'UI

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int initialEnergy = 1000; // Barre d’énergie initiale
    private int currentEnergy;
    private int collisions;
    private int wallsDestroyed;
    private int totalWalls;

    public Text scoreText; // Référence au texte UI pour afficher le score

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
        UpdateScoreDisplay(); // Met à jour l'affichage du score
    }

    public void RegisterCollision()
    {
        collisions++;
        currentEnergy -= 100; // Diminue l'énergie par collision
        if (currentEnergy < 0) currentEnergy = 0;
        UpdateScoreDisplay(); // Met à jour l'affichage du score après la collision
    }

    public void RegisterWallDestroyed()
    {
        wallsDestroyed++;
        UpdateScoreDisplay(); // Met à jour l'affichage du score après la destruction d'un mur
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

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            // Affiche les informations mises à jour sur l'UI
            scoreText.text = $"Energy: {currentEnergy}\n" +
                             $"Collisions: {collisions}\n" +
                             $"Walls Remaining: {GetRemainingWalls()}\n" +
                             $"Final Score: {CalculateFinalScore()}";
        }
    }
}
