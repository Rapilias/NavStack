using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace NavStack
{
    public interface IPage
    {
        /// <summary>
        /// Pageを有効にする際に操作するTransform
        /// </summary>
        [CanBeNull]
        UnityEngine.Transform transform { get; }

        /// <summary>
        /// <see cref="transform"/>に対する操作を行わないように要求する
        /// </summary>
        bool controlTransform { get; }

        UniTask OnNavigatedFrom(NavigationContext context, CancellationToken cancellationToken = default);
        UniTask OnNavigatedTo(NavigationContext context, CancellationToken cancellationToken = default);
    }
}
