namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


public class GamintojasRepo
{
	public static List<Gamintojas> ListGamintojas()
	{
		string query = $@"SELECT * FROM `gamintojai` ORDER BY gamintojo_id ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Gamintojas>(drc, (dre, t) => {
				t.Id = dre.From<int>("gamintojo_id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Salis = dre.From<string>("salis");
			});

		return result;
	}

	public static Gamintojas Find(int id)
	{
		var query = $@"SELECT * FROM `gamintojai` WHERE gamintojo_id=?id";
		var drc = 
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapOne<Gamintojas>(drc, (dre, t) => {
				t.Id = dre.From<int>("gamintojo_id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Salis = dre.From<string>("salis");
			});

		return result;
	}

	public static void Update(Gamintojas gamintojas)
	{			
		var query = 
			$@"UPDATE `gamintojai` 
			SET 
				pavadinimas=?pavadinimas,
				salis=?salis 
			WHERE 
				gamintojo_id=?id";

		Sql.Update(query, args => {
			args.Add("?pavadinimas", gamintojas.Pavadinimas);
			args.Add("?id", gamintojas.Id);
			args.Add("?salis", gamintojas.Salis);
		});							
	}

	public static void Insert(Gamintojas gamintojas)
	{			
		var query = $@"INSERT INTO `gamintojai` ( pavadinimas, salis ) VALUES ( ?pavadinimas, ?salis )";
		Sql.Insert(query, args => {
			args.Add("?pavadinimas", gamintojas.Pavadinimas);
			args.Add("?salis", gamintojas.Salis);
		});
	}

	public static void Delete(int id)
	{			
		var query = $@"DELETE FROM `gamintojai` where gamintojo_id=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});			
	}
}