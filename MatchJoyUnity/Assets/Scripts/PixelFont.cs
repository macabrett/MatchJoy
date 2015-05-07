namespace Assets.Scripts {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

	/// <summary>
	/// Pixel font.
	/// </summary>
    public class PixelFont {
		/// <summary>
		/// The instance.
		/// </summary>
        private static PixelFont _instance = new PixelFont();

		/// <summary>
		/// Dictionary of number characters to corresponding sprites.
		/// </summary>
		private Dictionary<char, Sprite> _letters = new Dictionary<char, Sprite>();

		/// <summary>
		/// Dictionary of number characters to corresponding sprites.
		/// </summary>
        private Dictionary<char, Sprite> _numbers = new Dictionary<char, Sprite>();

		/// <summary>
		/// Initializes the <see cref="Assets.Scripts.PixelFont"/> class.
		/// </summary>
		static PixelFont() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="Assets.Scripts.PixelFont"/> class.
		/// </summary>
        private PixelFont() {
            var numbers = Resources.LoadAll("Sprites/Numbers");

            foreach (var number in numbers) {
                var sprite = number as Sprite;
                if (sprite != null) {
                    this._numbers.Add(sprite.ToString()[0], sprite);
                }
            }

			var letters = Resources.LoadAll("Sprites/Letters");

			foreach (var letter in letters) {
				var sprite = letter as Sprite;
				if (sprite != null) {
					this._letters.Add(sprite.ToString()[0], sprite);
				}
			}
        }

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
        public static PixelFont Instance {
            get {
                return PixelFont._instance;
            }
        }

		/// <summary>
		/// Gets the character.
		/// </summary>
		/// <returns>The character.</returns>
		/// <param name="character">Character.</param>
        public Sprite GetCharacter(char character) {
            Sprite sprite;

			if (this._letters.TryGetValue(character, out sprite)) {
				return sprite;
			} else if (this._numbers.TryGetValue(character, out sprite)) {
				return sprite;
			}

            return null;
        }
    }
}