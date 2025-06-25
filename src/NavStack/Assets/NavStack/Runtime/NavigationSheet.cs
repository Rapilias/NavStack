using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NavStack.Internal;

namespace NavStack
{
    public class NavigationSheet : INavigationSheet
    {
        readonly NavigationSheetCore core = new();

        public IPage ActivePage => core.ActivePage;
        public IReadOnlyCollection<IPage> Pages => core.Pages;

        public event Action<IPage> OnPageAttached
        {
            add => core.OnPageAttached += value;
            remove => core.OnPageAttached -= value;
        }

        public event Action<IPage> OnPageDetached
        {
            add => core.OnPageDetached += value;
            remove => core.OnPageDetached -= value;
        }

        public event Action<(IPage Previous, IPage Current)> OnNavigateStart
        {
            add => core.OnNavigateStarted += value;
            remove => core.OnNavigateStarted -= value;
        }
        
        public event Action<(IPage Previous, IPage Current)> OnNavigating
        {
            add => core.OnNavigating += value;
            remove => core.OnNavigating -= value;
        }

        public event Action<(IPage Previous, IPage Current)> OnNavigated
        {
            add => core.OnNavigated += value;
            remove => core.OnNavigated -= value;
        }
        public event Action OnTransitionStart = delegate { };
        public event Action OnTransitionFinished = delegate { };
        
        public bool isTransitioning { get; private set; } = false;
        
        public UniTask AddAsync(IPage page, CancellationToken cancellationToken = default)
        {
            return core.AddAsync(page, cancellationToken);
        }

        public async UniTask HideAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.OnTransitionStart.Invoke();
            this.isTransitioning = true;
            await core.HideAsync(context, cancellationToken);
            this.isTransitioning = false;
            this.OnTransitionFinished.Invoke();
        }

        public async UniTask RemoveAllAsync(CancellationToken cancellationToken = default)
        {
            this.OnTransitionStart.Invoke();
            this.isTransitioning = true;
            await core.RemoveAllAsync(cancellationToken);
            this.isTransitioning = false;
            this.OnTransitionFinished.Invoke();
        }

        public async UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default)
        {
            this.OnTransitionStart.Invoke();
            this.isTransitioning = true;
            await core.RemoveAsync(page, cancellationToken);
            this.isTransitioning = false;
            this.OnTransitionFinished.Invoke();
        }

        public async UniTask ShowAsync(int index, NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.OnTransitionStart.Invoke();
            this.isTransitioning = true;
            await core.ShowAsync(index, context, cancellationToken);
            this.isTransitioning = false;
            this.OnTransitionFinished.Invoke();
        }
    }
}
