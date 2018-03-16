Dolittle.namespace("Dolittle.hubs", {
    hubConnection: Dolittle.Singleton(function () {
        var self = this;
        var hub = $.hubConnection("/signalr", { useDefaultPath: false });
        /* jshint ignore:start */
        $.signalR.hub = hub;
        /* jshint ignore:end */

        this.isConnected = false;
        this.connected = Dolittle.Event.create();

        this.createProxy = function (hubName) {
            var proxy = hub.createHubProxy(hubName);
            return proxy;
        };

        //$.connection.hub.logging = true;
        $.connection.hub.start().done(function () {
            console.log("Hub connection up and running");
            self.isConnected = true;
            self.connected.trigger();
        });
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.hubConnection = Dolittle.hubs.hubConnection;