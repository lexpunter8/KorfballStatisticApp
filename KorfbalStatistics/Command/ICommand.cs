using System;
namespace KorfbalStatistics.Command
{
    public interface ICommand
    {
        void Execute();
        void Redo();
        void Undo();
        string GetSnackBarText();

        bool IsCompleted { get; set; }
    }
}