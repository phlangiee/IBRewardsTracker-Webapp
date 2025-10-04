using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Dapper;
using AffinityCard_Namespace.Models;
using AffinityCard_Namespace.Data;
using AffinityProgram_Namespace.Models;
using AffinityProgram_Namespace.Data;
using Person_Namespace.Models;
using Person_Namespace.Data;
using Type_Namespace.Models;
using Type_Namespace.Data;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace travelapp.Pages;

public class HomeModel : PageModel
{
    private readonly ILogger<HomeModel> _logger;

    private readonly AffinityCardData _AffinityCardData;
    private readonly AffinityProgramData _AffinityProgramData;
    private readonly PersonData _PersonData;
    private readonly TypeData _TypeData;

    public HomeModel(ILogger<HomeModel> logger, AffinityCardData AffinityCardData, AffinityProgramData AffinityProgramData, PersonData PersonData, TypeData TypeData)
    {
        _logger = logger;
        _AffinityCardData = AffinityCardData;
        _AffinityProgramData = AffinityProgramData;
        _PersonData = PersonData;
        _TypeData = TypeData;
    }

    public IEnumerable<AffinityCard> Cards { get; set; }
    public IEnumerable<AffinityProgram> Programs { get; set; }
    public IEnumerable<Person> People { get; set; }
    public IEnumerable<RewardType> Types { get; set; }

    public IActionResult OnGet() 
    {
        int? IsRamy = HttpContext.Session.GetInt32("IsRamy");
        if (IsRamy == null || IsRamy == 0)
        {
            return Redirect("/");
        }

        Cards = _AffinityCardData.GetAllCards();
        Programs = _AffinityProgramData.GetAllPrograms();
        People = _PersonData.GetAllPeople();
        Types = _TypeData.GetAllTypes();

        return Page();
    }

    //UPDATE TABLE
    public async Task<IActionResult> OnPostUpdatePersonTable([FromBody] List<Person> data)
    {
        await _PersonData.UpdatePersonTable(data);
        People = _PersonData.GetAllPeople();
        return new JsonResult(data);
    }

    public async Task<IActionResult> OnPostUpdateCardTable([FromBody] List<AffinityCard> data)
    {
        await _AffinityCardData.UpdateCardTable(data);
        Cards = _AffinityCardData.GetAllCards();
        return new JsonResult(data);
    }

    public async Task<IActionResult> OnPostUpdateProgramTable([FromBody] List<AffinityProgram> data)
    {
        await _AffinityProgramData.UpdateProgramTable(data);
        Programs = _AffinityProgramData.GetAllPrograms();
        return new JsonResult(data);
    }

    public async Task<IActionResult> OnPostUpdateTypeTable([FromBody] List<RewardType> data)
    {
        await _TypeData.UpdateTypeTable(data);
        Types = _TypeData.GetAllTypes();
        return new JsonResult(data);
    }

    //GET ID
    public async Task<IActionResult> OnGetRefreshCardDataAsync()
    {
        var id = _AffinityCardData.ValidId(1);
        return new JsonResult(new { id });
    }
    public async Task<IActionResult> OnGetRefreshProgramDataAsync()
    {
        var id = _AffinityProgramData.ValidId(1);
        return new JsonResult(new { id });
    }
    public async Task<IActionResult> OnGetRefreshPersonDataAsync()
    {
        var id = _PersonData.ValidId(1);
        return new JsonResult(new { id });
    }
    public async Task<IActionResult> OnGetRefreshTypeDataAsync()
    {
        var id = _TypeData.ValidId(1);
        return new JsonResult(new { id });
    }

    //SEARCH
    public async Task<IActionResult> OnGetSearchCardAsync(string query)
    {
        List<AffinityCard> list = _AffinityCardData.Search(query);
        return new JsonResult(list);
    }

    public async Task<IActionResult> OnGetSearchProgramAsync(string query)
    {
        List<AffinityProgram> list = _AffinityProgramData.Search(query);
        return new JsonResult(list);
    }

    public async Task<IActionResult> OnGetSearchPeopleAsync(string query)
    {
        List<Person> list = _PersonData.Search(query);
        return new JsonResult(list);
    }

    public async Task<IActionResult> OnGetSearchTypeAsync(string query)
    {
        List<RewardType> list = _TypeData.Search(query);
        return new JsonResult(list);
    }

    //EDIT POINTS
   public async Task<IActionResult> OnPostEditPointsAsync([FromBody] IdPoints dto)
    { 
        _AffinityCardData.EditPoints(dto.Id, dto.Points);
        return new JsonResult(new { success = true, message = "Points updated successfully!" });
    }

