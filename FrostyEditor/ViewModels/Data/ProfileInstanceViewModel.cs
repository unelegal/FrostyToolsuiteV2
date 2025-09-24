using FrostyEditor.Models;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrostyEditor.ViewModels.Data;

public partial class ProfileInstanceViewModel : ReactiveObject
{
    [Reactive]
    private string m_slug = string.Empty;

    [Reactive]
    private string m_name = string.Empty;

    [Reactive]
    private string m_profileKey = string.Empty;

    [Reactive]
    private string m_gamePath = string.Empty;

    [Reactive]
    private byte[]? m_casKey;

    [Reactive]
    private byte[]? m_bundleKey;

    [Reactive]
    private byte[]? m_initFsKey;

    public ProfileInstanceViewModel()
    {
    }

    public ProfileInstanceViewModel(ProfileInstance profileInstance)
    {
        m_slug = profileInstance.Slug;
        m_name = profileInstance.Name;
        m_profileKey = profileInstance.ProfileKey;
        m_gamePath = profileInstance.GamePath;
        m_casKey = profileInstance.CasKey;
        m_bundleKey = profileInstance.BundleKey;
        m_initFsKey = profileInstance.InitFsKey;
    }

    public ProfileInstance ToModel()
    {
        return new ProfileInstance
        {
            Slug = m_slug,
            Name = m_name,
            ProfileKey = m_profileKey,
            GamePath = m_gamePath,
            CasKey = m_casKey,
            BundleKey = m_bundleKey,
            InitFsKey = m_initFsKey
        };
    }

    public override string ToString() => Name;
}