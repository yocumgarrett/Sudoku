using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class NumberButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public int value = 0;
    private int total_in_grid = 0;
    private int max_in_grid = 9;
    public TextMeshProUGUI numberText;
    private bool is_paused;

    void Start()
    {
        
    }

    void Update()
    {
        CountTotalOfNumber();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!is_paused)
            GameEvents.UpdateSquareNumberMethod(value);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable()
    {
        GameEvents.OnToggleNotes += OnToggleNotes;
        GameEvents.OnPauseGame += OnPauseGame;
        GameEvents.OnGameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GameEvents.OnToggleNotes -= OnToggleNotes;
        GameEvents.OnPauseGame -= OnPauseGame;
        GameEvents.OnGameEnded -= OnGameEnded;
    }

    public void CountTotalOfNumber()
    {
        total_in_grid = 0;
        var squares = GameObject.FindGameObjectsWithTag("GridSquare");
        foreach (var square in squares)
        {
            if (square.GetComponent<GridSquare>().GetNumber() == value)
                total_in_grid++;
        }
        if (total_in_grid >= max_in_grid && value > 0)
            numberText.color = Color.white;
        else
            numberText.color = Color.black;
    }

    public void OnToggleNotes(bool notes_mode)
    {
        if(!notes_mode)
            numberText.GetComponent<TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;
        else
            numberText.GetComponent<TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Normal;
    }

    public void OnPauseGame(bool paused)
    {
        if (paused)
        {
            numberText.GetComponent<TextMeshProUGUI>().text = " ";
            is_paused = paused;
        }
        else
        {
            if (value > 0)
                numberText.GetComponent<TextMeshProUGUI>().text = value.ToString();
            else
                numberText.GetComponent<TextMeshProUGUI>().text = "X";
            is_paused = paused;
        }
    }

    public void OnGameEnded()
    {
        interactable = false;
    }
}
