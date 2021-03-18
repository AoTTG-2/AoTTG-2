using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MapEditor
{
    //An abstract class that defines functions to do and undo edits
    public abstract class EditCommand
    {
        public abstract void ExecuteEdit();
        public abstract void RevertEdit();
    }

    //Keeps a history of the executed commands and enables undo/redo
    public class EditHistory : MonoBehaviour
    {
        #region Data Members

        //A self-reference to the singleton instance of this script
        public static EditHistory Instance { get; private set; }

        //Stores the commands that were executed and reverted
        private Stack<EditCommand> executedCommands;
        private Stack<EditCommand> revertedCommands;

        //Shortcut classes to set the shortcut combination and check when its pressed
        [SerializeField] private Shortcut undoShortcut;
        [SerializeField] private MultiShortcut redoShortcut;

        //Lists the shortcuts that can be held down
        private enum ShortcutCommand
        {
            None,
            Undo,
            Redo
        }

        //The duration in milliseconds that a command has to be held down until it is repeated
        [SerializeField] private float commandRepeatDelay = 500f;

        //The number of times the command is repeated per second
        [SerializeField] private float commandRepeatRate = 5f;

        //The shortcut currently held down
        private ShortcutCommand heldCommand;

        //Used to determine how long the command has been held
        private Stopwatch stopWatch = new Stopwatch();

        //Determines if the held command it repeating or not
        private bool commandRepeating = false;

        //The delay in seconds between each execution of the command
        private float commandExecutionDelay;

        #endregion

        #region MonoBehaviour Methods

        void Awake()
        {
            //Set this script as the only instance of the EditorManger script
            if (Instance == null)
                Instance = this;

            //Initialize the command stacks
            executedCommands = new Stack<EditCommand>();
            revertedCommands = new Stack<EditCommand>();

            //Calculate he delay in seconds between repeated commands
            commandExecutionDelay = 1f / commandRepeatRate;
        }

        private void Update()
        {
            CheckShortcutPressed();
            CheckShortcutReleased();
            CheckRepeatCommand();
        }

        #endregion

        #region Shortcut Methods

        //Check if a command shortcut was pressed
        private void CheckShortcutPressed()
        {
            if (undoShortcut.Pressed())
            {
                heldCommand = ShortcutCommand.Undo;
                stopWatch.Restart();
                Instance.Undo();
            }
            else if (redoShortcut.Pressed())
            {
                heldCommand = ShortcutCommand.Redo;
                stopWatch.Restart();
                Instance.Redo();
            }
        }

        //Check when a command shortcut is released
        private void CheckShortcutReleased()
        {
            //If no command was pressed, do nothing
            if (heldCommand == ShortcutCommand.None)
                return;

            //If the pressed shortcut is still held, do nothing
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand))
            {
                if (heldCommand == ShortcutCommand.Undo &&
                    (Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.LeftShift)))
                    return;

                if (heldCommand == ShortcutCommand.Redo &&
                    (Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift)))
                    return;
            }

            //If the held shortcut was released, reset the timer and stop repeating the command
            heldCommand = ShortcutCommand.None;
            commandRepeating = false;
            stopWatch.Reset();
        }

        //Check if the held command should be repeated
        private void CheckRepeatCommand()
        {
            //If the command is not repeating and a shortcut is held, check if the command should repeat
            if (!commandRepeating && heldCommand != ShortcutCommand.None)
            {
                //Stop the stopwatch so that the elapsed time can be read
                stopWatch.Stop();

                //If the shortcut has been held for longer than the repeat delay, start repeating the command
                if (stopWatch.ElapsedMilliseconds >= commandRepeatDelay)
                {
                    stopWatch.Reset();
                    commandRepeating = true;
                    StartCoroutine(RepeatCommand());
                }
                //Otherwise, continue the stopwatch
                else
                    stopWatch.Start();
            }
        }

        //Continue to repeat the held command until repeating is disabled
        private IEnumerator RepeatCommand()
        {
            //Execute the held command
            if (heldCommand == ShortcutCommand.Undo)
                Instance.Undo();
            else if (heldCommand == ShortcutCommand.Redo)
                Instance.Redo();

            //Wait for a delay before executing the next command
            yield return new WaitForSeconds(commandExecutionDelay);

            //If the shortcut is still held, execute the command again
            if (commandRepeating)
                StartCoroutine(RepeatCommand());
        }

        #endregion

        #region Command Methods

        //Add a command to the history
        public void AddCommand(EditCommand newCommand)
        {
            executedCommands.Push(newCommand);
            revertedCommands.Clear();
        }

        //Clear the history of executed and reverted commands
        public void ResetHistory()
        {
            executedCommands = new Stack<EditCommand>();
            revertedCommands = new Stack<EditCommand>();
        }

        //Undo the changes made in the last command
        public void Undo()
        {
            //If no commands have been executed yet, return
            if (executedCommands.Count == 0)
                return;

            EditCommand lastExecuted = executedCommands.Pop();
            lastExecuted.RevertEdit();
            revertedCommands.Push(lastExecuted);
        }

        //Reapply the changes that were last reverted
        public void Redo()
        {
            //If no commands were reverted, return
            if (revertedCommands.Count == 0)
                return;

            EditCommand lastReverted = revertedCommands.Pop();
            lastReverted.ExecuteEdit();
            executedCommands.Push(lastReverted);
        }

        #endregion
    }
}