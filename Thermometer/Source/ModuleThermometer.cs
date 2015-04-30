using System;

namespace Thermometer
{
	public class ModuleThermometer : PartModule
	{
		[KSPField(isPersistant = false, guiActive = true, guiName = "Temperature", guiUnits = "C")]
		public double temperature = 0;

		public override void OnUpdate() {
			temperature = Math.Round(this.part.temperature, 3) - 273.15;
		}
	}
}

