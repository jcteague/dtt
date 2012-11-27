using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleMixedEditorEvents
    {
        event CustomEventHandler<CodeWasAppended> CodeWasAppended;
        event CustomEventHandler<TextWasAppended> TextWasAppended;
        event CustomEventHandler<DataWasPasted> DataWasPasted;

        void OnCodeAppended(object source, CodeWasAppended eventArgs);
        void OnTextAppended(object source, TextWasAppended eventArgs);
        void OnDataWasPasted(object source, DataWasPasted eventArgs);
        void Clear();
    }
}