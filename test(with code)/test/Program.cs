using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Test
{
	public class AzurePostgresCreate
	{

		static void Main(string[] args)
		{
			NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=qweasdzxc;Database=MissedPeople;");
			conn.Open();
			NpgsqlCommand dt = new NpgsqlCommand("DROP TABLE test", conn);
			dt.ExecuteNonQuery();
			NpgsqlCommand crt_table = new NpgsqlCommand ("CREATE TABLE test(Фамилия CHAR(256), Отчество CHAR(256), Имя CHAR(256), ДатаРождения CHAR(256), Орган CHAR(256), Телефон CHAR(256), URL CHAR(256))", conn);
			crt_table.ExecuteNonQuery();
			List<string> values = new List<string> {"Фамилия","Отчество","Имя","ДатаРождения","Орган","Телефон","URL"};
			List<string> inserts = new List<string> { };
			for (int n = 1; n < 96; n++)
			{
				string url = "https://qamqor.gov.kz/portal/page/portal/POPageGroup/Services/SuRet?_piref36_264068_36_223091_223091.__ora_navigState=eventSubmit_doSearch%3Dgallery%26page_cnt%3D95%26page_num%3D1%26search%3Dgallery%26obl%3D19%26sureg%3D-1%26ret_type%3D2&_piref36_264068_36_223091_223091.__ora_navigValues=";
				url = url.Remove(178,1);
				url = url.Insert(178, n.ToString());
				HtmlWeb web = new HtmlWeb();
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc = web.Load(url);
				for (int i = 1; i < 25; i++)
				{
					inserts.Clear();
					var nodes = doc.DocumentNode.SelectNodes("//*[@onclick='showDetal(" + i + ")']/div/input");
					if (nodes == null)
						break;
					foreach (var item in nodes)
					{
						Console.WriteLine(item.Attributes["Value"].Value);
						inserts.Add(item.Attributes["Value"].Value);
						
					}
					
						var cmd = new NpgsqlCommand($"INSERT INTO test(Фамилия,Отчество,Имя,ДатаРождения,Орган,Телефон,URL) VALUES(@f,@o,@i,@d,@or,@t,@u)", conn);
					for (int v = 0; v < values.Count; v++)
					{
						if (v == 0)
							cmd.Parameters.AddWithValue("f", inserts[v]);
						else if (v == 1)
							cmd.Parameters.AddWithValue("o", inserts[v]);
						else if (v == 2)
							cmd.Parameters.AddWithValue("i", inserts[v]);
						else if (v == 3)
							cmd.Parameters.AddWithValue("d", inserts[v]);
						else if (v == 4)
							cmd.Parameters.AddWithValue("or", inserts[v]);
						else if (v == 5)
							cmd.Parameters.AddWithValue("t", inserts[v]);
						else if (v == 6)
							cmd.Parameters.AddWithValue("u", inserts[v]);
						else break;
					}
					cmd.ExecuteNonQuery();
					Console.WriteLine("");	
				}
			}
			conn.Close();
			Console.WriteLine("Парсинг завершен!");
			Console.ReadKey();
		}
    }
}