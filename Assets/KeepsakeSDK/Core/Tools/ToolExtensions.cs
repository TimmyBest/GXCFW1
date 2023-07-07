using KeepsakeSDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace KeepsakeSDK.Core.Tools
{
    public class ToolExtensions
    {
        public static string FindHighestNumber(List<CoinDataOwned> coins)
        {
            if (coins == null || coins.Count == 0)
            {
                throw new ArgumentException("The array is null or empty.");
            }
            double highestNumber = double.Parse(coins[0].Data.Content.fields.Balance, CultureInfo.InvariantCulture.NumberFormat);
            string result = coins[0].Data.ObjectId;

            for (int i = 1; i < coins.Count; i++)
            {
                double currentNumber = double.Parse(coins[i].Data.Content.fields.Balance, CultureInfo.InvariantCulture.NumberFormat);

                if (currentNumber > highestNumber)
                {
                    highestNumber = currentNumber;
                    result = coins[i].Data.ObjectId;
                }
            }

            return result;
        }
    }
}
