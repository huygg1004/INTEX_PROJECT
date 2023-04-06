using Microsoft.AspNetCore.Mvc;
using Intex_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Components
{
    public class LocationViewComponent : ViewComponent
    {
        private GamousContext context;
        public LocationViewComponent(GamousContext ctx)
        {
            context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            //return View(context.RecipeClasses
            //    .Select(x => x.RecipeClassDescription)
            //    .Distinct()
            //    .OrderBy(x => x)
            //    .ToList());

            ViewBag.SelectedLocation = RouteData?.Values["Square"];
            return View(context.LocationMeasurements
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
