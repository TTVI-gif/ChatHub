using ChatHub.Infrastructures.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ChatHub.WebAPI.Conventions
{
    public class ControllerNameAttributeConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNameAttribute = controller.Attributes.OfType<ControllerNameAttribute>().SingleOrDefault();
            if (controllerNameAttribute != null)
                controller.ControllerName = controllerNameAttribute.ControllerName;
        }
    }
}
