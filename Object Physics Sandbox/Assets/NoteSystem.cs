using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteSystem : MonoBehaviour
{
    private static TMP_InputField noteInputField;
    private static ScrollRect verticalScrollRect;
    public static string notesOutput;
    public TMP_Text printNotes;

    void Start()
    {
        noteInputField = GameObject.FindGameObjectWithTag("notesInput").GetComponent<TMP_InputField>();
        verticalScrollRect = GetComponent<ScrollRect>();
        notesOutput = "Saved Notes Appear Here";
    }
    void Update()
    {
        // what goes out
        printNotes.text = notesOutput;
        
        if (Input.GetKeyUp(KeyCode.Return))
        {
            grabNote();
        }
    }

    public static void grabNote()
    {
        if (noteInputField.text != "")
        {
            ActivityLogger.saveNote(noteInputField.text);
            noteInputField.text = "";
            // show it the way it is now
            updateOutNotes(ActivityLogger.getNotesList());
        }
    }

    // can also be updated during playback
    public static void updateOutNotes(List<string> notesList)
    {
        notesOutput = "";

        // displays whatever is passed as noteslist
        for (int i = notesList.Count-1; i >= 0; i--)
        {
            notesOutput += notesList[i];
            notesOutput += "\n\n";
        }
    }
}
