describe("when resolving and resolvers can not resolve", function () {
    var resolver = {
        canResolve: sinon.stub().returns(false)
    };
    var exception;

    var dependencyResolvers;
    beforeEach(function () {
        dependencyResolvers = Dolittle.dependencyResolvers;

        Dolittle.dependencyResolvers = {
            getAll: function () {
                return [resolver];
            }
        };
        try {
            Dolittle.dependencyResolver.resolve("Something");
        } catch (e) {
            exception = e;
        }
    });

    afterEach(function () {
        Dolittle.dependencyResolvers = dependencyResolvers;
    });

    it("should throw unresolved dependencies exception", function () {
        expect(exception instanceof Dolittle.UnresolvedDependencies).toBeTruthy();
    });

});