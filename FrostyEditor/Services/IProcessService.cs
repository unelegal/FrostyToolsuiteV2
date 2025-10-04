using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DynamicData;
using FrostyEditor.ViewModels.Data;

namespace FrostyEditor.Services;

public interface IProcessService
{
    public IObservable<IChangeSet<Process, int>> ConnectProcesses();
}