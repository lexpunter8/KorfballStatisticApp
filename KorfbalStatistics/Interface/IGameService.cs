using System;
using KorfbalStatistics.Model;

namespace KorfbalStatistics.Interface
{
    public interface IGameService
    {
        void AddAttackToDb(IAttack attack);
        void RemoveAttackFromDb(IAttack attack);
        DbGoalType GetGoalTypeById(Guid goalTypeId);
    }
}