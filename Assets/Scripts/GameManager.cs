using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    [SerializeField]private bool _gameOn;
    public bool GameOn { get => _gameOn; set => _gameOn = value; }
    private bool _gameOff;
    public bool GameOff { get => _gameOff; set => _gameOff = value; }

    private PlayerController playerController;
    private Animator playerAnimator;
    private SceneController sceneController;
    private float forwardSpeed;
    private float jumpPower;
    private float dogdeSpeed;
    [SerializeField] private int _waitTime;
    public int WaitTime { get => _waitTime; set => _waitTime = value; }

    GUIStyle myFont;
    GUIStyle skinButton;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        PlayGame();
    }
    private void PlayGame()
    {
        GameOn = false;
        forwardSpeed = playerController.ForwardSpeed;
        jumpPower = playerController.JumpPower;
        dogdeSpeed = playerController.DogdeSpeed;
        playerController.ForwardSpeed = 0;
        playerController.JumpPower = 0;
        StartCoroutine("GameTimer");
        StartCoroutine("Timer");
    }
    private IEnumerator EndGame()
    {
        GameOn = false;
        GameOff = true;
        playerController.ForwardSpeed = 0;
        playerController.JumpPower = 0;
        playerController.DogdeSpeed = 0;
        playerController.enabled = false;
        yield return new WaitForSeconds(0.9f);
        playerAnimator.enabled = false;
    }

    public void Death()
    {
        StartCoroutine("EndGame");
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(_waitTime);
        GameOn = true;
        playerController.ForwardSpeed = forwardSpeed;
        playerController.JumpPower = jumpPower;
        playerController.DogdeSpeed = dogdeSpeed;
    }

    private IEnumerator Timer()
    {
        while(_waitTime > 0.1f)
        {
            yield return new WaitForSeconds(1);
            _waitTime -= 1;
        }
    }
    private void OnGUI()
    {
        myFont = new GUIStyle();
        myFont.fontSize = 50;
        skinButton = new GUIStyle(GUI.skin.button);
        skinButton.fontSize = 25;
        if(_waitTime > 0.1)
        {
            GUILayout.BeginArea(new Rect(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 400, 400));
            GUILayout.TextField(_waitTime.ToString(), myFont);
            GUILayout.EndArea();
        }

        GUILayout.BeginArea(new Rect(Camera.main.pixelWidth-200, 0, 200, 200));
        if (GUILayout.Button("RESTART", skinButton))
        {
            sceneController.LoadScene();
        }
        GUILayout.EndArea();
    }

}
