Dolittle.namespace("Dolittle.markup", {
    BindingContext: Dolittle.Type.extend(function () {
        this.parent = null;
        this.current = null;

        this.changed = Dolittle.Event.create();
    })
});