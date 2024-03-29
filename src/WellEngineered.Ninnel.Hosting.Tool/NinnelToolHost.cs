/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Ninnel.Primitives;

namespace WellEngineered.Ninnel.Hosting.Tool
{
	public abstract partial class NinnelToolHost
		: NinnelHost<ToolHostConfiguration>,
			INinnelToolHost
	{
		#region Constructors/Destructors

		protected NinnelToolHost()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract void CoreHost(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties);

		public void Host(IDictionary<string, IList<object>> arguments, IDictionary<string, IList<object>> properties)
		{
			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				this.CoreHost(arguments, properties);
			}
			catch (Exception ex)
			{
				throw new NinnelException(string.Format("The tool host failed (see inner exception)."), ex);
			}
		}

		#endregion

// 		/*public void Host2(IDictionary<string, IList<object>> arguments,
// 			IDictionary<string, IList<object>> properties)
// 		{
// 			DateTime startUtc, endUtc;
//
// 			INinnelTemplate textMetalTemplate;
// 			__INinnelModel textMetalModel;
// 			INinnelOutput textMetalOutput;
//
// 			Dictionary<string, object> arguments_;
// 			Dictionary<string, object> properties_;
//
// 			Dictionary<string, object> globalVariableTable;
//
// 			IAssemblyInformationFascade assemblyInformationFascade;
//
// 			if ((object)arguments == null)
// 				throw new ArgumentNullException(nameof(arguments));
//
// 			if ((object)properties == null)
// 				throw new ArgumentNullException(nameof(properties));
//
// 			startUtc = DateTime.UtcNow;
//
// 			arguments_ = arguments.ToDictionary(kvp => kvp.Key,
// 				kvp => kvp.Value.Count == 1 ? kvp.Value[0] : kvp.Value.ToArray());
//
// 			properties_ = properties.ToDictionary(kvp => kvp.Key,
// 				kvp => kvp.Value.Count == 1 ? kvp.Value[0] : kvp.Value.ToArray());
//
// 			assemblyInformationFascade = new AssemblyInformationFascade(this.ReflectionFascade, typeof(NinnelToolHost).GetTypeInfo().Assembly);
//
// 			Lazy<object> lazyGuid = new Lazy<object>(() => Guid.NewGuid());
// 			Func<object> getDate = () => DateTime.UtcNow;
//
// 			var environment = new
// 							{
// 								Arguments = Environment.GetCommandLineArgs(),
// 								Variables = Environment.GetEnvironmentVariables(),
// 								NewLine = Environment.NewLine,
// 								Version = Environment.Version,
// 								MachineName = Environment.MachineName,
// 								UserName = Environment.UserName,
// 								OSVersion = Environment.OSVersion,
// 								UserDomainName = Environment.UserDomainName,
// 								Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
// 								Is64BitProcess = Environment.Is64BitProcess,
// 								ProcessorCount = Environment.ProcessorCount,
// 								GetDate = getDate,
// 								LazyGuid = lazyGuid
// 							};
//
// 			var tooling = new
// 						{
// 							AssemblyVersion = assemblyInformationFascade.AssemblyVersion,
// 							InformationalVersion = assemblyInformationFascade.InformationalVersion,
// 							NativeFileVersion = assemblyInformationFascade.NativeFileVersion,
// 							ModuleName = assemblyInformationFascade.ModuleName,
// 							Company = assemblyInformationFascade.Company,
// 							Configuration = assemblyInformationFascade.Configuration,
// 							Copyright = assemblyInformationFascade.Copyright,
// 							Description = assemblyInformationFascade.Description,
// 							Product = assemblyInformationFascade.Product,
// 							Title = assemblyInformationFascade.Title,
// 							Trademark = assemblyInformationFascade.Trademark,
// 							StartedWhenUtc = startUtc
// 						};
//
// 			// globals (GVT)
// 			globalVariableTable = new Dictionary<string, object>();
// 			globalVariableTable.Add("Tooling", tooling);
// 			globalVariableTable.Add("Environment", environment);
//
// 			globalVariableTable.Add("Properties", properties_);
// 			globalVariableTable.Add("Arguments", arguments_);
//
// 			// add arguments to GVT
// 			/*foreach (KeyValuePair<string, IList<object>> argument in arguments)
// 			{
// 				if (argument.Value.Count == 0)
// 					continue;
//
// 				globalVariableTable.Add(argument.Key,
// 					argument.Value.Count == 1 ? argument.Value[0] : argument.Value);
// 			}*/
//
// 			// add properties to GVT
// 			/*foreach (KeyValuePair<string, IList<object>> property in properties)
// 			{
// 				if (property.Value.Count == 0)
// 					continue;
//
// 				globalVariableTable.Add(property.Key,
// 					property.Value.Count == 1 ? property.Value[0] : property.Value);
// 			}*/
//
// 			// do the deal...
// 			using (INinnelTemplateFactory textMetalTemplateFactory = NewObjectFromType<INinnelTemplateFactory>(this.Configuration.GetTemplateFactoryType()))
// 			{
// 				//textMetalTemplateFactory.Configuration = ...
// 				textMetalTemplateFactory.Create();
//
// 				textMetalTemplate = textMetalTemplateFactory.GetTemplateObject(new Uri("urn:null"), properties_);
//
// 				using (INinnelModelFactory textMetalModelFactory = NewObjectFromType<INinnelModelFactory>(this.Configuration.GetModelFactoryType()))
// 				{
// 					//textMetalModelFactory.Configuration = ...
// 					textMetalModelFactory.Create();
//
// 					textMetalModel = textMetalModelFactory.GetModelObject(properties_);
//
// 					using (INinnelOutputFactory textMetalOutputFactory = NewObjectFromType<INinnelOutputFactory>(this.Configuration.GetOutputFactoryType()))
// 					{
// 						//textMetalOutputFactory.Configuration = ...
// 						textMetalOutputFactory.Create();
//
// 						textMetalOutput = textMetalOutputFactory.GetOutputObject(new Uri("urn:null"), properties_);
//
// 						//textMetalOutput.TextWriter.WriteLine("xxx");
//
// 						using (INinnelContext textMetalContext = this.CreateContext())
// 						{
// 							//textMetalContext.Configuration = ...
// 							textMetalContext.Create();
//
// 							//textMetalContext.DiagnosticOutput.WriteObject(textMetalTemplate, "#template.xml");
// 							//textMetalContext.DiagnosticOutput.WriteObject(textMetalModel, "#model.xml");
//
// 							textMetalContext.DiagnosticOutput.TextWriter.WriteLine("['{0:O}' (UTC)]\tText templating started.", tooling.StartedWhenUtc);
//
// 							textMetalContext.VariableTables.Push(globalVariableTable);
// 							textMetalContext.IteratorModels.Push(textMetalModel);
//
// 							textMetalTemplate.ExpandTemplate(textMetalContext);
//
// 							textMetalContext.IteratorModels.Pop();
// 							textMetalContext.VariableTables.Pop();
//
// 							endUtc = DateTime.UtcNow;
// 							textMetalContext.DiagnosticOutput.TextWriter.WriteLine("['{0:O}' (UTC)]\tText templating completed with duration: '{1}'.", endUtc, (endUtc - startUtc));
// 						}
// 					}
// 				}
// 			}
// 		}
	}
}