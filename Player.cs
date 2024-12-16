using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // Vitesse de mouvement du joueur
    public float speed = 1.5f;
    private Rigidbody2D rigidBody2D;
    private Vector2 movement;
    private PlayerData playerData;
    private bool _isGameOver;

    // Question 6 : Mode IA - Attributs pour activer le mode IA
    private bool isAIEnabled = true; // Active le mode IA par défaut
    private List<Vector2> path; // Chemin de Djikstra
    private int currentPathIndex; 
    private float pathRecalculationTime = 1f; // Fréquence de recalcul du chemin
    private float lastPathCalculationTime; // Dernière fois où le chemin a été calculé
    private float stuckCheckTime = 3.5f; // Fréquence de détection de chemin bloqué
    private float lastStuckCheckTime; // Dernière détection de chemin bloqué
    private Vector2 lastPosition; // Dernière position du joueur
    private int stuckCounter = 0; // Nombre de fois où le chemin est bloqué
    private float backtrackDistance = 2f; // Distance de recul pour essayer un nouveau chemin

    // Question 4 : Gestion de l'événement de fin du jeu
    public static event System.Action OnGameOver;

    //Question5 sur la sauvegarde du score
    private string connectionString = "your-mongodb-atlas-connection-string"; // Remplacez par votre chaîne de connexion
    private string databaseName = "game_database";
    private string collectionName = "progression";

    void Start()
    {
        _isGameOver = false;
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerData = new PlayerData();
        playerData.plummie_tag = "nraboy";
        path = new List<Vector2>();
        lastPosition = transform.position;

        // Question 4 : Gestion de l'événement
        if (OnGameOver != null)
            OnGameOver += HandleGameOver; // Abonnement à l'événement
    }

    void OnDestroy()
    {
        if (OnGameOver != null)
            OnGameOver -= HandleGameOver; // Désabonnement à l'événement
    }

    // Gestion de l'événement de fin de jeu
    private void HandleGameOver()
    {
        _isGameOver = true;
        Debug.Log("Game Over! The game will restart.");
        // Ici, vous pouvez ajouter un délai ou des effets avant le redémarrage
        RestartGame();
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Méthode qui calcule le chemin à prendre en Mode AI
    void CalculatePath()
    {
        if (Time.time - lastPathCalculationTime < pathRecalculationTime)
        return;

        lastPathCalculationTime = Time.time;

        // Position actuelle du joueur
        Vector2 startPos = transform.position;

        // Cible avec un décalage vertical pour éviter les obstacles
        float verticalOffset = Random.Range(-2f, 2f);
        Vector2 targetPos = new Vector2(25f, transform.position.y + verticalOffset);

        // Calcul du chemin avec l'algorithme de Djikstra
        path = Dijkstra.FindPath(startPos, targetPos, stuckCounter);
        currentPathIndex = 0;
        stuckCounter = 0; // Réinitialise le compteur de blocage
    
    }

    void CheckIfStuck()
    {
        if (Time.time - lastStuckCheckTime < stuckCheckTime)
        return;

       // Vérifie si le joueur a parcouru une distance suffisante
       float distanceMoved = Vector2.Distance(transform.position, lastPosition);
       if (distanceMoved < 0.1f && !_isGameOver)
    {
        stuckCounter++;
        if (stuckCounter > 3) // Si le joueur est bloqué trop longtemps
        {
            BacktrackAndFindNewPath();
        }
    }
        else
    {
        stuckCounter = 0; // Réinitialise le compteur si le joueur bouge
    }

        lastPosition = transform.position;
        lastStuckCheckTime = Time.time;
    
    }

        void BacktrackAndFindNewPath()
    {
        // Recule le joueur pour essayer un nouveau chemin
        Vector2 backtrackPosition = transform.position;
        backtrackPosition.x -= backtrackDistance;
        transform.position = backtrackPosition;

        // Efface le chemin actuel et force un recalcul
        path.Clear();
        currentPathIndex = 0;
        lastPathCalculationTime = 0f; // Force un recalcul immédiat
        CalculatePath();
    }

    void Update()
    {
       if (!_isGameOver)
    {
        if (isAIEnabled)
        {
            // Si le mode IA est activé, gérer les déplacements automatiques
            UpdateAIMovement();
            CheckIfStuck(); // Vérifie si le joueur est bloqué
        }
        else
        {
            // Sinon, déplacements manuels contrôlés par le joueur
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            movement = new Vector2(h * speed, v * speed);
        }
    }
    
    }

    void UpdateAIMovement()
    {
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count)
    {
        CalculatePath(); // Recalcule un chemin si le précédent est terminé ou inexistant
        return;
    }

    // Cible actuelle sur le chemin
    Vector2 currentTarget = path[currentPathIndex];
    Vector2 currentPosition = transform.position;
    Vector2 direction = (currentTarget - currentPosition);
    float distance = direction.magnitude;

    // Si le joueur atteint la cible actuelle, passer à la suivante
    if (distance < 0.1f)
    {
        currentPathIndex++;
        if (currentPathIndex >= path.Count)
        {
            CalculatePath(); // Recalcule le chemin si toutes les cibles sont atteintes
            return;
        }
    }

    // Déplacement vers la cible actuelle
    movement = direction.normalized * speed;
    }

    void FixedUpdate()
    {
        if (!_isGameOver)
        {
            Vector2 newPosition = rigidBody2D.position + movement * Time.fixedDeltaTime;
            rigidBody2D.MovePosition(newPosition);

            // Détection de fin du jeu : ligne d'arrivée ou énergie à 0
            if (rigidBody2D.position.x > 24.0f)
            {
                _isGameOver = true;
                Debug.Log("Reached the finish line!");
                ScoreManager.Instance.DisplayScore();
                OnGameOver?.Invoke(); // Déclenche l'événement
            }
        }
    }

    //Question5 sur la sauvegarde score
    async void SaveProgressionAsync(int finalScore)
    {
        try
        {
            // Connexion à la base de données MongoDB Atlas
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            // Création du document JSON pour la sauvegarde
            var progressionDocument = new BsonDocument
            {
                { "game", "NomPrenomEtudiant_fall_guy" }, // Remplacez par votre nom et prénom
                { "score", finalScore },
                { "timestamp", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
            };

            // Sauvegarde asynchrone avec await
            await collection.InsertOneAsync(progressionDocument);
            Debug.Log("Progression saved successfully in MongoDB.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save progression: {ex.Message}");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        playerData.collisions++;
        ScoreManager.Instance.RegisterCollision();

        if (collision.gameObject.CompareTag("Wall"))
        {
            ScoreManager.Instance.RegisterCollision();
        }

        if (ScoreManager.Instance.GetCurrentEnergy() <= 0)
        {
            _isGameOver = true;
            Debug.Log("Energy depleted!");
            OnGameOver?.Invoke(); // Déclenche l'événement
        }
    }

    void OnDestroy()
    {
        if (gameObject.CompareTag("Wall"))
        {
            ScoreManager.Instance.RegisterWallDestroyed();
        }
    }

    void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }
}
