using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // D�claration de l'�v�nement de fin de jeu
    public static event System.Action OnGameOver;

    private void OnEnable()
    {
        // Abonnement � l'�v�nement OnGameOver pour appeler RestartGame
        OnGameOver += RestartGame;
    }

    private void OnDisable()
    {
        // D�sabonnement de l'�v�nement OnGameOver
        OnGameOver -= RestartGame;
    }

    private void RestartGame()
    {
        // Fonction qui red�marre le jeu (recharge la sc�ne actuelle)
        Debug.Log("Game Over! Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fonction pour d�tecter la fin du jeu
    public void CheckGameOver(int currentEnergy)
    {
        if (currentEnergy <= 0 || PlayerCrossedFlags()) // V�rifie si la ligne d'arriv�e est franchie
        {
            OnGameOver?.Invoke();  // D�clenche l'�v�nement de fin de jeu
        }
    }

    
    private bool PlayerCrossedFlags()
    {
        //d�tectez si le joueur a franchi la ligne d'arriv�e
        return false; 
    }
}

