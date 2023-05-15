namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;


/// <summary>
/// Controller for working with 'Sutartis' entity. Implementation of F2 version.
/// </summary>
public class SutartisF2Controller : Controller
{
	/// <summary>
	/// This is invoked when either 'Index' action is requested or no action is provided.
	/// </summary>
	/// <returns>Entity list view.</returns>
	[HttpGet]
	public ActionResult Index()
	{
		return View(SutartisF2Repo.ListSutartis());
	}

	/// <summary>
	/// This is invoked when creation form is first opened in a browser.
	/// </summary>
	/// <returns>Entity creation form.</returns>
	[HttpGet]
	public ActionResult Create()
	{
		var sutCE = new SutartisCE();

		sutCE.Sutartis.SutartiesData = DateTime.Now;
		sutCE.Sutartis.NuomosDataLaikas = DateTime.Now;
		sutCE.Sutartis.PlanuojamaGrDataLaikas = DateTime.Now;
		
		PopulateLists(sutCE);

		return View(sutCE);
	}


	/// <summary>
	/// This is invoked when buttons are pressed in the creation form.
	/// </summary>
	/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
	/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
	/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
	/// <param name="sutCE">Entity view model filled with latest data.</param>
	/// <returns>Returns creation from view or redirets back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Create(int? save, int? add, int? remove, SutartisCE sutCE)
	{
		//addition of new 'UzsakytosPaslaugos' record was requested?
		if( add != null )
		{
			//add entry for the new record
			var up =
				new SutartisCE.UzsakytaPaslaugaM {
					InListId =
						sutCE.UzsakytosPaslaugos.Count > 0 ?
						sutCE.UzsakytosPaslaugos.Max(it => it.InListId) + 1 :
						0,

					Paslauga = null,
					Kiekis = 0,
					Kaina = 0
				};
			sutCE.UzsakytosPaslaugos.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		//removal of existing 'UzsakytosPaslaugos' record was requested?
		if( remove != null )
		{
			//filter out 'UzsakytosPaslaugos' record having in-list-id the same as the given one
			sutCE.UzsakytosPaslaugos =
				sutCE
					.UzsakytosPaslaugos
					.Where(it => it.InListId != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		//save of the form data was requested?
		if( save != null )
		{
			//check for attemps to create duplicate 'UzsakytaPaslauga'records
			for( var i = 0; i < sutCE.UzsakytosPaslaugos.Count-1; i ++ )
			{
				var refUp = sutCE.UzsakytosPaslaugos[i];

				for( var j = i+1; j < sutCE.UzsakytosPaslaugos.Count; j++ )
				{
					var testUp = sutCE.UzsakytosPaslaugos[j];
					
					if( testUp.Paslauga == refUp.Paslauga )
						ModelState.AddModelError($"UzsakytosPaslaugos[{j}].Paslauga", "Duplicate of another added service.");
				}
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				//create new 'Sutartis'
				sutCE.Sutartis.Nr = SutartisF2Repo.InsertSutartis(sutCE);

				//create new 'UzsakytosPaslaugos' records
				foreach( var upVm in sutCE.UzsakytosPaslaugos )
					SutartisF2Repo.InsertUzsakytaPaslauga(sutCE.Sutartis.Nr, upVm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			else
			{
				PopulateLists(sutCE);
				return View(sutCE);
			}
		}

		//should not reach here
		throw new Exception("Should not reach here.");
	}

	/// <summary>
	/// This is invoked when editing form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to edit.</param>
	/// <returns>Editing form view.</returns>
	[HttpGet]
	public ActionResult Edit(int id)
	{
		var sutCE = SutartisF2Repo.FindSutartisCE(id);
		
		sutCE.UzsakytosPaslaugos = SutartisF2Repo.ListUzsakytaPaslauga(id);			
		PopulateLists(sutCE);

		return View(sutCE);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the editing form.
	/// </summary>
	/// <param name="id">ID of the entity being edited</param>
	/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
	/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
	/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
	/// <param name="sutCE">Entity view model filled with latest data.</param>
	/// <returns>Returns editing from view or redired back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Edit(int id, int? save, int? add, int? remove, SutartisCE sutCE)
	{
		//addition of new 'UzsakytosPaslaugos' record was requested?
		if( add != null )
		{
			//add entry for the new record
			var up =
				new SutartisCE.UzsakytaPaslaugaM {
					InListId =
						sutCE.UzsakytosPaslaugos.Count > 0 ?
						sutCE.UzsakytosPaslaugos.Max(it => it.InListId) + 1 :
						0,

					Paslauga = null,
					Kiekis = 0,
					Kaina = 0
				};
			sutCE.UzsakytosPaslaugos.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		//removal of existing 'UzsakytosPaslaugos' record was requested?
		if( remove != null )
		{
			//filter out 'UzsakytosPaslaugos' record having in-list-id the same as the given one
			sutCE.UzsakytosPaslaugos =
				sutCE
					.UzsakytosPaslaugos
					.Where(it => it.InListId != remove.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		//save of the form data was requested?
		if( save != null )
		{
			//check for attemps to create duplicate 'UzsakytaPaslauga'records
			for( var i = 0; i < sutCE.UzsakytosPaslaugos.Count-1; i ++ )
			{
				var refUp = sutCE.UzsakytosPaslaugos[i];

				for( var j = i+1; j < sutCE.UzsakytosPaslaugos.Count; j++ )
				{
					var testUp = sutCE.UzsakytosPaslaugos[j];
					
					if( testUp.Paslauga == refUp.Paslauga )
						ModelState.AddModelError($"UzsakytosPaslaugos[{j}].Paslauga", "Duplicate of another added service.");
				}
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				//update 'Sutartis'
				SutartisF2Repo.UpdateSutartis(sutCE);

				//delete all old 'UzsakytosPaslaugos' records
				SutartisF2Repo.DeleteUzsakytaPaslaugaForSutartis(sutCE.Sutartis.Nr);

				//create new 'UzsakytosPaslaugos' records
				foreach( var upVm in sutCE.UzsakytosPaslaugos )
					SutartisF2Repo.InsertUzsakytaPaslauga(sutCE.Sutartis.Nr, upVm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			else
			{
				PopulateLists(sutCE);
				return View(sutCE);
			}
		}

		//should not reach here
		throw new Exception("Should not reach here.");
	}

	/// <summary>
	/// This is invoked when deletion form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view.</returns>
	[HttpGet]
	public ActionResult Delete(int id)
	{
		var sutCE = SutartisF2Repo.FindSutartisCE(id);
		return View(sutCE);
	}

	/// <summary>
	/// This is invoked when deletion is confirmed in deletion form
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view on error, redirects to Index on success.</returns>
	[HttpPost]
	public ActionResult DeleteConfirm(int id)
	{
		//load 'Sutartis'
		var sutCE = SutartisF2Repo.FindSutartisCE(id);

		//'Sutartis' is in the state where deletion is permitted?
		if( sutCE.Sutartis.FkBusena == 1 || sutCE.Sutartis.FkBusena == 3 )
		{
			//delete the entity
			SutartisF2Repo.DeleteUzsakytaPaslaugaForSutartis(id);
			SutartisF2Repo.DeleteSutartis(id);

			//redired to list form
			return RedirectToAction("Index");
		}
		//'Sutartis' is in state where deletion is not permitted
		else
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;
			return View("Delete", sutCE);
		}
	}

	/// <summary>
	/// Populates select lists used to render drop down controls.
	/// </summary>
	/// <param name="sutCE">'Sutartis' view model to append to.</param>
	private void PopulateLists(SutartisCE sutCE)
	{
		//load entities for the select lists
		var automobiliai = AutomobilisRepo.ListAutomobilis();
		var busenos = SutartisF2Repo.ListSutartiesBusena();
		var darbuotojai = DarbuotojasRepo.ListDarbuotojas();
		var klientai = KlientasRepo.List();
		var aiksteles = AiksteleRepo.List();

		//build select lists
		sutCE.Lists.Automobiliai =
			automobiliai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = Convert.ToString(it.Id),
							Text = $"{it.ValstybinisNr} - {it.Marke} {it.Modelis}"
						};
				})
				.ToList();

		sutCE.Lists.Busenos =
			busenos
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = Convert.ToString(it.Id),
							Text = it.Name
						};
				})
				.ToList();

		sutCE.Lists.Klientai =
			klientai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							
							Text = $"{it.Vardas} {it.Pavarde}"
						};
				})
				.ToList();

		sutCE.Lists.Vietos =
			aiksteles
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();

		//build select list for 'UzsakytosPaslaugos'
		{
			//initialize the destination list
			sutCE.Lists.Paslaugos = new List<SelectListItem>();

			//load 'Paslaugos' to use for item groups
			var paslaugos = PaslaugaRepo.List();

			//create select list items from 'PaslauguKainos' related to each 'Paslaugos'
			foreach( var paslauga in paslaugos )
			{
				//create list item group for current 'Paslaugos' entity
				var itemGrp = new SelectListGroup() { Name = paslauga.Pavadinimas };

				//load related 'PaslauguKaina' entities
				var kainos = PaslaugosKainaRepo.LoadForPaslauga(paslauga.Id);

				//build list items for the group
				foreach( var kaina in kainos )
				{
					var sle =
						new SelectListItem {
							Value =
								//we use JSON here to make serialization/deserializaton of composite key more convenient
								JsonConvert.SerializeObject(new {
									FkPaslauga = paslauga.Id,
									GaliojaNuo = kaina.GaliojaNuo
								}),
							Text = $"{paslauga.Pavadinimas} {kaina.Kaina} EUR ({kaina.GaliojaNuo.ToString("yyyy-MM-dd")})",
							Group = itemGrp
						};
					sutCE.Lists.Paslaugos.Add(sle);
				}
			}
		}
	}
}
