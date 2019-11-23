using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        Wait,
        TakeAction,
        PerformAction,
        CheckAlive,
        Win,
        Lose

    }
    public PerformAction BattleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject>();

    public GameObject enemyButton;
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    public GameObject MagicPanel;

    // attacks of heroes
    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject magicButton;
    private List<GameObject> atkBtns = new List<GameObject>();

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();

    public enum HeroGUI
    {
        Activate,
        Waiting,
        Input1,
        Input2,
        Done
    }
    public HeroGUI HeroInput;

    public List<GameObject> HerosToManage = new List<GameObject>();
    private HandleTurn HeroChoise;
    //SPAWN POINTS
    public List<Transform> spawnPoints = new List<Transform>();
    private void Awake()
    {
        int i;
        for(i = 0; i < GameManager.instance.enemyAmount; i++)
        {
            GameObject NewEnemy = Instantiate(GameManager.instance.enemysToBattle[i],spawnPoints[i].position,Quaternion.identity)as GameObject;
            NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName + "_" + (i + 1);
            NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName = NewEnemy.name;
            EnemysInBattle.Add(NewEnemy);
        }
    }

    void Start()
    {
        BattleStates = PerformAction.Wait;
        //EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGUI.Activate;
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanel.SetActive(false);

        EnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (BattleStates)
        {
            case (PerformAction.Wait):
                if (PerformList.Count > 0)
                {
                    BattleStates = PerformAction.TakeAction;
                }
                break;
            case (PerformAction.TakeAction):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    for (int i = 0; i < HerosInBattle.Count; i++)
                    {
                        if (PerformList[0].AttackersTarget == HerosInBattle[i])
                        {
                            ESM.HeroToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.Action;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttackersTarget = HerosInBattle[Random.Range(0, HerosInBattle.Count)];
                            ESM.HeroToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.Action;
                        }
                    }
                }
                if (PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.Action;
                }
                BattleStates = PerformAction.PerformAction;
                break;
            case (PerformAction.PerformAction):
                //idle
                break;

            case (PerformAction.CheckAlive):
                if (HerosInBattle.Count < 1)
                {
                    BattleStates = PerformAction.Lose;
                    //lose the battle
                }
                else if (EnemysInBattle.Count < 1)
                {
                    BattleStates = PerformAction.Win;
                    //win the battle
                }
                else
                {
                    ClearAttackPanel();
                    //call function
                    HeroInput = HeroGUI.Activate;
                }
                break;
            case (PerformAction.Lose):
                {
                    Debug.Log("KAYBETTİN");
                }
                break;
            case (PerformAction.Win):
                {
                    Debug.Log("KAZANDIN");
                    for(int i = 0; i < HerosInBattle.Count; i++)
                    {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.Waiting;
                    }
                    GameManager.instance.LoadSceneAfterBattle();
                    GameManager.instance.gameStates = GameManager.GameStates.World_State;
                    GameManager.instance.enemysToBattle.Clear();
                }
                break;
        }
        switch (HeroInput)
        {
            case (HeroGUI.Activate):
                if (HerosToManage.Count > 0)
                {
                    HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    //create new handleturn instance
                    HeroChoise = new HandleTurn();

                    AttackPanel.SetActive(true);
                    //populate action buttons
                    CreateAttackButtons();
                    HeroInput = HeroGUI.Waiting;
                }
                break;
            case (HeroGUI.Waiting):

                break;

            case (HeroGUI.Done):
                HeroInputDone();
                break;
        }
    }
    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }
    public void EnemyButtons()
    {
        // cleanup
        foreach(GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();
        // create buttons
        foreach (GameObject enemy in EnemysInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.theName;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer, false);
            enemyBtns.Add(newButton);

        }
    }
    public void Input1()//attack button
    {
        HeroChoise.Attacker = HerosToManage[0].name;
        HeroChoise.AttackersGameObject = HerosToManage[0];
        HeroChoise.Type = "Hero";
        HeroChoise.choosenAttack = HerosToManage[0].GetComponent<HeroStateMachine>().hero.attacks[Random.Range(0,2)];
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }
    public void Input2(GameObject choosenEnemy)// enemy select
    {
        HeroChoise.AttackersTarget = choosenEnemy;
        HeroInput = HeroGUI.Done;
    }
    void HeroInputDone()
    {
        PerformList.Add(HeroChoise);
        //clean the attack panel
        ClearAttackPanel();
        
        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HerosToManage.RemoveAt(0);
        HeroInput = HeroGUI.Activate;
    }
    void ClearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);

        foreach (GameObject atkbtn in atkBtns)
        {
            Destroy(atkbtn);
        }
        atkBtns.Clear();
    }
    //create action buttons
    void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(AttackButton);

        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;
        Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(MagicAttackButton);

        if (HerosToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
        {
            foreach(BaseAttack magicAtk in HerosToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                atkBtns.Add(MagicButton);

            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }
    public void Input4(BaseAttack choosenMagic)//choosen magic attacks
    {
        HeroChoise.Attacker = HerosToManage[0].name;
        HeroChoise.AttackersGameObject = HerosToManage[0];
        HeroChoise.Type = "Hero";

        HeroChoise.choosenAttack = choosenMagic;
        MagicPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }
    public void Input3()//switching to magic attacks
    {
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(true);
    }
}
