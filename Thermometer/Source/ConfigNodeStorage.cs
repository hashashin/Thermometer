using System;

namespace Thermometer
{
	//All credit to TriggerAu for this code (I made slight modifications to logging) 
	public abstract class ConfigNodeStorage : IPersistenceLoad, IPersistenceSave
	{
		public ConfigNodeStorage() { }
		public ConfigNodeStorage(String FilePath) { this.FilePath = FilePath; }

		private String _FilePath;

		public String FilePath
		{
			get { return _FilePath; }
			set
			{
				_FilePath = System.IO.Path.Combine(_AssemblyFolder, value).Replace("\\","/");
			}
		}

		public String FileName
		{
			get { return System.IO.Path.GetFileName(FilePath); }
		}

		void IPersistenceLoad.PersistenceLoad()
		{
			OnDecodeFromConfigNode();
		}
		void IPersistenceSave.PersistenceSave()
		{
			OnEncodeToConfigNode();
		}
		public virtual void OnDecodeFromConfigNode() { }
		public virtual void OnEncodeToConfigNode() { }
		public Boolean FileExists
		{
			get
			{
				return System.IO.File.Exists(FilePath);
			}
		}
		public Boolean Load()
		{
			return this.Load(FilePath);
		}
		public Boolean Load(String fileFullName)
		{
			Boolean blnReturn = false;
			try
			{
				if (FileExists)
				{
					ConfigNode cnToLoad = ConfigNode.Load(fileFullName);
					ConfigNode cnUnwrapped = cnToLoad.GetNode(this.GetType().Name);
					ConfigNode.LoadObjectFromConfig(this, cnUnwrapped);
					blnReturn = true;
				}
				else
				{
					LogFormatted("File could not be found to load({0})", fileFullName);
					blnReturn = false;
				}
			}
			catch (Exception ex)
			{
				LogFormatted("Failed to Load ConfigNode from file({0})-Error:{1}", fileFullName, ex.Message);
				LogFormatted("Storing old config - {0}", fileFullName + ".err-" + string.Format("ddMMyyyy-HHmmss", DateTime.Now));
				System.IO.File.Copy(fileFullName, fileFullName + ".err-" + string.Format("ddMMyyyy-HHmmss", DateTime.Now),true);
				blnReturn = false;
			}
			return blnReturn;
		}
		public Boolean Save()
		{
			LogFormatted_DebugOnly("Saving ConfigNode");
			return this.Save(FilePath);
		}
		public Boolean Save(String fileFullName)
		{
			Boolean blnReturn = false;
			try
			{
				ConfigNode cnToSave = this.AsConfigNode;
				ConfigNode cnSaveWrapper = new ConfigNode(this.GetType().Name);
				cnSaveWrapper.AddNode(cnToSave);
				cnSaveWrapper.Save(fileFullName);
				blnReturn = true;
			}
			catch (Exception ex)
			{
				LogFormatted("Failed to Save ConfigNode to file({0})-Error:{1}", fileFullName, ex.Message);
				blnReturn = false;
			}
			return blnReturn;
		}
		public ConfigNode AsConfigNode
		{
			get
			{
				try
				{
					ConfigNode cnTemp = new ConfigNode(this.GetType().Name);
					cnTemp = ConfigNode.CreateConfigFromObject(this, cnTemp);
					return cnTemp;
				}
				catch (Exception ex)
				{
					LogFormatted("Failed to generate ConfigNode-Error;{0}", ex.Message);
					return new ConfigNode(this.GetType().Name);
				}
			}
		}

		internal static String _AssemblyName
		{ get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

		internal static String _AssemblyLocation
		{ get { return System.Reflection.Assembly.GetExecutingAssembly().Location; } }

		internal static String _AssemblyFolder
		{ get { return System.IO.Path.GetDirectoryName(_AssemblyLocation); } }

		[System.Diagnostics.Conditional("DEBUG")]
		internal static void LogFormatted_DebugOnly(String Message, params object[] strParams)
		{
			LogFormatted("DEBUG: " + Message, strParams);
		}

		internal static void LogFormatted(String Message, params object[] strParams)
		{
			Message = String.Format(Message, strParams); 
			String strMessageLine = String.Format("{0},{2},{1}",
				DateTime.Now, Message,
				_AssemblyName);  
			UnityEngine.Debug.Log(strMessageLine);
		}

	}
}

