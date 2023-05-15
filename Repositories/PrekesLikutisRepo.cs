namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;

/// <summary>
/// Database operations related to 'PrekesLikutis' entity.
/// </summary>
public class PrekesLikutisRepo
{
	public static List<PrekesLikutis> LoadForPreke(int id)
	{
		var query =
			$@"SELECT
				pl.fk_PARDUOTUVEparduotuves_id,
				pl.kiekis,
				pl.likucio_id,
				pl.fk_PREKEprekes_kodas
			FROM
			 `likuciai` as pl
			WHERE pl.fk_PREKEprekes_kodas=?id
			ORDER BY pl.kiekis DESC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapAll<PrekesLikutis>(drc, (dre, t) => {
				t.Likutis.FkPreke = dre.From<int>("fk_PREKEprekes_kodas");
				t.Likutis.Kiekis = dre.From<int>("kiekis");
				t.Likutis.Id = dre.From<int>("likucio_id");
				t.Likutis.FkParduotuve = dre.From<int>("fk_PARDUOTUVEparduotuves_id");
			});

		// sito mums reikia, nes kitaip mes negalime zinoti, koks yra likucio indeksas sarase (kai istriname likuti prekes redagavime)
		for (int i = 0; i < result.Count; i++)
		{
			result[i].Likutis.InListId = i;
		}

		return result;
	}

	public static void Delete(int Id)
	{
		var query = 
			$@"DELETE FROM `likuciai`
			WHERE 
				likucio_id=?Id";

		Sql.Delete(query, args => {
			args.Add("?Id", Id);
		});
	}

	public static void Insert(PrekesLikutis likutis)
	{
		string query = 
			$@"INSERT INTO `likuciai`
			(
				kiekis,
				fk_PARDUOTUVEparduotuves_id,
				fk_PREKEprekes_kodas
			)
			VALUES(
				?kiekis,
				?fk_PARDUOTUVEparduotuves_id,
				?fk_PREKEprekes_kodas
			)";

		Sql.Insert(query, args => {
			args.Add("?kiekis", likutis.Likutis.Kiekis);
			args.Add("?fk_PARDUOTUVEparduotuves_id", likutis.Likutis.FkParduotuve);
			args.Add("?fk_PREKEprekes_kodas", likutis.Likutis.FkPreke);
		});
	}

	public static void Update(PrekesLikutis likutis)
	{
		string query = 
		$@"UPDATE `likuciai`
		SET
			kiekis = ?kiekis,
			fk_PARDUOTUVEparduotuves_id = ?fk_PARDUOTUVEparduotuves_id
		WHERE 
			likucio_id = ?likucio_id";

		Sql.Insert(query, args => {
			args.Add("?likucio_id", likutis.Likutis.Id);
			args.Add("?kiekis", likutis.Likutis.Kiekis);
			args.Add("?fk_PARDUOTUVEparduotuves_id", likutis.Likutis.FkParduotuve);
		});
	}

	public static int SumLikutisForPreke(int prekeskodas)
	{
		string query = 
		$@"SELECT SUM(kiekis)
		FROM `likuciai`
		WHERE fk_PREKEprekes_kodas = ?id";
		var result = Sql.Query(query, args => {
			args.Add("?id", prekeskodas);
		});

    if (result.Count > 0)
    {
        return Convert.ToInt32(result[0][0]);
    }
    else
    {
        return 0;
	}
	}
}