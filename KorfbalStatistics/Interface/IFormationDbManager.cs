using KorfbalStatistics.Model;
using System;
using System.Collections.Generic;

namespace KorfbalStatistics.Interface
{
    public interface IFormationDbManager
    {
        void AddFormation(List<DbFormation> dbFormations);
        DbFormation[] GetFormationByGameId(Guid gameId);
        void UpdateFormation(List<Player> formation, Guid gameId);
    }
}