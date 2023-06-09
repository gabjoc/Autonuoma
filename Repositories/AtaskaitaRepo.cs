﻿namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using MySql.Data.MySqlClient;

using LateContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.LateContractsReport;
using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;
using ServicesReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.ServicesReport;


/// <summary>
/// Database operations related to reports.
/// </summary>
public class AtaskaitaRepo
{
	public static List<ServicesReport.Uzsakymas> GetServicesOrdered(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				uz.uzsakymo_nr,
				uz.uzsakymo_data,
				CASE
					WHEN uz.busena = 3 THEN 0
					ELSE SUM(kiek.kiekis)
					END AS prekes_kiekis,
				CASE
					WHEN uz.busena = 3 THEN 0
					ELSE SUM(kiek.kiekis * prekes.kaina)
					END AS bendra_suma,
				CONCAT(k.vardas, ' ', k.pavarde) AS klientas,
				CONCAT(d.vardas, ' ', d.pavarde) AS darbuotojas
			FROM
				`uzsakymai` uz
				LEFT JOIN `uzsakymo_prekes` kiek ON kiek.fk_UZSAKYMASuzsakymo_nr = uz.uzsakymo_nr
				LEFT JOIN `prekes` prekes ON prekes.prekes_kodas = kiek.fk_PREKEprekes_kodas
				INNER JOIN `klientai` k ON k.pirkejo_nr = uz.fk_KLIENTASpirkejo_nr
				INNER JOIN `darbuotojai` d on d.tabelio_nr = uz.fk_DARBUOTOJAStabelio_nr
			WHERE
				uz.uzsakymo_data >= IFNULL(?nuo, uz.uzsakymo_data)
				AND uz.uzsakymo_data <= IFNULL(?iki, uz.uzsakymo_data)
			GROUP BY
				uz.uzsakymo_nr, uz.uzsakymo_data, uz.busena
			ORDER BY
				uz.uzsakymo_nr ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapAll<ServicesReport.Uzsakymas>(drc, (dre, t) => {
				t.UzsakymoNr = dre.From<int>("uzsakymo_nr");
				t.UzsakymoData = dre.From<DateTime>("uzsakymo_data");
				t.Kiekis = dre.From<int>("prekes_kiekis");
				t.Suma = dre.From<decimal>("bendra_suma");
				t.Klientas = dre.From<string>("klientas");
				t.darbuotojas = dre.From<string>("darbuotojas");
			});
		return result;
	}

