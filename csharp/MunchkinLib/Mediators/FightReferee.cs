using MunchkinLib.Models;
using System.Collections.Generic;

namespace MunchkinLib.Mediators
{
    public class FightReferee
    {
        //TODO
        public static bool CheckFight(Player player, List<BaseCard> monsters)
        {
            int level = player.Level + player.BonusCount + player.ArmorBonusCount;

            int monsterPower = 0;
            foreach(var monster in monsters)
            {
                monsterPower += monster.Bonus;
            }

            return level >= monsterPower;
        }

        //TODO
        public static bool CheckFightWithHelper(Player player, Player helper, List<BaseCard> monsters)
        {
            bool result = false;
            helper.Helper = true;


            helper.Helper = false;
            return result;
        }
    }
}
