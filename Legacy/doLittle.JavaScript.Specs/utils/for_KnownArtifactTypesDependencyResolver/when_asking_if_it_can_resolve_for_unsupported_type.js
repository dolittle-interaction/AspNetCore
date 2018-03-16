describe("when asking if it can resolve for unsupported", sinon.test(function () {

    var resolver,
        canResolve,
        propertyToResolve,
        namespace;

    beforeEach(function () {
        Dolittle.commands = sinon.stub().returns({ Command: function () { } });
        Dolittle.read = sinon.stub().returns({
            ReadModelOf: function () { },
            Query: function () { }
        });

        resolver = new Dolittle.KnownArtifactTypesDependencyResolver();
        canResolve = false;
        propertyToResolve = "someOtherType";
        namespace = {};
        
        canResolve = resolver.canResolve(namespace, propertyToResolve);
    });

    it("should return false", function () {
        expect(canResolve).toBe(false);
    })

}));