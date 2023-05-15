namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using System;

using Org.Ktu.Isk.P175B602.Autonuoma.Models.Preke;


/// <summary>
/// Database operations related to 'Preke' entity.
/// </summary>
public class PrekeRepo
{
	public static List<PrekeL> ListPreke()
	{
		var query = 
		$@"SELECT
		 p.prekes_kodas,
		 p.pavadinimas,
		 p.kaina,
		 p.galiojimo_trukme,
		 k.pavadinimas AS kategorijos_pavadinimas,
		 g.pavadinimas AS gamintojo_pavadinimas
		 FROM
		`prekes` p
		 LEFT JOIN `kategorijos` k ON k.kategorijos_id = p.fk_KATEGORIJAkategorijos_id
		 LEFT JOIN `gamintojai` g ON g.gamintojo_id = p.fk_GAMINTOJASgamintojo_id
		 ORDER BY p.prekes_kodas ASC";

		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<PrekeL>(drc, (dre, t) => {
				t.PrekesKodas = dre.From<int>("prekes_kodas");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Kaina = dre.From<decimal>("kaina");
				t.Galiojimas = dre.From<int>("galiojimo_trukme");
				t.Kategorija = dre.From<string>("kategorijos_pavadinimas");
				t.Gamintojas = dre.From<string>("gamintojo_pavadinimas");
			});
		return result;
	}

		public static PrekeL FindL(int id)
	{
		var query =	 
		$@"SELECT
		 p.prekes_kodas,
		 p.pavadinimas,
		 p.kaina,
		 p.galiojimo_trukme,
		 k.pavadinimas AS kategorijos_pavadinimas,
		 g.pavadinimas AS gamintojo_pavadinimas
		 FROM
		 `prekes` p
		 LEFT JOIN `kategorijos` k ON k.kategorijos_id = p.fk_KATEGORIJAkategorijos_id
		 LEFT JOIN `gamintojai` g ON g.gamintojo_id = p.fk_GAMINTOJASgamintojo_id
		 WHERE prekes_kodas = ?id
		 ORDER BY p.prekes_kodas ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result =
			Sql.MapOne<PrekeL>(drc, (dre, t) => {
				t.PrekesKodas = dre.From<int>("prekes_kodas");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Kaina = dre.From<decimal>("kaina");
				t.Galiojimas = dre.From<int>("galiojimo_trukme");
				t.Kategorija = dre.From<string>("kategorijos_pavadinimas");
				t.Gamintojas = dre.From<string>("gamintojo_pavadinimas");
			});
		return result;
	}
	//ieskojimas vyksta duombazej, nes parde duombazej yra numeriukas
	public static PrekeCE FindCE(int id)
	{
		var query = $@"SELECT * FROM `prekes` WHERE prekes_kodas=?id";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result =
			Sql.MapOne<PrekeCE>(drc, (dre, t) => {
				t.Preke.PrekesKodas = dre.From<int>("prekes_kodas");
				t.Preke.Pavadinimas = dre.From<string>("pavadinimas");
				t.Preke.Sudetis = dre.From<string>("sudetis");
				t.Preke.Kaina = dre.From<decimal>("kaina");
				t.Preke.Aprasymas = dre.From<string>("aprasymas");
				t.Preke.Ispejimai = dre.From<string>("ispejimai");
				t.Preke.Galiojimas = dre.From<int>("galiojimo_trukme");
				t.Preke.FkKategorija = dre.From<int>("fk_KATEGORIJAkategorijos_id");
				t.Preke.FkGamintojas = dre.From<int>("fk_GAMINTOJASgamintojo_id");
			});

		return result;
	}
	//kuriama nauja Preke
	public static int Insert(PrekeCE preke)
	{
		var query =
			$@"INSERT INTO `prekes`
			(
				`pavadinimas`,
				`sudetis`,
				`kaina`,
				`aprasymas`,
				`ispejimai`,
				`galiojimo_trukme`,
				`fk_KATEGORIJAkategorijos_id`,
				`fk_GAMINTOJASgamintojo_id`
			)
			VALUES (
				?pavadinimas,
				?sudetis,
				?kaina,
				?aprasymas,
				?ispejimai,
				?galiojimas,
				?FkKategorija,
				?FkGamintojas
			)";

		int id = (int)Sql.Insert(query, args => {
			args.Add("?pavadinimas", preke.Preke.Pavadinimas);
			args.Add("?sudetis", preke.Preke.Sudetis);
			args.Add("?kaina", preke.Preke.Kaina);
			args.Add("?aprasymas", preke.Preke.Aprasymas);
			args.Add("?ispejimai", preke.Preke.Ispejimai);
			args.Add("?galiojimas", preke.Preke.Galiojimas);
			args.Add("?FkKategorija", preke.Preke.FkKategorija);
			args.Add("?FkGamintojas", preke.Preke.FkGamintojas);
		});

		return id;
	}
	//atnaujinamas Preke (imama ce nes pardes id - numeriukas)
	public static void Update(PrekeCE preke)
	{
		var query =
			$@"UPDATE `prekes`
			SET
				`pavadinimas` = ?pavadinimas,
				`sudetis` = ?sudetis,
				`kaina` = ?kaina,
				`aprasymas` = ?aprasymas,
				`ispejimai` = ?ispejimai,
				`galiojimo_trukme` = ?galiojimas,
				`fk_KATEGORIJAkategorijos_id` = ?kategorija,
				`fk_GAMINTOJASgamintojo_id` = ?gamintojas
			WHERE
				prekes_kodas=?kodas";

		Sql.Update(query, args => {
			
			args.Add("?pavadinimas", preke.Preke.Pavadinimas);
			args.Add("?sudetis", preke.Preke.Sudetis);
			args.Add("?kaina", preke.Preke.Kaina);
			args.Add("?aprasymas", preke.Preke.Aprasymas);
			args.Add("?ispejimai", preke.Preke.Ispejimai);
			args.Add("?galiojimas", preke.Preke.Galiojimas);
			args.Add("?kategorija", preke.Preke.FkKategorija);
			args.Add("?gamintojas", preke.Preke.FkGamintojas);
			args.Add("?kodas", preke.Preke.PrekesKodas);
		});
	}
	//istrinimas
	public static void Delete(int id)
	{
		var query = $@"DELETE FROM `prekes` WHERE prekes_kodas=?kodas";
		Sql.Delete(query, args => {
			args.Add("?kodas", id);
		});
	}
	// cia paziuret ar jo tikrai reik, veikia be sito 
    internal static void InsertPreke(int tabelis)
    {
        throw new NotImplementedException();
    }
}
