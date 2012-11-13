using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleMixedEditorEvents
    {
        event CustomEventHandler<CodeWasAppended> CodeWasAppended;

        void OnCodeAppend(object source, CodeWasAppended eventArgs);
        void Clear();
    }
}