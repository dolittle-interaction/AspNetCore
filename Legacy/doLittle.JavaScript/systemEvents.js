Dolittle.namespace("Dolittle",{
    systemEvents: Dolittle.Singleton(function () {
        this.readModels = Dolittle.read.readModelSystemEvents.create();
        this.commands = Dolittle.commands.commandEvents.create();
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.systemEvents = Dolittle.systemEvents;