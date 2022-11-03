using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        IDLE,
        QUEST1_ACTIVE,
        QUEST1_FINISHED,
        QUEST2_ACTIVE,
        QUEST2_FINISHED, 
        GAME_OVER
    }
    public static GameManager Instance;

    [SerializeField] private GameObject questObjects;
    [SerializeField] private GameObject dummy;
    [SerializeField] private GameObject bubbleBackground;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI bubble;
    [SerializeField] private TextMeshProUGUI hints;
    [SerializeField] private GameState m_GameState = 0;

    private GameObject path1;
    private GameObject path2;
    private GameObject axe;
    private GameObject bucket;
    private GameObject collectorBucket;
    private int treeCount = 0;
    private int gameState = 0;
    private bool isActive = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        TreeBehavior.OnFallEvent += () =>
        {
            IncrementTree();
            if(treeCount >= 5)
            {
                m_GameState = GameState.QUEST1_FINISHED;
                bubbleBackground.gameObject.SetActive(false);
                hints.text = "Retournez-voir le mannequin";
            }
        };
        DummyBehavior.OnDummyHitEvent += () =>
        {
            m_GameState = GameState.QUEST1_ACTIVE;
            Quest1();
        };
        CollectorBucketBehavior.OnRequiredApplesEvent += () =>
        {
            m_GameState = GameState.QUEST2_FINISHED;
            bubbleBackground.gameObject.SetActive(false);
            hints.text = "Retournez-voir le mannequin";
        };

        CollectorBucketBehavior.OnNotEnoughApplesEvent += () =>
        {
            Quest2();
            m_GameState = GameState.QUEST2_ACTIVE;
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        path1 = questObjects.transform.GetChild(0).gameObject;
        path2 = questObjects.transform.GetChild(1).gameObject;
        axe = questObjects.transform.GetChild(2).gameObject;
        bucket = questObjects.transform.GetChild(3).gameObject;
        collectorBucket = questObjects.transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Gerer passage quete 2
        switch (m_GameState)
        {
            case GameState.QUEST1_FINISHED:
                if (IsPlayer3m())
                {
                    Quest2();
                    m_GameState = GameState.QUEST2_ACTIVE;
                }
                break;
            //case GameState.QUEST2_ACTIVE:
            //    if (isQuest2Finished())
            //    {
            //        m_GameState = GameState.QUEST2_FINISHED;
            //        bubbleBackground.gameObject.SetActive(false);
            //        hints.text = "Retournez-voir le mannequin";
            //    }
            //    break;
            case GameState.QUEST2_FINISHED:
                if (IsPlayer3m())
                {
                    m_GameState = GameState.GAME_OVER;
                    bubbleBackground.gameObject.SetActive(true);
                    bubble.text = "Merci d'avoir joué !";
                    hints.text = "";
                }
                break;
            default: break;
        }


        /*if (gameState == 1 && isActive == false)
        {
            //print("lancement de la quête");
            Quest1();
        }
        else if(gameState == 1 && isActive == true)
        {
            //print("en cours de la 1ere quête");
            
            if(isQuest1Finished())
            {
                bubbleBackground.gameObject.SetActive(false);
                hints.text = "Retournez-voir le mannequin";
                gameState = 2;
                isActive = false;
            }
        }
        
        if(gameState == 2 && isActive == false)
        {
            //Si le joueur s'appoche à 3 mètres du mannequin
            if(IsPlayer3m())
            {
                //print("lancement quête 2");
                Quest2();
            }
        }
        else if(gameState == 2 && isActive == true)
        {
            //print("en cours de la 2e quête");

            if(isQuest2Finished())
            {
                bubbleBackground.gameObject.SetActive(false);
                hints.text = "Retournez-voir le mannequin";
                gameState = 3;
                isActive = false;
            }
        }

        if(gameState == 3 && isActive == false)
        {
            //Si le joueur s'appoche à 3 mètres du mannequin
            if(IsPlayer3m())
            {
                //print("fin du jeu");
                bubbleBackground.gameObject.SetActive(true);
                bubble.text = "Merci d'avoir joué !";
                hints.text = "";
            }
        }*/
    }

    public int GetGameState()
    {
        return gameState;
    }
    public void SetGameState(int state)
    {
        gameState = state;
    }

    private void IncrementTree()
    {
        treeCount++;
        print(treeCount);
    }

    private bool IsPlayer3m()
    {
        return Vector3.Distance(player.position, dummy.transform.position) < 3.01;
    }

    private bool isPlayer3m()
    {
        //TD: Fonction simplifiée plus haut
        float x, y, z;
        x = player.position.x - dummy.transform.position.x;
        y = player.position.y - dummy.transform.position.y;
        z = player.position.z - dummy.transform.position.z;

        float distance = Mathf.Sqrt((x*x) + (y*y) + (z*z));

        if (distance <= 3.0f)
            return true;
        else
            return false;
    }

    private void Quest1()
    {
        bubbleBackground.gameObject.SetActive(true);
        bubble.text = "Oooooh me tape pas ! Si tu veux vraiment t'amuser avec une hache," +
            " va plutôt prendre celle sur la bûche à côté de moi pur abattre ces arbres plus loin." +
            " Reviens me voir quand tu auras fini.";

        hints.text = "Coupez 5 arbres";

        axe.SetActive(true);
        path1.SetActive(false);

        isActive = true;
    }

    private bool isQuest1Finished()
    {
        if (treeCount == 5)
            return true;
        else
            return false;
    }

    private void Quest2()
    {
        bubbleBackground.gameObject.SetActive(true);
        bubble.text = "Bien joué ! Maintenant, puisque tu es là, pourrais-tu m'aider ?" +
            " Je dois faire de la compote pour mes neveux. Peux-tu aller me cueillir 1Kg de pommes ?" +
            " Tu trouveras un seau à côté de moi pour ramasser des pommes. Tu pourras les verser directement" +
            " dans le seau avec le poids indiqué dessus, dans le verger.";

        hints.text = "Ramassez 1Kg de pommes. Le seau dans le verger vous donnera le poids total de pommes ramassées.";

        bucket.SetActive(true);
        path2.SetActive(false);

        isActive = true;
    }

    private bool isQuest2Finished()
    {
        if (collectorBucket.GetComponentInChildren<CollectorBucketBehavior>().GetMass() == 1.0M)
            return true;
        else
            return false;
    }
}
