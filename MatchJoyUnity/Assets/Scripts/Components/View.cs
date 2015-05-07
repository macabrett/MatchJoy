namespace Assets.Scripts.Components {
	using System;
	using System.Collections;

    using BrettMStory.Unity;
	using UnityEngine;

    /// <summary>
    /// The background color.
    /// </summary>
    public enum BackgroundColor {
        Black,
        Brown
    }

	/// <summary>
	/// View type.
	/// </summary>
	public enum ViewType {
        About,
		End,
		Gameboard,
		Help,
		Leaderboard,
        OpenSource,
        Rating,
		Start
	}

	/// <summary>
	/// View.
	/// </summary>
	public class View : BaseBehaviour {

        /// <summary>
        /// The background color.
        /// </summary>
        [SerializeField]
        private BackgroundColor _backgroundColor;

		/// <summary>
		/// The view type..
		/// </summary>
		[SerializeField]
		private ViewType _viewType;

		/// <summary>
		/// Gets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
        public BackgroundColor BackgroundColor {
            get {
                return this._backgroundColor;
            }
        }

		/// <summary>
		/// Gets the type of the view.
		/// </summary>
		/// <value>The type of the view.</value>
		public ViewType ViewType {
			get {
				return this._viewType;
			}
		}
	}
}