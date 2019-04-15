using System.Collections.Generic;

namespace CSharpLab05
{
    internal abstract class BaseNavigationModel : INavigationModel
    {
        protected BaseNavigationModel(IContentOwner contentOwner)
        {
            ContentOwner = contentOwner;
            ViewsDictionary = new Dictionary<ViewType, INavigatable>();
        }

        protected IContentOwner ContentOwner { get; }

        protected Dictionary<ViewType, INavigatable> ViewsDictionary { get; }

        public void Navigate(ViewType viewType, ProcessClass selectedProcess)
        {
            if ((!ViewsDictionary.ContainsKey(viewType))||viewType==ViewType.ShowInfo)
                InitializeView(viewType, selectedProcess);
            ContentOwner.ContentControl.Content = ViewsDictionary[viewType];
        }

        protected abstract void InitializeView(ViewType viewType,ProcessClass selectedProcess);

    }
}
