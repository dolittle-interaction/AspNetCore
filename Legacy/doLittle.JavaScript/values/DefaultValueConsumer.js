Dolittle.namespace("Dolittle.values", {
    DefaultValueConsumer: Dolittle.values.ValueConsumer.extend(function (target, property) {
        this.consume = function(value) {
            target[property] = value;
        };
    })
});