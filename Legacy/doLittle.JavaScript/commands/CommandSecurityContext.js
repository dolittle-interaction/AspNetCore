Dolittle.namespace("Dolittle.commands", {
    CommandSecurityContext: Dolittle.Type.extend(function () {
        this.isAuthorized = ko.observable(false);
    })
});