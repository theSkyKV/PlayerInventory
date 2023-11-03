using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Project.Models
{
	public class AssortmentModel
	{
		private readonly string _path = $"{Application.dataPath}/Config/traderAssortment.json";
		
		public List<string> Get()
		{
			List<string> list = null;
			try
			{
				var data = File.ReadAllText(_path);
				list = JsonConvert.DeserializeObject<List<string>>(data);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			return list;
		}
	}
}