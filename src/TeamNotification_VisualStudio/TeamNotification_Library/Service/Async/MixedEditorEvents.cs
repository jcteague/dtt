using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class MixedEditorEvents : AbstractEventHandler, IHandleMixedEditorEvents
    {
        public event CustomEventHandler<CodeWasAppended> CodeWasAppended;
        public event CustomEventHandler<TextWasAppended> TextWasAppended;
        public event CustomEventHandler<DataWasPasted> DataWasPasted;

        public void OnCodeAppended(object source, CodeWasAppended eventArgs)
        {
            Handle(source, CodeWasAppended, eventArgs);
        }

        public void OnTextAppended(object source, TextWasAppended eventArgs)
        {
            Handle(source, TextWasAppended, eventArgs);
        }

        public void OnDataWasPasted(object source, DataWasPasted eventArgs)
        {
            Handle(source, DataWasPasted, eventArgs);
        }

        public void Clear()
        {
            CodeWasAppended = null;
            TextWasAppended = null;
            DataWasPasted = null;
        }
    }
}