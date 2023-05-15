namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using System;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;


/// <summary>
/// Database operations related to 'Parduotuve' entity.
/// </summary>
public class ParduotuveRepo
{
	public static List<ParduotuveL> ListParduotuve()
	{
		var query = 
		$@"SELECT 
		p.parduotuves_id,
		p.pavadinimas,
		p.adresas,
		p.telefono_nr,
		p.el_pastas,
		m.pavadinimas AS miesto_pavadinimas
		FROM
		`parduotuves` p 
		LEFT JOIN `miestai` m ON m.id_MIESTAS  = p.fk_MIESTASid_MIESTAS
		ORDER BY p.parduotuves_id ASC";
		
		var drc = Sql.Query(query);

		var result = 
			Sql.MapAll<ParduotuveL>(drc, (dre, t) => {
				t.Parduotuvesid = dre.From<int>("parduotuves_id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
				t.Adresas = dre.From<string>("adresas");
				t.Telefonas = dre.From<string>("telefono_nr");
				t.Pastas = dre.From<string>("el_pastas");
				t.Miestas = dre.From<string>("miesto_pavadinimas");
			});

		return result;
	}

	public static ParduotuveL FindL(int id)
	{
		var query =
		$@"SELECT 
		 p.parduotuves_id,
		 p.pavadinimas,
		 p.adresas,
		 p.telefono_nr,
		 p.el_pastas,
		 m.pavadinimas AS miesto_pavadinimas
		FROM
		 `parduotuves` p 
		 LEFT JOIN `miestai` m ON m.id_MIESTAS  = p.fk_MIESTASid_MIESTAS
		WHERE parduotuves_id=?id
		ORDER BY p.parduotuves_id ASC";

		var drc = 
			Sql.Query(query, args => {
				args.Add("?id",  id);
			});

		var result = 
			 Sql.MapOne<ParduotuveL>(drc, (dre, t) => {

			 	t.Parduotuvesid = dre.From<int>("parduotuves_id");
				t.Pavadinimas = dre.From<string>("pavadinimas");
			 	t.Adresas = dre.From<string>("adresas");
			 	t.Telefonas = dre.From<string>("telefono_nr");
			 	t.Pastas = dre.From<string>("el_pastas");
			 	t.Miestas = dre.From<string>("miesto_pavadinimas");
		    });
			
	    return result;
	}

	public static ParduotuveCE FindCE(int id)
	{
		var query = $@"SELECT * FROM `parduotuves` WHERE parduotuves_id=?id";

		var drc =
			Sql.Query(query, args => {
				args.Add("?id", id);
			});
		
		var result = 
			Sql.MapOne<ParduotuveCE>(drc, (dre, t) => {

			 	t.Parduotuve.Parduotuvesid = dre.From<int>("parduotuves_id");
				t.Parduotuve.Pavadinimas = dre.From<string>("pavadinimas");
			 	t.Parduotuve.Adresas = dre.From<string>("adresas");
			 	t.Parduotuve.Telefonas = dre.From<string>("telefono_nr");
			 	t.Parduotuve.Pastas = dre.From<string>("el_pastas");
			 	t.Parduotuve.FkMiestas = dre.From<int>("fk_MIESTASid_MIESTAS");
			});
		return result;
	}
	
	public static void Insert(ParduotuveCE pard)
	{							
		var query = 
			$@"INSERT INTO `parduotuves`
			(
				`parduotuves_id`,
                `pavadinimas`,
                `adresas`,
                `telefono_nr`,
                `el_pastas`,
				`fk_MIESTASid_MIESTAS`
			)
			VALUES(
				?parduotuves_id,
				?pavadinimas,
				?adresas,
                ?telefonas,
                ?el_pastas,
				?FkMiestas
			)";

		Sql.Insert(query, args => {

			args.Add("?parduotuves_id", pard.Parduotuve.Parduotuvesid);
			args.Add("?pavadinimas", pard.Parduotuve.Pavadinimas);
			args.Add("?adresas", pard.Parduotuve.Adresas);
            args.Add("?telefonas", pard.Parduotuve.Telefonas);
            args.Add("?el_pastas", pard.Parduotuve.Pastas);
			args.Add("?FkMiestas", pard.Parduotuve.FkMiestas);
		});				
	}

	public static void Update(ParduotuveCE pard)
	{						
		var query = 
			$@"UPDATE `parduotuves`
			SET 
				`pavadinimas` = ?pavadinimas, 
				`adresas` = ?adresas,
                `telefono_nr` = ?telefonas,
                `el_pastas` = ?el_pastas,
				`fk_MIESTASid_MIESTAS` = ?miestas
			WHERE 
				parduotuves_id=?parduotuves_id";

		Sql.Update(query, args => {

			args.Add("?pavadinimas", pard.Parduotuve.Pavadinimas);
			args.Add("?adresas", pard.Parduotuve.Adresas);
            args.Add("?telefonas", pard.Parduotuve.Telefonas);
            args.Add("?el_pastas", pard.Parduotuve.Pastas);
			args.Add("?miestas", pard.Parduotuve.FkMiestas);
			args.Add("?parduotuves_id", pard.Parduotuve.Parduotuvesid);
		});				
	}

	public static void Delete(int id)
	{			
		var query = $@"DELETE FROM `parduotuves` WHERE parduotuves_id=?id";
		Sql.Delete(query, args => {
			args.Add("?id", id);
		});			
	}
}