namespace Assets.Scripts.Components {
	using System;
	using System.Collections;

	using Assets.Scripts.Events;
    using UnityEngine;

    /// <summary>
    /// The scoreboard.
    /// </summary>
    public class Scoreboard : MonoBehaviour {

		/// <summary>
		/// The high score key for player prefs.
		/// </summary>
		private const string HIGH_SCORE_PLAYER_PREFS = "HighScore";

        /// <summary>
        /// The number of digits allowed for the scoreboard.
        /// </summary>
        private const int NUM_DIGITS = 3;

        /// <summary>
        /// The sprite renderers.
        /// </summary>
        private readonly SpriteRenderer[] _spriteRenderers = new SpriteRenderer[NUM_DIGITS];

		/// <summary>
		/// The best highscore.
		/// </summary>
		[SerializeField]
		private Highscore _bestHighscore;

		/// <summary>
		/// The current highscore.
		/// </summary>
		[SerializeField]
		private Highscore _currentHighscore;

        /// <summary>
        /// The gameboard.
        /// </summary>
        private GameBoard _gameBoard;

        /// <summary>
        /// The current score.
        /// </summary>
        private int _score = 0;

        /// <summary>
        /// Gets the score.
        /// </summary>
        public int Score {
            get {
                return this._score;
            }

            private set {
                if (value > 999)
                    this._score = 999;
                else if (value < 0)
                    this._score = 0;
                else
                    this._score = value;

                this.SetScore(this._score.ToString());
            }
        }

        /// <summary>
        /// The awake call.
        /// </summary>
        protected void Awake() {
            for (int i = 0; i < NUM_DIGITS; i++) {
                var spriteGameObject = new GameObject(i.ToString());
                spriteGameObject.SetActive(true);
                spriteGameObject.transform.parent = this.transform;
                spriteGameObject.transform.position = new Vector3(this.transform.position.x - i * 0.5f, this.transform.position.y, 0f);
                spriteGameObject.transform.parent = this.transform;
                var spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = PixelFont.Instance.GetCharacter('0');
                this._spriteRenderers[i] = spriteRenderer;
            }
        }

        /// <summary>
        /// The on disable call.
        /// </summary>
        protected void OnDisable() {
            if (this._gameBoard == null)
                this._gameBoard = GameObject.FindObjectOfType<GameBoard>();

            if (this._gameBoard != null) {
                this._gameBoard.SuccessfulMatch -= this.OnSuccessfulMatch;
                this._gameBoard.UnsuccessfulMatch -= this.OnUnsuccessfulMatch;
            }
        }

        /// <summary>
        /// The on enable call.
        /// </summary>
        protected void OnEnable() {
            if (this._gameBoard == null)
                this._gameBoard = GameObject.FindObjectOfType<GameBoard>();

            if (this._gameBoard != null) {
                this._gameBoard.SuccessfulMatch += this.OnSuccessfulMatch;
                this._gameBoard.UnsuccessfulMatch += this.OnUnsuccessfulMatch;
            }
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected void Start() {
			Navigator.Instance.Navigate += this.OnNavigate;
		}

        /// <summary>
        /// Raises the navigate event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
		private void OnNavigate(object sender, NavigationEventArgs e) {
			if (e.NextViewType == ViewType.Gameboard)
				this.Score = 0;
			else if (e.NextViewType == ViewType.End && e.PreviousViewType == ViewType.Gameboard)
				this.TryAddHighScore();
		}

        /// <summary>
        /// Called on successful match. Increments score.
        /// </summary>
        private void OnSuccessfulMatch(object sender, EventArgs e) {
            this.Score += 3;
        }

        /// <summary>
        /// Called on unsuccessful match. Decrements score.
        /// </summary>
        private void OnUnsuccessfulMatch(object sender, EventArgs e) {
            this.Score--;
        }

        /// <summary>
        /// Sets the score on the GUI.
        /// </summary>
        private void SetScore(string score) {
            var length = score.Length;

            if (length > NUM_DIGITS) {
                return; //PANIC OH NO
            }

            var j = 0;
            for (int i = length - 1; i >= 0 && j < NUM_DIGITS; i--) {
                this._spriteRenderers[j].sprite = PixelFont.Instance.GetCharacter(score[i]);
                j++;
            }

            for (int i = NUM_DIGITS - 1; i > length - 1; i--) {
                this._spriteRenderers[i].sprite = PixelFont.Instance.GetCharacter('0');
            }
        }

		private void TryAddHighScore() {
			var previousHighScore = PlayerPrefs.HasKey(HIGH_SCORE_PLAYER_PREFS) ? PlayerPrefs.GetInt(HIGH_SCORE_PLAYER_PREFS) : 0;
			this._currentHighscore.Score = this.Score;

			if (this.Score > previousHighScore) {
				PlayerPrefs.SetInt(HIGH_SCORE_PLAYER_PREFS, this.Score);
				this._bestHighscore.Score = this.Score;
			} else {
				this._bestHighscore.Score = previousHighScore;
			}
		}
    }
}