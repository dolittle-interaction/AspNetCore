Dolittle.namespace("Dolittle.views", {
    UIManager: Dolittle.Singleton(function(documentService) {
        var elementVisitorTypes = Dolittle.markup.ElementVisitor.getExtenders();
        var elementVisitors = [];
        var postBindingVisitorTypes = Dolittle.views.PostBindingVisitor.getExtenders();
        var postBindingVisitors = [];

        elementVisitorTypes.forEach(function (type) {
            elementVisitors.push(type.create());
        });

        postBindingVisitorTypes.forEach(function (type) {
            postBindingVisitors.push(type.create());
        });

        this.handle = function (root) {
            documentService.traverseObjects(function(element) {
                elementVisitors.forEach(function(visitor) {
                    var actions = Dolittle.markup.ElementVisitorResultActions.create();
                    visitor.visit(element, actions);
                });
            }, root);
        };

        this.handlePostBinding = function (root) {
            documentService.traverseObjects(function (element) {
                postBindingVisitors.forEach(function (visitor) {
                    visitor.visit(element);
                });
            }, root);
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.UIManager = Dolittle.views.UIManager;