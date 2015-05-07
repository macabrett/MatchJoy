namespace Assets.Scripts.Components {
	using System;
    using System.Collections;

    using Assets.Scripts.Interfaces;
    using Assets.Scripts;
	using BrettMStory.Events;
    using BrettMStory.Unity.SimpleInput;
	using UnityEngine;

	/// <summary>
	/// Game camera.
	/// </summary>
    public class GameCamera : MonoBehaviour {

        /// <summary>
        /// The minimum world height.
        /// </summary>
        private const float MINIMUM_WORLD_HEIGHT = 11f;

        /// <summary>
        /// The minimum world width.
        /// </summary>
        private const float MINIMUM_WORLD_WIDTH = 12f;

        /// <summary>
        /// The camera attached to this object.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The touchable layer.
        /// </summary>
        private int _selectableLayer;

        /// <summary>
        /// The screen width in pixels.
        /// </summary>
        private int _screenHeight;

        /// <summary>
        /// The screen height in pixels.
        /// </summary>
        private int _screenWidth;

        /// <summary>
        /// Gets or sets the CameraAction for CameraAdjusted.
        /// </summary>
		public event EventHandler<EventArgs> CameraAdjusted;

        /// <summary>
        /// The awake call.
        /// </summary>
        protected void Awake() {
            this._camera = this.GetComponent<Camera>();
            this._selectableLayer = 1 << LayerMask.NameToLayer("Selectable");
            this.AdjustCamera();
        }

        /// <summary>
        /// Called on disable.
        /// </summary>
        protected void OnDisable() {
            if (SimpleMobile.InstanceExists) {
                SimpleMobile.Instance.BeganTouch -= this.OnTouch;
            }

            if (SimpleMouse.InstanceExists) {
                SimpleMouse.Instance.LeftMouseButtonDown -= this.OnClick;
            }
        }

		/// <summary>
		/// Raises the draw gizmos event.
		/// </summary>
		protected void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(MINIMUM_WORLD_WIDTH, MINIMUM_WORLD_HEIGHT, 1f));
		}

        /// <summary>
        /// Called on enable.
        /// </summary>
        protected void OnEnable() {
            SimpleMobile.Instance.BeganTouch += this.OnTouch;
            SimpleMouse.Instance.LeftMouseButtonDown += this.OnClick;
        }

        protected void Start() {
            var audioSource = this.GetComponent<AudioSource>();
            audioSource.PlayDelayed(1f);
        }

        /// <summary>
        /// The update call.
        /// </summary>
        protected void Update() {
            if (Screen.width != this._screenWidth || Screen.height != this._screenHeight) {
                this.AdjustCamera();
            }
        }

        /// <summary>
        /// Adjusts the camera to fit.
        /// </summary>
        private void AdjustCamera() {
            this._camera.orthographicSize = MINIMUM_WORLD_HEIGHT * 0.5f;
            var worldWidth = this._camera.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x - this._camera.ScreenToWorldPoint(Vector2.zero).x;

            if (worldWidth < MINIMUM_WORLD_WIDTH) {
                this._camera.orthographicSize *= MINIMUM_WORLD_WIDTH / worldWidth;
            }

            this._screenHeight = Screen.height;
            this._screenWidth = Screen.width;

            this.CameraAdjusted.SafeInvoke(this, new EventArgs());
        }

        /// <summary>
        /// Called on click.
        /// </summary>
        /// <param name="screenPosition">The screen coordinates of the click.</param>
        private void OnClick(object sender, MouseInputEventArgs e) {
            this.Select(e.ClickPosition);
        }

        /// <summary>
        /// Called on tap.
        /// </summary>
        /// <param name="touch">The touch.</param>
        private void OnTouch(object sender, TouchEventArgs e) {
            this.Select(e.Touch.rawPosition);
        }

        /// <summary>
        /// Selects a tile.
        /// </summary>
        /// <param name="screenPosition">The screen position to check for a card.</param>
        private void Select(Vector2 screenPosition) {
            var worldPosition = this._camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, this._camera.nearClipPlane));
            var collider = Physics2D.OverlapPoint(worldPosition, this._selectableLayer);
            if (collider != null) {
                var selectable = collider.GetComponent(typeof(ISelectable)) as ISelectable;

                if (selectable != null) {
                    selectable.Select();
                }
            }
        }
    }
}