	public static ServicesReport.Report GetTotalServicesOrdered(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				COUNT(DISTINCT uz.uzsakymo_nr) AS bendras_kiekis,
				SUM(
					CASE
					WHEN uz.busena <> 3 THEN kiek.kiekis
					ELSE 0
					END
				) AS kiekis,
				SUM(
					CASE
					WHEN uz.busena <> 3 THEN kiek.kiekis * prekes.kaina
					ELSE 0
					END
				) AS suma
			FROM
				`uzsakymai` uz
				LEFT JOIN `uzsakymo_prekes` kiek ON kiek.fk_UZSAKYMASuzsakymo_nr = uz.uzsakymo_nr
				LEFT JOIN `prekes` prekes ON prekes.prekes_kodas = kiek.fk_PREKEprekes_kodas
			WHERE
				uz.uzsakymo_data >= IFNULL(?nuo, uz.uzsakymo_data)
				AND uz.uzsakymo_data <= IFNULL(?iki, uz.uzsakymo_data)";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapOne<ServicesReport.Report>(drc, (dre, t) => {
				t.VisoUzsakytaPrekiu = dre.From<int?>("kiekis") ;
				t.VisoBendraSuma = dre.From<decimal?>("suma") ;
				t.VisoBendrasUzakKiekis = dre.From<int?>("bendras_kiekis");
			});

		return result;
	}

	public static List<ServicesReport.Uzsakymas> GetContracts(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				uz.uzsakymo_nr,
				uz.uzsakymo_data,
				CASE
					WHEN uz.busena = 3 THEN 0
					ELSE SUM(kiek.kiekis)
					END AS prekes_kiekis,
				CASE
					WHEN uz.busena = 3 THEN 0
					ELSE SUM(kiek.kiekis * prekes.kaina)
					END AS bendraUzsakymo_suma,
				CONCAT(k.vardas, ' ', k.pavarde) AS klientas,
				CONCAT(d.vardas, ' ', d.pavarde) AS darbuotojas,
				k.pirkejo_nr,
				COUNT(DISTINCT uz.uzsakymo_nr) AS bendras_kiekis,
				SUM(
					CASE
					WHEN uz.busena <> 3 THEN kiek.kiekis
					ELSE 0
					END
				) AS kiekis,
				SUM(
					CASE
					WHEN uz.busena <> 3 THEN kiek.kiekis * prekes.kaina
					ELSE 0
					END
				) AS bendra_suma
			FROM
				`uzsakymai` uz
				LEFT JOIN `uzsakymo_prekes` kiek ON kiek.fk_UZSAKYMASuzsakymo_nr = uz.uzsakymo_nr
				LEFT JOIN `prekes` prekes ON prekes.prekes_kodas = kiek.fk_PREKEprekes_kodas
				INNER JOIN `klientai` k ON k.pirkejo_nr = uz.fk_KLIENTASpirkejo_nr
				INNER JOIN `darbuotojai` d on d.tabelio_nr = uz.fk_DARBUOTOJAStabelio_nr
				LEFT JOIN
					(
						SELECT
							k1.pirkejo_nr,
							COUNT(DISTINCT uz1.uzsakymo_nr) AS bendras_kiekis,
							SUM(
								CASE
									WHEN uz1.busena <> 3 THEN kiek1.kiekis
									ELSE 0
								END
							) AS kiekis,
							SUM(
								CASE
									WHEN uz1.busena <> 3 THEN kiek1.kiekis * prekes.kaina
									ELSE 0
								END
							) AS bendra_suma
						FROM
							`uzsakymai` uz1
							INNER JOIN `klientai` k1 ON k1.pirkejo_nr = uz1.fk_KLIENTASpirkejo_nr
							INNER JOIN `uzsakymo_prekes` kiek1 ON kiek1.fk_UZSAKYMASuzsakymo_nr = uz1.uzsakymo_nr
							INNER JOIN `prekes` prekes ON prekes.prekes_kodas = kiek1.fk_PREKEprekes_kodas
						WHERE
							uz1.uzsakymo_data >= IFNULL(?nuo, uz1.uzsakymo_data)
							AND uz1.uzsakymo_data <= IFNULL(?iki, uz1.uzsakymo_data)
						GROUP BY k1.pirkejo_nr
					) AS klie ON klie.pirkejo_nr = k.pirkejo_nr
			WHERE
				uz.uzsakymo_data >= IFNULL(?nuo, uz.uzsakymo_data)
				AND uz.uzsakymo_data <= IFNULL(?iki, uz.uzsakymo_data)
			GROUP BY
				uz.uzsakymo_nr, k.pirkejo_nr
			ORDER BY
				k.pavarde ASC";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapAll<ServicesReport.Uzsakymas>(drc, (dre, t) => {
				t.UzsakymoNr = dre.From<int>("uzsakymo_nr");
				t.UzsakymoData = dre.From<DateTime>("uzsakymo_data");
				t.Klientas = dre.From<string>("klientas");
				t.darbuotojas = dre.From<string>("darbuotojas");
				t.Suma = dre.From<decimal>("bendraUzsakymo_suma");
				t.Kiekis = dre.From<int>("prekes_kiekis");
				t.BendrasPrekiuKiekis = dre.From<int>("kiekis");
				t.BendraSuma = dre.From<decimal>("bendra_suma");
				t.BendrasUzsakymuKiekis = dre.From<int>("bendras_kiekis");
			});

		return result;
	}

	public static List<LateContractsReport.Sutartis> GetLateReturnContracts(DateTime? dateFrom, DateTime? dateTo)
	{
		var query =
			$@"SELECT
				sut.nr,
				sut.sutarties_data,
				CONCAT(kln.vardas, ' ',kln.pavarde) as klientas,
				sut.planuojama_grazinimo_data_laikas,
				IF(
					IFNULL(sut.faktine_grazinimo_data_laikas,'0000-00-00') LIKE '0000%',
					'negražinta',
					sut.faktine_grazinimo_data_laikas
				) as grazinimo_data
			FROM `{Config.TblPrefix}sutartys` sut, `{Config.TblPrefix}klientai` kln
			WHERE
				kln.asmens_kodas = sut.fk_klientas
				AND (
					DATEDIFF(sut.faktine_grazinimo_data_laikas, sut.planuojama_grazinimo_data_laikas) >= 1
					OR IFNULL(sut.faktine_grazinimo_data_laikas, '0000-00-00') like '0000%'
					AND DATEDIFF(NOW(), sut.planuojama_grazinimo_data_laikas) >=1
				)
				AND sut.sutarties_data >= IFNULL(?nuo, sut.sutarties_data)
				AND sut.sutarties_data <= IFNULL(?iki, sut.sutarties_data)";

		var drc =
			Sql.Query(query, args => {
				args.Add("?nuo", dateFrom);
				args.Add("?iki", dateTo);
			});

		var result = 
			Sql.MapAll<LateContractsReport.Sutartis>(drc, (dre, t) => {
				t.Nr = dre.From<int>("nr");
				t.SutartiesData = dre.From<DateTime>("sutarties_data");
				t.Klientas = dre.From<string>("klientas");
				t.PlanuojamaGrData = dre.From<DateTime>("planuojama_grazinimo_data_laikas");
				t.FaktineGrData = dre.From<string>("grazinimo_data");
			});

		return result;
	}
}
