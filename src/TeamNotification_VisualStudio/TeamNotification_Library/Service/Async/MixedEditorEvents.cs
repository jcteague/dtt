using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class MixedEditorEvents : AbstractEventHandler, IHandleMixedEditorEvents
    {
        public event CustomEventHandler<CodeWasAppended> CodeWasAppended;

        public void OnCodeAppend(object source, CodeWasAppended eventArgs)
        {
            Handle(source, CodeWasAppended, eventArgs);
        }

        public void Clear()
        {
            CodeWasAppended = null;
        }
    }
}