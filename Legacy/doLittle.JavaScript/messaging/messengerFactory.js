Dolittle.namespace("Dolittle.messaging", {
    messengerFactory: Dolittle.Singleton(function () {
        this.create = function () {
            var messenger = Dolittle.messaging.Messenger.create();
            return messenger;
        };

        this.global = function () {
            return Dolittle.messaging.Messenger.global;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.messengerFactory = Dolittle.messaging.messengerFactory;