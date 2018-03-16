Dolittle.namespace("Dolittle.commands", {
    commandSecurityContextFactory: Dolittle.Singleton(function () {
        this.create = function () {
            var context = Dolittle.commands.CommandSecurityContext.create();
            return context;
        };
    })
});