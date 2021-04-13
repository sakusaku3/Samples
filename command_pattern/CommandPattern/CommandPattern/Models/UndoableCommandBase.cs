namespace CommandPattern.Models
{
    public abstract class UndoableCommandBase : CommandBase
    {
        public IRecreationTicket RecreationTicket { get; set; }
        public UndoableCommandBase() { }
        public abstract void Undo();
        public abstract void Redo();
    }
}
