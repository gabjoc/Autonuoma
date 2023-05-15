namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models.Automobilis;


/// <summary>
/// Database operations related to 'Automobilis'.
/// </summary>
public class AutomobilisRepo
{
	public static List<AutomobilisL> ListAutomobilis()
	{
		var query =
			$@"SELECT
				a.id,
				a.valstybinis_nr,
				b.name AS busena,
				m.pavadinimas AS modelis,
				mm.pavadinimas AS marke
			FROM
				{Config.TblPrefix}automobiliai a
				LEFT JOIN `{Config.TblPrefix}auto_busenos` b ON b.id = a.busena
				LEFT JOIN `{Config.TblPrefix}modeliai` m ON m.id = a.fk_modelis
				LEFT JOIN `{Config.TblPrefix}markes` mm ON mm.id = m.fk_marke
			ORDER BY a.id ASC";

		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<AutomobilisL>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.ValstybinisNr = dre.From<string>("valstybinis_nr");
				t.Busena = dre.From<string>("busena");
				t.Modelis = dre.From<string>("modelis");
				t.Marke = dre.From<string>("marke");
			});

		return result;
	}

	public static AutomobilisCE FindAutomobolisCE(int id)
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}automobiliai` WHERE id=?id";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result =
			Sql.MapOne<AutomobilisCE>(drc, (dre, t) => {
				//make a shortcut
				var auto = t.Automobilis;

				//
				auto.Id = dre.From<int>("id");
				auto.ValstybinisNr = dre.From<string>("valstybinis_nr");
				auto.PagaminimoData = dre.From<DateTime>("pagaminimo_data");
				auto.Rida = dre.From<int>("rida");
				auto.Radijas = dre.From<bool>("radijas");
				auto.Grotuvas = dre.From<bool>("grotuvas");
				auto.Kondicionierius = dre.From<bool>("kondicionierius");
				auto.VietuSkaicius = dre.From<int>("vietu_skaicius");
				auto.RegistravimoData = dre.From<DateTime>("registravimo_data");
				auto.Verte = dre.From<decimal>("verte");
				auto.FkPavaruDeze = dre.From<int>("pavaru_deze");
				auto.FkDegaluTipas = dre.From<int>("degalu_tipas");
				auto.FkKebuloTipas = dre.From<int>("kebulas");
				auto.FkLagaminas = dre.From<int>("bagazo_dydis");
				auto.FkBusena = dre.From<int?>("busena");
				auto.FkModelis = dre.From<int>("fk_modelis");
			});

		return result;
	}

	public static void InsertAutomobilis(AutomobilisCE autoCE)
	{
		var query =
			$@"INSERT INTO `{Config.TblPrefix}automobiliai`
			(
				`valstybinis_nr`,
				`pagaminimo_data`,
				`rida`,
				`radijas`,
				`grotuvas`,
				`kondicionierius`,
				`vietu_skaicius`,
				`registravimo_data`,
				`verte`,
				`pavaru_deze`,
				`degalu_tipas`,
				`kebulas`,
				`bagazo_dydis`,
				`busena`,
				`fk_modelis`
			)
			VALUES (
				?vlst_nr,
				?pag_data,
				?rida,
				?radijas,
				?grotuvas,
				?kond,
				?viet_sk,
				?reg_dt,
				?verte,
				?pav_deze,
				?dega_tip,
				?kebulas,
				?bagaz_tip,
				?busena,
				?fk_mod
			)";

		Sql.Insert(query, args => {
			//make a shortcut
			var auto = autoCE.Automobilis;

			//
			args.Add("?vlst_nr", auto.ValstybinisNr);
			args.Add("?pag_data", auto.PagaminimoData?.ToString("yyyy-MM-dd"));
			args.Add("?rida", auto.Rida);
			args.Add("?radijas", (auto.Radijas ? 1 : 0));
			args.Add("?grotuvas", (auto.Grotuvas ? 1 : 0));
			args.Add("?kond", (auto.Kondicionierius ? 1 : 0));
			args.Add("?viet_sk", auto.VietuSkaicius);
			args.Add("?reg_dt", auto.RegistravimoData?.ToString("yyyy-MM-dd"));
			args.Add("?verte", auto.Verte);
			args.Add("?pav_deze", auto.FkPavaruDeze);
			args.Add("?dega_tip", auto.FkDegaluTipas);
			args.Add("?kebulas", auto.FkKebuloTipas);
			args.Add("?bagaz_tip", auto.FkLagaminas);
			args.Add("?busena", auto.FkBusena);
			args.Add("?fk_mod", auto.FkModelis);
		});
	}

	public static void UpdateAutomobilis(AutomobilisCE autoCE)
	{
		var query =
			$@"UPDATE `{Config.TblPrefix}automobiliai`
			SET
				`valstybinis_nr` = ?vlst_nr,
				`pagaminimo_data` = ?pag_data,
				`rida` = ?rida,
				`radijas` = ?radijas,
				`grotuvas` = ?grotuvas,
				`kondicionierius` = ?kond,
				`vietu_skaicius` = ?viet_sk,
				`registravimo_data` = ?reg_dt,
				`verte` = ?verte,
				`pavaru_deze` = ?pav_deze,
				`degalu_tipas` = ?dega_tip,
				`kebulas` = ?kebulas,
				`bagazo_dydis` = ?bagaz_tip,
				`busena` = ?busena,
				`fk_modelis` = ?fk_mod
			WHERE
				id=?id";

		Sql.Update(query, args => {
			//make a shortcut
			var auto = autoCE.Automobilis;

			//
			args.Add("?vlst_nr", auto.ValstybinisNr);
			args.Add("?pag_data", auto.PagaminimoData?.ToString("yyyy-MM-dd"));
			args.Add("?rida", auto.Rida);
			args.Add("?radijas", (auto.Radijas ? 1 : 0));
			args.Add("?grotuvas", (auto.Grotuvas ? 1 : 0));
			args.Add("?kond", (auto.Kondicionierius ? 1 : 0));
			args.Add("?viet_sk", auto.VietuSkaicius);
			args.Add("?reg_dt", auto.RegistravimoData?.ToString("yyyy-MM-dd"));
			args.Add("?verte", auto.Verte);
			args.Add("?pav_deze", auto.FkPavaruDeze);
			args.Add("?dega_tip", auto.FkDegaluTipas);
			args.Add("?kebulas", auto.FkKebuloTipas);
			args.Add("?bagaz_tip", auto.FkLagaminas);
			args.Add("?busena", auto.FkBusena);
			args.Add("?fk_mod", auto.FkModelis);

			args.Add("?id", auto.Id);
		});
	}

	public static void DeleteAutomobilis(int id)
	{
		var query = $@"DELETE FROM `{Config.TblPrefix}automobiliai` WHERE id=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});
	}

	public static List<AutoBusena> ListAutoBusena()
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}auto_busenos` ORDER BY id ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<AutoBusena>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}

	public static List<PavaruDeze> ListPavaruDeze()
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}pavaru_dezes` ORDER BY id ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<PavaruDeze>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}

	public static List<KebuloTipas> ListKebuloTipas()
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}kebulu_tipai` ORDER BY id ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<KebuloTipas>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}

	public static List<DegaluTipas> ListDegaluTipas()
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}degalu_tipai` ORDER BY id ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<DegaluTipas>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}

	public static List<Lagaminas> ListLagaminas()
	{
		var query = $@"SELECT * FROM `{Config.TblPrefix}lagaminai` ORDER BY id ASC";
		var drc = Sql.Query(query);

		var result =
			Sql.MapAll<Lagaminas>(drc, (dre, t) => {
				t.Id = dre.From<int>("id");
				t.Pavadinimas = dre.From<string>("name");
			});

		return result;
	}
}
