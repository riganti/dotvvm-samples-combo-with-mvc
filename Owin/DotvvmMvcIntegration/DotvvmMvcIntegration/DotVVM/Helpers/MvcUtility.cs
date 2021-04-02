using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotvvmMvcIntegration.DotVVM.Helpers
{
    public static class MvcUtility
    {
        public static void RenderView(string controllerName, string viewName, RouteValueDictionary parameters, object model, TextWriter writer)
        {
            // Get the HttpContext
            var httpContextBase = new HttpContextWrapper(HttpContext.Current);
            
            // Build the route data, pointing to the dummy controller
            var routeData = new RouteData();
            routeData.Values.Add("controller", controllerName);
            foreach (var pair in parameters)
            {
                routeData.Values[pair.Key] = pair.Value;
            }

            // Create the controller context
            var requestContext = new RequestContext(httpContextBase, routeData);
            var controller = new DefaultControllerFactory().CreateController(requestContext, controllerName);
            var controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);
            
            // Find the partial view
            var view = FindPartialView(controllerContext, viewName);
            
            // create the view context and pass in the model
            var viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary() { Model = model }, new TempDataDictionary(), writer);
            
            // finally, render the view
            view.Render(viewContext, writer);
        }

        private static IView FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            // try to find the partial view
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controllerContext, partialViewName);
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
            throw new InvalidOperationException(String.Format("Partial view {0} not found. Locations Searched: {1}",  partialViewName, locationsText));
        }
    }
}