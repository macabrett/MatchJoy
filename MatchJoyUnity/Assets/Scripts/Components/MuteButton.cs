
namespace Assets.Scripts.Components {
	using System;
	using System.Collections;
	using BrettMStory.Unity;
	using Assets.Scripts.Interfaces;
	using UnityEngine;

	/// <summary>
	/// Mute button.
	/// </summary>
	public class MuteButton : BaseBehaviour, ISelectable {

		/// <summary>
		/// The time to delay.
		/// </summary>
		private const float DELAY_TIME = 0.2f;

		/// <summary>
		/// The key used to pull out the mute attribute from player preferences.
		/// </summary>
		private const string PLAYER_PREFS_KEY = "MUTE";

		/// <summary>
		/// A value indicating whether or not the game is muted.
		/// </summary>
		private bool _isMuted = false;

		/// <summary>
		/// The mute sprite.
		/// </summary>
		[SerializeField]
		private Sprite _mutedSprite;

		/// <summary>
		/// The sprite renderer.
		/// </summary>
		private SpriteRenderer _spriteRenderer;

		/// <summary>
		/// The unmuted sprite.
		/// </summary>
		[SerializeField]
		private Sprite _unmutedSprite;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is muted.
		/// </summary>
		/// <value><c>true</c> if this instance is muted; otherwise, <c>false</c>.</value>
		public bool IsMuted {
			get {
				return this._isMuted;
			}

			set {
				this._isMuted = value;
				PlayerPrefs.SetInt(PLAYER_PREFS_KEY, Convert.ToInt32(this._isMuted));

				if (this._isMuted) {
					this._spriteRenderer.sprite = this._mutedSprite;
					AudioListener.volume = 0f;
				} else {
					this._spriteRenderer.sprite = this._unmutedSprite;
					AudioListener.volume = 1.0f;
				}
			}
		}

		/// <summary>
		/// Selects the object.
		/// </summary>
		public void Select() {
			if (!this.IsBusy) {
				this.IsMuted = !this.IsMuted;
				this.StartCoroutine("BusyDelay", DELAY_TIME);
			}
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected void Awake() {
			this._spriteRenderer = this.GetComponent<SpriteRenderer>();
			this.gameObject.layer = LayerMask.NameToLayer("Selectable");

			if (PlayerPrefs.HasKey(PLAYER_PREFS_KEY)) {
				this.IsMuted = Convert.ToBoolean(PlayerPrefs.GetInt(PLAYER_PREFS_KEY));
			} else {
				this.IsMuted = false;
			}
		}
	}
}