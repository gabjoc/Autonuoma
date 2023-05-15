namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.Preke;


/// <summary>
/// Controller for working with 'Preke' entity.
/// </summary>
public class PrekeController : Controller
{
	/// <summary>
	/// This is invoked when either 'Index' action is requested or no action is provided.
	/// </summary>
	/// <returns>Entity list view.</returns>
	[HttpGet]
	//naudojam list nes tai reikia matyti
	public ActionResult Index()
	{
		return View(PrekeRepo.ListPreke());
	}
	
	/// <summary>
	/// This is invoked when creation form is first opened in browser.
	/// </summary>
	/// <returns>Creation form view.</returns>
	[HttpGet]
	//naudoja CE nes CE yra pritaikytas create ir edit mode, t.y. naudojami skaiciukai
	//su populate selections konvertuojame skaiciuka i pavadinima
	public ActionResult Create()
	{
		var preke = new PrekeCE();
		PopulateSelections(preke);
		return View(preke);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the creation form.
	/// </summary>
	/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
	/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
	/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains id of the item to remove.</param>
	/// <param name="model">Entity model filled with latest data.</param>
	/// <returns>Returns creation from view or redirects back to Index if save is successful.</returns>
	[HttpPost]
	public ActionResult Create(int? save, int? add, int? remove, PrekeCE model)
	{
		//addition of new 'Likutis' record was requested?
		if( add != null )
		{
			//add entry for the new record
			var pl =
				new PrekesLikutis {
					Likutis = {
						InListId = model.Likuciai.Count,
						Kiekis = 1,
						FkParduotuve = 0
					}
				};
			model.Likuciai.Add(pl);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(model);
			return View(model);
		}
		
		//removal of existing 'Likutis' record was requested?
		if( remove != null )
		{
			//filter out 'Likutis' record having in-list-id the same as the given one
			model.Likuciai =
				model
					.Likuciai
					.Where(it => it.Likutis.InListId != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(model);
			return View(model);
		}

		//save of the form data was requested?
		if( save != null )
		{	
			//check for duplicate 'FkParduotuve' fields in 'Likutis' list
			for( var index = 0; index < model.Likuciai.Count; index++ )
			{
				//find all entries that are not current one and have matching 'FkParduotuve' field
				var matches = 
					model.Likuciai.Where((other, otherIndex) => {
						return 
							other.Likutis.FkParduotuve == model.Likuciai[index].Likutis.FkParduotuve &&
							otherIndex != index;
					})
					.ToList();

				//entries found? mark current field as invalid by adding error message to model state
				if( matches.Count > 0 )
					ModelState.AddModelError($"Likuciai[{index}].Likutis.FkParduotuve", "Field value already exists");
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				//insert 'Preke'
				int prekesId = PrekeRepo.Insert(model);

				//insert related 'Likutis'
				foreach( var likutisInForm in model.Likuciai )
				{					
					likutisInForm.Likutis.FkPreke = prekesId;
					PrekesLikutisRepo.Insert(likutisInForm);
				}

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			{
				PopulateSelections(model);
				return View(model);
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
		var preke = PrekeRepo.FindCE(id);
		PopulateSelections(preke);
		// sito reikia atskiro (populateLikuciai), nes mes norime kad mums likucius is duombazes uzkrautu tik tuomet, kai mes pirma kart atidarome forma
		// (nes kitaip istrinus is formos likuti bet neissaugojus prekes visos tai ta likuti sugrazintu nes vel uzkrove is duombazes)
		PopulateLikuciai(preke);
		return View(preke);
	}
	/// <summary>
	/// This is invoked when buttons are pressed in the editing form.
	/// </summary>
	/// <param name="id">ID of the entity being edited</param>		
	/// <param name="prek">Entity model filled with latest data.</param>
	/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Edit(int? save, int? add, int? remove, PrekeCE model)
	{
		//addition of new 'Likutis' record was requested?
		if( add != null )
		{
			//add entry for the new record
			var pl =
				new PrekesLikutis {
					Likutis = {
						InListId = model.Likuciai.Count,
						FkPreke = model.Preke.PrekesKodas,
						Kiekis = 1,
						FkParduotuve = 0
					}
				};
			model.Likuciai.Add(pl);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(model);
			return View(model);
		}
		
		//removal of existing 'Likutis' record was requested?
		if( remove != null )
		{
			//filter out 'Likutis' record having in-list-id the same as the given one
			model.Likuciai =
				model
					.Likuciai
					.Where(it => it.Likutis.InListId != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateSelections(model);
			return View(model);
		}

		//save of the form data was requested?
		if( save != null )
		{	
			//check for duplicate 'FkParduotuve' fields in 'Likutis' list
			for( var index = 0; index < model.Likuciai.Count; index++ )
			{
				//find all entries that are not current one and have matching 'FkParduotuve' field
				var matches = 
					model.Likuciai.Where((other, otherIndex) => {
						return 
							other.Likutis.FkParduotuve == model.Likuciai[index].Likutis.FkParduotuve &&
							otherIndex != index;
					})
					.ToList();

				//entries found? mark current field as invalid by adding error message to model state
				if( matches.Count > 0 )
					ModelState.AddModelError($"Likuciai[{index}].Likutis.FkParduotuve", "Field value already exists");
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				//update 'Preke'
				PrekeRepo.Update(model);

				//update related 'PrekesLikutis'
				{
					//load related 'PrekesLikutis' from DB to have most up to date data
					var likuciaiInDb = PrekesLikutisRepo.LoadForPreke(model.Preke.PrekesKodas);

					//delete 'PrekesLikutis' that are not present in form (if deletable)
					foreach( var likutisInDb in likuciaiInDb )
					{
						var delete = model.Likuciai.Find(it => it.Likutis.Id == likutisInDb.Likutis.Id) == null;
						if( delete )
							PrekesLikutisRepo.Delete((int)likutisInDb.Likutis.Id);
					}

					//insert 'PrekesLikutis' that are not present in DB
					foreach( var likutisInForm in model.Likuciai )
					{
						var insert = likuciaiInDb.Find(it => it.Likutis.Id == likutisInForm.Likutis.Id) == null;
						if( insert )
							PrekesLikutisRepo.Insert(likutisInForm);
					}

					//update non-readonly 'PrekesLikutis' in DB (deleted entities will simply result in no-action as far as SQL is concerned)
					foreach( var likutisInDb in likuciaiInDb )
					{
						var update = model.Likuciai.Find(it => it.Likutis.Id == likutisInDb.Likutis.Id);
						if( update != null )
							PrekesLikutisRepo.Update(update);
					}						
				}

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			{
				PopulateSelections(model);
				return View(model);
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
		var preke = PrekeRepo.FindL(id);
		return View(preke);
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
			PrekeRepo.Delete(id);

			//deletion success, redired to list form
			return RedirectToAction("Index");
		}
		//entity in use, deletion not permitted
		catch( MySql.Data.MySqlClient.MySqlException )
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;

			var preke = PrekeRepo.FindL(id);
			return View("Delete", preke);
		}
	}

	/// <summary>
	/// Populates select lists used to render drop down controls.
	/// </summary>
	/// <param name="prek">'PrekeCE' view model to append to.</param>
	//skirtas tam kad parduotuvesid galetume matyti kaip jos pavadinima
	public void PopulateSelections(PrekeCE prek)
	{
		//load entities for the select lists
		var kategorijos = KategorijaRepo.ListKategorija();
		var gamintojai = GamintojasRepo.ListGamintojas();
		var parduotuves = ParduotuveRepo.ListParduotuve();

		//build select lists
		prek.Lists.Kategorijos = 
			kategorijos.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Id), 
						Text = it.Pavadinimas
					};
			})
			.ToList();

		//build select lists
		prek.Lists.Gamintojai = 
			gamintojai.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Id), 
						Text = it.Pavadinimas + " - " + it.Salis
					};
			})
			.ToList();

		//build select lists
		prek.Lists.Parduotuves = 
			parduotuves.Select(it => {
				return
					new SelectListItem() { 
						Value = Convert.ToString(it.Parduotuvesid), 
						Text = it.Pavadinimas + " - " + it.Adresas
					};
			})
			.ToList();
	}

	public void PopulateLikuciai(PrekeCE prek)
	{
		prek.Likuciai = PrekesLikutisRepo.LoadForPreke(prek.Preke.PrekesKodas);
	}
}
