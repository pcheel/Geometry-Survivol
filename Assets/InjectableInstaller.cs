using Scellecs.Morpeh;
using Zenject;

public class InjectableInstaller : Scellecs.Morpeh.Installer
{
    [Inject] private DiContainer _container;

    private void Start()
    {
        foreach (var updateSystem in updateSystems)
        {
            _container.Inject(updateSystem.System);
        }
        foreach (var fixedUpdateSystem in fixedUpdateSystems)
        {
            _container.Inject(fixedUpdateSystem.System);
        }
    }
}
