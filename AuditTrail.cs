using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditTrail : MonoBehaviour
{
    public struct AuditTrailEntry
    {
        public int index;
        public int old_val;
        public int new_val;
        public bool notes_mode;
        public bool[] notes_at_index;
        public List<int> removed_notes;

        public AuditTrailEntry(int _index, int _old_val, int _new_val, bool _notes_mode, bool[] _notes_at_index, List<int> _removed_notes) : this()
        {
            this.index = _index;
            this.old_val = _old_val;
            this.new_val = _new_val;
            this.notes_mode = _notes_mode;
            this.notes_at_index = _notes_at_index;
            this.removed_notes = _removed_notes;
        }
    }

    private List<AuditTrailEntry> updates = new List<AuditTrailEntry>();

    void Update()
    {
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(updates.Count > 0)
                    UndoLastUpdate();
            }
        //}
    }

    public void UndoLastUpdate()
    {
        GameEvents.SquareSelectedMethod(updates[updates.Count - 1].index);
        GameEvents.UndoMethod(updates[updates.Count - 1].old_val, updates[updates.Count - 1].new_val, updates[updates.Count - 1].notes_at_index, updates[updates.Count - 1].removed_notes);
        updates.RemoveAt(updates.Count - 1);
    }

    public void OnEnable()
    {
        GameEvents.OnUpdateAuditTrail += OnUpdateAuditTrail;
        GameEvents.OnReset += OnReset;
    }

    public void OnDisable()
    {
        GameEvents.OnUpdateAuditTrail -= OnUpdateAuditTrail;
        GameEvents.OnReset -= OnReset;
    }

    // ON AUDIT TRAIL EVENT CALL ADD A NEW ENTRY TO THE AUDIT TRAIL
    public void OnUpdateAuditTrail(int index, int old_num, int new_num, bool notes, bool[] old_notes, List<int> removed_notes)
    {
        if (old_num != new_num || notes)
        {
            updates.Add(new AuditTrail.AuditTrailEntry(index, old_num, new_num, notes, old_notes, removed_notes));
        }
    }

    // CLEAR AUDIT TRAIL ON RESET
    public void OnReset()
    {
        for (int i = 0; i < updates.Count; i++)
        {
            updates.RemoveAt(i);
        }
    }
}
