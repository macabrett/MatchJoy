namespace Assets.Scripts.Components {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Scripts.Events;
    using BrettMStory.Events;
    using BrettMStory.Linq;
    using UnityEngine;

    /// <summary>
    /// The game board class.
    /// </summary>
    public class GameBoard : MonoBehaviour {

        /// <summary>
        /// All inactive match tiles in the entire game.
        /// </summary>
        private readonly HashSet<MatchSet> _inactiveMatchSets = new HashSet<MatchSet>();

        /// <summary>
        /// The 9 match tiles currently on the board.
        /// </summary>
        private Tile[] _tiles;

        /// <summary>
        /// Called on successful match.
        /// </summary>
		public event EventHandler<EventArgs> SuccessfulMatch;

        /// <summary>
        /// Called on an unsuccessful match.
        /// </summary>
		public event EventHandler<EventArgs> UnsuccessfulMatch;

        /// <summary>
        /// The awake call. 
        /// </summary>
        protected void Awake() {
            var sprites = Resources.LoadAll<Sprite>("Sprites/MatchSets");

            for (var c = 0; c < 3; c++) {
                for (var n = 0; n < 3; n++) {
                    for (var s = 0; s < 3; s++) {
                        var matchSet = new MatchSet {
                            color = (MatchColor)c,
                            number = n,
                            symbol = (MatchSymbol)s,
                            sprite = sprites.FirstOrDefault(x => x.name == string.Concat(c.ToString(), n.ToString(), s.ToString()))
                        };

                        this._inactiveMatchSets.Add(matchSet);
                    }
                }
            }

        }

        /// <summary>
        /// The start call.
        /// </summary>
        protected void Start() {
            Navigator.Instance.Navigate += this.OnNavigate;

            this._tiles = this.GetComponentsInChildren<Tile>();

            for (var i = 0; i < this._tiles.Length; i++) {
                this._tiles[i].IsSelectedChanged -= this.OnIsSelectedChange;
                this._tiles[i].IsSelected = true;
                this._tiles[i].IsSelectedChanged += this.OnIsSelectedChange;
            }

            this.ResetTiles();
        }

		/// <summary>
		/// Called when IsSelected on any tile is changed.
		/// </summary>
		/// <param name="tile">The tile that changed.</param>
		private void OnIsSelectedChange(object sender, EventArgs e) {
            var tile = sender as Tile;

			if (tile == null || !tile.IsSelected)
				return;
			
			var selectedTiles = this._tiles.Where(x => x.IsSelected).ToList();
			if (selectedTiles.Count != 3)
				return;
			
			var matches = selectedTiles.Select(x => x.CurrentMatchSet).ToArray();
			if (MatchUtility.CheckMatch(matches)) {
                AudioController.Instance.PlayerSuccessClip();
                this.ResetTiles(selectedTiles);
				this.SuccessfulMatch.SafeInvoke(this, new EventArgs());
			} else {
                AudioController.Instance.PlayFailureClip();

				foreach (var selectedTile in selectedTiles) {
					selectedTile.ShakeTile();
				}
				
				this.UnsuccessfulMatch.SafeInvoke(this, new EventArgs());
			}
		}

        /// <summary>
        /// Raises the navigate event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnNavigate(object sender, NavigationEventArgs e) {
            if (e.NextViewType == ViewType.Gameboard) {
                for (var i = 0; i < this._tiles.Length; i++) {
                    this._tiles[i].IsSelectedChanged -= this.OnIsSelectedChange;
                    this._tiles[i].IsSelected = true;
                    this._tiles[i].IsSelectedChanged += this.OnIsSelectedChange;
                }

                this.ResetTiles();
            }
        }

        /// <summary>
        /// Called when a match set is released.
        /// </summary>
        /// <param name="matchSet">The matchset that is released.</param>
        private void OnRelease(MatchSet matchSet) {
            this._inactiveMatchSets.Add(matchSet);
        }

		/// <summary>
		/// Raises the restart event.
		/// </summary>
		private void OnRestart(object sender, EventArgs e) {
			for (var i = 0; i < this._tiles.Length; i++) {
				this._tiles[i].IsSelectedChanged -= this.OnIsSelectedChange;
				this._tiles[i].IsSelected = true;
				this._tiles[i].IsSelectedChanged += this.OnIsSelectedChange;
			}
			
			this.ResetTiles();
		}

		/// <summary>
		/// Resets the tiles.
		/// </summary>
		private void ResetTiles() {
			this.ResetTiles(this._tiles.ToList());
		}

		/// <summary>
		/// Resets the tiles.
		/// </summary>
		/// <param name="tiles">Tiles.</param>
		private void ResetTiles(IList<Tile> tiles) {
			// Needs to be at least 3
			if (tiles.Count < 3)
				return;

			// Random so player cannot predict which tile will be used to guarantee a match exists
			tiles.Randomize();

			// Quick way to copy over array so we don't reuse any of the tiles as they're removed.
			var availableMatchSets = this._inactiveMatchSets.ToList();
			availableMatchSets.Randomize();

			for (var i = 0; i < tiles.Count - 1; i++) {
				var matchSet = availableMatchSets.First();
				availableMatchSets.Remove(matchSet);

				if (tiles[i].CurrentMatchSet.sprite != null)
					this._inactiveMatchSets.Add(tiles[i].CurrentMatchSet);

				this._inactiveMatchSets.Remove(matchSet);
				tiles[i].DisappearTile(matchSet);
			}

			var guaranteedMatchSet = MatchUtility.GetMatch(tiles[0].CurrentMatchSet, tiles[1].CurrentMatchSet);
			var guaranteedMatchSetEnumerable = availableMatchSets.Where(x => x.color == guaranteedMatchSet.color && x.number == guaranteedMatchSet.number && x.symbol == guaranteedMatchSet.symbol);
			var lastMatchSet = guaranteedMatchSetEnumerable.Count() > 0 ? guaranteedMatchSetEnumerable.First() : availableMatchSets.First();

			if (tiles.Last().CurrentMatchSet.sprite != null)
				this._inactiveMatchSets.Add(tiles.Last().CurrentMatchSet);

			tiles.Last().DisappearTile(lastMatchSet);
			this._inactiveMatchSets.Remove(lastMatchSet);
		}
    }
}
