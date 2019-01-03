using System;

namespace KorfbalStatistics.Model
{
    public interface IPlayerDbManager
    {
        void AddPlayer(DbPlayer player);
        DbPlayer GetPlayerById(Guid playerId);
        DbPlayer[] GetPlayersForTeamId(Guid teamId);
        Guid GetTeamIdByUserId(Guid userId);
        void RemovePlayer(DbPlayer player);
        void UpdatePlayer(DbPlayer player);
    }
}