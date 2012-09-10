namespace TeamNotification_Library.Service.ToolWindow
{
    public interface IGetToolWindowPosition
    {
        int Get(int x, int y, int w, int h, bool isDocked);
    }
}