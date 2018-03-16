Dolittle.namespace("Dolittle.commands", {
    commandEvents: Dolittle.Singleton(function () {
        this.succeeded = Dolittle.Event.create();
        this.failed = Dolittle.Event.create();
    })
});