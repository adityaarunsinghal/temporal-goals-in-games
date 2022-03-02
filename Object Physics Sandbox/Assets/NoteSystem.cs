using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteSystem : MonoBehaviour
{
    private static TMP_InputField noteInputField;
    public static string notesOutput;
    public TMP_Text printNotes;

    void Start()
    {
        noteInputField = GameObject.FindGameObjectWithTag("notesInput").GetComponent<TMP_InputField>();
        notesOutput = "Saved Notes Appear Here";
    }
    void Update()
    {
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
            updateOutNotes(ActivityLogger.getNotesList());
        }
    }

    // can also be updated during playback
    public static void updateOutNotes(List<string> notesList)
    {
        notesOutput = "";
        for (int i = 0; i < notesList.Count; i++)
        {
            notesOutput += notesList[i];
            notesOutput += "\n\n";
        }
    }
}
