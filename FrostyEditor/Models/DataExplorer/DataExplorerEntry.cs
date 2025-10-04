using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using FrostyEditor.ViewModels.Tools;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.Models.DataExplorer;

public enum EntryType
{
    Folder,
    Ebx
}

public sealed partial class DataExplorerEntry : ReactiveObject, IDisposable
{
    [Reactive]
    private string m_name;

    [Reactive]
    private string m_path;

    [Reactive]
    private EntryType m_logicalType;

    [Reactive]
    private string? m_visibleType;

    [Reactive]
    private Guid? m_guid;

    public ReadOnlyObservableCollection<DataExplorerEntry>? Children { get; private set; }

    private readonly CompositeDisposable m_disposables = new();

    public DataExplorerEntry(Node<DataExplorerViewModel.PathNode, string> node)
    {
        Name = node.Item.Name;
        Path = node.Item.Id;
        LogicalType = node.Item.Type;

        if (LogicalType == EntryType.Ebx)
        {
            VisibleType = node.Item.EbxType ?? "Unknown Type";
            Guid = node.Item.Guid;
        }
        else
        {
            VisibleType = "";
        }

        node.Children
            .Connect()
            .Transform(n => new DataExplorerEntry(n))
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out var children)
            .Subscribe()
            .DisposeWith(m_disposables);

        Children = children;
    }

    public void Dispose() => m_disposables.Dispose();
}