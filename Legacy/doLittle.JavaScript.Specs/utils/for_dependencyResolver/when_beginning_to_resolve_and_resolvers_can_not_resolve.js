describe("when beginning to resolve and resolvers can not resolve", function () {
    var resolver = {
        canResolve: sinon.stub().returns(false)
    };
    var exception;
    var readyCallback;

    var configure = null;
    var dependencyResolvers;

    beforeEach(function () {
        configure = Dolittle.configure;
        Dolittle.configure = {
            ready: function (callback) {
                readyCallback = callback;
            }
        };
        dependencyResolvers = Dolittle.dependencyResolvers;
        Dolittle.dependencyResolvers = {
            getAll: function () {
                return [resolver];
            }
        };

        try {
            Dolittle.dependencyResolver.beginResolve("Something").onFail(function (e) {
                exception = e;
            });

            readyCallback();
        } catch (e) {
            exception = e;
        }
    });

    afterEach(function () {
        Dolittle.dependencyResolvers = dependencyResolvers;
        Dolittle.configure = configure;
    });


    it("should throw unresolved dependencies exception", function () {
        expect(exception instanceof Dolittle.UnresolvedDependencies).toBeTruthy();
    });

});