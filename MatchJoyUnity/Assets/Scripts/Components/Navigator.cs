namespace Assets.Scripts.Components {
	using System;
	using System.Collections;
	using System.Collections.Generic;

	using Assets.Scripts.Events;
	using BrettMStory.Events;
	using UnityEngine;

	/// <summary>
	/// Navigator.
	/// </summary>
	public class Navigator : MonoBehaviour {

        /// <summary>
        /// The instance.
        /// </summary>
        private static Navigator _instance;

        /// <summary>
        /// The black color.
        /// </summary>
        private Color _blackColor = new Color(0f, 0f, 0f);

        /// <summary>
        /// The brown color.
        /// </summary>
        private Color _brownColor = new Color(80f / 255f, 48f / 255f, 0f);

        /// <summary>
        /// The counter.
        /// </summary>
        private Counter _counter;

        /// <summary>
        /// The current view.
        /// </summary>
        private View _currentView;

        /// <summary>
        /// A dictionary of view types to views.
        /// </summary>
        private Dictionary<ViewType, View> _views = new Dictionary<ViewType, View>();

        /// <summary>
        /// Creates a new Navigator.
        /// </summary>
        static Navigator() {}

        /// <summary>
        /// Creates a new Navigator.
        /// </summary>
        private Navigator() {}

		/// <summary>
		/// Occurs when navigate.
		/// </summary>
		public event EventHandler<NavigationEventArgs> Navigate;

        /// <summary>
        /// Gets the current view.
        /// </summary>
        public View CurrentView {
            get {
                return this._currentView;
            }

            private set {
                if (this.CurrentView != null)
                    this._currentView.GameObject.SetActive(false);

                this._currentView = value;
                Camera.main.backgroundColor = this.GetColorFromBackgroundColor(this._currentView.BackgroundColor);
                this._currentView.GameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Navigator Instance {
            get {
                return Navigator._instance;
            }
        }

        /// <summary>
        /// Navigates to the specified view.
        /// </summary>
        /// <param name="viewType">The viewtype to navigate to.</param>
        public void NavigateTo(ViewType viewType) {
            if (this._views.ContainsKey(viewType)) {
				var e = new NavigationEventArgs 
				{
					PreviousView = this.CurrentView,
					PreviousViewType = this.CurrentView.ViewType,
					NextView = this._views[viewType],
					NextViewType = viewType
				};

                this.CurrentView = e.NextView;
				this.Navigate.SafeInvoke(this, e);
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        protected void Awake() {
            if (Navigator._instance == null) {
                Navigator._instance = this;
            }

            this._counter = GameObject.FindObjectOfType<Counter>();
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        protected void OnDisable() {
            if (this._counter == null)
                this._counter = GameObject.FindObjectOfType<Counter>();

            if (this._counter != null)
                this._counter.RoundOver -= this.OnRoundOver;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        protected void OnEnable() {
            if (this._counter == null)
                this._counter = GameObject.FindObjectOfType<Counter>();

            if (this._counter != null)
                this._counter.RoundOver += this.OnRoundOver;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        protected void Start() {
			var views = GameObject.FindObjectsOfType<View>();
			foreach (var view in views) {
				this._views.Add(view.ViewType, view);
				view.GameObject.SetActive(false);
			}
			
			this.CurrentView = this._views[ViewType.Start];      
		}

        /// <summary>
        /// Gets a color from the background color enum.
        /// </summary>
        /// <param name="backgroundColor">The background color.</param>
        /// <returns>A color.</returns>
        private Color GetColorFromBackgroundColor(BackgroundColor backgroundColor) {
            switch (backgroundColor) {
                case BackgroundColor.Black:
                    return this._blackColor;
                case BackgroundColor.Brown:
                    return this._brownColor;
                default:
                    return this._blackColor;
            }
        }

		/// <summary>
		/// Raises the restart event.
		/// </summary>
		private void OnRestart() {
			this.NavigateTo(ViewType.Gameboard);
		}

		/// <summary>
		/// Raises the round over event.
		/// </summary>
		private void OnRoundOver(object sender, EventArgs e) {
			this.NavigateTo(ViewType.End);
		}
	}
}