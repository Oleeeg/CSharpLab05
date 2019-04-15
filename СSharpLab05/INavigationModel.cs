namespace CSharpLab05
{
    internal enum ViewType
    {
        TaskManager,
        ShowInfo
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType,ProcessClass selectedProcess);
    }
}
