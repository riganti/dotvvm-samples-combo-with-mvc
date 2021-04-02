using System.IO;
using System.Threading.Tasks;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Compilation.ControlTree;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using DotvvmMvcIntegration.DotVVM.Helpers;
using Microsoft.AspNetCore.Routing;

namespace DotvvmMvcIntegration.DotVVM.Controls
{
    public class RenderMvcAction : DotvvmControl
    {
        private readonly MvcUtility mvcUtility;

        [MarkupOptions(AllowBinding = false)]
        public string ControllerName
        {
            get { return (string)GetValue(ControllerNameProperty); }
            set { SetValue(ControllerNameProperty, value); }
        }
        public static readonly DotvvmProperty ControllerNameProperty
            = DotvvmProperty.Register<string, RenderMvcAction>(c => c.ControllerName, null);

        [MarkupOptions(AllowBinding = false)]
        public string ViewName
        {
            get { return (string)GetValue(ViewNameProperty); }
            set { SetValue(ViewNameProperty, value); }
        }
        public static readonly DotvvmProperty ViewNameProperty
            = DotvvmProperty.Register<string, RenderMvcAction>(c => c.ViewName, null);

        [PropertyGroup("Param-")]
        [MarkupOptions(AllowBinding = false)]
        public VirtualPropertyGroupDictionary<object> Params => new VirtualPropertyGroupDictionary<object>(this, ParamsGroupDescriptor);
        public static DotvvmPropertyGroup ParamsGroupDescriptor = DotvvmPropertyGroup.Register<object, RenderMvcAction>("Param-", nameof(Params));

        [MarkupOptions(AllowBinding = false)]
        public object Model
        {
            get { return (object)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        public static readonly DotvvmProperty ModelProperty
            = DotvvmProperty.Register<object, RenderMvcAction>(c => c.Model, null);


        public RenderMvcAction(MvcUtility mvcUtility)
        {
            this.mvcUtility = mvcUtility;
        }


        protected override void RenderContents(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            var parameters = new RouteValueDictionary();
            foreach (var param in Params.RawValues)
            {
                var value = param.Value is IValueBinding ? ((IValueBinding)param.Value).Evaluate(this) : param.Value;
                parameters.Add(param.Key, value);
            }

            var w = new StringWriter();
            Task.Run(async () => await mvcUtility.RenderAction(ControllerName, ViewName, parameters, Model, w)).Wait();
            writer.WriteUnencodedText(w.ToString());
        }

    }
}