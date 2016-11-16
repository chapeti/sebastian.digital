using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal.Models;

namespace Personal.Services.Contracts
{
    public interface ITeamWorkService
    {
        TeamWorkResume GeTeamWorkResume(TeamWorkIdentity teamWorkIdentity);
    }
}
