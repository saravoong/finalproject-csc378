using UnityEngine;
using System; // for Action
using System.Collections;

// Tutorial: https://www.youtube.com/watch?v=0QU0yV0CYT4&t=2s
// i changed some of the names for convenience 

public class CharacterBattle : MonoBehaviour
{
    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    public GameObject turnIndicator;
    public CharacterHealth charHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake() {
        charHP = GetComponent<CharacterHealth>();
        state = State.idle;
        hideTurnIndicator();    // hide green circle at start
    }

    private enum State {
        idle,
        sliding,    // to opponent 
        busy,
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.idle:
                break;
            case State.busy:
                break;
            case State.sliding:
                float slideSpeed = 10f;
                transform.position += (slideTargetPosition - getPosition()) * slideSpeed * Time.deltaTime;
                
                float reachedDistance = 5f;
                if (Vector3.Distance(getPosition(), slideTargetPosition) < reachedDistance) {
                    //Debug.Log(Vector3.Distance(getPosition(), slideTargetPosition));
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;
        }
        
    }

    public Vector3 getPosition() {
        return transform.position;
    }

    public bool isDead() {
        return charHP.isDead();
    }
     
    public void Attack(CharacterBattle targetCB, CharacterHealth targetHealth, HealthSlider targetHB, Action onAttackComplete) {
        Debug.Log("attack!");
        Vector3 slideTargetPosition = targetCB.getPosition() + (getPosition() - targetCB.getPosition()).normalized*10f;
        Vector3 startPosition = getPosition();

        // to target
        slideToPosition(targetCB.slideTargetPosition, () => {
            // at target, attack
            state = State.busy;
            Debug.Log("at target!");
            targetHealth.takeDamage(20);
            targetHB.updateHealthBar(-20);

            // (for animation perhaps) Vector3 attackDir = (targetCB.getPosition() - getPosition()).normalized;
            // finish attack, return to start
            slideToPosition(startPosition, () => {
                Debug.Log("returned!");
                // returned, next turn
                state = State.idle;
                onAttackComplete();
            });

        });
    }

    private void slideToPosition(Vector3 slideTargetPosition, Action onSlideComplete) {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.sliding;
    }

    public void hideTurnIndicator() {
        turnIndicator.SetActive(false);
    }
    
    public void showTurnIndicator() {
        turnIndicator.SetActive(true);

    }
}
