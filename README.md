# Examen-Final-concepts-et-programamtion-Plummet-Game
*par Mouad Labed*

Description du jeu

**Plummet Game est un jeu de plateforme dynamique où le joueur doit atteindre la ligne d'arrivée en évitant ou en détruisant des obstacles. Le joueur contrôle un personnage qui avance sur un tableau et rencontre des murs qui peuvent être détruits par collision. Chaque collision réduit la barre d'énergie du joueur. À la fin du niveau, un score est calculé en fonction du nombre de collisions, de la barre d'énergie restante et du nombre de murs encore en place. Le jeu propose aussi un mode AI, où l'algorithme de Dijkstra guide automatiquement le personnage à travers les obstacles.**

Fonctionnalités principales éviter les obstacles.

- Mode AI basé sur l'algorithme de Dijkstra pour calculer et suivre des chemins optimaux.
- Gestion des collisions avec réduction de l'énergie.
- Score final basé sur les performances du joueur.

## Hiérarchie des Classes

| Classe principale | Description |
|-------------------|-------------|
| **Player**        | Gère le mouvement du joueur, soit manuellement via les entrées clavier, soit automatiquement en mode AI. Détecte les collisions avec les murs, réduit la barre d'énergie et calcule le score final en fonction des performances. |
| **Dijkstra**      | Implémente l'algorithme de Dijkstra pour calculer les chemins optimaux et gérer les cas où le joueur est bloqué en recalculant un nouveau chemin. |
| **CameraPosition**| Synchronise la position de la caméra avec celle du joueur pour garantir une vue dynamique. |
| **Door**          | Représente les obstacles (portes) dans le jeu. Permet de configurer les portes comme étant "réelles" (dynamiques) ou "fausses" (statiques). |
| **DoorManager**   | Initialise et gère toutes les portes d'un niveau. Définit aléatoirement quelles portes sont fausses. |
| **PlayerData**    | Stocke les informations relatives au joueur (collisions, énergie restante, etc.). Sauvegarde et charge les données du joueur. |

### Système de score avec affichage en temps réel
Un système de score a été ajouté pour suivre les progrès du joueur en fonction de l’énergie restante, du nombre de collisions et du nombre de murs détruits. Ce système affiche les informations sur le score en temps réel dans l'interface utilisateur du jeu.

#### Détails de fonctionnement :
- Le score est calculé en fonction de l'énergie restante, des collisions subies et des murs détruits.
- L'affichage est mis à jour automatiquement après chaque collision ou destruction de mur.
- Le score est affiché en haut de l'écran à l'aide d'un élément de texte UI.

### Gestion de la fin du jeu avec événements
Un système d'événements a été mis en place pour gérer la fin du jeu. L'événement `OnGameOver` est déclenché dans deux cas :
1. Lorsque l'énergie du joueur atteint 0.
2. Lorsque le joueur franchit la ligne d'arrivée.

L'événement déclenche la fonction `RestartGame` qui recharge la scène pour recommencer le jeu. La fonction `PlayerCrossedFlags` est utilisée pour détecter si la ligne d'arrivée a été franchie. Les événements sont abonnés et désabonnés dans le script `GameManager` avec les syntaxes `+=` et `-=` pour assurer une gestion propre des événements.
