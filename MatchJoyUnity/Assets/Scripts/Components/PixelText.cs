namespace Assets.Scripts.Components {
	using System.Collections;
    using BrettMStory.Unity;
    using UnityEngine;

	public class PixelText : BaseBehaviour {
		/// <summary>
		/// The sorting order.
		/// </summary>
		[SerializeField]
		private int _sortingOrder = 0;

		/// <summary>
		/// The text.
		/// </summary>
		[SerializeField]
		private string _text = string.Empty;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected void Awake() {
			this._text = this._text.ToUpper();
			this.gameObject.name = this._text;

			for (int i = 0; i < this._text.Length; i++) {
				var sprite = PixelFont.Instance.GetCharacter(this._text[i]);

				if (sprite == null)
					continue;

				var spriteGameObject = new GameObject(this._text[i].ToString());
				spriteGameObject.SetActive(true);
				spriteGameObject.transform.parent = this.transform;
				spriteGameObject.transform.position = new Vector3(this.transform.position.x + i * 0.5f, this.transform.position.y, 0f);
				spriteGameObject.transform.parent = this.transform;
				var spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = sprite;
				spriteRenderer.sortingOrder = this._sortingOrder;
			}
		}

		protected void OnDrawGizmos() {
			if (string.IsNullOrEmpty(this._text))
				return;

			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			Gizmos.DrawCube(
				this.transform.position + Vector3.right * 0.25f * (this._text.Length - 1f), 
				new Vector3(this._text.Length * 0.5f, 0.5f, 1f));
		}
	}
}