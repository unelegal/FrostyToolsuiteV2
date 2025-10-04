using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Dock.Model.ReactiveUI.Controls;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Alias;
using DynamicData.Binding;
using Frosty.Sdk.Managers.Entries;
using FrostyEditor.Models.DataExplorer;
using FrostyEditor.Services;
using FrostyEditor.Services.Implementation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Tools;

public sealed partial class DataExplorerViewModel : Tool, IDisposable
{
    private IProjectService m_projectService;
    public required IDockingService DockingService { private get; init; }

    public HierarchicalTreeDataGridSource<DataExplorerEntry> DataEntries { get; }

    public record PathNode(string Id, string ParentId, string Name, EntryType Type, string? EbxType, Guid? Guid);

    private readonly CompositeDisposable m_disposables = new();

    [ObservableAsProperty]
    private bool m_hasElements;

    public DataExplorerViewModel(IProjectService projectService)
    {
        m_projectService = projectService;

        Id = "DataExplorer";
        Title = "Data Explorer";
        CanClose = false;

        ReadOnlyObservableCollection<DataExplorerEntry> dataExplorerEntries;

        m_projectService.ConnectEbxList()
            .TransformMany(ExplodePath, node => node.Id)
            .TransformToTree(node => node.ParentId)
            .Transform(node => new DataExplorerEntry(node))
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out dataExplorerEntries)
            .SubscribeOn(RxApp.TaskpoolScheduler)
            .Subscribe()
            .DisposeWith(m_disposables);

        m_hasElementsHelper = dataExplorerEntries
            .ToObservableChangeSet()
            .Count()
            .Select(count => count > 0)
            .DistinctUntilChanged()
            .ToProperty(this, x => x.HasElements, scheduler: RxApp.MainThreadScheduler);

        DataEntries = new HierarchicalTreeDataGridSource<DataExplorerEntry>(dataExplorerEntries)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<DataExplorerEntry>(
                    new TemplateColumn<DataExplorerEntry>("Name", "NameCell",
                        options: new()
                        {
                            CompareAscending = (x, y) => string.Compare(x?.Name, y?.Name, StringComparison.Ordinal),
                            CompareDescending = (x, y) => string.Compare(y?.Name, x?.Name, StringComparison.Ordinal)
                        }), x => x.Children),
                new TextColumn<DataExplorerEntry, string>("Type", x => x.VisibleType)
            }
        };
        DataEntries.SortBy(DataEntries.Columns[0], ListSortDirection.Ascending);
    }

    private static IEnumerable<PathNode> ExplodePath(EbxAssetEntry entry)
    {
        if (string.IsNullOrWhiteSpace(entry.Name))
        {
            yield break;
        }

        var parts = entry.Name.Split('/', StringSplitOptions.RemoveEmptyEntries);

        string parent = string.Empty;
        for (int i = 0; i < parts.Length; i++)
        {
            var segmentName = parts[i];
            var isFile = i == parts.Length - 1;
            var id = parent.Length == 0 ? segmentName : $"{parent}/{segmentName}";
            var parentId = parent.Length == 0 ? string.Empty : parent;
            var nodeType = isFile ? EntryType.Ebx : EntryType.Folder;

            yield return new PathNode(id, parentId, segmentName, nodeType, isFile ? entry.Type : null, isFile ? entry.Guid : null);
            parent = id;
        }
    }

    [ReactiveCommand]
    private void OpenAsset(Guid guid)
    {
        DockingService.OpenAsset(guid);
    }

    public void Dispose() => m_disposables.Dispose();
}