namespace Assets.Scripts.Interfaces {
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Interface for selectable objects.
    /// </summary>
    public interface ISelectable {

        /// <summary>
        /// Selects the object.
        /// </summary>
        void Select();
    }
}