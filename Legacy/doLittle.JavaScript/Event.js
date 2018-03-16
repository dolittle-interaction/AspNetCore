Dolittle.namespace("Dolittle",{
    Event: Dolittle.Type.extend(function () {
        var subscribers = [];

        this.subscribe = function (subscriber) {
            subscribers.push(subscriber);
        };

        this.trigger = function (data) {
            subscribers.forEach(function (subscriber) {
                subscriber(data);
            });
        };
    })
});