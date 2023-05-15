namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class KategorijaRepo
{
	public static List<Kategorija> ListKategorija()
	{
		string query = $@"SELECT * FROM `kategorijos` ORDER BY kategorijos_id ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Kategorija>(drc, (dre, t) => {
				t.Id = dre.From<int>("kategorijos_id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			});

		return result;
	}

	public static Kategorija Find(int id)
	{
		var query = $@"SELECT * FROM `kategorijos` WHERE kategorijos_id=?id";
		var drc = 
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapOne<Kategorija>(drc, (dre, t) => {
				t.Id = dre.From<int>("kategorijos_id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			});
		return result;
	}

	public static void Update(Kategorija kategorija)
	{			
		var query = 
			$@"UPDATE `kategorijos` 
			SET 
				pavadinimas=?pavadinimas 
			WHERE 
				kategorijos_id=?id";

		Sql.Update(query, args => {
			args.Add("?pavadinimas", kategorija.Pavadinimas);
			args.Add("?id", kategorija.Id);
		});							
	}

	public static void Insert(Kategorija kategorija)
	{			
		var query = $@"INSERT INTO `kategorijos` ( pavadinimas ) VALUES ( ?pavadinimas )";
		Sql.Insert(query, args => {
			args.Add("?pavadinimas", kategorija.Pavadinimas);
		});
	}

	public static void Delete(int id)
	{			
		var query = $@"DELETE FROM `kategorijos`  WHERE kategorijos_id=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});			
	}
}