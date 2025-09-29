using System;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace FrostyEditor.Services;

public interface IAppFlowService
{
    public const string AppFlowTag = "AppFlow";

    public class FlowId
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }

    public enum FlowState
    {
        ProjectScreen,
        LoadingSplash,
        Editor
    }

    public void Init(string? projectPath);

    public FlowState GetCurrentFlowState();

    public Task SwitchToLoadingSplash();

    public Task SwitchToEditor();

}