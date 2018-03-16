Dolittle.namespace("Dolittle.values", {
    ValueConsumer: Dolittle.Type.extend(function () {

        this.canNotifyChanges = function () {
            return false;
        };

        this.notifyChanges = function (callback) {
        };

        this.consume = function(value) {
        };
    })
});