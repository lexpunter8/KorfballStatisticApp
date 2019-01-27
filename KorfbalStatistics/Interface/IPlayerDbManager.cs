using KorfbalStatistics.Model;
using System;

namespace KorfbalStatistics.Interface
{
    public interface IPlayerDbManager
    {
        void AddPlayer(DbPlayer player);
        DbPlayer GetPlayerById(Guid playerId);
        DbPlayer[] GetPlayersForTeamId(Guid teamId);
        void RemovePlayer(DbPlayer player);
        void UpdatePlayer(DbPlayer player);
    }
}