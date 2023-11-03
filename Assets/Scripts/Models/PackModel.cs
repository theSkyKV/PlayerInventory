using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Project.Entities;
using UnityEngine;

namespace Project.Models
{
	public class PackModel
	{
		private readonly string _path = $"{Application.dataPath}/Config/items.json";

		public List<Pack> GetAll()
		{
			List<Pack> list = null;
			try
			{
				var data = File.ReadAllText(_path);
				list = JsonConvert.DeserializeObject<IEnumerable<Pack>>(data)
					.Where(i => i.Type == ItemType.Pack)
					.ToList();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			return list;
		}

		public Pack Get(string id)
		{
			return GetAll()?.FirstOrDefault(i => i.Id == id);
		}
	}
}