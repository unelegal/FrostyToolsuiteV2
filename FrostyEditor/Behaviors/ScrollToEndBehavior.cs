using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace FrostyEditor.Behaviors;

// Inspired by https://github.com/arnirichard/avalonia_scroll/pull/1

public class ScrollToEndBehavior : Behavior<ItemsControl>
{
    public static readonly StyledProperty<bool> ShouldAutoscrollProperty = AvaloniaProperty.Register<ScrollToEndBehavior, bool>(nameof(ShouldAutoscroll), true);

    private CompositeDisposable? m_disposables;
    private ScrollViewer? m_scrollViewer;

    public bool ShouldAutoscroll
    {
        get => GetValue(ShouldAutoscrollProperty);
        set => SetValue(ShouldAutoscrollProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        m_disposables = new CompositeDisposable();

        if (AssociatedObject is null)
        {
            return;
        }

        AssociatedObject.Events()
            .LayoutUpdated
            .Subscribe(_ => TryFindScrollViewer())
            .DisposeWith(m_disposables);

        if (AssociatedObject.Items is INotifyCollectionChanged collection)
        {
            collection.Events()
                .CollectionChanged
                .Where(e => e.Action == NotifyCollectionChangedAction.Add)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => ScrollToEnd())
                .DisposeWith(m_disposables);
        }

        this.GetObservable(ShouldAutoscrollProperty)
            .Where(enabled => enabled)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => ScrollToEnd())
            .DisposeWith(m_disposables);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        m_disposables?.Dispose();
        m_disposables = null;
    }

    private void TryFindScrollViewer()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        m_scrollViewer = AssociatedObject.GetSelfAndVisualDescendants()
            .OfType<ScrollViewer>()
            .FirstOrDefault();
    }

    private void ScrollToEnd()
    {
        if (!ShouldAutoscroll)
        {
            return;
        }

        if (m_scrollViewer is null)
        {
            TryFindScrollViewer();
            if (m_scrollViewer is null)
            {
                return;
            }
        }

        Dispatcher.UIThread.Post(() =>
        {
            if (m_scrollViewer is null)
            {
                return;
            }

            double targetY = Math.Max(0.0, m_scrollViewer.Extent.Height - m_scrollViewer.Viewport.Height);
            m_scrollViewer.Offset = new Vector(m_scrollViewer.Offset.X, targetY);
        }, DispatcherPriority.Background);
    }
}