using System;

namespace Toggle.Net.Autofac;

public class ToggledRegistrationIsNotEnabledException() : Exception(
    "You need to enable toggled registrations in order to use them. Use builder.EnableToggledRegistrations(...) when building your container.");