namespace Assets.Scripts.Components {
    using System;

	using Assets.Scripts.Events;
	using BrettMStory.Events;
	using UnityEngine;

	/// <summary>
	/// The counter.
	/// </summary>
    public class Counter : MonoBehaviour {

		/// <summary>
		/// The number of digits allowed for the counter.
		/// </summary>
        private const int NUM_DIGITS = 2;

		/// <summary>
		/// The start time.
		/// </summary>
		private const int START_TIME = 60;

		/// <summary>
		/// The sprite renderers.
		/// </summary>
        private readonly SpriteRenderer[] _spriteRenderers = new SpriteRenderer[NUM_DIGITS];

		/// <summary>
		/// The display time as an integer.
		/// </summary>
		private int _displayTime = START_TIME;

        /// <summary>
        /// The counter.
        /// </summary>
		private float _timeLeft = (float)START_TIME;

		/// <summary>
		/// Gets or sets the action for reaching zero.
		/// </summary>
		public event EventHandler<EventArgs> RoundOver;

		/// <summary>
		/// Gets or sets the time left.
		/// </summary>
		/// <value>The time left.</value>
        public float TimeLeft {
            get {
                return this._timeLeft;
            }

            set {
                this._timeLeft = value;
                var timeLeftInt = Mathf.RoundToInt(this._timeLeft);
                // update counter
                if (timeLeftInt != this._displayTime) {
                    this._displayTime = timeLeftInt;
                    this.SetTimeLeft(this._displayTime);
                }


                if (this._timeLeft <= 0) {
					this.enabled = false;
					this.RoundOver.SafeInvoke(this, new EventArgs());
                }
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
		/// The start call.
		/// </summary>
		protected void Start() {
			this.SetTimeLeft(this._displayTime.ToString());
			Navigator.Instance.Navigate += this.OnNavigate;
		}

		/// <summary>
		/// The update call.
		/// </summary>
        protected void Update() {
            this.TimeLeft -= Time.deltaTime;
        }

		/// <summary>
		/// Raises the navigate event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void OnNavigate(object sender, NavigationEventArgs e) {
			if (e.NextViewType == ViewType.Gameboard) {
				this.enabled = true;
				this.TimeLeft = START_TIME + 0.5f;
			}
		}

		/// <summary>
		/// Sets the time left.
		/// </summary>
		/// <param name="timeLeft">Time left.</param>
		private void SetTimeLeft(int timeLeft) {
			this.SetTimeLeft(timeLeft.ToString());
		}

		/// <summary>
		/// Sets the time left on the GUI.
		/// </summary>
		/// <param name="timeLeft">Time left.</param>
        private void SetTimeLeft(string timeLeft) {
            var length = timeLeft.Length;

            if (length > NUM_DIGITS || this._displayTime < 0) {
                return; //PANIC OH NO
            }

            var j = 0;
            for (int i = length - 1; i >= 0 && j < NUM_DIGITS; i--) {
                this._spriteRenderers[j].sprite = PixelFont.Instance.GetCharacter(timeLeft[i]);
                j++;
            }

            for (int i = NUM_DIGITS - 1; i > length - 1; i--) {
                this._spriteRenderers[i].sprite = PixelFont.Instance.GetCharacter('0');
            }
        }
    }
}
