using System;
namespace KorfbalStatistics.Command
{
    public interface ICommand
    {
        void Execute();
        void Redo();
        void Undo();
        bool IsCompleted { get; set; }
    }
}