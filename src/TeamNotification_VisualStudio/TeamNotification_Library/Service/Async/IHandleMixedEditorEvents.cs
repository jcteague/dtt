using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleMixedEditorEvents
    {
        event CustomEventHandler<CodeWasAppended> CodeWasAppended;
        event CustomEventHandler<DataWasPasted> DataWasPasted;

        void OnCodeAppend(object source, CodeWasAppended eventArgs);
        void OnDataWasPasted(object source, DataWasPasted eventArgs);
        void Clear();
    }
}