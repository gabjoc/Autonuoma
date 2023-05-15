namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models.Uzsakymas;


/// <summary>
/// Database operations related to 'uzsakmobilis'.
/// </summary>
public class UzsakymasRepo
{
	public static List<UzsakymasL> ListUzsakymas()
	{
		var query =
			$@"SELECT
				u.uzsakymo_nr,
				u.uzsakymo_data,
                u.suma,
				b.name AS busena,
				p.name AS pristatymas,
                CONCAT(k.vardas, ' ', k.pavarde) AS klientas 
			FROM
				`uzsakymai` u
				LEFT JOIN `busenos` b ON b.id_busena = u.busena
				LEFT JOIN `pristatymai` p ON p.id_pristatymas = u.pristatymo_tipas 
				LEFT JOIN `klientai` k ON k.pirkejo_nr = u.fk_KLIENTASpirkejo_nr
			ORDER BY u.uzsakymo_nr ASC";

		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<UzsakymasL>(drc, (dre, t) => {
				t.Uzsakymas = dre.From<int>("uzsakymo_nr");
				t.UzsakymoData = dre.From<DateTime>("uzsakymo_data");
				t.Suma = dre.From<double>("suma");
                t.Busena = dre.From<string>("busena");
				t.Pristatymas = dre.From<string>("pristatymas");
                t.Klientas = dre.From<string>("klientas");
			});

		return result;
	}
	public static UzsakymasL FindUzsakymasL(int id)
	{
		var query =	 
		$@"SELECT
		 u.uzsakymo_nr,
		 u.uzsakymo_data,
         u.suma,
		 b.name AS busena,
		 p.name AS pristatymas,
         CONCAT(k.vardas, ' ', k.pavarde) AS klientas 
		 FROM
				`uzsakymai` u
				LEFT JOIN `busenos` b ON b.id_busena = u.busena
				LEFT JOIN `pristatymai` p ON p.id_pristatymas = u.pristatymo_tipas 
				LEFT JOIN `klientai` k ON k.pirkejo_nr = u.fk_KLIENTASpirkejo_nr
		 WHERE uzsakymo_nr=?id
		 ORDER BY u.uzsakymo_nr ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result =
			Sql.MapOne<UzsakymasL>(drc, (dre, t) => {

				t.Uzsakymas = dre.From<int>("uzsakymo_nr");
				t.UzsakymoData = dre.From<DateTime>("uzsakymo_data");
				t.Suma = dre.From<double>("suma");
                t.Busena = dre.From<string>("busena");
				t.Pristatymas = dre.From<string>("pristatymas");
                t.Klientas = dre.From<string>("klientas");
			});

		return result;
	}

	public static UzsakymasCE FindUzsakymasCE(int uzsakymo_nr)
	{
		var query = $@"SELECT * FROM `uzsakymai` WHERE uzsakymo_nr=?uzsakymo_nr";

		var drc =
			Sql.Query(query, args => {
				args.Add("?uzsakymo_nr", uzsakymo_nr);
			});

		var result =
			Sql.MapOne<UzsakymasCE>(drc, (dre, t) => {
				//make a shortcut
				var uzsak = t.Uzsakymas;

				//
				uzsak.Uzsakymas = dre.From<int>("uzsakymo_nr");
				uzsak.UzsakymoData = dre.From<DateTime>("uzsakymo_data");
				uzsak.Suma = dre.From<double>("suma");
				uzsak.Adresas = dre.From<string>("pristatymo_adresas");
				uzsak.FkDarbuotojas = dre.From<int>("fk_DARBUOTOJAStabelio_nr");
				uzsak.FkKlientas= dre.From<int>("fk_KLIENTASpirkejo_nr");
				uzsak.FkBusena = dre.From<int>("busena");
				uzsak.FkPristatymas = dre.From<int>("pristatymo_tipas");
			});

		return result;
	}

	public static int InsertUzsakymas(UzsakymasCE uzsakCE)
	{
		var query =
			$@"INSERT INTO `uzsakymai`
			(
				`uzsakymo_data`,
				`suma`,
				`pristatymo_adresas`,
				`fk_KLIENTASpirkejo_nr`,
				`fk_DARBUOTOJAStabelio_nr`,
				`busena`,
				`pristatymo_tipas`
			)
			VALUES (
				?data,
				?suma,
				?adresas,
				?klientas,
				?darbuotojas,
				?busena,
				?pristatymas
			)";

		int id = (int)Sql.Insert(query, args => {
			//make a shortcut
			var uzsak = uzsakCE.Uzsakymas;

			//
			args.Add("?data", uzsak.UzsakymoData.ToString("yyyy-MM-dd"));
			args.Add("?suma", uzsak.Suma);
			args.Add("?adresas", (uzsak.Adresas));
			args.Add("?klientas", (uzsak.FkKlientas));
			args.Add("?darbuotojas", (uzsak.FkDarbuotojas));
			args.Add("?busena", uzsak.FkBusena);
			args.Add("?pristatymas", uzsak.FkPristatymas);
		});

		return id;
	}

	public static void UpadateUzsakymas(UzsakymasCE uzsakCE)
	{
		var query =
			$@"UPDATE `uzsakymai`
			SET
				`uzsakymo_data` = ?data,
				`suma` = ?suma,
				`pristatymo_adresas` = ?adresas,
				`fk_KLIENTASpirkejo_nr` = ?klientas,
				`fk_DARBUOTOJAStabelio_nr` = ?darbuotojas,
				`busena` = ?busena,
				`pristatymo_tipas` = ?pristatymas
			WHERE
				uzsakymo_nr=?uzsakymo_nr";

		Sql.Update(query, args => {
			//make a shortcut
			var uzsak = uzsakCE.Uzsakymas;

			//
			
			args.Add("?adresas", (uzsak.Adresas));
			args.Add("?suma", uzsak.Suma);
			args.Add("?data", uzsak.UzsakymoData.ToString("yyyy-MM-dd"));
			args.Add("?klientas", uzsak.FkKlientas);
			args.Add("?darbuotojas", uzsak.FkDarbuotojas);
			args.Add("?busena", uzsak.FkBusena);
			args.Add("?pristatymas", uzsak.FkPristatymas);

			args.Add("?uzsakymo_nr", uzsak.Uzsakymas);
		});
	}

	public static void DeleteUzsakymas(int uzsakymo_nr)
	{
		var query = $@"DELETE FROM `uzsakymai` WHERE uzsakymo_nr=?uzsakymo_nr";
		Sql.Delete(query, args => {
			args.Add("?uzsakymo_nr", uzsakymo_nr);
		});
	}

	public static List<UzsakymoBusena> ListUzsakymoBusena()
	{
		var query = $@"SELECT * FROM `busenos` ORDER BY id_busena ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<UzsakymoBusena>(drc, (dre, t) => {
				t.Id = dre.From<int>("id_busena");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}

	
	public static List<Pristatymai> ListPristatymas()
	{
		var query = $@"SELECT * FROM `pristatymai` ORDER BY id_pristatymas ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<Pristatymai>(drc, (dre, t) => {
				t.Id = dre.From<int>("id_pristatymas");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}
}
