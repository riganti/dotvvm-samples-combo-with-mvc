using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.ViewModel;
using DotvvmMvcIntegration.Models;

namespace DotvvmMvcIntegration.DotVVM.ViewModels
{
	public class DotvvmSampleViewModel : DotvvmViewModelBase
	{

        public DetailComponentModel MyModel { get; set; } = new DetailComponentModel()
        {
            Id = 15,
            Title = "My Component",
            Description = "Description"
        };

    }
}

