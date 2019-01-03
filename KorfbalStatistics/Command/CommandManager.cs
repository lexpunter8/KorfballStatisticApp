using System.Collections.Generic;
using System.Linq;
namespace KorfbalStatistics.Command
{
    public class CommandManager
    {
        private List<ICommand> myCommandsToUndo = new List<ICommand>();
        private List<ICommand> myCommandsToRedo = new List<ICommand>();

        public bool HasPendingCommands => myCommandsToUndo.Any(c => !c.IsCompleted);

        public void AddCommand(ICommand command)
        {
            myCommandsToUndo.Add(command);
        }

        /// <summary>
        /// Execute all commands that haven't been completed
        /// </summary>
        public void ProccespendingCommands()
        {
            List<ICommand> pendingCommands = myCommandsToUndo.Where(c => !c.IsCompleted).ToList();
            pendingCommands.ForEach(pc => pc.Execute());
        }

        public void RemoveLastAction()
        {
            myCommandsToUndo.RemoveAt(myCommandsToUndo.Count - 1);
        }

        /// <summary>
        /// Get the latest performed command and undo, remove this command from the to undo-list 
        /// and add command to the to redo-list 
        /// </summary>
        public void UndoLastAction()
        {
            var lastCommand = myCommandsToUndo.LastOrDefault(c => c.IsCompleted);
            if (lastCommand == null)
                return;
            lastCommand.Undo();
            myCommandsToUndo.Remove(lastCommand);
            myCommandsToRedo.Add(lastCommand);
        }

        /// <summary>
        /// Get the latest performed undo-command and redo, remove this command from the to redo-list 
        /// and add command to the to undo-list 
        /// </summary>
        public void RedoLastUndoneAction()
        {
            var lastUndoneCommand = myCommandsToRedo.LastOrDefault();
            if (lastUndoneCommand == null)
                return;
            lastUndoneCommand.Execute();
            myCommandsToUndo.Add(lastUndoneCommand);
            myCommandsToRedo.Remove(lastUndoneCommand);

        }
    }
}