    //DELETE
   public async Task<IActionResult> OnPostDeleteCardAsync([FromBody] AffinityCard data)
    {
        _AffinityCardData.DeleteCard(data.Id);
        return new JsonResult(new { success = true, message = "Card deleted successfully." });
    }
    public async Task<IActionResult> OnPostDeleteProgramAsync([FromBody] AffinityProgram data)
    {
        _AffinityProgramData.DeleteProgram(data.Id);
        return new JsonResult(new { success = true, message = "Program deleted successfully." });
    }
    public async Task<IActionResult> OnPostDeletePersonAsync([FromBody] Person data)
    {
        _PersonData.DeletePerson(data.Id);
        return new JsonResult(new { success = true, message = "Person deleted successfully." });
    }
    public async Task<IActionResult> OnPostDeleteTypeAsync([FromBody] RewardType data)
    {
        _TypeData.DeleteType(data.Id);
        return new JsonResult(new { success = true, message = "Type deleted successfully." });
    }

    //ADD
    public IActionResult OnPostAddCard(int Id, int AffinityProgramID, int PersonID, string RewardCompany, int Points, DateTime DateOpen, DateTime? DateClose, int? AnnualFee, int? CreditLine, string Notes)
    {
        if (DateClose != null)
        {
            Points = 0;
        }

        var newCard = new AffinityCard
        {
            Id = Id,
            AffinityProgramID = AffinityProgramID,
            PersonID = PersonID,
            RewardCompany = RewardCompany,
            Points = Points,
            DateOpen = DateOpen.ToString("yyyy-MM-dd"),
            DateClose = DateClose?.ToString("yyyy-MM-dd"),
            AnnualFee = AnnualFee,
            CreditLine = CreditLine,
            Notes = Notes
        };

        _AffinityCardData.AddCard(newCard);
        return RedirectToPage("/Home");
    }
    public IActionResult OnPostAddProgram(int Id, int TypeID, string ProgramCompany, string AffinityNum, string Level)
    {

        var newProgram = new AffinityProgram
        {
            Id = Id,
            TypeID = TypeID,
            ProgramCompany = ProgramCompany,
            AffinityNum = AffinityNum,
            Level = Level
        };

        _AffinityProgramData.AddProgram(newProgram);
        return RedirectToPage("/Home");
    }
    public IActionResult OnPostAddPerson(int Id, string Name)
    {

        var newPerson = new Person
        {
            Id = Id,
            Name = Name
        };

        _PersonData.AddPerson(newPerson);
        return RedirectToPage("/Home");
    }
    public IActionResult OnPostAddType(int Id, string Description)
    {

        var newType = new RewardType
        {
            Id = Id,
            Description = Description
        };

        _TypeData.AddType(newType);
        return RedirectToPage("/Home");
    }

    //EDIT
    public IActionResult OnPostEditCard(int Id, int AffinityProgramID, int PersonID, string RewardCompany, int Points, DateTime DateOpen, DateTime? DateClose, int? AnnualFee, int? CreditLine, string Notes)
    {
        if (DateClose != null)
        {
            Points = 0;
        }

        var newCard = new AffinityCard
        {
            Id = Id,
            AffinityProgramID = AffinityProgramID,
            PersonID = PersonID,
            RewardCompany = RewardCompany,
            Points = Points,
            DateOpen = DateOpen.ToString("yyyy-MM-dd"),
            DateClose = DateClose?.ToString("yyyy-MM-dd") ?? null,
            AnnualFee = AnnualFee,
            CreditLine = CreditLine,
            Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes
        };

        _AffinityCardData.EditCard(newCard);
        return RedirectToPage("/Home");
    }
    public IActionResult OnPostEditProgram(int Id, int TypeID, string ProgramCompany, string AffinityNum, string Level)
    {

        var newProgram = new AffinityProgram
        {
            Id = Id,
            TypeID = TypeID,
            ProgramCompany = ProgramCompany,
            AffinityNum =AffinityNum,
            Level = Level
        };

        _AffinityProgramData.EditProgram(newProgram);
        return RedirectToPage("/Home");
    }
    public IActionResult OnPostEditPerson(int Id, string Name)
    {

        var newPerson = new Person
        {
            Id = Id,
            Name = Name
        };

        _PersonData.EditPerson(newPerson);
        return RedirectToPage("/Home");
    }
    public IActionResult OnPostEditType(int Id, string Description)
    {

        var newType = new RewardType
        {
            Id = Id,
            Description = Description
        };

        _TypeData.EditType(newType);
        return RedirectToPage("/Home");
    }

    public void OnPost()
    {

    }
    
}
