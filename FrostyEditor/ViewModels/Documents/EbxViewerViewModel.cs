using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using Dock.Model.ReactiveUI.Controls;
using DynamicData;
using Frosty.Sdk.Ebx;
using Frosty.Sdk.Managers;
using Frosty.Sdk.Managers.Entries;
using FrostyEditor.Services;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Documents;

public partial class EbxViewerViewModel : Document
{
    [ObservableAsProperty]
    private bool m_isLoaded;

    [ObservableAsProperty]
    private string m_placeholderText = string.Empty;

    [ObservableAsProperty]
    private EbxPartition? m_currentEbxPartition;

    [Reactive]
    private Guid? m_currentGuid;

    public EbxViewerViewModel(IProjectService projectService)
    {
        var assetShared = this.WhenAnyValue(x => x.CurrentGuid)
            .Select(guid =>
            {
                if (guid is null)
                {
                    return Observable.Return<EbxAssetEntry?>(null);
                }

                return projectService.WatchAssetEntry(guid.Value)
                    .Select(change => change.Reason == ChangeReason.Remove ? null : change.Current);
            })
            .Switch()
            .DistinctUntilChanged()
            .Publish();

        assetShared
            .SubscribeOn(RxApp.TaskpoolScheduler)
            .Select(asset => asset is null ? null : AssetManager.GetEbxPartition(asset))
            .ToProperty(this, nameof(CurrentEbxPartition), out m_currentEbxPartitionHelper, scheduler: RxApp.MainThreadScheduler);

        assetShared.SubscribeOn(RxApp.MainThreadScheduler)
            .Subscribe(asset =>
            {
                Title = asset?.Filename ?? string.Empty;
                Id = asset?.Guid.ToString() ?? "NULL-" + Guid.NewGuid();
            });

        assetShared.Connect();

        this.WhenAnyValue(x => x.CurrentEbxPartition)
            .Select(partition =>
            {
                if (partition is null)
                {
                    return string.Empty;
                }

                StringBuilder sb = new();

                sb.AppendLine($"Partition GUID: {partition.PartitionGuid}");
                sb.AppendLine("Instances:");

                foreach (var instance in partition.Instances)
                {
                    Type actualType = instance.GetType();
                    PropertyInfo[] properties = actualType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    sb.AppendLine($"    + Instance GUID: {instance.GetInstanceGuid()} [{actualType.Name}]");

                    foreach (var property in properties)
                    {
                        object? value = property.GetValue(instance);
                        sb.AppendLine($"        - {property.Name}: {value}");
                    }
                }

                return sb.ToString();
            })
            .ToProperty(this, nameof(PlaceholderText), out m_placeholderTextHelper, scheduler: RxApp.MainThreadScheduler);

        this.WhenAnyValue(x => x.PlaceholderText)
            .Select(t => !string.IsNullOrEmpty(t))
            .DistinctUntilChanged()
            .ToProperty(this, nameof(IsLoaded), out m_isLoadedHelper);
    }
}