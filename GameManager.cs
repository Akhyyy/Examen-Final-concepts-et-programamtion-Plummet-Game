using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Déclaration de l'événement de fin de jeu
    public static event System.Action OnGameOver;

    private void OnEnable()
    {
        // Abonnement à l'événement OnGameOver pour appeler RestartGame
        OnGameOver += RestartGame;
    }

    private void OnDisable()
    {
        // Désabonnement de l'événement OnGameOver
        OnGameOver -= RestartGame;
    }

    private void RestartGame()
    {
        // Fonction qui redémarre le jeu (recharge la scène actuelle)
        Debug.Log("Game Over! Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fonction pour détecter la fin du jeu
    public void CheckGameOver(int currentEnergy)
    {
        if (currentEnergy <= 0 || PlayerCrossedFlags()) // Vérifie si la ligne d'arrivée est franchie
        {
            OnGameOver?.Invoke();  // Déclenche l'événement de fin de jeu
        }
    }

    
    private bool PlayerCrossedFlags()
    {
        //détectez si le joueur a franchi la ligne d'arrivée
        return false; 
    }
}

