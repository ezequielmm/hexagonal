using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public interface IWebApiPresenter
    {
        IActionResult ActionResult { get; }
    }
}
