namespace ChatHub.Infrastructures.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerNameAttribute : Attribute
    {
        public string ControllerName { get; }

        public ControllerNameAttribute(string controllerName)
        {
            ControllerName = controllerName;
        }
    }
}
