namespace Toggle.Net.Ioc;

public interface IToggledRegistrationPicker
{
    object PickService<TOn, TOff>(string toggleName);
}