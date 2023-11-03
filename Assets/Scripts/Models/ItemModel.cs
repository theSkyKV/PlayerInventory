using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Project.Entities;
using UnityEngine;

namespace Project.Models
{
	public class ItemModel
	{
		private readonly string _path = $"{Application.dataPath}/Config/items.json";

		public List<Item> GetAll()
		{
			List<Item> list = null;
			try
			{
				var data = File.ReadAllText(_path);
				list = JsonConvert.DeserializeObject<List<Item>>(data);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}

			return list;
		}

		public Item Get(string id)
		{
			return GetAll()?.FirstOrDefault(i => i.Id == id);
		}
	}
}