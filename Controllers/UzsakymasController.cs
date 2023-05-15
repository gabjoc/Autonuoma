namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.Uzsakymas;


/// <summary>
/// Controller for working with 'Uzsakymas' entity.
/// </summary>
public class UzsakymasController : Controller
{
	/// <summary>
	/// This is invoked when either 'Index' action is requested or no action is provided.
	/// </summary>
	/// <returns>Entity list view.</returns>
	[HttpGet]
	public ActionResult Index()
	{
		return View(UzsakymasRepo.ListUzsakymas());
	}

	/// <summary>
	/// This is invoked when creation form is first opened in browser.
	/// </summary>
	/// <returns>Creation form view.</returns>
	[HttpGet]
	public ActionResult Create()
	{
		var uzsak = new UzsakymasCE();
		PopulateSelections(uzsak);

		return View(uzsak);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the creation form.
	/// </summary>
	/// <param name="autoCE">Entity model filled with latest data.</param>
	/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Create(int? save, int? add, int? remove, UzsakymasCE uzsakCE)
	{
		//addition of new 'PaslauguKainos' record was requested?
		if( add != null )
		{
			//add entry for the new record
			var up =
				new UzsakymoPreke {
					Uzsakymopreke = {
						InListId = uzsakCE.Uzsakymoprekes.Count,
						Kiekis = 1,
						FkPreke = 0
					}
				};
			uzsakCE.Uzsakymoprekes.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(uzsakCE);
			return View(uzsakCE);
		}

		//removal of existing 'Likutis' record was requested?
		if( remove != null )
		{
			//filter out 'Likutis' record having in-list-id the same as the given one
			uzsakCE.Uzsakymoprekes =
				uzsakCE
					.Uzsakymoprekes
					.Where(it => it.Uzsakymopreke.InListId != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(uzsakCE);
			return View(uzsakCE);
		}

		//save of the form data was requested?
		if( save != null )
		{	
			//check for duplicate 'FkParduotuve' fields in 'Likutis' list
			for( var index = 0; index < uzsakCE.Uzsakymoprekes.Count; index++ )
			{
				//find all entries that are not current one and have matching 'FkParduotuve' field
				var matches = 
					uzsakCE.Uzsakymoprekes.Where((other, otherIndex) => {
						return 
							other.Uzsakymopreke.FkPreke == uzsakCE.Uzsakymoprekes[index].Uzsakymopreke.FkPreke &&
							otherIndex != index;
					})
					.ToList();

				//entries found? mark current field as invalid by adding error message to model state
				if( matches.Count > 0 )
					ModelState.AddModelError($"Uzsakymoprekes[{index}].Uzsakymopreke.FkPreke", "Field value already exists");
				
				var kiekis = PrekesLikutisRepo.SumLikutisForPreke(uzsakCE.Uzsakymoprekes[index].Uzsakymopreke.FkPreke);

				if (kiekis < uzsakCE.Uzsakymoprekes[index].Uzsakymopreke.Kiekis)
				{
					ModelState.AddModelError($"Uzsakymoprekes[{index}].Uzsakymopreke.Kiekis", "Kiekis exceeds store likutis");
				}
			
			}
		//form field validation passed?
		if( ModelState.IsValid )
		{
			//insert 'Uzsakymas'
			int uzsakymoId = UzsakymasRepo.InsertUzsakymas(uzsakCE);

			//insert related 'Likutis'
				foreach( var UzsakymoprekeInForm in uzsakCE.Uzsakymoprekes )
				{					
					UzsakymoprekeInForm.Uzsakymopreke.FkUzsakymas = uzsakymoId;
					UzsakymoPrekeRepo.Insert(UzsakymoprekeInForm);
				}

			//save success, go back to the entity list
			return RedirectToAction("Index");
		}
		
		//form field validation failed, go back to the form
		
		{
			PopulateSelections(uzsakCE);
			return View(uzsakCE);
		}
		}
		throw new Exception("Klaida");
	}

	/// <summary>
	/// This is invoked when editing form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to edit.</param>
	/// <returns>Editing form view.</returns>
	[HttpGet]
	public ActionResult Edit(int id)
	{
		var uzsakCE = UzsakymasRepo.FindUzsakymasCE(id);
		PopulateSelections(uzsakCE);
		PopulateUzsakymoPrekes(uzsakCE);
		return View(uzsakCE);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the editing form.
	/// </summary>
	/// <param name="id">ID of the entity being edited</param>		
	/// <param name="uzsakCE">Entity model filled with latest data.</param>
	/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Edit(int? save, int? add, int? remove, UzsakymasCE uzsakCE)
	{
		//addition of new 'Likutis' record was requested?
		if( add != null )
		{
			//add entry for the new record
			var up =
				new UzsakymoPreke {
					Uzsakymopreke = {
						InListId = uzsakCE.Uzsakymoprekes.Count,
						FkUzsakymas = uzsakCE.Uzsakymas.Uzsakymas,
						Kiekis = 1,
						FkPreke = 0
					}
				};
			uzsakCE.Uzsakymoprekes.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(uzsakCE);
			return View(uzsakCE);
		}
		
		//removal of existing 'Likutis' record was requested?
		if( remove != null )
		{
			//filter out 'Likutis' record having in-list-id the same as the given one
			uzsakCE.Uzsakymoprekes =
				uzsakCE
					.Uzsakymoprekes
					.Where(it => it.Uzsakymopreke.InListId != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(uzsakCE);
			return View(uzsakCE);
		}

		//save of the form data was requested?
		if( save != null )
		{	
			//check for duplicate 'FkParduotuve' fields in 'Likutis' list
			for( var index = 0; index < uzsakCE.Uzsakymoprekes.Count; index++ )
			{
				//find all entries that are not current one and have matching 'FkParduotuve' field
				var matches = 
					uzsakCE.Uzsakymoprekes.Where((other, otherIndex) => {
						return 
							other.Uzsakymopreke.FkPreke == uzsakCE.Uzsakymoprekes[index].Uzsakymopreke.FkPreke &&
							otherIndex != index;
					})
					.ToList();

				//entries found? mark current field as invalid by adding error message to model state
				if( matches.Count > 0 )
					ModelState.AddModelError($"Uzsakymoprekes[{index}].Uzsakymopreke.FkPreke", "Field value already exists");

				var kiekis = PrekesLikutisRepo.SumLikutisForPreke(uzsakCE.Uzsakymoprekes[index].Uzsakymopreke.FkPreke);

				if (kiekis < uzsakCE.Uzsakymoprekes[index].Uzsakymopreke.Kiekis)
				{
					ModelState.AddModelError($"Uzsakymoprekes[{index}].Uzsakymopreke.Kiekis", "Kiekis exceeds store likutis");
				}
			}

		//form field validation passed?
		if (ModelState.IsValid)
		{
			//update 'Uzsakymas'
			UzsakymasRepo.UpadateUzsakymas(uzsakCE);

			//update related 'PrekesLikutis'
				{
					//load related 'PrekesLikutis' from DB to have most up to date data
					var uzsakymoprekesInDb = UzsakymoPrekeRepo.LoadForUzsakymas(uzsakCE.Uzsakymas.Uzsakymas);

					//delete 'PrekesLikutis' that are not present in form (if deletable)
					foreach( var uzsakymoprekeInDb in uzsakymoprekesInDb )
					{
						var delete = uzsakCE.Uzsakymoprekes.Find(it =>it.Uzsakymopreke.FkPreke == uzsakymoprekeInDb.Uzsakymopreke.FkPreke) == null;
						if( delete )
							UzsakymoPrekeRepo.Delete(uzsakymoprekeInDb.Uzsakymopreke.FkPreke, (int)uzsakymoprekeInDb.Uzsakymopreke.FkUzsakymas);
					}

					//insert 'PrekesLikutis' that are not present in DB
					foreach( var UzsakymoprekeInForm in uzsakCE.Uzsakymoprekes )
					{
						var insert = uzsakymoprekesInDb.Find(it => it.Uzsakymopreke.FkPreke == UzsakymoprekeInForm.Uzsakymopreke.FkPreke) == null;
						if( insert )
							UzsakymoPrekeRepo.Insert(UzsakymoprekeInForm);
					}

					//update non-readonly 'PrekesLikutis' in DB (deleted entities will simply result in no-action as far as SQL is concerned)
					foreach( var uzsakymoprekeInDb in uzsakymoprekesInDb )
					{
						var update = uzsakCE.Uzsakymoprekes.Find(it => it.Uzsakymopreke.FkPreke == uzsakymoprekeInDb.Uzsakymopreke.FkPreke);
						if( update != null )
							UzsakymoPrekeRepo.Update(update);
					}						
				}

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			{
				PopulateSelections(uzsakCE);
				return View(uzsakCE);
			}			
		}

			throw new Exception("Klaida");
	}

	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view.</returns>
	[HttpGet]
	public ActionResult Delete(int id)
	{
		var uzsakCE = UzsakymasRepo.FindUzsakymasL(id);
		return View(uzsakCE);
	}

	/// <summary>
	/// This is invoked when deletion is confirmed in deletion form
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view on error, redirects to Index on success.</returns>
	[HttpPost]
	public ActionResult DeleteConfirm(int id)
	{
		//try deleting, this will fail if foreign key constraint fails
		try 
		{
			UzsakymasRepo.DeleteUzsakymas(id);

			//deletion success, redired to list form
			return RedirectToAction("Index");
		}
		//entity in use, deletion not permitted
		catch( MySql.Data.MySqlClient.MySqlException )
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;

			var uzsakCE = UzsakymasRepo.FindUzsakymasL(id);

			return View("Delete", uzsakCE);
		}
	}

	/// <summary>
	/// Populates select lists used to render drop down controls.
	/// </summary>
	/// <param name="autoCE">'Automobilis' view model to append to.</param>
	public void PopulateSelections(UzsakymasCE uzsakCE)
	{
		//load entities for the select lists
		var pristatymai = UzsakymasRepo.ListPristatymas();
		var busenos = UzsakymasRepo.ListUzsakymoBusena();
		var klientai = KlientasRepo.List();
		var darbuotojai = DarbuotojasRepo.ListDarbuotojas();
		var prekes = PrekeRepo.ListPreke();

		//build select lists
		uzsakCE.Lists.Klientai = 
			klientai.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.PirkejasNr), 
						Text = it.Vardas + " " + it.Pavarde 
					};
			})
			.ToList();

		uzsakCE.Lists.Darbuotojai = 
			darbuotojai.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Tabelis), 
						Text = it.Vardas + " " + it.Pavarde
					};
			})
			.ToList();

			uzsakCE.Lists.Pristatymai = 
			pristatymai.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Id), 
						Text = it.Pavadinimas
					};
			})
			.ToList();

			uzsakCE.Lists.Busenos = 
			busenos.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Id), 
						Text = it.Pavadinimas
					};
			})
			.ToList();

			//build select lists
		     uzsakCE.Lists.Prekes = 
			prekes.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.PrekesKodas), 
						Text = it.PrekesKodas + " - " + it.Pavadinimas
					};
			})
			.ToList();
	}
	public void PopulateUzsakymoPrekes(UzsakymasCE uzsakCE)
	{
		uzsakCE.Uzsakymoprekes = UzsakymoPrekeRepo.LoadForUzsakymas(uzsakCE.Uzsakymas.Uzsakymas);
	}
}
