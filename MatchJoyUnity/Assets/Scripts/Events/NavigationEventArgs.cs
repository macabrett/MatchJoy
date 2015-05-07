namespace Assets.Scripts.Events {
	using System;
	using System.Collections;

	using Assets.Scripts.Components;

	/// <summary>
	/// Navigation event arguments.
	/// </summary>
	public class NavigationEventArgs : EventArgs {

		/// <summary>
		/// Gets or sets the previous view.
		/// </summary>
		/// <value>The previous view.</value>
		public View PreviousView { get; set; }

		/// <summary>
		/// Gets or sets the type of the previous view.
		/// </summary>
		/// <value>The type of the previous view.</value>
		public ViewType PreviousViewType { get; set; }

		/// <summary>
		/// Gets or sets the next view.
		/// </summary>
		/// <value>The next view.</value>
		public View NextView { get; set; }
		
		/// <summary>
		/// Gets or sets the type of the next view.
		/// </summary>
		/// <value>The type of the next view.</value>
		public ViewType NextViewType { get; set; }
	}
}