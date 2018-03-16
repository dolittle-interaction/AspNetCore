Dolittle.namespace("Dolittle.markup", {
    AttachedProperty: Dolittle.values.ValueConsumer.extend(function () {
        this.canNotifyChanges = function () {
            return false;
        };

        this.notifyChanges = function (callback) {
        };

        this.consume = function (value) {
        };
    })
});