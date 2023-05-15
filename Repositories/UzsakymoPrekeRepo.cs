namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;

/// <summary>
/// Database operations related to 'PrekesLikutis' entity.
/// </summary>
public class UzsakymoPrekeRepo
{
	public static List<UzsakymoPreke> LoadForUzsakymas(int id)
	{
		var query =
			$@"SELECT
				up.kiekis,
				up.fk_PREKEprekes_kodas,
				up.fk_UZSAKYMASuzsakymo_nr
			FROM
			 `uzsakymo_prekes` as up
			WHERE up.fk_UZSAKYMASuzsakymo_nr=?id
			ORDER BY up.kiekis DESC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});

		var result = 
			Sql.MapAll<UzsakymoPreke>(drc, (dre, t) => {
				t.Uzsakymopreke.FkUzsakymas = dre.From<int>("fk_UZSAKYMASuzsakymo_nr");
				t.Uzsakymopreke.Kiekis = dre.From<int>("kiekis");
				t.Uzsakymopreke.FkPreke = dre.From<int>("fk_PREKEprekes_kodas");
			});

		// sito mums reikia, nes kitaip mes negalime zinoti, koks yra likucio indeksas sarase (kai istriname likuti prekes redagavime)
		for (int i = 0; i < result.Count; i++)
		{
			result[i].Uzsakymopreke.InListId = i;
		}
		return result;
	}

	public static void Delete(int Id, int id1)
	{
		var query = 
			$@"DELETE FROM `uzsakymo_prekes`
			WHERE 
				fk_PREKEprekes_kodas=?prekeskodas AND fk_UZSAKYMASuzsakymo_nr=?uzsakymas";

		Sql.Delete(query, args => {
			args.Add("?prekeskodas", Id);
			args.Add("?uzsakymas", id1);
		});
	}

	public static void Insert(UzsakymoPreke uzsakymas)
	{
		string query = 
			$@"INSERT INTO `uzsakymo_prekes`
			(
				kiekis,
				fk_PREKEprekes_kodas,
				fk_UZSAKYMASuzsakymo_nr
			)
			VALUES(
				?kiekis,
				?prekeskodas,
				?uzsakymas
			)";

		Sql.Insert(query, args => {
			args.Add("?kiekis", uzsakymas.Uzsakymopreke.Kiekis);
			args.Add("?prekeskodas", uzsakymas.Uzsakymopreke.FkPreke);
			args.Add("?uzsakymas", uzsakymas.Uzsakymopreke.FkUzsakymas);
		});
	}

	public static void Update(UzsakymoPreke uzsakymas)
	{
		string query = 
		$@"UPDATE `uzsakymo_prekes`
		SET
			kiekis = ?kiekis,
			fk_PREKEprekes_kodas = ?prekeskodas
		WHERE 
			fk_PREKEprekes_kodas = ?prekeskodas AND fk_UZSAKYMASuzsakymo_nr = ?uzsakymas";

		Sql.Insert(query, args => {
			args.Add("?prekeskodas", uzsakymas.Uzsakymopreke.FkPreke);
			args.Add("?kiekis", uzsakymas.Uzsakymopreke.Kiekis);
			args.Add("?uzsakymas", uzsakymas.Uzsakymopreke.FkUzsakymas);
		});
	}
}