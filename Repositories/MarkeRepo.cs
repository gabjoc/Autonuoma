namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class MarkeRepo
{
	public static List<Marke> List()
	{
		string query = $@"SELECT * FROM `{Config.TblPrefix}markes` ORDER BY id ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Marke>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			});

		return result;
	}

	public static Marke Find(int id)
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}markes` WHERE id=?id";
		var drc = 
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapOne<Marke>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			});

		return result;
	}

	public static void Update(Marke marke)
	{			
		var query = 
			$@"UPDATE `{Config.TblPrefix}markes` 
			SET 
				pavadinimas=?pavadinimas 
			WHERE 
				id=?id";

		Sql.Update(query, args => {
			args.Add("?pavadinimas", marke.Pavadinimas);
			args.Add("?id", marke.Id);
		});							
	}

	public static void Insert(Marke marke)
	{			
		var query = $@"INSERT INTO `{Config.TblPrefix}markes` ( pavadinimas ) VALUES ( ?pavadinimas )";
		Sql.Insert(query, args => {
			args.Add("?pavadinimas", marke.Pavadinimas);
		});
	}

	public static void Delete(int id)
	{			
		var query = $@"DELETE FROM `{Config.TblPrefix}markes` where id=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});			
	}
}
