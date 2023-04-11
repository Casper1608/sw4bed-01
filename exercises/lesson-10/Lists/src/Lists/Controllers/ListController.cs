using System.Net;
using Microsoft.AspNetCore.Mvc;
using Lists.Services;
using Lists.Models;

namespace Lists.Controllers;

[ApiController]
[Route("[controller]")]
public class ListController : ControllerBase
{
    private readonly ILogger<ListController> _logger;
    private readonly ListService<string> _list;

    public ListController(ILogger<ListController> logger, ListService<string> list)
    {
        _logger = logger;
        _list = list;
    }

    [HttpGet]
    public ActionResult<List<ListItem<string>>> Get(int? index)
    {
        
        var items = _list.GetItems();
        if (items == null)
        {
            _logger.LogError("GetItemsFailed");
            return StatusCode(500);
        }
        if (index == null)
        {
            _logger.LogInformation("Getting all items");
            return items;
        }

        if (index >= items.Count || index < 0)
        {
            _logger.LogInformation("Bad request for get");
            return BadRequest();
        }
        _logger.LogInformation("Returning specific item");
        return new List<ListItem<string>>() { items[(Index)index] };
        
    }

    [HttpPost]
    public ActionResult<List<ListItem<string>>> Post(ListItem<string> item)
    {
        _logger.LogInformation("Adding new item with name {item}", item.Item);
        return _list.AddItemToList(item);
    }

    [Route("{index}")]
    [HttpDelete]
    public ActionResult<List<ListItem<string>>> DeleteByIndex(int index)
    {
        _logger.LogInformation("Deleting item with index {index}", index);
        return _list.RemoveItem(index);
    }

}
