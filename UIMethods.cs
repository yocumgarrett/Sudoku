using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMethods : MonoBehaviour
{
    // NOTES VARIABLES
    public bool notes = false;
    public GameObject NotesButton;
    public Sprite NotesOffSprite;
    public Sprite NotesOnSprite;

    public bool GetNotesOnOff() { return notes; }
    public void SetNotesOnOff(bool mode) { notes = mode; }

    // TIMER VARIABLES
    public TextMeshProUGUI TimerText;
    public GameObject PauseButton;
    public GameObject PauseOverlayPanel;
    public GameObject OverlayPlayButton;
    public Sprite PauseOn;
    public Sprite PauseOff;
    private float secondsCount = 0;
    private int minutesCount = 0;
    private bool game_paused = false;

    public bool GetPauseStatus() { return game_paused; }
    public void SetPauseStatus(bool status) { game_paused = status; }

    // MISTAKES VARIABLES
    public TextMeshProUGUI MistakesText;
    private int mistakes = 0;
    public int max_mistakes = 3;

    // SETTINGS VARIABLES
    public GameObject SettingsOverlayPanel;
    public GameObject WinText;
    public GameObject[] Settings;
    private bool show_settings = false;
    private bool timer_on = true;
    private bool track_mistakes = true;
    private bool show_duplicates = true;
    private bool show_identical = true;
    private bool remove_notes = true;

    public bool GetMistakesSetting() { return track_mistakes; }

    public bool GetSettingsStatus() { return show_settings; }
    public void SetSettingsStatus(bool status) { show_settings = status; }

    // HINTS VARIABLES
    public TextMeshProUGUI HintsText;
    public int max_hints = 2;
    private int hints = 0;

    // CHECK SOLUTION VARIABLES
    public GameObject CheckSolutionButton;

    // ON START
    void Start()
    {
        MistakesText.text = "Mistakes: " + mistakes + "/" + max_mistakes;
        HintsText.text = max_hints.ToString();
        hints = max_hints;
        SettingsOverlayPanel.SetActive(false);
        PauseOverlayPanel.SetActive(false);
    }

    // EVERY FRAME
    void Update()
    {
        if (!game_paused)
        {
            if(timer_on)
                UpdateTimerUI();
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetBoard();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                SwitchNotesOnOff();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SwitchPauseOnOff();
            }
        }
    }

    public void OnEnable()
    {
        GameEvents.OnWrongNumber += OnWrongNumber;
        GameEvents.OnGameEnded += OnGameEnded;
    }

    public void OnDisable()
    {
        GameEvents.OnWrongNumber -= OnWrongNumber;
        GameEvents.OnGameEnded -= OnGameEnded;
    }

    // RESET BUTTON AND INPUT R
    public void ResetBoard()
    {
        secondsCount = 0;
        minutesCount = 0; 
        GameEvents.ResetBoardMethod();
    }

    // NOTES BUTTON AND INPUT N
    public void SwitchNotesOnOff()
    {
        notes = !notes;
        if (notes)
        {
            NotesButton.GetComponent<Image>().sprite = NotesOnSprite;
        }
        else
        {
            NotesButton.GetComponent<Image>().sprite = NotesOffSprite;
        }
        GameEvents.ToggleNotesMethod(notes);
    }

    // PAUSE BUTTON AND INPUT P
    public void SwitchPauseOnOff()
    {
        game_paused = !game_paused;
        if (game_paused)
        {
            PauseButton.GetComponent<Image>().sprite = PauseOff;
            if (!show_settings)
            {
                SettingsOverlayPanel.SetActive(false);
                PauseOverlayPanel.SetActive(true);
            }
        }
        else
        {
            PauseButton.GetComponent<Image>().sprite = PauseOn;
            if (!show_settings)
            {
                PauseOverlayPanel.SetActive(false);
            }
            SettingsOverlayPanel.SetActive(false);
        }
        GameEvents.PauseGameMethod(game_paused);
    }

    // TIMER UPDATE AS LONG AS NOT PAUSED
    public void UpdateTimerUI()
    {
        secondsCount += Time.deltaTime;
        TimerText.text = minutesCount.ToString("00") + ":" + secondsCount.ToString("00");
        if (secondsCount >= 60)
        {
            minutesCount++;
            secondsCount %= 60;
        }
    }

    // CALLED WHEN A WRONG NUMBER EVENT TRIGGERS
    public void OnWrongNumber()
    {
        if(track_mistakes)
        {
            mistakes++;
            MistakesText.text = "Mistakes: " + mistakes + "/" + max_mistakes;
            if (mistakes >= max_mistakes)
            {
                GameEvents.GameEndedMethod();
            }
        }
    }

    public void HintUsed()
    {
        if(hints > 0)
        {
            hints--;
            HintsText.text = hints.ToString();
            GameEvents.HintMethod();
        }
    }

    // SETTINGS (GEAR) BUTTON
    public void ShowSettingsOnOff()
    {
        show_settings = !show_settings;
        if (show_settings)
        {
            if(!game_paused)
                SwitchPauseOnOff();
            PauseOverlayPanel.SetActive(false);
            SettingsOverlayPanel.SetActive(true);
        }
        else
        {
            if(game_paused)
                SwitchPauseOnOff();
            SettingsOverlayPanel.SetActive(false);
        }
    }

    // UPDATE SETTINGS
    public void UpdateSetting(int i)
    {
        // Settings[i] normal color change to green
        switch (i)
        {
            case 1:
                timer_on = !timer_on;
                GameEvents.SettingUpdatedMethod(1);
                if (timer_on)
                    TimerText.text = minutesCount.ToString("00") + ":" + secondsCount.ToString("00");
                else
                {
                    TimerText.text = "";
                    secondsCount = 0;
                    minutesCount = 0;
                }
                UpdateSettingButtonColor(1, timer_on);
                break;
            case 2:
                track_mistakes = !track_mistakes;
                GameEvents.SettingUpdatedMethod(2);
                if (track_mistakes)
                    MistakesText.text = "Mistakes: " + mistakes + "/" + max_mistakes;
                else
                    MistakesText.text = "";
                UpdateSettingButtonColor(2, track_mistakes);
                break;

            case 3:
                show_duplicates = !show_duplicates;
                GameEvents.SettingUpdatedMethod(3);
                UpdateSettingButtonColor(3, show_duplicates);
                break;
            case 4:
                show_identical = !show_identical;
                GameEvents.SettingUpdatedMethod(4);
                UpdateSettingButtonColor(4, show_identical);
                break;
            case 5:
                remove_notes = !remove_notes;
                GameEvents.SettingUpdatedMethod(5);
                UpdateSettingButtonColor(5, remove_notes);
                break;
        }
    }

    public void UpdateSettingButtonColor(int setting, bool on)
    {
        var colors = Settings[setting - 1].GetComponent<Button>().colors;
        if (on)
        {
            colors.normalColor = Color.green;
            colors.pressedColor = Color.green;
            colors.selectedColor = Color.green;
        }
        else
        {
            colors.normalColor = Color.red;
            colors.pressedColor = Color.red;
            colors.selectedColor = Color.red;
        }
        Settings[setting - 1].GetComponent<Button>().colors = colors;
    }

    // TRIGGER SUDOKU GRID TO CHECK SOLUTION
    public void CheckSolution()
    {
        GameEvents.CheckSolutionMethod();
    }

    // WHEN THE GAME IS OVER
    public void OnGameEnded()
    {
        //WinText.SetActive(true);
    }
}
