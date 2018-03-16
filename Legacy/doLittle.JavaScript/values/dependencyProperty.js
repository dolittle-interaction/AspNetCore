Dolittle.namespace("Dolittle.values", {
    dependencyProperty: Dolittle.Type.extend(function (propertyName) {
        this.initialize = function (UIElement) { };
        this.dispose = function (UIElement) {};

        this.set = function (UIElement, value) {
            UIElement[propertyName] = value;
        };

        this.get = function (UIElement) {
            return UIElement[propertyName];
        };
    })
});

Dolittle.values.DependencyProperty.register = function (owningType, name, dependencyPropertyType) {
};

Dolittle.namespace("Dolittle.DOM", {
    inputValueDependencyProperty: Dolittle.values.dependencyProperty.extend(function() {

        function inputChanged(e) {
            if( Dolittle.isFunction(e.target._changed) ) {
                e.target._changed(e.value);
            }
        }

        this.initialize = function (UIElement, changed) {
            UIElement._changed = changed;
            UIElement.addEventListener("change", inputChanged);
        };

        this.dispose = function (UIElement) {
            UIElement.removeEventListener("change", inputChanged);
        };
    })
});


Dolittle.values.DependencyProperty.register(HTMLInputElement, "value", Dolittle.DOM.inputValueDependencyProperty);
