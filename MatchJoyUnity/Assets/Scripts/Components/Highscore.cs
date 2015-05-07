namespace Assets.Scripts.Components {
	using UnityEngine;
	using System.Collections;

	using Assets.Scripts;

	/// <summary>
	/// High score.
	/// </summary>
	public class Highscore : MonoBehaviour {

		/// <summary>
		/// The number of digits.
		/// </summary>
		private const int NUM_DIGITS = 3;

		/// <summary>
		/// The sorting order.
		/// </summary>
		private const int SORTING_ORDER = 3;

		/// <summary>
		/// The sprite renderers.
		/// </summary>
		private SpriteRenderer[] _spriteRenderers = new SpriteRenderer[NUM_DIGITS];

		/// <summary>
		/// The current score.
		/// </summary>
		[SerializeField]
		private int _score = 0;

		/// <summary>
		/// Gets the score.
		/// </summary>
		public int Score {
			get {
				return this._score;
			}
			
			set {
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
				spriteGameObject.transform.position = new Vector3(this.transform.position.x - i * 0.5f + (NUM_DIGITS - 1) * 0.5f, this.transform.position.y, 0f);
				spriteGameObject.transform.parent = this.transform;
				var spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = PixelFont.Instance.GetCharacter('0');
				spriteRenderer.sortingOrder = SORTING_ORDER;
				this._spriteRenderers[i] = spriteRenderer;
			}
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
	}
}