namespace Assets.Scripts.Components {
    using System;
    using System.Collections;
    using Assets.Scripts.Events;
    using Assets.Scripts.Interfaces;
    using BrettMStory.Events;
    using BrettMStory.Unity;
    using UnityEngine;

    /// <summary>
    /// A match set.
    /// </summary>
    public struct MatchSet {
        public MatchColor color;
        public int number;
        public MatchSymbol symbol;
        public Sprite sprite;
    }

    /// <summary>
    /// A position for tiles on the board.
    /// </summary>
    public class Tile : BaseBehaviour, ISelectable {

        /// <summary>
        /// The disappear delay.
        /// </summary>
        private const float DISAPPEAR_DELAY = 0.5f;

        /// <summary>
        /// The selection delay.
        /// </summary>
        private const float SELECTION_DELAY = 0.1f;

        /// <summary>
        /// The delay between shake moves.
        /// </summary>
        private const float SHAKE_DELAY = 0.05f;
        
        /// <summary>
        /// The current tile at this position.
        /// </summary>
        private MatchSet _currentMatchSet;

        /// <summary>
        /// Is this ready to be selected.
        /// </summary>
        private bool _isReady = true;

        /// <summary>
        /// A value indicating whether or not this tile is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// The match sprite game object.
        /// </summary>
        [SerializeField]
        private GameObject _matchSpriteGameObject;

        /// <summary>
        /// The sprite renderer for the match set.
        /// </summary>
        private SpriteRenderer _matchSetSpriteRenderer;

        /// <summary>
        /// The original position of this tile.
        /// </summary>
        private Vector2 _originalPosition;

        /// <summary>
        /// The sprite when this is selected.
        /// </summary>
        [SerializeField]
        private Sprite _selectedSprite;
        
        /// <summary>
        /// The sprite renderer for selected / unselected.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// The sprite when this is unselected.
        /// </summary>
        [SerializeField]
        private Sprite _unselectedSprite;

        /// <summary>
        /// Gets or sets the current tile.
        /// </summary>
        public MatchSet CurrentMatchSet {
            get {
                return this._currentMatchSet;
            }

            set {
                this._currentMatchSet = value;
                this._matchSetSpriteRenderer.sprite = this._currentMatchSet.sprite;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this tile is selected.
        /// </summary>
        public bool IsSelected {
            get {
                return this._isSelected;
            }

            set {
                this._isSelected = value;

                if (this._isSelected)
                    this._spriteRenderer.sprite = this._selectedSprite;
                else
                    this._spriteRenderer.sprite = this._unselectedSprite;

                this.IsSelectedChanged.SafeInvoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// The Is Selected Changed event handler.
        /// </summary>
        public event EventHandler<EventArgs> IsSelectedChanged;

        /// <summary>
        /// Disappears a tile.
        /// </summary>
        /// <param name="newMatchSet">The new match set.</param>
        public void DisappearTile(MatchSet newMatchSet) {
            this.StartCoroutine("Disappear", newMatchSet);
        }

        /// <summary>
        /// Shakes this tile!
        /// </summary>
        public void ShakeTile() {
            this.StartCoroutine("Shake");
        }

        /// <summary>
        /// Selects or deselects this instance.
        /// </summary>
        public void Select() {
            if (this._isReady) {
                this.IsSelected = !this.IsSelected;
                AudioController.Instance.PlayBloopClip();
                this.StartCoroutine("Delay");
            }
        }
        
        /// <summary>
        /// The awake call.
        /// </summary>
        protected void Awake() {
            this._matchSetSpriteRenderer = this._matchSpriteGameObject.GetComponent<SpriteRenderer>();
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.IsSelected = false;
            this.gameObject.layer = LayerMask.NameToLayer("Selectable");
            this._originalPosition = this.Position;
        }

        /// <summary>
        /// Delays selection.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        private IEnumerator Delay() {
            try {
                this._isReady = false;
                yield return new WaitForSeconds(SELECTION_DELAY);
            } finally {
                this._isReady = true;
            }
        }

        /// <summary>
        /// Disappears the tile for a period.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        private IEnumerator Disappear(MatchSet newMatchSet) {
            this.StopCoroutine("Delay");
            try {
                this._isReady = false;
                this._isSelected = false;
                this._matchSetSpriteRenderer.sprite = null;
                yield return new WaitForSeconds(DISAPPEAR_DELAY);
            } finally {
                this.IsSelected = false;
                this.CurrentMatchSet = newMatchSet;
                this._isReady = true;
            }
        }

        /// <summary>
        /// Shakes this tile.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        private IEnumerator Shake() {
            this.StopCoroutine("Delay");
            try {
                this._isReady = false;
                this._isSelected = false;
                this.Position2D += Vector2.right * Constants.UnitsPerPixel;
                yield return new WaitForSeconds(SHAKE_DELAY);
                this.Position2D -= Vector2.right * 2f * Constants.UnitsPerPixel;
                yield return new WaitForSeconds(SHAKE_DELAY);
                this.Position2D = this._originalPosition;
            } finally {
                this.IsSelected = false;
                this._isReady = true;
            }
        }
    }
}
