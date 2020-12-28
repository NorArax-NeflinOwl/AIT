using MunchkinLib.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MunchkinLib.Helpers
{
    public class Randomizer
    {
        public static void Shuffling(List<BaseCard> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                BaseCard value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool ThrowDice(int bonus)
        {
            Random random = new Random();
            int randomNumber = random.Next(GameProperties.MinDiceScore, GameProperties.MaxDiceScore);

            return randomNumber + bonus >= GameProperties.MinToWinThrowDice;
        }
    }
}
