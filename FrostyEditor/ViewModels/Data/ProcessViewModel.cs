using System.Diagnostics;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Data;

public partial class ProcessViewModel : ViewModelBase
{
    [Reactive]
    private int m_pid;

    [Reactive]
    private string m_name;

    public ProcessViewModel(Process process)
    {
        m_pid = process.Id;
        m_name = process.ProcessName;
    }
}