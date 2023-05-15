namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


/// <summary>
/// Database operations related to 'Klientas' entity.
/// </summary>
public class KlientasRepo
{
	public static List<Klientas> List()
	{
		var query = $@"SELECT * FROM `klientai` ORDER BY pirkejo_nr ASC";
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<Klientas>(drc, (dre, t) => {
				t.PirkejasNr = dre.From<int>("pirkejo_nr");
				t.Vardas = dre.From<string>("vardas");
				t.Pavarde = dre.From<string>("pavarde");
				t.Saskaita = dre.From<string>("saskaitos_nr");
				t.Adresas = dre.From<string>("adresas");
				t.Telefonas = dre.From<string>("telefonas");
				t.Epastas = dre.From<string>("el_pastas");
			});

		return result;
	}

	public static Klientas Find(string pirknr)
	{
		var query = $@"SELECT * FROM `klientai` WHERE pirkejo_nr=?pirknr";

		var drc =
			Sql.Query(query, args => {
				args.Add("?pirknr", pirknr);
			});

			var result = 
				Sql.MapOne<Klientas>(drc, (dre, t) => {
					t.PirkejasNr = dre.From<int>("pirkejo_nr");
					t.Vardas = dre.From<string>("vardas");
					t.Pavarde = dre.From<string>("pavarde");
					t.Saskaita = dre.From<string>("saskaitos_nr");
					t.Adresas = dre.From<string>("adresas");
					t.Telefonas = dre.From<string>("telefonas");
					t.Epastas = dre.From<string>("el_pastas");
				});

			return result;
	}

	public static void Insert(Klientas klientas)
	{
		var query =
			$@"INSERT INTO `klientai`
			(
				vardas,
				pavarde,
				saskaitos_nr,
				adresas,
				telefonas,
				el_pastas
			)
			VALUES(
				?vardas,
				?pavarde,
				?saskaita,
				?adresas,
				?tel,
				?email
			)";

		Sql.Insert(query, args => {
			args.Add("?vardas", klientas.Vardas);
			args.Add("?pavarde", klientas.Pavarde);
			args.Add("?saskaita", klientas.Saskaita);
			args.Add("?adresas", klientas.Adresas);
			args.Add("?tel", klientas.Telefonas);
			args.Add("?email", klientas.Epastas);
		});
	}

	public static void Update(Klientas klientas)
	{
		var query =
			$@"UPDATE `klientai`
			SET
				vardas=?vardas,
				pavarde=?pavarde,
				saskaitos_nr=?saskaita,
				adresas=?adresas,
				telefonas=?tel,
				el_pastas=?email
			WHERE
				pirkejo_nr=?pirknr";

		Sql.Update(query, args => {
			args.Add("?pirknr", klientas.PirkejasNr);
			args.Add("?vardas", klientas.Vardas);
			args.Add("?pavarde", klientas.Pavarde);
			args.Add("?saskaita", klientas.Saskaita);
			args.Add("?adresas", klientas.Adresas);
			args.Add("?tel", klientas.Telefonas);
			args.Add("?email", klientas.Epastas);
		});
	}

	public static void Delete(string id)
	{
		var query = $@"DELETE FROM `klientai` WHERE pirkejo_nr=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});
	}
}
