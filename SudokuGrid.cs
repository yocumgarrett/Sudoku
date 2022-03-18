using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SudokuGrid : MonoBehaviour
{
    // GRID SQUARE GENERATION VARIABLES
    public int columns = 0;
    public int rows = 0;
    public float square_offset = 0.0f; 
    public float square_scale = 1.0f;
    public Vector2 start_position = new Vector2(0.0f, 0.0f);
    public int title_pos_x = 0;
    public int title_pos_y = 0;
    public GameObject gridSquare;
    private List<GameObject> gridSquares = new List<GameObject>();
    private int selected_grid_data = -1;

    // SELECTED SQUARE TRACKING VARIABLES
    private int selected_index = -1;
    private int selected_row = -1;
    private int selected_col = -1;

    // GAME INFO - TITLE AND STAR SCORE
    public TextMeshProUGUI titleText;
    public GameObject[] stars;
    public int difficulty = 0;

    // LINK TO UIMETHODS
    public GameObject CameraUIMethods;
    private bool show_duplicates = true;
    private bool show_identical = true;

    // SOUNDS
    public AudioSource checkSolutionAudio;
    public AudioClip[] checkSolutionClips;

    void Start()
    {
        if (gridSquare.GetComponent<GridSquare>() == null)
            Debug.LogError("This Game Object is missing GridSquare script");

        CreateGrid();
        SetGridNumber(MenuButtons.difficulty);
    }

    void Update()
    {
        // navigate grid with arrow keys.
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NavigateGrid("right");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NavigateGrid("left");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateGrid("up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateGrid("down");
        }
        // this will break the game. makes it way easier than expected lol.
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ShowAllPossibleNotes();
            }
        }
    }

    public void NavigateGrid(string direction)
    {
        switch (direction)
        {
            case "right":
                if (selected_col == 8)
                    GameEvents.SquareSelectedMethod(selected_index - 8);
                else
                    GameEvents.SquareSelectedMethod(selected_index + 1);
                break;
            case "left":
                if (selected_col == 0)
                    GameEvents.SquareSelectedMethod(selected_index + 8);
                else
                    GameEvents.SquareSelectedMethod(selected_index - 1);
                break;
            case "up":
                if (selected_row == 0)
                {
                    Debug.Log("go to index " + (selected_index + 72));
                    GameEvents.SquareSelectedMethod(selected_index + 72);
                }
                    
                else
                    GameEvents.SquareSelectedMethod(selected_index - 9);
                break;
            case "down":
                if (selected_row == 8)
                {
                    Debug.Log("go to index " + (selected_index - 72));
                    GameEvents.SquareSelectedMethod(selected_index - 72);
                }
                    
                else
                    GameEvents.SquareSelectedMethod(selected_index + 9);
                break;
            default:
                break;
        }
    }

    public void ShowAllPossibleNotes()
    {
        // do this process for each cell
        for (int i = 0; i < gridSquares.Count; i++)
        {
            if (gridSquares[i].GetComponent<GridSquare>().GetNumber() == 0)
            {
                int num_notes = 9;
                for (int j = 1; j <= num_notes; j++)
                {
                    bool found = false;
                    int k = 0;
                    while (!found && k < gridSquares.Count)
                    {
                        if (gridSquares[k].GetComponent<GridSquare>().IsSameRegion(gridSquares[i]) &&
                            gridSquares[k].GetComponent<GridSquare>().GetNumber() == j)
                            found = true;
                        k++;
                    }
                    if (!found)
                    {
                        gridSquares[i].GetComponent<GridSquare>().Notes[j - 1].SetActive(true);
                    }
                }
            }
        }
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquarePositions();
    }

    // Instantiate rows * columns grid squares at the transform of the parent with the specified scale.
    private void SpawnGridSquares()
    {
        int square_index = 0;
        for(int row = 0; row < rows; row++)
        {
            for(int column = 0; column < columns; column++)
            {
                gridSquares.Add(Instantiate(gridSquare) as GameObject);
                gridSquares[gridSquares.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                gridSquares[gridSquares.Count - 1].transform.parent = this.transform; // Not sure exactly what this is doing. I think it retains child/parent relationship
                gridSquares[gridSquares.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale); // (1, 1, 1).
                square_index++;
            }
        }
        square_index = 0;
    }

    // We then move the squares to the desired positions, relative to the offset and scale of the squares.
    private void SetSquarePositions()
    {
        var squareRect = gridSquares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2(); // (x, y)
        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + square_offset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + square_offset;

        int col_num = 0;
        int row_num = 0;
        int extra_offset = 5;

        foreach (GameObject square in gridSquares)
        {
            if (col_num + 1 > columns) // Go to next row and reset columns.
            {
                row_num++;
                col_num = 0;
            }
            
            var pos_x_offset = offset.x * col_num;
            var pos_y_offset = offset.y * row_num;
            
            if (col_num >= 3 && col_num <= 5)
                pos_x_offset = pos_x_offset + extra_offset;
            if (col_num >= 6 && col_num <= 8)
                pos_x_offset = pos_x_offset + (extra_offset * 2);

            if (row_num >= 3 && row_num <= 5)
                pos_y_offset = pos_y_offset + extra_offset;
            if (row_num >= 6 && row_num <= 8)
                pos_y_offset = pos_y_offset + (extra_offset * 2);

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(start_position.x + pos_x_offset, start_position.y - pos_y_offset);
            col_num++;
        }
    }

    private void SetGridNumber(string level)
    {
        selected_grid_data = Random.Range(0, SudokuData.Instance.sudoku_game[level].Count);
        var data = SudokuData.Instance.sudoku_game[level][selected_grid_data];

        setGridSquareData(data);
        titleText.text = data.title_data;
        setDifficultyStars(data.difficulty_score);
    }

    private void setGridSquareData(SudokuData.SudokuBoardData data)
    {
        for(int i=0; i<gridSquares.Count; i++)
        {
            gridSquares[i].GetComponent<GridSquare>().SetNumber(data.unsolved_data[i]);
            gridSquares[i].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[i]);
            gridSquares[i].GetComponent<GridSquare>().SetGiven(data.unsolved_data[i]);
            gridSquares[i].GetComponent<GridSquare>().SetLocation(i);
        }
    }

    private void setDifficultyStars(float score)
    {
        difficulty = (int) (score * 2);
        int j = 0;
        while (j < difficulty)
        {
            stars[j].SetActive(true);
            j++;
        }
    }

    public void OnEnable()
    {
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnCheckSolution += OnCheckSolution;
        GameEvents.OnShowDuplicates += OnShowDuplicates;
    }

    public void OnDisable()
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnCheckSolution -= OnCheckSolution;
        GameEvents.OnShowDuplicates -= OnShowDuplicates;
    }

    public void OnSquareSelected(int square_index)
    {
        selected_index = square_index;
        selected_row = selected_index / 9;
        selected_col = selected_index % 9;
    }

    public void OnCheckSolution()
    {
        bool found_incorrect = false;
        int index = 0;
        while (!found_incorrect && index < gridSquares.Count)
        {
            var current_num = gridSquares[index].GetComponent<GridSquare>().GetNumber();
            var correct_num = gridSquares[index].GetComponent<GridSquare>().GetCorrectNumber();
            if (current_num != correct_num)
                found_incorrect = true;
            index++;
        }

        if (found_incorrect == false)
        {
            // GAME WON
            CameraUIMethods.GetComponent<UIMethods>().SetPauseStatus(true);
            checkSolutionAudio.clip = checkSolutionClips[0];
            checkSolutionAudio.Play();
            Debug.Log("you win!");
            GameEvents.GameEndedMethod();
        }
        else
        {
            checkSolutionAudio.clip = checkSolutionClips[1];
            checkSolutionAudio.Play();
            Debug.Log("not quite right");
        }
    }

    public void OnShowDuplicates()
    {
        for (int i = 0; i < gridSquares.Count; i++)
        {
            gridSquares[i].GetComponent<GridSquare>().SetDuplicate(false);
            var num = gridSquares[i].GetComponent<GridSquare>().GetNumber();

            if (num > 0)
            {
                for (int j = 0; j < gridSquares.Count; j++)
                {
                    if (gridSquares[j].GetComponent<GridSquare>().GetNumber() == num &&
                        i != j &&
                        gridSquares[j].GetComponent<GridSquare>().IsSameRegion(gridSquares[i]))
                    {
                        gridSquares[i].GetComponent<GridSquare>().SetDuplicate(true);
                    }

                }
            }
        }

        for (int i = 0; i < gridSquares.Count; i++)
        {
            if (gridSquares[i].GetComponent<GridSquare>().GetDuplicate() == true)
                gridSquares[i].GetComponent<GridSquare>().SetTextColor(Color.red);
            else if(gridSquares[i].GetComponent<GridSquare>().GetNumber() == gridSquares[i].GetComponent<GridSquare>().GetCorrectNumber() ||
                    gridSquares[i].GetComponent<GridSquare>().GetNumber() == 0)
                gridSquares[i].GetComponent<GridSquare>().SetTextColor(gridSquares[i].GetComponent<GridSquare>().GetBaseTextColor());
        }
    }
}
