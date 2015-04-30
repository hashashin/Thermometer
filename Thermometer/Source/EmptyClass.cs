using System;

namespace Thermometer
{
	public class ModuleThermometer : PartModule
	{
		[KSPField(isPersistant = false, guiActive = true, guiName = "Temperature", guiFormat = "F3", guiUnits = "C")]
		public double temperature = 0;

		public override void OnUpdate() {
			temperature = this.part.temperature - 273.15;
		}
	}
}

