using System.Threading;
using Cysharp.Threading.Tasks;

namespace NavStack
{
    public interface INavigationSheet : INavigation
    {
        void AssigneParent(IPage page);
        UniTask AddAsync(IPage page, CancellationToken cancellationToken = default);
        UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default);
        UniTask RemoveAllAsync(CancellationToken cancellationToken = default);
        UniTask ShowAsync(int index, NavigationContext context, CancellationToken cancellationToken = default);
        UniTask HideAsync(NavigationContext context, CancellationToken cancellationToken = default);
    }
}
