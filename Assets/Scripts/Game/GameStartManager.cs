using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject BGBlur;

    [SerializeField] private GameObject Interaction1;
    [SerializeField] private GameObject Interaction2;
    [SerializeField] private GameObject Interaction3;

    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private GameObject FinalBtn;


    private int _networkType = 0;
    private bool stated = false;
    public void Awake()
    {
        BG.SetActive(true);
        BGBlur.SetActive(false);

        Interaction1.SetActive(true);
        Interaction2.SetActive(false);
        Interaction3.SetActive(false);
    }

    public void Interaction1Done()
    {
        BG.SetActive(false);
        BGBlur.SetActive(true);

        Interaction1.SetActive(false);
        Interaction2.SetActive(true);
        Interaction3.SetActive(false);
    }

    public void Interaction2Done()
    {
        BG.SetActive(false);
        BGBlur.SetActive(true);

        Interaction1.SetActive(false);
        Interaction2.SetActive(false);
        Interaction3.SetActive(true);
    }

    public void HostBtnClicked()
    {
        _networkType = 0;
        Interaction2Done();
    }

    public void ClientBtnClicked()
    {
        _networkType = 1;
        Interaction2Done();
    }

    public void FinalBtnClicked()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("NetworkType", _networkType);
        PlayerPrefs.SetString("Name", nameField.text);
        PlayerPrefs.Save();
        LoadGame();
    }

    private void Update()
    {
        if (nameField.text == "") FinalBtn.GetComponent<Button>().interactable = false;
        else FinalBtn.GetComponent<Button>().interactable = true;

        if (!stated)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                stated = true;
                Interaction1Done();
            }
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
