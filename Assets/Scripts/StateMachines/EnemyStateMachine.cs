using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    public BaseEnemy enemy;
    public enum TurnState
    {
        Processing,
        ChoseAction,
        Waiting,
        Action,
        Dead
    }

    public TurnState currentState;

    // ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 10f;
    //this gameobject
    private Vector3 startposition;
    // for action
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 10f;
    public GameObject Selector;

    void Start()
    {
        currentState = TurnState.Processing;
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.Processing):
                UpgradeProgressBar();
                break;
            case (TurnState.ChoseAction):
                chooseAction();
                currentState = TurnState.Waiting;
                break;
            case (TurnState.Waiting):
                //idle state
                break;
            
            case (TurnState.Action):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.Dead):

                break;
        }
        void UpgradeProgressBar()
        {
            cur_cooldown = cur_cooldown + Time.deltaTime;
            
            if (cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.ChoseAction;
            }
        }
    }
    void chooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.theName;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];

        int num = Random.Range(0, enemy.attacks.Count);
        myAttack.choosenAttack = enemy.attacks[num];
        Debug.Log(this.gameObject.name + " has choosen " + myAttack.choosenAttack.attackName + " and do " + myAttack.choosenAttack.attackDamage + " damage");

        BSM.CollectActions(myAttack);
    }
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        Vector3 heroPosition = new Vector3 (HeroToAttack.transform.position.x-1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);

        while (MoveTowardsEnemy(heroPosition))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.8f);

        DoDamage();

        Vector3 firstPosition = startposition;
        while (MoveTowardsStart(firstPosition)) { yield return null; }

        BSM.PerformList.RemoveAt(0);
        BSM.BattleStates = BattleStateMachine.PerformAction.Wait;
        actionStarted = false;
        cur_cooldown = 0f;
        currentState = TurnState.Processing;
    }
    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    void DoDamage()
    {
        float calc_damage = enemy.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
    }
    public void TakeDamage(float getDamageAmount)
    {
        enemy.curHP -= getDamageAmount;
        if(enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.Dead;
        }
    }
}