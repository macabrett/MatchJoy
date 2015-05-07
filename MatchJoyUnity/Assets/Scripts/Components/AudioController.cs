namespace Assets.Scripts.Components {
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// The audio controller.
    /// </summary>
    public class AudioController : MonoBehaviour {

        /// <summary>
        /// The instance.
        /// </summary>
        private static AudioController _instance;

        /// <summary>
        /// Audio clip for bloop.
        /// </summary>
        [SerializeField]
        private AudioClip _bloopAudioClip;

        /// <summary>
        /// Audio clip for button click.
        /// </summary>
        [SerializeField]
        private AudioClip _buttonAudioClip;

        /// <summary>
        /// Audio clip for unsuccessful match.
        /// </summary>
        [SerializeField]
        private AudioClip _failureAudioClip;

        /// <summary>
        /// Audio clip for successful match.
        /// </summary>
        [SerializeField]
        private AudioClip _successAudioClip;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static AudioController Instance {
            get {
                return AudioController._instance;
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        protected void Awake() {
            if (AudioController._instance == null)
                AudioController._instance = this;
        }

        /// <summary>
        /// Plays bloop clip.
        /// </summary>
        public void PlayBloopClip() {
            AudioSource.PlayClipAtPoint(this._bloopAudioClip, Vector3.zero, 0.75f);
        }

        /// <summary>
        /// Plays button clip.
        /// </summary>
        public void PlayButtonClip() {
            AudioSource.PlayClipAtPoint(this._buttonAudioClip, Vector3.zero, 0.6f);
        }

        /// <summary>
        /// Plays failure clip.
        /// </summary>
        public void PlayFailureClip() {
            AudioSource.PlayClipAtPoint(this._failureAudioClip, Vector3.zero, 0.4f);
        }

        /// <summary>
        /// Plays success clip.
        /// </summary>
        public void PlayerSuccessClip() {
            AudioSource.PlayClipAtPoint(this._successAudioClip, Vector3.zero, 0.3f);
        }
    }
}