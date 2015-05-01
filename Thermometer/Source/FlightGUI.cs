using System.IO;
using UnityEngine;

namespace Thermometer
{
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class FlightGUI : MonoBehaviour
	{
		private Texture2D texture;
		private MainWindow window;
		private ApplicationLauncherButton button;

		void Start()
		{
			texture = LoadPNG (Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../app.png");
			if (ApplicationLauncher.Ready) {
				//onReady ();
			} else {
				GameEvents.onGUIApplicationLauncherReady.Add (onReady);
			}

			GameEvents.onGUIApplicationLauncherDestroyed.Add (onDestroyed);
			GameEvents.onGUIApplicationLauncherUnreadifying.Add (onUnReady);
		}

		void OnDestroy()
		{
		}

		public void onReady() {
			if (button == null) {
				button = ApplicationLauncher.Instance.AddModApplication (onTrue, onFalse, onHover, onHoverOut, onEnable, onDisable, ApplicationLauncher.AppScenes.FLIGHT, texture);
			}
		}

		public void onDestroyed() {
			button = null;
		}
		public void onUnReady(GameScenes scene) {
			if (button != null) {
				ApplicationLauncher.Instance.RemoveModApplication (button);
			}
			if (window != null) {
				onFalse ();
			}
			onDestroyed ();
		}

		public void onTrue() {
			if (window == null) {
				window = new MainWindow ();
				window.initGui (new WindowSettings(100, 100, 200, 250, "ThermometerSettings.cfg"));
			}
		}
		public void onFalse() {
			window.remove ();
			window = null;
		}
		public void onHover() {
		}
		public void onHoverOut() {
		}
		public void onEnable() {
		}
		public void onDisable() {
		}

		public static Texture2D LoadPNG(string filePath) {
			Texture2D tex = null;
			byte[] fileData;

			if (File.Exists(filePath))     {
				fileData = File.ReadAllBytes(filePath);
				tex = new Texture2D(2, 2);
				tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			}
			return tex;
		}
	}
}

