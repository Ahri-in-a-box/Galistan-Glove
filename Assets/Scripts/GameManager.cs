using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject questObjects;
    [SerializeField] private GameObject dummy;
    [SerializeField] private GameObject bubbleBackground;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI bubble;
    [SerializeField] private TextMeshProUGUI hints;

    private GameObject path1;
    private GameObject path2;
    private GameObject axe;
    private GameObject bucket;
    private GameObject collectorBucket;
    private GameObject tree1;
    private GameObject tree2;
    private GameObject tree3;
    private GameObject tree4;
    private GameObject tree5;
    private int gameState = 0;
    private int treeCount = 0;
    private bool isActive = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        TreeBehavior.OnFallEvent += IncrementTree; 
    }

    // Start is called before the first frame update
    void Start()
    {
        path1 = questObjects.transform.GetChild(0).gameObject;
        path2 = questObjects.transform.GetChild(1).gameObject;
        axe = questObjects.transform.GetChild(2).gameObject;
        bucket = questObjects.transform.GetChild(3).gameObject;
        collectorBucket = questObjects.transform.GetChild(4).gameObject;
        tree1 = questObjects.transform.GetChild(5).gameObject;
        tree2 = questObjects.transform.GetChild(6).gameObject;
        tree3 = questObjects.transform.GetChild(7).gameObject;
        tree4 = questObjects.transform.GetChild(8).gameObject;
        tree5 = questObjects.transform.GetChild(9).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == 1 && isActive == false)
        {
            print("lancement de la qu�te");
            Quest1();
        }
        else if(gameState == 1 && isActive == true)
        {
            print("en cours de la 1ere qu�te");
            
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
            //Si le joueur s'appoche � 3 m�tres du mannequin
            if(isPlayer3m())
            {
                print("lancement qu�te 2");
                Quest2();
            }
        }
        else if(gameState == 2 && isActive == true)
        {
            print("en cours de la 2e qu�te");

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
            //Si le joueur s'appoche � 3 m�tres du mannequin
            if(isPlayer3m())
            {
                print("fin du jeu");
                bubbleBackground.gameObject.SetActive(true);
                bubble.text = "Merci d'avoir jou� !";
                hints.text = "";
            }
        }
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

    private bool isPlayer3m()
    {
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
            " va plut�t prendre celle sur la b�che � c�t� de moi pur abattre ces arbres plus loin." +
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
        bubble.text = "Bien jou� ! Maintenant, puisque tu es l�, pourrais-tu m'aider ?" +
            " Je dois faire de la compote pour mes neveux. Peux-tu aller me cueillir 1Kg de pommes ?" +
            " Tu trouveras un seau � c�t� de moi pour ramasser des pommes. Tu pourras les verser directement" +
            " dans le seau avec le poids indiqu� dessus.";

        hints.text = "Ramassez 1Kg de pommes. Le seau dans le jardin vous donnera le poids total de pommes ramass�es.";

        bucket.SetActive(true);
        path2.SetActive(false);

        isActive = true;
    }

    private bool isQuest2Finished()
    {
        if (collectorBucket.GetComponentInChildren<BucketBehavior>().GetMass() == 1.0M)
            return true;
        else
            return false;
    }
}
