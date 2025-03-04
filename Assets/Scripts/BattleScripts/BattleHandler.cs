using UnityEngine;
using System; // for Action

// Tutorial: https://www.youtube.com/watch?v=0QU0yV0CYT4&t=2s
// i changed some of the names for convenience 

public class BattleHandler : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    private CharacterBattle playerCB;
    private CharacterBattle enemyCB;
    private CharacterBattle activeCB;

    private CharacterHealth playerHealth;
    private CharacterHealth enemyHealth;

    // the health bars
    public HealthSlider playerHealthBar;
    public HealthSlider enemyHealthBar;

    private State state;

    private enum State{
        waitingForPlayer,   // player's turn
        busy,               // enemy's turn
    }

    void Start() {
        // script for attacking, attack movement, etc
        playerCB = player.GetComponent<CharacterBattle>();
        enemyCB = enemy.GetComponent<CharacterBattle>();

        // script for health (value)
        playerHealth = player.GetComponent<CharacterHealth>();
        enemyHealth = enemy.GetComponent<CharacterHealth>();

        setActiveCB(playerCB); // player starts first
        state = State.waitingForPlayer;
    }
    
    private void Update() {
        if (state == State.waitingForPlayer) {
            if (Input.GetKeyDown(KeyCode.Space)) {     // space to attack (temp)
                state = State.busy;                    // execute player's attack                   
                playerCB.Attack(enemyCB, enemyHealth, enemyHealthBar, () => {       // and change turns
                    Debug.Log("attack finished");
                    chooseNextActiveChar();
                });
            }
        }
    }

    // current turn
    private void setActiveCB(CharacterBattle cb) {
        if (activeCB != null) {
            activeCB.hideTurnIndicator();
        }
        activeCB = cb;
        activeCB.showTurnIndicator();
    }

    // swap player, enemy turns
    private void chooseNextActiveChar() {
        if (testBattleOver()) {
            return;
        }

        if (activeCB == playerCB) {
            setActiveCB(enemyCB);
            state = State.busy;

            // enemy attacking 
            enemyCB.Attack(playerCB, playerHealth, playerHealthBar, () => {
                    Debug.Log("attack finished");
                    chooseNextActiveChar();
                });
        } else {
            setActiveCB(playerCB);
            state = State.waitingForPlayer;
        }
    }

    private bool testBattleOver() {
        if (playerCB.isDead()) {
            return true;
        }

        if (enemyCB.isDead()) {
            return true;
        }

        return false;       // continue game
    }
}

