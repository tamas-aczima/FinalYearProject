using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button resetProgressButton;
    [SerializeField] private Button quitButton;
    private Button selectedButton;
    private List<Button> buttons = new List<Button>();
    private YostSkeletonRig myPrioRig;
    private bool hasChangedButton;
    [SerializeField] private AudioSource buttonChange;
    [SerializeField] private AudioSource buttonClick;

    void Start () {
        myPrioRig = FindObjectOfType<YostSkeletonRig>();
        myPrioRig.gameObject.GetComponent<CharacterController>().enabled = false;

        if (PlayerPrefs.HasKey("StartedLevel") && PlayerPrefs.GetInt("StartedLevel") > 0)
        {
            buttons.Add(continueButton);
        }
        else
        {
            continueButton.GetComponentInChildren<Text>().color = Color.gray;
        }

        buttons.Add(newGameButton);
        buttons.Add(settingsButton);
        buttons.Add(resetProgressButton);
        buttons.Add(quitButton);

        selectedButton = buttons[0];

        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(NewGame);
        settingsButton.onClick.AddListener(GoToSettings);
        resetProgressButton.onClick.AddListener(ResetProgress);
        quitButton.onClick.AddListener(Quit);
	}
	
	void Update () {
        NavigateMenu();
        UpdateColor();
	}

    private void NavigateMenu()
    {
        //go down
        if (myPrioRig.GetJoyStickAxis(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_LEFT_Y_AXIS) < 0 && !hasChangedButton)
        {
            hasChangedButton = true;
            if (buttons.IndexOf(selectedButton) < buttons.Count - 1)
            {
                buttonChange.Play();
                selectedButton = buttons[buttons.IndexOf(selectedButton) + 1];
            }
        }

        //go up
        if (myPrioRig.GetJoyStickAxis(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_LEFT_Y_AXIS) > 0 && !hasChangedButton)
        {
            hasChangedButton = true;
            if (buttons.IndexOf(selectedButton) > 0)
            {
                buttonChange.Play();
                selectedButton = buttons[buttons.IndexOf(selectedButton) - 1];
            }
        }

        //allow change
        if (myPrioRig.GetJoyStickAxis(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_LEFT_Y_AXIS) == 0)
        {
            hasChangedButton = false;
        }

        //invoke selected button's function
        if (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_A_BUTTON))
        {
            buttonClick.Play();
            selectedButton.onClick.Invoke();
        }
    }

    private void UpdateColor()
    {
        foreach (Button button in buttons)
        {
            if (button.name == selectedButton.name)
            {
                button.GetComponentInChildren<Text>().color = Color.white;
            }
            else
            {
                button.GetComponentInChildren<Text>().color = Color.gray;
            }
        }
    }

    private void ContinueGame()
    {
        myPrioRig.gameObject.GetComponent<CharacterController>().enabled = true;
        SceneManager.LoadScene(PlayerPrefs.GetInt("StartedLevel") + 1);
    }

    private void NewGame()
    {
        myPrioRig.gameObject.GetComponent<CharacterController>().enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void GoToSettings()
    {

    }

    private void ResetProgress()
    {
        PlayerPrefs.SetInt("StartedLevel", 0);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
