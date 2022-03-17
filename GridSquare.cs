using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GridSquare : Selectable, ISelectHandler, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    // SQUARE OBJECTS AND ATTRIBUTES
    public GameObject numberText;
    public GameObject[] Notes;
    private Color baseTextColor = Color.black;
    private ColorBlock baseColors = new ColorBlock();
    public float opacity_level = 1f;
    public AudioSource audioUpdated;
    public AudioClip[] numberClips;

    // SQUARE VALUES
    private int squareIndex = -1;
    private int correct_number = 0;
    private bool given;
    private int col_num = 0;
    private int row_num = 0;
    private int box_num = 0;
    private bool selected = false;
    private bool notes_mode = false;
    private int number = 0;
    
    // SETTINGS
    private bool track_mistakes = true;
    private bool remove_notes = true;
    private bool show_identical = true;
    private bool show_duplicates = true;
    private bool is_duplicate = false;

    // SETS
    public void SetSquareIndex(int index) { squareIndex = index; }
    public void SetCorrectNumber(int number) { correct_number = number; }
    public void SetGiven(int num)
    {
        if (num >= 1 && num <= 9)
        {
            given = true;
            baseTextColor = Color.black;
            numberText.GetComponent<Text>().color = baseTextColor;
        }
        else
        {
            given = false;
            baseTextColor = Color.blue;
            numberText.GetComponent<Text>().color = baseTextColor;
        }
    }
    public void SetLocation(int num)
    {
        col_num = num % 9;
        row_num = num / 9;
        box_num = 3 * (row_num / 3) + (col_num / 3);
    }
    public void SetSelected() { selected = true; }
    public void SetMode (bool mode) { notes_mode = mode; }
    public void SetNote (int index, bool active) { Notes[index].SetActive(active); }
    public void SetNumber(int num)
    {
        number = num;
        DisplayText();
        //audioUpdated.Play();
    }
    public void SetTextColor(Color text_color) { numberText.GetComponent<Text>().color = text_color; }
    public void SetDuplicate(bool _is_duplicate) { is_duplicate = _is_duplicate; }

    // GETS
    public int GetSquareIndex() { return squareIndex; }
    public int GetCorrectNumber() { return correct_number; }
    public bool GetGiven() { return given; }
    public int GetLocation(string region)
    {
        switch (region)
        {
            case "row":
                return row_num;
            case "column":
                return col_num;
            case "box":
                return box_num;
            default:
                return -1;
        }
    }
    public bool GetSelected() { return selected; }
    public bool GetMode() { return notes_mode; }
    public bool GetNote(int index) { return Notes[index].activeSelf; }
    public int GetNumber() { return number; }
    public Color GetTextColor() { return numberText.GetComponent<Text>().color; }
    public Color GetBaseTextColor() { return baseTextColor; }
    public bool GetDuplicate() { return is_duplicate; }

    void Start()
    {
        baseColors = this.colors;
        audioUpdated = GetComponent<AudioSource>();
    }

    void Update()
    {
        ShowIfSelected(selected, baseColors);
        
        if (selected)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameEvents.UpdateSquareNumberMethod(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameEvents.UpdateSquareNumberMethod(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameEvents.UpdateSquareNumberMethod(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameEvents.UpdateSquareNumberMethod(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameEvents.UpdateSquareNumberMethod(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                GameEvents.UpdateSquareNumberMethod(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                GameEvents.UpdateSquareNumberMethod(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                GameEvents.UpdateSquareNumberMethod(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                GameEvents.UpdateSquareNumberMethod(9);
            }
            if (Input.GetKeyDown(KeyCode.Backspace) ||
                Input.GetKeyDown(KeyCode.Delete) ||
                Input.GetKeyDown(KeyCode.Alpha0))
            {
                GameEvents.UpdateSquareNumberMethod(0);
            }
            if (show_identical)
                IndicateSquaresSameValue(baseColors);
        }
    }

    public void DisplayText()
    {
        if (number <= 0)
            numberText.GetComponent<Text>().text = " ";
        else
            numberText.GetComponent<Text>().text = number.ToString();
    }

    public void ShowIfSelected(bool selected, ColorBlock baseColors)
    {
        if(selected)
        {
            ColorBlock selected_colors = baseColors;
            selected_colors.normalColor = baseColors.selectedColor;
            colors = selected_colors;
        }
        else 
        {
            colors = baseColors;
        }
    }

    // MOVE TO SUDOKUGRID SCRIPT
    public void IndicateSquaresSameValue(ColorBlock baseColors)
    {
        var squares = GameObject.FindGameObjectsWithTag("GridSquare");
        foreach (var square in squares)
        {
            if (square.GetComponent<GridSquare>().number == number && number > 0)
            {
                var same_num_colors = baseColors;
                same_num_colors.normalColor = new Color(same_num_colors.selectedColor.r, 
                                                        same_num_colors.selectedColor.g, 
                                                        same_num_colors.selectedColor.b, 
                                                        opacity_level * 0.7f);
                square.GetComponent<GridSquare>().colors = same_num_colors;
            }
            else if (IsSameRegion(square))
            {
                var same_region_colors = baseColors;
                same_region_colors.normalColor = new Color(same_region_colors.selectedColor.r, 
                                                           same_region_colors.selectedColor.g, 
                                                           same_region_colors.selectedColor.b, 
                                                           opacity_level);
                square.GetComponent<GridSquare>().colors = same_region_colors;
            }
            else
            {
                square.GetComponent<GridSquare>().colors = baseColors;
            }
        }
    }

    public bool IsSameRegion(GameObject square)
    {
        if (square.GetComponent<GridSquare>().col_num == col_num ||
            square.GetComponent<GridSquare>().row_num == row_num ||
            square.GetComponent<GridSquare>().box_num == box_num)
            return true;
        else
            return false;
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }
    // maybe add a BIG SQWARE BOI behind everything ???
    public void OnPointerClick(PointerEventData eventData)
    {
        //selected = true;
        GameEvents.SquareSelectedMethod(squareIndex);
    }

    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnReset += OnReset;
        GameEvents.OnToggleNotes += OnToggleNotes;
        GameEvents.OnPauseGame += OnPauseGame;
        GameEvents.OnGameEnded += OnGameEnded;
        GameEvents.OnSettingUpdated += OnSettingUpdated;
        GameEvents.OnHint += OnHint;
        GameEvents.OnUndo += OnUndo;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnReset -= OnReset;
        GameEvents.OnToggleNotes -= OnToggleNotes;
        GameEvents.OnPauseGame -= OnPauseGame;
        GameEvents.OnGameEnded -= OnGameEnded;
        GameEvents.OnSettingUpdated -= OnSettingUpdated;
        GameEvents.OnHint -= OnHint;
        GameEvents.OnUndo -= OnUndo;
    }

    public void OnSetNumber(int num)
    {
        // SQUARE SELECTED AND NOT GIVEN
        if (selected && !given)
        {
            // NOT NOTES MODE
            if (!notes_mode)
            {
                var old_num = number;
                bool[] old_notes = new bool[9];
                List<int> removed_notes_at = new List<int>();
                audioUpdated.clip = numberClips[num - 1];
                audioUpdated.Play();
                SetNumber(num);
                for (int i = 0; i < Notes.Length; i++)
                {
                    old_notes[i] = Notes[i].activeSelf;
                    if (Notes[i].activeSelf)
                    {
                        Notes[i].SetActive(false);
                    }
                }
                if (number != correct_number && number > 0 && track_mistakes)
                {
                    SetTextColor(Color.red);
                    GameEvents.WrongNumberMethod();
                }
                else
                    SetTextColor(baseTextColor);
                if (remove_notes)
                {
                    var squares = GameObject.FindGameObjectsWithTag("GridSquare");
                    foreach (var square in squares)
                    {
                        if (square.GetComponent<GridSquare>().squareIndex != squareIndex &&
                        (IsSameRegion(square)) && square.GetComponent<GridSquare>().Notes[num - 1].activeSelf &&
                        number > 0)
                        {
                            square.GetComponent<GridSquare>().Notes[num - 1].SetActive(false);
                            removed_notes_at.Add(square.GetComponent<GridSquare>().GetSquareIndex());
                        }
                    }
                }
                GameEvents.UpdateAuditTrailMethod(squareIndex, old_num, number, notes_mode, old_notes, removed_notes_at);

                if (show_duplicates)
                {
                    Show_Duplicates(num);
                }
            }

            // NOTES MODE AND NUMBER > 0
            if (notes_mode && num > 0)
                HideShowNotes(num - 1);

            // TRIGGER EVENT TO SHOW DUPLICATES
            if (show_duplicates)
                GameEvents.ShowDuplicatesMethod();
        }
    }

    // ISSUE: THIS RUNS ON CELLS OF LOWER INDEX BEFORE THE CHANGED NUMBER ENTRY, THUS SHOWING DUPLICATES OF PREVIOUS ON LOWER INDEXED CELLS
    // ALSO, NEED TO FIND A WAY TO INDICATE THAT IT'S A DUPLICATE, RUN FOR ALL CELLS, THEN DO NOT REMOVE A DUPLICATE OTHERWISE!! ARGHH!!
    public void Show_Duplicates(int num)
    {
        is_duplicate = false;
        var squares = GameObject.FindGameObjectsWithTag("GridSquare");

        foreach (var square in squares)
        {
            if (square.GetComponent<GridSquare>().GetNumber() == num &&
                square.GetComponent<GridSquare>().GetSquareIndex() != squareIndex &&
                IsSameRegion(square) &&
                num > 0)
                Debug.Log("for index " + squareIndex + " duplicate value " + num + " at index " + square.GetComponent<GridSquare>().GetSquareIndex());
        }
    }

    public void HideShowNotes(int i)
    {
        bool[] old_notes = new bool[9];
        for (int j = 0; j < Notes.Length; j++)
            old_notes[j] = Notes[j].activeSelf;

        var old_num = number;
        if (number > 0)
            SetNumber(0);
        if (Notes[i].activeSelf)
        {
            Notes[i].SetActive(false);
            GameEvents.UpdateAuditTrailMethod(squareIndex, old_num, i, notes_mode, old_notes, null);
        }
        else
        {
            Notes[i].SetActive(true);
            GameEvents.UpdateAuditTrailMethod(squareIndex, old_num, i, notes_mode, old_notes, null);
        }
    }

    public void OnSquareSelected(int square_index)
    {
        if (squareIndex != square_index)
            selected = false;
        else if (squareIndex == square_index)
            selected = true;
    }

    public void OnReset()
    {
        if (!given)
        {
            SetNumber(0);
            for (int i = 0; i < Notes.Length; i++)
            {
                Notes[i].SetActive(false);
            }
        }
    }

    public void OnToggleNotes(bool notes_mode)
    {
        SetMode(notes_mode);
    }

    public void OnPauseGame(bool paused)
    {
        if (paused)
            selected = false;
    }

    public void OnGameEnded()
    {
        var game_ended_colors = baseColors;
        game_ended_colors.normalColor = new Color(game_ended_colors.selectedColor.r, game_ended_colors.selectedColor.g, game_ended_colors.selectedColor.b, opacity_level * 0.9f);
        colors = game_ended_colors;
    }

    public void OnSettingUpdated(int setting)
    {
        switch (setting)
        {
            case 2:
                track_mistakes = !track_mistakes;
                break;
            case 3:
                show_duplicates = !show_duplicates;
                break;
            case 4:
                show_identical = !show_identical;
                break;
            case 5:
                remove_notes = !remove_notes;
                break;
        }
    }

    public void OnHint()
    {
        if (selected)
        {
            SetNumber(correct_number);
        }
    }

    public void OnUndo(int num, int old_num, bool[] old_notes, List<int> removed_notes_at)
    {
        if (selected)
        {
            SetNumber(num);
            if (number != correct_number && number > 0 && track_mistakes)
                SetTextColor(Color.red);
            else
                SetTextColor(baseTextColor);
            for (int i = 0; i < old_notes.Length; i++)
                Notes[i].SetActive(old_notes[i]);
            if (removed_notes_at != null)
            {
                var squares = GameObject.FindGameObjectsWithTag("GridSquare");
                foreach (var square in squares)
                {
                    if (removed_notes_at.Contains(square.GetComponent<GridSquare>().GetSquareIndex()))
                        square.GetComponent<GridSquare>().SetNote(old_num - 1, true);
                }
            }
        }

        if (show_duplicates)
            GameEvents.ShowDuplicatesMethod();
    }
}
