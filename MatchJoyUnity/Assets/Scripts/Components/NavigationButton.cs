namespace Assets.Scripts.Components {
	using System;
    using System.Collections;

    using Assets.Scripts.Interfaces;
    using BrettMStory.Unity;
	using UnityEngine;

    /// <summary>
    /// A navigation button.
    /// </summary>
    public class NavigationButton : BaseBehaviour, ISelectable {
        
		/// <summary>
		/// The default sprite.
		/// </summary>
		[SerializeField]
		private Sprite _defaultSprite;

		/// <summary>
		/// The delay time.
		/// </summary>
		[SerializeField]
		private float _delayTime = 0.25f;

        /// <summary>
        /// The view type to navigate to.
        /// </summary>
        [SerializeField]
        private ViewType _navigatesToViewType;

		/// <summary>
		/// The secondary sprite.
		/// </summary>
		[SerializeField]
		private Sprite _secondarySprite;

		/// <summary>
		/// The sprite renderer.
		/// </summary>
		private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Selects this instance.
        /// </summary>
        public virtual void Select() {
            if (this.IsBusy)
                return;

            AudioController.Instance.PlayButtonClip();

			if (this._secondarySprite != null)
				this.StartCoroutine("WaitThenNavigate");
			else
				this.NavigateAway();
        }

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected void Awake() {
			this.gameObject.layer = LayerMask.NameToLayer("Selectable");
			this._spriteRenderer = this.GetComponent<SpriteRenderer>();

			if (this._spriteRenderer.sprite == null)
				this._spriteRenderer.sprite = this._defaultSprite;
			else if (this._defaultSprite == null)
				this._defaultSprite = this._spriteRenderer.sprite;
		}

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		protected virtual void OnEnable() {
			this._spriteRenderer.sprite = this._defaultSprite;
		}

        /// <summary>
        /// Navigates away.
        /// </summary>
		private void NavigateAway() {
			if (this._navigatesToViewType == ViewType.Rating) {
				// TODO: Store rating
			} else if (this._navigatesToViewType == ViewType.Leaderboard) {
				// TODO: Google scores.
			} else {
				Navigator.Instance.NavigateTo(this._navigatesToViewType);
			}
		}

		/// <summary>
		/// Waits then navigates.
		/// </summary>
		/// <returns>The then navigate.</returns>
		private IEnumerator WaitThenNavigate() {
            try {
                this.IsBusy = true;
                this._spriteRenderer.sprite = this._secondarySprite;
                yield return new WaitForSeconds(this._delayTime);
                this._spriteRenderer.sprite = this._defaultSprite;
                this.NavigateAway();
            } finally {
                this.IsBusy = false;
            }
		}
    }
}