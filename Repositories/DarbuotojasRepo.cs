namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using System;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


/// <summary>
/// Database operations related to 'Darbuotojas' entity.
/// </summary>
public class DarbuotojasRepo
{
	public static List<DarbuotojasL> ListDarbuotojas()
	{
		var query = 
		$@"SELECT
		 d.tabelio_nr,
		 d.vardas,
		 d.pavarde,
		 d.telefono_nr,
		 d.el_pastas,
		 p.pavadinimas AS parduotuves_pavadinimas
		 FROM
		`darbuotojai` d
		LEFT JOIN `parduotuves` p ON p.parduotuves_id = d.fk_PARDUOTUVEparduotuves_id
		ORDER BY d.tabelio_nr ASC";

		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<DarbuotojasL>(drc, (dre, t) => {
				t.Tabelis = dre.From<int>("tabelio_nr");
				t.Vardas = dre.From<string>("vardas");
				t.Pavarde = dre.From<string>("pavarde");
				t.Telefonas = dre.From<string>("telefono_nr");
				t.Pastas = dre.From<string>("el_pastas");
				t.Parduotuve = dre.From<string>("parduotuves_pavadinimas");
			});

		return result;
	}

		public static DarbuotojasL FindL(int id)
	{
		var query =	 
		$@"SELECT
		 d.tabelio_nr,
		 d.vardas,
		 d.pavarde,
		 d.telefono_nr,
		 d.el_pastas,
		 p.pavadinimas AS parduotuves_pavadinimas
		 FROM
		 `darbuotojai` d
		 LEFT JOIN `parduotuves` p ON p.parduotuves_id = d.fk_PARDUOTUVEparduotuves_id
		 WHERE tabelio_nr=?id
		 ORDER BY d.tabelio_nr ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result =
			Sql.MapOne<DarbuotojasL>(drc, (dre, t) => {

				t.Tabelis = dre.From<int>("tabelio_nr");
				t.Vardas = dre.From<string>("vardas");
				t.Pavarde = dre.From<string>("pavarde");
				t.Telefonas = dre.From<string>("telefono_nr");
				t.Pastas = dre.From<string>("el_pastas");
				t.Parduotuve = dre.From<string>("parduotuves_pavadinimas");
			});

		return result;
	}
	//ieskojimas vyksta duombazej, nes parde duombazej yra numeriukas
	public static DarbuotojasCE FindCE(int id)
	{
		var query = $@"SELECT * FROM `darbuotojai` WHERE tabelio_nr=?id";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result =
			Sql.MapOne<DarbuotojasCE>(drc, (dre, t) => {

				t.Darbuotojas.Tabelis = dre.From<int>("tabelio_nr");
				t.Darbuotojas.Vardas = dre.From<string>("vardas");
				t.Darbuotojas.Pavarde = dre.From<string>("pavarde");
				t.Darbuotojas.Telefonas = dre.From<string>("telefono_nr");
				t.Darbuotojas.Pastas = dre.From<string>("el_pastas");
				t.Darbuotojas.FkParduotuve = dre.From<int>("fk_PARDUOTUVEparduotuves_id");
			});

		return result;
	}
	//kuriamas naujas darbuotojas
	public static void Insert(DarbuotojasCE darb)
	{
		var query =
			$@"INSERT INTO `darbuotojai`
			(
				`tabelio_nr`,
				`vardas`,
				`pavarde`,
				`telefono_nr`,
				`el_pastas`,
				`fk_PARDUOTUVEparduotuves_id`
			)
			VALUES (
				?tabelis,
				?vardas,
				?pavarde,
				?telefonas,
				?pastas,
				?FkParduotuve
			)";

		Sql.Insert(query, args => {
			
			args.Add("?tabelis", darb.Darbuotojas.Tabelis);
			args.Add("?vardas", darb.Darbuotojas.Vardas);
			args.Add("?pavarde", darb.Darbuotojas.Pavarde);
			args.Add("?telefonas", darb.Darbuotojas.Telefonas);
			args.Add("?pastas", darb.Darbuotojas.Pastas);
			args.Add("?FkParduotuve", darb.Darbuotojas.FkParduotuve);
		});
	}
	//atnaujinamas darbuotojas (imama ce nes pardes id - numeriukas)
	public static void Update(DarbuotojasCE darb)
	{
		var query =
			$@"UPDATE `darbuotojai`
			SET
				`vardas` = ?vardas,
				`pavarde` = ?pavarde,
				`telefono_nr` = ?telefonas,
				`el_pastas` = ?pastas,
				`fk_PARDUOTUVEparduotuves_id` = ?parde
			WHERE
				tabelio_nr=?tabelis";

		Sql.Update(query, args => {
			
			args.Add("?vardas", darb.Darbuotojas.Vardas);
			args.Add("?pavarde", darb.Darbuotojas.Pavarde);
			args.Add("?telefonas", darb.Darbuotojas.Telefonas);
			args.Add("?pastas", darb.Darbuotojas.Pastas);
			args.Add("?parde", darb.Darbuotojas.FkParduotuve);
			args.Add("?tabelis", darb.Darbuotojas.Tabelis);
		});
	}
	//istrinimas
	public static void Delete(int id)
	{
		var query = $@"DELETE FROM `darbuotojai` WHERE tabelio_nr=?tabelis";
		Sql.Delete(query, args => {
			args.Add("?tabelis", id);
		});
	}
	// cia paziuret ar jo tikrai reik, veikia be sito 
    internal static void InsertDarbuotojas(int tabelis)
    {
        throw new NotImplementedException();
    }
}
