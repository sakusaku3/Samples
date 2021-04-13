using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandPattern.Models
{
    public abstract class CommandOwnerBase
    {
		private readonly Stack<UndoableCommandBase> undoStack = new();

		private readonly Stack<UndoableCommandBase> redoStack = new();

		public void Execute(CommandBase command)
        {
			this.ExecuteCore(command.Execute);
            this.ChangeState();
            this.ResetStacks();
        }

		public void Execute(UndoableCommandBase command)
        {
			this.ExecuteCore(command.Execute);
			command.RecreationTicket = this.GetRecreationTicket();
			this.ChangeState();
			this.redoStack.Clear();
			this.undoStack.Push(command);
        }

		private void ExecuteCore(Action execution)
        {
            this.BeforeCommandExecute();
			execution.Invoke();
            this.AfterCommandExecute();
        }

		public void Undo()
        {
			if (!this.undoStack.Any()) return;
			var command = this.undoStack.Pop();
			command.Undo();
			this.RecreateSituation(command.RecreationTicket);
			this.redoStack.Push(command);
        }

		public void Redo()
        {
			if (!this.redoStack.Any()) return;
			var command = this.redoStack.Pop();
			this.ExecuteCore(command.Redo);
            this.ChangeState();
			this.RecreateSituation(command.RecreationTicket);
			this.undoStack.Push(command);
        }

		public void ResetStacks()
        {
			this.undoStack.Clear();
			this.redoStack.Clear();
        }

		/// <summary>
		/// 再現用のチケットを取得する
		/// </summary>
		/// <returns></returns>
		protected abstract IRecreationTicket GetRecreationTicket();

		/// <summary>
		/// 状況を再現する
		/// </summary>
		/// <param name="recreationTicket">再現用チケット</param>
		protected abstract void RecreateSituation(IRecreationTicket recreationTicket);

		/// <summary>
		/// コマンド実行直前に呼び出す
		/// </summary>
		protected abstract void BeforeCommandExecute();

		/// <summary>
		/// コマンド実行直後に呼び出す
		/// </summary>
		protected abstract void AfterCommandExecute();

		/// <summary>
		/// 状態を変化させたときに呼び出す
		/// </summary>
		protected abstract void ChangeState();
    }
}
