using System.Windows.Controls;

namespace CSharpLab05
{
    internal interface IContentOwner
    {
        ContentControl ContentControl { get; }
    }
}
