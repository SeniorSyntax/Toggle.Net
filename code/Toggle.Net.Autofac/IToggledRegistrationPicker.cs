namespace Toggle.Net.Autofac;

public interface IToggledRegistrationPicker
{
    object PickService<TOn, TOff>(string toggleName);
}