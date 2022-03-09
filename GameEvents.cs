using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour
{
    public delegate void UpdateSquareNumber(int number);
    public static event UpdateSquareNumber OnUpdateSquareNumber;

    public static void UpdateSquareNumberMethod(int number)
    {
        if (OnUpdateSquareNumber != null)
            OnUpdateSquareNumber(number);
    }
    
    public delegate void UpdateAuditTrail(int index, int old_num, int new_num, bool notes, bool[] old_notes, List<int> removed_notes);
    public static event UpdateAuditTrail OnUpdateAuditTrail;

    public static void UpdateAuditTrailMethod(int index, int old_num, int new_num, bool notes, bool[] old_notes, List<int> removed_notes)
    {
        if (OnUpdateAuditTrail != null)
            OnUpdateAuditTrail(index, old_num, new_num, notes, old_notes, removed_notes);
    }

    public delegate void Undo(int number, int old_number, bool[] old_notes, List<int> removed_notes_at);
    public static event Undo OnUndo;

    public static void UndoMethod(int number, int old_number, bool[] old_notes, List<int> removed_notes_at)
    {
        if (OnUndo != null)
            OnUndo(number, old_number, old_notes, removed_notes_at);
    }

    public delegate void WrongNumber();
    public static event WrongNumber OnWrongNumber;

    public static void WrongNumberMethod()
    {
        if (OnWrongNumber != null)
            OnWrongNumber();
    }

    public delegate void SquareSelected(int square_index);
    public static event SquareSelected OnSquareSelected;

    public static void SquareSelectedMethod(int square_index)
    {
        if (OnSquareSelected != null)
            OnSquareSelected(square_index);
    }

    public delegate void ResetBoard();
    public static event ResetBoard OnReset;

    public static void ResetBoardMethod()
    {
        if (OnReset != null)
            OnReset();
    }

    public delegate void ToggleNotes(bool notes_mode);
    public static event ToggleNotes OnToggleNotes;

    public static void ToggleNotesMethod(bool notes_mode)
    {
        if (OnToggleNotes != null)
            OnToggleNotes(notes_mode);
    }

    public delegate void PauseGame(bool paused);
    public static event PauseGame OnPauseGame;

    public static void PauseGameMethod(bool paused)
    {
        if (OnPauseGame != null)
            OnPauseGame(paused);
    }

    public delegate void GameEnded();
    public static event GameEnded OnGameEnded;

    public static void GameEndedMethod()
    {
        if (OnGameEnded != null)
            OnGameEnded();
    }

    public delegate void SettingUpdated(int setting);
    public static event SettingUpdated OnSettingUpdated;

    public static void SettingUpdatedMethod(int setting)
    {
        if (OnSettingUpdated != null)
            OnSettingUpdated(setting);
    }

    public delegate void Hint();
    public static event Hint OnHint;

    public static void HintMethod()
    {
        if (OnHint != null)
            OnHint();
    }

    public delegate void CheckSolution();
    public static event CheckSolution OnCheckSolution;

    public static void CheckSolutionMethod()
    {
        if (OnCheckSolution != null)
            OnCheckSolution();
    }
}
