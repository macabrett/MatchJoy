using UnityEngine;
using System.Collections;

public class InGameLogger : MonoBehaviour {
    private static InGameLogger _instance;
    private string _text;
    [SerializeField]
    private float _textBoxHeight = 100f;
    [SerializeField]
    private float _textBoxWidth = 100f;

    public string Text {
        get {
            return this._text;
        }

        set {
            this._text = value;
        }
    }

    public static void Log(object format) {
        InGameLogger._instance.Text = format.ToString();
    }

    public static void Log(object format, object arg0) {
        InGameLogger.Log(format, new object[1] { arg0 });
    }

    public static void Log(object format, object arg0, object arg1) {
        InGameLogger.Log(format, new object[2] { arg0, arg1 });
    }

    public static void Log(object format, object arg0, object arg1, object arg2) {
        InGameLogger.Log(format, new object[3] { arg0, arg1, arg2 });
    }

    public static void Log(object format, object[] args) {
        InGameLogger.Log(string.Format(format.ToString(), args));
    }

    protected void Awake() {
        if (InGameLogger._instance == null) {
            InGameLogger._instance = this;
        }
    }

    protected void OnGUI() {
        if (!string.IsNullOrEmpty(this._text)) {
            GUI.TextArea(new Rect(0, 0, this._textBoxWidth, this._textBoxHeight), this._text);
        }
    }
}
