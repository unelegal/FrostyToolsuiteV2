using System.Collections.Generic;
using System.Threading.Tasks;
using FrostyEditor.Models;

namespace FrostyEditor.Services.Implementation;

public class ProfileService : IProfileService
{
    public async Task<IEnumerable<ProfileInstance>> GetProfilesAsync()
    {
        return
        [
            new ProfileInstance { Slug = "battlefield6", Name = "Battlefield 6", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "bf6event" },
            new ProfileInstance { Slug = "bf2042", Name = "Battlefield 2042", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "BF2042" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" },
            new ProfileInstance { Slug = "whatever", Name = "Cool Game", GamePath = "C:\\Game\\Path\\Game.exe", ProfileKey = "coolgame" }
        ];
    }
}