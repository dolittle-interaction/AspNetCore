Dolittle.namespace("Dolittle", {
    dispatcher: Dolittle.Singleton(function () {
        this.schedule = function (milliseconds, callback) {
            setTimeout(callback, milliseconds);
        };
    })
});