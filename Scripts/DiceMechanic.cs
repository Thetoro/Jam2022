using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DiceMechanic : MonoBehaviour
{
    public static bool diceRollPanel;
    [SerializeField]
    private Sprite[] _seisCaras;
    [SerializeField]
    private GameObject _dicePanel;
    private float _index;
    private int result;
    private SpriteRenderer _sr;
    private DiceControls _diceControl;
    private bool _rollDice;

    private AudioManager _audioManager;

    private void Start () 
    {
        _diceControl = new DiceControls();
        _sr = GetComponent<SpriteRenderer>();
        Time.timeScale = 0f;
        diceRollPanel = true;
        _audioManager = FindObjectOfType<AudioManager>();
	}

    public void onRollDice(InputAction.CallbackContext context)
    {
        _rollDice = context.action.triggered;
    }

    // Update is called once per frame
    void Update()
    {
        if(_rollDice && diceRollPanel)
        {   
            if(!_audioManager.IsPlaying("Dado"))
                _audioManager.Play("Dado");
            StartCoroutine("RollDice");
            Debug.Log("HOLA");
            diceRollPanel = false;
        }
            
    }

    IEnumerator RollDice()
    {
        int rand = 0;
        for(int i = 0; i < 20; i++)
        {
            rand = Random.Range(0,5);
            _sr.sprite = _seisCaras[rand];
            yield return new WaitForSeconds(0.4f);
        }

        Debug.Log("HOLA");
        result = rand + 1;
    }

    
}
