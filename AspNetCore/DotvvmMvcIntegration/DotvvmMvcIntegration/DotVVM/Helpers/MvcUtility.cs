using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace DotvvmMvcIntegration.DotVVM.Helpers
{
    public class MvcUtility
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IModelMetadataProvider modelMetadataProvider;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IRazorViewEngine viewEngine;

        public MvcUtility(IHttpContextAccessor httpContextAccessor, IModelMetadataProvider modelMetadataProvider, ITempDataProvider tempDataProvider, IRazorViewEngine viewEngine)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.modelMetadataProvider = modelMetadataProvider;
            this.tempDataProvider = tempDataProvider;
            this.viewEngine = viewEngine;
        }

        public async Task RenderAction(string controllerName, string actionName, RouteValueDictionary parameters, object model, TextWriter writer)
        {
            // Build the route data, pointing to the dummy controller
            var routeData = new RouteData();
            routeData.Values.Add("controller", controllerName);
            routeData.Values.Add("action", actionName);
            foreach (var pair in parameters)
            {
                routeData.Values[pair.Key] = pair.Value;
            }

            // Create the controller context
            var context = httpContextAccessor.HttpContext;
            
            var actionContext = new ActionContext(context, routeData, new ControllerActionDescriptor()
            {
                ControllerName = controllerName,
                ActionName = actionName
            });

            // Find the partial view
            var view = FindPartialView(actionContext, actionName);

            // create the view context and pass in the model
            var viewContext = new ViewContext(actionContext, view, new ViewDataDictionary(modelMetadataProvider, new ModelStateDictionary()) { Model = model }, new TempDataDictionary(context, tempDataProvider), writer, new HtmlHelperOptions());

            // finally, render the view
            await view.RenderAsync(viewContext);
        }

        private IView FindPartialView(ActionContext actionContext, string partialViewName)
        {
            // try to find the partial view
            var result = viewEngine.FindView(actionContext, partialViewName, false);
            if (result.View != null)
            {
                return result.View;
            }

            // wasn't found - construct error message
            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations)
            {
                locationsText.AppendLine();
                locationsText.Append(location);
            }
            throw new InvalidOperationException(String.Format("Partial view {0} not found. Locations Searched: {1}", partialViewName, locationsText));
        }
    }
}