Dolittle.namespace("Dolittle.commands");
Dolittle.commands.CommandRequest = function(command) {
    var self = this;

    var builtInCommand = {};
    if (typeof Dolittle.commands.Command !== "undefined") {
        builtInCommand = Dolittle.commands.Command.create({
            region: { commands: [] },
            commandCoordinator: {},
            commandValidationService: {},
            commandSecurityService: { getContextFor: function () { return { continueWith: function () { } }; } },
            mapper: {},
            options: {}
        });
    }

    function shouldSkipProperty(target, property) {
        if (!target.hasOwnProperty(property)) {
            return true;
        }
        if (builtInCommand.hasOwnProperty(property)) {
            return true;
        }
        if (ko.isObservable(target[property])) {
            return false;
        }
        if (typeof target[property] === "function") {
            return true;
        }
        if (property === "_type") {
            return true;
        }
        if (property === "_commandType") {
            return true;
        }
        if (property === "_namespace") {
            return true;
        }

        return false;
    }

    function getPropertiesFromCommand(command) {
        var properties = {};

        for (var property in command) {
            if (!shouldSkipProperty(command, property) ) {
                properties[property] = command[property];
            }
        }
        return properties;
    }

    this.type = command._commandType;
    this.correlationId = Dolittle.Guid.create();

    var properties = getPropertiesFromCommand(command);
    var commandContent = ko.toJS(properties);
    this.content = ko.toJSON(commandContent);
};


Dolittle.commands.CommandRequest.createFrom = function (command) {
    var commandDescriptor = new Dolittle.commands.CommandRequest(command);
    return commandDescriptor;
};