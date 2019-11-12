using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClaimHandlingApp.Models;
using ClaimHandlingApp.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ClaimHandlingApp.Controllers
{
  
    public class ClaimController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        private ServiceBusSender _serviceBusSender;
        public ClaimController(ICosmosDbService cosmosDbService, ServiceBusSender serviceBusSender)
        {
            _cosmosDbService = cosmosDbService;
            _serviceBusSender = serviceBusSender;
        }

        
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost(Name = "Create")]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(Claim), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Claim), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateAsync([Bind("Id,Year,Name,DamageCost,Type")] Claim claim)
        {
            if (claim != null)
            {
                if (claim.DamageCost > 100.000m)
                {
                    ModelState.AddModelError(nameof(Claim.DamageCost), "Damage Cost cannot exceed 100.000");
                }

                var currentYear = DateTime.Now.Year;
                if(claim.Year > currentYear || claim.Year < (currentYear-10))
                    ModelState.AddModelError(nameof(Claim.Year), "Year cannot be in the future and more than 10 years back");
            }

            if (ModelState.IsValid)
            {
                claim.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(claim);

                // Send this to the bus for the other services
                await _serviceBusSender.SendMessage(new ClaimAudit
                {
                    ClaimId = claim.Id,
                    TimeStamp = DateTime.Now.ToString(),
                    Operation = "httppost-create"
                });
                return RedirectToAction("Index");
            }

            return View(claim);
        }

        [HttpPost(Name = "Edit")]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(Claim), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditAsync([Bind("Id,Year,Name,DamageCost,Type")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateItemAsync(claim.Id, claim);
                // Send this to the bus for the other services
                await _serviceBusSender.SendMessage(new ClaimAudit
                {
                    ClaimId = claim.Id,
                    TimeStamp = DateTime.Now.ToString(),
                    Operation = "httppost-update"
                });
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Claim claim = await _cosmosDbService.GetItemAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Claim claim = await _cosmosDbService.GetItemAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [HttpPost(Name ="Delete")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await _cosmosDbService.DeleteItemAsync(id);
            // Send this to the bus for the other services
            await _serviceBusSender.SendMessage(new ClaimAudit
            {
                ClaimId = id,
                TimeStamp = DateTime.Now.ToString(),
                Operation = "httppost-delete"
            });

            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetItemAsync(id));
        }
    }
}