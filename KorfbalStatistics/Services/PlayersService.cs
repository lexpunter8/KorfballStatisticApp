using KorfbalStatistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KorfbalStatistics.Services
{
    public class PlayersService
    {
        private readonly PlayerDbManager myPlayerDbManager;

        public PlayersService(PlayerDbManager playerDbManager)
        {
            myPlayerDbManager = playerDbManager;
        }
        public List<DbPlayer> GetPlayersForTeamId(Guid teamId)
        {
            List<DbPlayer> allPlayer = new List<DbPlayer>(myPlayerDbManager.GetPlayersForTeamId(teamId));
            List<DbPlayer> players = allPlayer.Where(p => p.TeamId.Equals(teamId)).ToList();
            if (players.Count < 1)
            {
                return allPlayer;
            }

            return players.ToList();
        }
        public string GetPlayerNameById(Guid playerId)
        {
            DbPlayer player = myPlayerDbManager.GetPlayerById(playerId);
            return player.FirstName;
        }

        internal Player GetUnkownPlayer()
        {
            DbPlayer unkownDbPlayer = myPlayerDbManager.GetPlayerById(Guid.Parse("6467eac9-0164-4041-adc6-4b7b038c1a7d"));
            return new Player
            {
                FirstName = "Onbekend",
                Id = unkownDbPlayer.Id,
                Number = -1
            };
        }
    }
}