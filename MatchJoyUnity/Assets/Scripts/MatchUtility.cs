namespace Assets.Scripts {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Assets.Scripts.Components;

    /// <summary>
    /// An enum for colors.
    /// </summary>
    public enum MatchColor {
        Blue = 0,
        Red = 1,
        Yellow = 2
    }

    /// <summary>
    /// An enum for symbols.
    /// </summary>
    public enum MatchSymbol {
        At = 0,
        Plus = 1,
        Slash = 2
    }

    /// <summary>
    /// Utility class for dealing with an detecting matches.
    /// </summary>
    public static class MatchUtility {

        /// <summary>
        /// Checks three GameTiles for a match.
        /// </summary>
        /// <param name="matchTiles">An array of match tiles (of size 3).</param>
        /// <returns>A value indicating whether or not this is a match.</returns>
        public static bool CheckMatch(MatchSet[] matchTiles) {
            return matchTiles.Length == 3 &&
                ((int)matchTiles[0].color + (int)matchTiles[1].color + (int)matchTiles[2].color) % 3 == 0 &&
                (matchTiles[0].number + matchTiles[1].number + matchTiles[2].number) % 3 == 0 &&
                ((int)matchTiles[0].symbol + (int)matchTiles[1].symbol + (int)matchTiles[2].symbol) % 3 == 0;
        }

        /// <summary>
        /// Gets the match set associated with the two match sets passed in.
        /// </summary>
        /// <param name="first">The first match set.</param>
        /// <param name="second">The second match set.</param>
        /// <returns></returns>
        public static MatchSet GetMatch(MatchSet first, MatchSet second) {
            MatchSet third = new MatchSet();

            for (int color = 0; color < 3; color++) {
                if ((color + (int)first.color + (int)second.color % 3) == 0)
                    third.color = (MatchColor)color;
            }

            for (int number = 0; number < 3; number++) {
                if ((number + first.number + second.number % 3) == 0)
                    third.number = number;
            }

            for (int symbol = 0; symbol < 3; symbol++) {
                if ((symbol + (int)first.symbol + (int)second.symbol % 3) == 0)
                    third.symbol = (MatchSymbol)symbol;
            }

            return third;
        }

        /// <summary>
        /// Combines all the peroperties of a MatchSet as ints.
        /// </summary>
        /// <param name="matchSet">The matchset to combine.</param>
        /// <returns>The combined value of all of its properties.</returns>
        private static int CombineMatchProperties(MatchSet matchSet) {
            return (int)matchSet.color + matchSet.number + (int)matchSet.symbol;
        }
    }
}