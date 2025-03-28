using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NavStack
{
    public static class NavigationStackExtensions
    {        
        public static UniTask PushAsync(this INavigationStack navigationStack, IPage page, GameObject currentSelection, CancellationToken cancellationToken = default)
        {
            var context = new NavigationContext()
            {
                LastSelection = currentSelection,
            };
            return navigationStack.PushAsync(page, context, cancellationToken);
        }
        
        public static UniTask PushAsync(this INavigationStack navigationStack, Func<UniTask<IPage>> factory, GameObject currentSelection, CancellationToken cancellationToken = default)
        {
            var context = new NavigationContext()
            {
                LastSelection = currentSelection,
            };
            return navigationStack.PushAsync(factory, context, cancellationToken);
        }
        
        public static UniTask PopAsync(this INavigationStack navigationStack, GameObject currentSelection, CancellationToken cancellationToken = default)
        {
            var context = new NavigationContext()
            {
                LastSelection = currentSelection,
            };
            return navigationStack.PopAsync(currentSelection, cancellationToken);
        }
    }
}
