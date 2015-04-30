using System;

namespace Thermometer
{
	public class ModuleThermometer : PartModule
	{
		public static readonly String DEGREE_SYMBOL = "\\u00B0";

		[KSPField(isPersistant = false, guiActive = true, guiName = "Temperature")]
		public String temperature = "";

		public override void OnUpdate() {
			temperature = getTemperature() + " " + DEGREE_SYMBOL + getUnitString();
		}

		private double getTemperature() {
			return Math.Round(this.part.temperature, 3) - 273.15;
		}

		private String getUnitString() {
			return "C";
		}
	}
}

